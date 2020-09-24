using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime;

namespace ArchiveReader
{
	public delegate double StatCalculateDelegate(List<double> xs, List<double> values);
	static class STAT_METHODS
	{
		public static double Std(List<double> values)
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
		public static double Mean(List<double> values)
		{
			double retVal = 0;
			if(values.Count > 0)
				retVal = values.Average();
			
			return retVal;
		}
	}
	public class KPI_RANGE // Key indicator class
	{
		public static Color COLOR_OK = Color.LightGreen;
		public static Color COLOR_WARNING = Color.Gold;
		public static Color COLOR_ERROR = Color.Tomato;
		enum TYPE{
			ERROR,
			RANGE,
			GREATER_THAN,
			SMALLER_THAN
		}
		TYPE type;
		double[] range;
		public Color color;
		/// <summary>
		/// Sets KPI_RANGE values.
		/// </summary>
		/// <param name="color">Color code for a value in this range.</param>
		/// <param name="rangeBot"> Bottom value of range. If null, range is <= rangeTop. </param>
		/// <param name="rangeTop"> Top value of range. If null, range is >= rangeBot. </param>
		public KPI_RANGE(Color color, double ? rangeBot = null, double ? rangeTop = null)
		{
			range = new double[2]{ (double)(rangeBot == null ? 0 : rangeBot), (double)(rangeTop == null ? 0 : rangeTop)};
			type = TYPE.ERROR;
			if(rangeBot == null && rangeTop != null)
				type = TYPE.SMALLER_THAN;
			else if(rangeBot != null && rangeTop == null)
				type = TYPE.GREATER_THAN;
			else if(rangeBot != null && rangeTop != null)
				type = TYPE.RANGE;
				
			this.color = color;
		}
		public bool InRange(double val)		
		{
			bool inRange = true;
			if( (type == TYPE.RANGE || type == TYPE.GREATER_THAN) && val < range[0])
				inRange = false;
			if( (type == TYPE.RANGE || type == TYPE.SMALLER_THAN) && val > range[1])
				inRange = false;
			return inRange;
		}
	}
	public class KPI
	{
		public double value;
		public List<KPI_RANGE> ranges;
		public KPI(List<KPI_RANGE> ranges = null)
		{
			value = new double();
			if(ranges == null)
				this.ranges = new List<KPI_RANGE>();
			else
				this.ranges = ranges;
		}
		public Color GetKpiColor()
		{
			Color clr = Color.White;
			foreach(var r in ranges)
			{
				if(r.InRange(value))
				{
					clr = r.color;
					break;
				}
			}
			return clr;
		}
	}
	public class TEMP_STATISTICS
	{
		public bool updated;
		public int tempID;
		public KPI mean;
		public KPI std;
		public KPI max;
		public KPI performance;
		public double timeInRange;
		public double timeRange;
		public TEMP_STATISTICS(int temperatureID, List<KPI_RANGE> meanRange = null, List<KPI_RANGE> stdRange = null, List<KPI_RANGE> maxRange = null, List<KPI_RANGE> performanceRange = null)
		{
			updated = false;
			tempID = temperatureID;
			mean = new KPI(meanRange);
			std = new KPI(stdRange);
			max = new KPI(maxRange);
			performance = new KPI(performanceRange);
			timeInRange = new double();
			timeRange = new double();
		}
		public void CalculateStats( List<double> xs,  List<double> data, double? tempSetpoint = null, double? setpointTolerance = 0)
		{
			updated = true;
			this.mean.value = STAT_METHODS.Mean(data);
			this.std.value = STAT_METHODS.Std(data);
			this.max.value = data.Max();
			if(tempSetpoint != null)
			{
				this.timeInRange = TempInRange(xs, data, (double)tempSetpoint, (double)setpointTolerance);
				this.timeRange = TempTimeRange(xs, data);
				this.performance.value = timeInRange / timeRange * 100 ;
			}
		}
		public double TempInRange(List<double> xs,  List<double> values, double setpoint, double setpointError)
		{
			double top = setpoint + setpointError;
			double bottom = setpoint - setpointError;
			int i=0;
			double timeAccumulator = 0;
			for(i=0 ; i < values.Count-1 ; i++)
			{
				if(values[i] <= top && values[i] >= bottom )
					timeAccumulator += (xs[i+1]-xs[i]);
			}

			return timeAccumulator;
		}
		public static double TempTimeRange(List<double> xs,  List<double> values)
		{
			return (xs.Last()-xs[0]);
		}
		public static List<double> TempDeltaToRef( List<double> values, List<double> refTemp)
		{
			List<double> deltaSignal = new List<double>(0);
			for(int i = 0 ; i < values.Count ; i++)
				deltaSignal.Add(refTemp[i]-values[i]);
			
			return deltaSignal;
		}
		
	}
	public class PRESS_STATISTICS
	{
		public int pressureID;
		public bool updated;
		public double onRatio;	// Indicates the ratio: #registers with on / total registers
		public KPI meanOn;
		public KPI stdOn;
		public KPI minOn;
		public KPI maxOn;
		public PRESS_STATISTICS(int pressureID, List<KPI_RANGE> meanOnRange = null, List<KPI_RANGE> stdOnRange = null, List<KPI_RANGE> minOnRange = null, List<KPI_RANGE> maxOnRange = null)
		{
			this.updated = false;
			this.pressureID = pressureID;
			this.meanOn = new KPI(meanOnRange);
			this.stdOn = new KPI(stdOnRange);
			this.minOn = new KPI(minOnRange);
			this.maxOn = new KPI(maxOnRange);
		}
		public void UpdateStatRanges(List<KPI_RANGE> meanOnRange = null, List<KPI_RANGE> stdOnRange = null, List<KPI_RANGE> minOnRange = null, List<KPI_RANGE> maxOnRange = null)
		{
			if(meanOnRange != null)
				this.meanOn.ranges = meanOnRange;
			if(stdOnRange != null)
				this.stdOn.ranges = stdOnRange;
			if(minOnRange != null)
				this.minOn.ranges = minOnRange;
			if(maxOnRange != null)
				this.maxOn.ranges = maxOnRange;
		}
		public void CalculateStats(List<double> xs,  List<double> data, List<int> digital, GWF_DECODER decoder)
		{
			updated = true;
			List<double> dataOn = new List<double>();
			List<double> timeOn = new List<double>();
			List<double> dataOff = new List<double>();

			int cmpToWatch = GetCmpToWatchFromPressureId(decoder);
			// Separate data frames
			for(int i = 0 ; i < xs.Count ; i++)
			{
				if(decoder.digital.GetFlag(digital[i], cmpToWatch) ==  true)	// Cmp is ON
				{
					dataOn.Add(data[i]);
					timeOn.Add(xs[i]);
				}
				else
					dataOff.Add(data[i]);
			}
			if(dataOn.Count > 0)
			{
				meanOn.value = STAT_METHODS.Mean(dataOn);
				stdOn.value = STAT_METHODS.Std(dataOn);
				minOn.value = dataOn.Min();
				maxOn.value = dataOn.Max();
			}
			onRatio = (data.Count > 0 ? (double)dataOn.Count / (double)data.Count : 0);
		}
		private int GetCmpToWatchFromPressureId(GWF_DECODER decoder)
		{
			int cmpToWatch = 0;
			switch(decoder.model)
			{
				case MacModel.A80TR:
					switch(pressureID)
					{
						case GWF80TR_PRESSURE_FIELDS.P_HIGH_A:
						case GWF80TR_PRESSURE_FIELDS.P_LOW_A:
						case GWF80TR_PRESSURE_FIELDS.P_OIL_A:
							cmpToWatch = GWF80TR_DIG_BITFIELD.CMP_A;
							break;
						case GWF80TR_PRESSURE_FIELDS.P_HIGH_B:
						case GWF80TR_PRESSURE_FIELDS.P_LOW_B:
						case GWF80TR_PRESSURE_FIELDS.P_OIL_B:
							cmpToWatch = GWF80TR_DIG_BITFIELD.CMP_B;
							break;
						default:
							return -1;
					}
					break;
				case MacModel.W90TR:
					switch(pressureID)
					{
						case GWF90TR_PRESSURE_FIELDS.P_HIGH_TA:
						case GWF90TR_PRESSURE_FIELDS.P_LOW_TA:
						case GWF90TR_PRESSURE_FIELDS.P_OIL_TA:
						case GWF90TR_PRESSURE_FIELDS.P_OIL_TB:
							cmpToWatch = GWF90TR_DIG_BITFIELD.CMP_TA;
							break;
						case GWF90TR_PRESSURE_FIELDS.P_HIGH_S:
						case GWF90TR_PRESSURE_FIELDS.P_LOW_S:
						case GWF90TR_PRESSURE_FIELDS.P_OIL_S:
							cmpToWatch = GWF90TR_DIG_BITFIELD.CMP_S;
							break;
						default:
							return -1;
					}
					break;
				default:
					return -1;
			}
			return cmpToWatch;
		}
		public double PressStdOn(List<double> values, List<int> digital, GWF_DECODER decoder)
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
	}

