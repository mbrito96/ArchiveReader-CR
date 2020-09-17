using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime;

namespace ArchiveReader
{
	public delegate double StatCalculateDelegate(List<double> xs, List<double> values);

	public class S_STAT_DATA	// Class for single value statistical data
	{
		public double value;
		public StatCalculateDelegate Calculate;
		public S_STAT_DATA(StatCalculateDelegate callback)
		{
			this.value = 0;
			this.Calculate = callback;
		}
	}
	public class M_STAT_DATA	// Class for single value statistical data
	{
		public List<double> value;
		public StatCalculateDelegate Calculate;
		public M_STAT_DATA(StatCalculateDelegate callback)
		{
			this.value = new List<double>();
			this.Calculate = callback;
		}
	}
	public class TEMP_STATISTICS
	{
		public S_STAT_DATA mean;
		public S_STAT_DATA std;
		public S_STAT_DATA timeInRange;
		public S_STAT_DATA timeRange;
		public S_STAT_DATA deltaToRef;
		public TEMP_STATISTICS(StatCalculateDelegate MeanCallbk = null, StatCalculateDelegate StdCallbk = null, StatCalculateDelegate InRangeCallbk = null, StatCalculateDelegate RangeCallbk = null, StatCalculateDelegate DeltaCallbk = null)
		{
			if(MeanCallbk != null)
				this.mean = new S_STAT_DATA(MeanCallbk);
			if(StdCallbk != null)
				this.std = new S_STAT_DATA(StdCallbk);
			if(InRangeCallbk != null)
				this.timeInRange = new S_STAT_DATA(InRangeCallbk);
			if(RangeCallbk != null)
				this.timeRange = new S_STAT_DATA(RangeCallbk);
			if(DeltaCallbk != null)
				this.deltaToRef = new S_STAT_DATA(DeltaCallbk);
		}
		public void CalculateStats(List<double> xs,  List<double> data)
		{
			if(this.mean != null)
				this.mean.value = this.mean.Calculate(xs, data);
			if(this.std != null)
				this.std.value = this.std.Calculate(xs, data);
			if(this.timeInRange != null)
				this.timeInRange.value = this.timeInRange.Calculate(xs, data);
			if(this.timeRange != null)
				this.timeRange.value = this.timeRange.Calculate(xs, data);
			if(this.deltaToRef != null)
				this.deltaToRef.value = this.deltaToRef.Calculate(xs, data);
		}
	}
	public class PRESS_STATISTICS
	{
		int pressureID;
		public double meanOn;
		public double meanOff;
		public double stdOn;
		public double timeInRange;
		public double timeRange;
		public PRESS_STATISTICS(int pressureID)
		{
			this.pressureID = pressureID;
			this.meanOn = new double();
			this.meanOff = new double();
			this.stdOn = new double();
			this.timeInRange = new double();
			this.timeRange = new double();
		}
		public void CalculateStats(List<double> xs,  List<double> data, List<int> digital, GWF_DECODER decoder)
		{
			double [] meanPressuresOnOff = CalculateMeanOnOff(data, digital, decoder);
			meanOff = meanPressuresOnOff[0];
			meanOn = meanPressuresOnOff[1];

		}
		private double[] CalculateMeanOnOff(List<double> values, List<int> digital, GWF_DECODER decoder)
		{
			double[] meanArray = new double[]{0,0};
			int samplesOff = 0;
			int cmpToWatch = 0;
			switch(decoder.model)
			{
				case MacModel.A80TR:
					switch(pressureID)
					{
						case GWF80TR_PRESSURE_FIELDS.P_HIGH_A:
							cmpToWatch = GWF80TR_DIG_BITFIELD.CMP_A;
							break;
						case GWF80TR_PRESSURE_FIELDS.P_HIGH_B:
							cmpToWatch = GWF80TR_DIG_BITFIELD.CMP_B;
							break;
						default:
							return meanArray;
					}
					break;
				case MacModel.W90TR:
					switch(pressureID)
					{
						case GWF90TR_PRESSURE_FIELDS.P_HIGH_TA:
							cmpToWatch = GWF90TR_DIG_BITFIELD.CMP_TA;
							break;
						case GWF90TR_PRESSURE_FIELDS.P_HIGH_S:
							cmpToWatch = GWF90TR_DIG_BITFIELD.CMP_S;
							break;
						default:
							return meanArray;
					}
					break;
				default:
					return meanArray;
			}

			for(int i = 0 ; i < values.Count ; i++)
			{
				if(decoder.digital.GetFlag(digital[i], cmpToWatch) ==  false)
				{
					meanArray[0] += values[i];
					samplesOff++;
				}
				else
					meanArray[1] += values[i];
			}
			meanArray[0] = meanArray[0]/samplesOff;
			meanArray[1] = meanArray[1]/(values.Count - samplesOff);

			return meanArray;
		}
	}
	partial class ArchivePlotter
	{	
		
		public double Clbk_MeanTempOut(List<double> xs,  List<double> values)
		{
			double retVal = 0;
			if(values.Count > 0)
				retVal = values.Average();
			
			return retVal;
		}
		public double Clbk_StdTempOut(List<double> xs,  List<double> values)
		{
			double retVal = 0;

			if(values.Count > 0)
			{
				// Compute the average.     
				double avg = values.Average();
				// Perform the Sum of (value-avg)_2_2.      
				double sum = values.Sum(d => Math.Pow(d - avg, 2));
			     // Put it all together.      
				retVal = Math.Sqrt((sum) / (values.Count()-1)); 
			}
			return retVal;
		}
		public double Clbk_InRangeTempOut(List<double> xs,  List<double> values)
		{
			double top = (double) numUpDown_setpoint.Value + (double) numUpDown_setpointError.Value;
			double bottom = (double) numUpDown_setpoint.Value - (double) numUpDown_setpointError.Value;
			int i=0;
			double timeAccumulator = 0;
			for(i=0 ; i < values.Count-1 ; i++)
			{
				if(values[i] <= top && values[i] >= bottom )
					timeAccumulator += (xs[i+1]-xs[i]);
			}

			return timeAccumulator;
		}
		public static double Clbk_RangeTempOut(List<double> xs,  List<double> values)
		{
			return (xs.Last()-xs[0]);
		}
		public static double Clbk_DeltaTempOut(List<double> xs,  List<double> values)
		{
			double retVal = 0;
			return retVal;
		}
		
	}
}