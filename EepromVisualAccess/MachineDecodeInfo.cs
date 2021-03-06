﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveReader
{
	#region GWF20TR-A	// Used for GWF20, 30 and 40 TR-A
	public class GWF20TR_DIG_BITFIELD : GWF_DIGITAL_FIELD{
		// Archive: digital cell bitfield defines
		public const int FLOW = 0;
		public const int CMP = 1;
		public const int FAN_1 = 2;
		public const int FAN_2 = 3;
		public const int FAN_3 = 4;
		public const int FIT_CMP = 5;
		public const int FIT_FAN_1 = 6;
		public const int FIT_FAN_2 = 7;
		public const int FIT_FAN_3 = 8;
		public const int FIT_PUMP = 9;
		public const int OP_STATE = 10;
		public const int OP_STATE_1 = 11;
		public GWF20TR_DIG_BITFIELD()
		{
			COLUMN_LUT = new int[] {};
			COLUMN_NAME = new string[] {"Estado", "FS", "Cmp", "V1", "V2", "V3", "Bomba"};
			COLUMN_SIZE = new int[]{62, 30, 39, 28, 28, 28, 50};
		}
		public override int GetCoolingPower(int digVar)
		{
			int pwr = 0;
			if(GetFlag(digVar, CMP) == true)
				pwr++;
			return pwr;
		}
		public override int GetCmpToWatchFromPressureId(int pressureID)
		{
			return CMP;	// Only 1 cmp
		}
	}
	public class GWF20TR_TEMP_FIELDS : GWF_TEMPERATURE_FIELD{
		// Archive: Temperature field position
		public const int TEMP_IN = 0;
		public const int TEMP_OUT = 1;
		public const int TEMP_EVAP = 2;
		public const int TEMP_AMB = 3;
		public GWF20TR_TEMP_FIELDS()
		{
			COLUMN_LUT = new int[]{TEMP_IN, TEMP_OUT, TEMP_EVAP, TEMP_AMB};		// represents the id associated with each column
			COLUMN_NAME = new string[]{ "Temp Ent.", "Temp Sal.", "Temp Evap.", "Temp Amb."};
			COLUMN_SIZE = new int[] {120, 65, 70, 65};
		}
	}
	public class GWF20TR_PRESSURE_FIELDS : GWF_PRESSURE_FIELD{
		// Archive: Pressure field position
		public const int P_HIGH = 0;
		public const int P_LOW = 1;
		public const int P_OIL = 2;
		public GWF20TR_PRESSURE_FIELDS()
		{
			COLUMN_LUT = new int[]{P_HIGH, P_LOW, P_OIL};
			COLUMN_NAME = new string[]{"P. Alta", "P. Baja", "P. Aceite"};
			COLUMN_SIZE = new int[] {60, 60, 65};
		}

	}
	#endregion
	#region GWF80TR-A
	public class GWF80TR_DIG_BITFIELD : GWF_DIGITAL_FIELD{
		// Archive: digital cell bitfield defines
		public const int FLOW = 0;
		public const int CMP_A = 1;
		public const int CMP_B = 2;
		public const int FAN_A1 = 3;
		public const int FAN_A2 = 4;
		public const int FAN_A3 = 5;
		public const int FAN_B1 = 6;
		public const int FAN_B2 = 7;
		public const int FAN_B3 = 8;
		public const int FIT_CMP_A = 9;
		public const int FIT_CMP_B = 10;
		public const int FIT_FAN_A1 = 11;
		public const int FIT_FAN_A2 = 12;
		public const int FIT_FAN_A3 = 13;
		public const int FIT_FAN_B1 = 14;
		public const int FIT_FAN_B2 = 15;
		public const int FIT_FAN_B3 = 16;
		public const int FIT_PUMP = 17;
		public const int OP_STATE = 18;
		public const int OP_STATE_1 = 19;
		public const int OP_MODE = 20;
		public const int OP_MODE_1 = 21;
		public const int OP_MODE_2 = 22;
		public GWF80TR_DIG_BITFIELD()
		{
			COLUMN_LUT = new int[] {2, 3, 4, 5, 6, 7, 8, 9, 10, 3, 4, 5, 6, 7, 8, 9, 10, 11, 1, 1, 0, 0, 0};	// Used for accessing Columns. To be index with const int defined above
			COLUMN_NAME = new string[] {"Modo", "Estado", "FS", "Cmp.A", "Cmp.B", "V.A1", "V.A2", "V.A3", "V.B1", "V.B2", "V.B3", "Bomba"};
			COLUMN_SIZE = new int[]{};
		}
		public override int GetCoolingPower(int digVar)
		{
			int pwr = 0;
			if(GetFlag(digVar, CMP_A) == true)
				pwr++;
			if(GetFlag(digVar, CMP_B) == true)
				pwr++;
			return pwr;
		}
		public override int GetCmpToWatchFromPressureId(int pressureID)
		{
			int cmpToWatch = 0;
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
			}
			return cmpToWatch;
		}
	}
	public class GWF80TR_TEMP_FIELDS : GWF_TEMPERATURE_FIELD{
		// Archive: Temperature field position
		public const int TEMP_IN = 0;
		public const int TEMP_OUT = 1;
		public const int TEMP_EVAP = 2;
		public const int TEMP_AMB = 3;
		public GWF80TR_TEMP_FIELDS()
		{
			COLUMN_LUT = new int[]{TEMP_IN, TEMP_OUT, TEMP_EVAP, TEMP_AMB};		// represents the id associated with each column
			COLUMN_NAME = new string[]{ "Temp Ent.", "Temp Sal.", "Temp Evap.", "Temp Amb."};
			COLUMN_SIZE = new int[] {120, 65, 70, 65};
		}
	}
	public class GWF80TR_PRESSURE_FIELDS : GWF_PRESSURE_FIELD{
		// Archive: Pressure field position
		public const int P_HIGH_A = 0;
		public const int P_LOW_A = 1;
		public const int P_OIL_A = 2;
		public const int P_HIGH_B = 3;
		public const int P_LOW_B = 4;
		public const int P_OIL_B = 5;
		public GWF80TR_PRESSURE_FIELDS()
		{
			COLUMN_LUT = new int[]{P_HIGH_A, P_LOW_A, P_OIL_A, P_HIGH_B, P_LOW_B, P_OIL_B};
			COLUMN_NAME = new string[]{"P. Alta A", "P. Baja A", "P. Aceite A", "P. Alta B", "P. Baja B", "P. Aceite B"};
			COLUMN_SIZE = new int[] {60, 60, 65, 60, 60, 65};
		}

	}
	#endregion

	#region GWF90TR-W
	public class GWF90TR_DIG_BITFIELD : GWF_DIGITAL_FIELD{
		public const int FLOW_EVAP = 0;
		public const int FLOW_COND = 1;
		public const int CMP_TA = 2;
		public const int CMP_TB = 3;
		public const int CMP_S = 4;
		public const int VENT_1 = 5;
		public const int FIT_CMP_TA = 6;
		public const int FIT_CMP_TB = 7;
		public const int FIT_CMP_S = 8;
		public const int FIT_VENT_1 = 9;
		public const int FIT_PUMP_EV = 10;
		public const int FIT_PUMP_CO = 11;
		public const int OP_STATE = 12;
		public const int OP_STATE_1 = 13;
		public const int OP_MODE = 14;
		public const int OP_MODE_1 = 15;
		public const int OP_MODE_2 = 16;
		public GWF90TR_DIG_BITFIELD()
		{
			COLUMN_LUT = new int[]{};
			COLUMN_NAME = new string[] {"FS", "Cmp.A", "Cmp.B", "V.A1", "V.A2", "V.A3", "V.B1", "V.B2", "V.B3", "", "", "", "", "", "", "", "", "Bomba", "Modo", "", "Estado",  "", ""};
			COLUMN_SIZE = new int[] {};
		}
		public override int GetCoolingPower(int digVar)
		{
			int pwr = 0;
			if(GetFlag(digVar, CMP_TA) == true)
				pwr++;
			if(GetFlag(digVar, CMP_TB) == true)
				pwr++;
			if(GetFlag(digVar, CMP_S) == true)
				pwr++;
			return pwr;
		}
		public override int GetCmpToWatchFromPressureId(int pressureID)
		{
			int cmpToWatch = 0;
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
			}
			return cmpToWatch;
		}

	}
	public class GWF90TR_TEMP_FIELDS : GWF_TEMPERATURE_FIELD{
		// Archive: Temperature field position
		public const int TEMP_IN = 0;
		public const int TEMP_OUT = 1;
		public const int TEMP_EVAP = 2;
		public const int TEMP_COND = 3;
		public GWF90TR_TEMP_FIELDS()
		{
			COLUMN_LUT = new int[]{TEMP_IN, TEMP_OUT, TEMP_EVAP, TEMP_COND};	// represents the id associated with each column
			COLUMN_NAME = new string[] {"Temp Ent.", "Temp Sal.", "Temp Evap.", "Temp Ent. Cond."};
			COLUMN_SIZE = new int[] {120, 65, 70, 100};
		}

	}
	public class GWF90TR_PRESSURE_FIELDS : GWF_PRESSURE_FIELD{
		// Archive: Pressure field position
		public const int P_HIGH_TA = 0;
		public const int P_LOW_TA = 1;
		public const int P_OIL_TA = 2;
		public const int P_OIL_TB = 3;
		public const int P_HIGH_S = 4;
		public const int P_LOW_S = 5;
		public const int P_OIL_S = 6;
		public GWF90TR_PRESSURE_FIELDS()
		{
			COLUMN_LUT = new int[]{P_HIGH_TA, P_LOW_TA, P_OIL_TA, P_OIL_TB, P_HIGH_S, P_LOW_S, P_OIL_S};
			COLUMN_NAME = new string[] {"P. Alta (T)", "P. Baja (T)", "P. Aceite A (T)", "P. Aceite B (T)", "P. Alta (S)", "P. Baja (S)", "P. Aceite (S)"};
			COLUMN_SIZE = new int[] {60, 65, 83, 83, 60, 65, 83};
		}
	}
	#endregion
}