	public partial class ArchivePlotter
	{
		private void selBox_Temps_Stats_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox selector = (ComboBox) sender;
			TempData selectedTemp = null;
			// Determine temperature that was selected
			foreach(TempData x in seriesData.temps)
			{
				if(selector.Text.Equals(x.name))
				{
					selectedTemp = x;
					break;
				}
			}
			if(selectedTemp != null)
			{
				if(selectedTemp.stats.updated == false)
					CalculateTempStat(selectedTemp);
				DisplayTempNumericStats(selectedTemp);
			}
		}
		private void selBox_Press_Stats_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox selector = (ComboBox) sender;
			PressureData selectedPressure = null;

			// Determine temperature that was selected
			foreach(PressureData x in seriesData.pressures)
			{
				if(selector.Text.Equals(x.name))
				{
					selectedPressure = x;
					break;
				}
			}
			if(selectedPressure != null)
			{
				if(selectedPressure.stats.updated == false)
					CalculatePressStat(selectedPressure);
				DisplayPressNumericStats(selectedPressure);
			}
		}
		private void selBox_Press_Configs_SelectedIndexChanged(object sender, EventArgs e)
		{
			int n = selBox_Press_Configs.SelectedIndex;
			switch(decoder.model)
			{
				case MacModel.A80TR:
					seriesData.pressures[GWF80TR_PRESSURE_FIELDS.P_HIGH_A].stats.UpdateStatRanges(meanOnRange:KpiPresHigh_GetMeanOnRanges(n), minOnRange:KpiPresHigh_GetMinOnRanges(n), maxOnRange:KpiPresHigh_GetMaxOnRanges(n));
					seriesData.pressures[GWF80TR_PRESSURE_FIELDS.P_HIGH_B].stats.UpdateStatRanges(meanOnRange:KpiPresHigh_GetMeanOnRanges(n), minOnRange:KpiPresHigh_GetMinOnRanges(n), maxOnRange:KpiPresHigh_GetMaxOnRanges(n));
					break;
				case MacModel.W90TR:
					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_HIGH_TA].stats.UpdateStatRanges(meanOnRange:KpiPresHigh_GetMeanOnRanges(n), minOnRange:KpiPresHigh_GetMinOnRanges(n), maxOnRange:KpiPresHigh_GetMaxOnRanges(n));
					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_HIGH_S].stats.UpdateStatRanges(meanOnRange:KpiPresHigh_GetMeanOnRanges(n), minOnRange:KpiPresHigh_GetMinOnRanges(n), maxOnRange:KpiPresHigh_GetMaxOnRanges(n));
					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_LOW_TA].stats.UpdateStatRanges(meanOnRange:KpiPresLow_GetMeanOnRanges(n), minOnRange:KpiPresLow_GetMinOnRanges(n), maxOnRange:KpiPresLow_GetMaxOnRanges(n));
					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_LOW_S].stats.UpdateStatRanges(meanOnRange:KpiPresLow_GetMeanOnRanges(n), minOnRange:KpiPresLow_GetMinOnRanges(n), maxOnRange:KpiPresLow_GetMaxOnRanges(n));
					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_TA].stats.UpdateStatRanges(meanOnRange:KpiPresLow_GetMeanOnRanges(n), minOnRange:KpiPresLow_GetMinOnRanges(n), maxOnRange:KpiPresLow_GetMaxOnRanges(n));
					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_TB].stats.UpdateStatRanges(meanOnRange:KpiPresLow_GetMeanOnRanges(n), minOnRange:KpiPresLow_GetMinOnRanges(n), maxOnRange:KpiPresLow_GetMaxOnRanges(n));
					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_S].stats.UpdateStatRanges(meanOnRange:KpiPresLow_GetMeanOnRanges(n), minOnRange:KpiPresLow_GetMinOnRanges(n), maxOnRange:KpiPresLow_GetMaxOnRanges(n));
					break;
			}
			DisplayPressNumericStats( seriesData.pressures.Find(x => (x.name == selBox_Press_Stats.Text) ));
		}
		private void DisplayTempNumericStats(TempData selectedTemp)
		{
			txt_StatsTempsMean.Text = selectedTemp.stats.mean.value.ToString("N3");
			txt_StatsTempsMean.BackColor = selectedTemp.stats.mean.GetKpiColor();
			
			txt_StatsTempsMax.Text = selectedTemp.stats.max.value.ToString("N1");
			txt_StatsTempsMax.BackColor = selectedTemp.stats.max.GetKpiColor();

			txt_StatsTempsSTD.Text = selectedTemp.stats.std.value.ToString("N3");
			txt_StatsTempsSTD.BackColor = selectedTemp.stats.std.GetKpiColor();

			txt_StatsTempsPerf.Enabled = (selectedTemp.stats.timeRange != 0);
			if(txt_StatsTempsPerf.Enabled == true)	// Then performance was computed
			{
				txt_StatsTempsPerf.BackColor = selectedTemp.stats.performance.GetKpiColor();
				txt_StatsTempsPerf.Text = selectedTemp.stats.performance.value.ToString("N3") + "%";
			}
			else
				txt_StatsTempsPerf.Text = "";
		}
		private void DisplayPressNumericStats(PressureData selectedPressure)
		{
			txt_StatsPressMeanOn.Text = selectedPressure.stats.meanOn.value.ToString("N3");
			txt_StatsPressMeanOn.BackColor = selectedPressure.stats.meanOn.GetKpiColor();

			txt_StatsPressMinOn.Text = selectedPressure.stats.minOn.value.ToString("N1");
			txt_StatsPressMinOn.BackColor = selectedPressure.stats.minOn.GetKpiColor();

			txt_StatsPressMaxOn.Text = selectedPressure.stats.maxOn.value.ToString("N1");
			txt_StatsPressMaxOn.BackColor = selectedPressure.stats.maxOn.GetKpiColor();

			txt_StatsPressStdOn.Text = selectedPressure.stats.stdOn.value.ToString("N3");
			txt_StatsPressStdOn.BackColor = selectedPressure.stats.stdOn.GetKpiColor();
		}

		private void TempStatsUpdate()
		{
			CalculateTempStat();	// Compute all stats
			TempData selectedTemp = null;
			foreach(TempData x in seriesData.temps)
			{
				if(selBox_Temps_Stats.Text.Equals(x.name))
				{
					selectedTemp = x;
					break;
				}
			}
			if(selectedTemp != null)
				DisplayTempNumericStats(selectedTemp);
		}
		
		private void PressStatsUpdate()
		{
			CalculatePressStat();	// Compute all stats
			PressureData selectedPress = null;
			foreach(PressureData x in seriesData.pressures)
			{
				if(selBox_Press_Stats.Text.Equals(x.name))
				{
					selectedPress = x;
					break;
				}
			}
			if(selectedPress != null)
				DisplayPressNumericStats(selectedPress);
		}
		
		private void numUpDown_setpoint_ValueChanged(object sender, EventArgs e)
		{
			seriesData.temps[decoder.temperatures.GetTempOut()].stats.mean.ranges = KpiTempOut_GetMeanRanges();
			TempStatsUpdate();
		}
		private void CalculateTempStat(TempData y = null)
		{
			if(y == null)	// then update all temps
			{
				foreach(TempData x in seriesData.temps)
				{
					if(x.stats != null)
						CalculateTempStat(x);
				}
			}
			else	// Calculate stats for this temperature
			{
				y.stats.CalculateStats(seriesData.xs.ToList(), y.values, (double)numUpDown_setpoint.Value, (double)numUpDown_setpointError.Value);
					
			}
		}
		private void CalculatePressStat(PressureData y = null)
		{
			if(y == null)	// then update all pressures
			{
				foreach(PressureData x in seriesData.pressures)
				{
					if(x.stats != null)
						CalculatePressStat(x);
				}
			}
			else
				y.stats.CalculateStats(seriesData.xs.ToList(), y.values, seriesData.digital, decoder);
		}
		#region KPI_RANGES
		private List<KPI_RANGE> KpiTempOut_GetMeanRanges()
		{
			List<KPI_RANGE> ranges = new List<KPI_RANGE>();
			
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_OK, null, (double)(numUpDown_setpoint.Value + numUpDown_setpointError.Value)));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, (double)(numUpDown_setpoint.Value + numUpDown_setpointError.Value), (double)(numUpDown_setpoint.Value + 2 * numUpDown_setpointError.Value)));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, (double)(numUpDown_setpoint.Value + 2*numUpDown_setpointError.Value), null));
			return ranges;
		}
		private List<KPI_RANGE> KpiTempOut_GetPerformanceRanges()
		{
			List<KPI_RANGE> ranges = new List<KPI_RANGE>();
			
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, null, 50));	// Perf <= 50%
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, 50, 80));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_OK, 80, null));
			return ranges;
		}
		private List<KPI_RANGE> KpiPresHigh_GetMeanOnRanges(int configNumber = 0)
		{
			List<KPI_RANGE> ranges = new List<KPI_RANGE>();
			double g1=0, g2=0, yA1=0, yA2=0, yB1=0, yB2=0, rA2=0, rB1=0; 
			switch(configNumber)
			{
				case 0:
					rA2 = 160;
					yA1 = 160;
					yA2 = 180;
					g1 = 180;
					g2= 240;
					yB1 = 240;
					yB2 = 270;
					rB1 = 270;
					break;
				default:
					return null;
			}
			
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_OK, g1, g2));	// This goes first so that entire range is included as ok
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, null, rA2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, yA1, yA2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, yB1, yB2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, rB1, null));
			return ranges;
		}
		private List<KPI_RANGE> KpiPresHigh_GetMinOnRanges(int configNumber = 0)
		{
			List<KPI_RANGE> ranges = new List<KPI_RANGE>();
			double g1=0, yA1=0, yA2=0, rA2=0; 
			switch(configNumber)
			{
				case 0:
					rA2 = 160;
					yA1 = 160;
					yA2 = 180;
					g1 = 180;
					break;
				default:
					return null;
			}
			
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_OK, g1, null));	// This goes first so that entire range is included as ok
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, null, rA2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, yA1, yA2));
			return ranges;
		}
		private List<KPI_RANGE> KpiPresHigh_GetMaxOnRanges(int configNumber = 0)
		{
			List<KPI_RANGE> ranges = new List<KPI_RANGE>();
			double g2=0, yB1=0, yB2=0, rB1=0; 
			switch(configNumber)
			{
				case 0:
					g2= 250;
					yB1 = 250;
					yB2 = 280;
					rB1 = 280;
					break;
				default:
					return null;
			}
			
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_OK, null, g2));	// This goes first so that entire range is included as ok
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, yB1, yB2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, rB1, null));
			return ranges;
		}
		private List<KPI_RANGE> KpiPresLow_GetMeanOnRanges(int configNumber = 0)
		{
			List<KPI_RANGE> ranges = new List<KPI_RANGE>();
			double g1=0, g2=0, yA1=0, yA2=0, yB1=0, yB2=0, rA2=0, rB1=0; 
			switch(configNumber)
			{
				case 0:
					rA2 = 30;
					yA1 = 30;
					yA2 = 55;
					g1 = 55;
					g2= 65;
					yB1 = 65;
					yB2 = 75;
					rB1 = 75;
					break;
				default:
					return null;
			}
			
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_OK, g1, g2));	// This goes first so that entire range is included as ok
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, null, rA2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, yA1, yA2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, yB1, yB2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, rB1, null));
			return ranges;
		}
		private List<KPI_RANGE> KpiPresLow_GetMinOnRanges(int configNumber = 0)
		{
			List<KPI_RANGE> ranges = new List<KPI_RANGE>();
			double g1=0, yA1=0, yA2=0, rA2=0; 
			switch(configNumber)
			{
				case 0:
					rA2 = 30;
					yA1 = 30;
					yA2 = 40;
					g1 = 40;
					break;
				default:
					return null;
			}
			
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_OK, g1, null));	// This goes first so that entire range is included as ok
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, null, rA2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, yA1, yA2));
			return ranges;
		}
		private List<KPI_RANGE> KpiPresLow_GetMaxOnRanges(int configNumber = 0)
		{
			List<KPI_RANGE> ranges = new List<KPI_RANGE>();
			double g2=0, yB1=0, yB2=0, rB1=0; 
			switch(configNumber)
			{
				case 0:
					g2= 75;
					yB1 = 75;
					yB2 = 90;
					rB1 = 90;
					break;
				default:
					return null;
			}
			
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_OK, null, g2));	// This goes first so that entire range is included as ok
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_WARNING, yB1, yB2));
			ranges.Add(new KPI_RANGE(KPI_RANGE.COLOR_ERROR, rB1, null));
			return ranges;
		}
		#endregion
	}
}