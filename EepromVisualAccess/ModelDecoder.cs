using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArchiveReader
{
	public class GWF_DIGITAL_FIELD
	{
		public int[] COLUMN_LUT;
		public string[] COLUMN_NAME;
		public int[] COLUMN_SIZE;
		public virtual bool GetFlag(int digVar, int bitfield) {		return ( ( (digVar >> bitfield) & 0x1) == 1? true : false);		}
		public virtual OpStatus GetOpStatus(int digVar, int opModePos, int opModeSize) {	return (OpStatus)((digVar >> opModePos) & 0x3);		}
		public virtual int GetCoolingPower(int digVar){return 0;}
	}
	public class GWF_TEMPERATURE_FIELD
	{
		public int[] COLUMN_LUT;
		public string[] COLUMN_NAME;
		public int[] COLUMN_SIZE;
	}
	public class GWF_PRESSURE_FIELD
	{
		public int[] COLUMN_LUT;
		public string[] COLUMN_NAME;
		public int[] COLUMN_SIZE;
	}

	public class GWF_DECODER
	{
		public MacModel model;
		public GWF_DIGITAL_FIELD digital;
		public GWF_TEMPERATURE_FIELD temperatures;
		public GWF_PRESSURE_FIELD pressures;
		public GWF_DECODER(MacModel m)
		{
			this.model = m;
			switch(m)
			{
				case MacModel.A80TR:
					digital = new GWF80TR_DIG_BITFIELD();
					temperatures = new GWF80TR_TEMP_FIELDS();
					pressures = new GWF80TR_PRESSURE_FIELDS();
					break;
				case MacModel.W90TR:
					digital = new GWF90TR_DIG_BITFIELD();
					temperatures = new GWF90TR_TEMP_FIELDS();
					pressures = new GWF90TR_PRESSURE_FIELDS();
					break;
			}
		}
	}
}
