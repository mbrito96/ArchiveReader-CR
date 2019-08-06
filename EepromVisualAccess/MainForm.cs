using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using stx8xxx;
using System.IO;

/// <summary>
/// ----------------------------------------------------------------------------------------------
/// File        : MainForm.cs
/// Title       : Lector de historial de operaciones para máquinas GWF
/// 
/// Revised     : 25/Abr/2019
/// Created     : 07/Feb/2019
/// 
/// Author      : Marcos Brito
/// 
/// ----------------------------------------------------------------------------------------------
/// </summary>


namespace EepromVisualAccess
{
public partial class MainForm : Form
{
public class ArchiveInterpreter
{
	public int entryCount;
	public MacModel model { get; set; }

	static Color ERROR_ENTRY_COLOR = Color.FromArgb(BACK_COLOR_ERROR[0], BACK_COLOR_ERROR[1], BACK_COLOR_ERROR[2]);
	static Color OP_ENTRY_COLOR = Color.FromArgb(BACK_COLOR_OP[0], BACK_COLOR_OP[1], BACK_COLOR_OP[2]);
	static Color INVALID_ENTRY_FORECOLOR = Color.FromArgb(FORE_COLOR_BAD_DATA[0], FORE_COLOR_BAD_DATA[1], FORE_COLOR_BAD_DATA[2]);

	public ArchiveInterpreter(MacModel model, ListView arcViewer, ListView detViewer, ListView mViewer)
	{
		this.model = model;
		entryCount = 0;
		InitializeViewers(arcViewer, detViewer, mViewer);
	}
	private void InitializeViewers(ListView archiveViewer, ListView detailViewer, ListView mapViewer)
	{
		switch(model)
		{
			case MacModel.A40TR:
					archiveViewer.Columns.Add("#", 43);
					archiveViewer.Columns.Add("Fecha/Hora", 145);
					archiveViewer.Columns.Add("Codigo", 55);
					archiveViewer.Columns.Add("Temp Ent/ Parametro", 120);
					archiveViewer.Columns.Add("Temp Sal.", 65);
					archiveViewer.Columns.Add("Temp Evap.", 70);
					archiveViewer.Columns.Add("Temp Amb.", 65);
					archiveViewer.Columns.Add("P. Alta", 60);
					archiveViewer.Columns.Add("P. Baja", 60);
					archiveViewer.Columns.Add("P. Dif", 65);

					detailViewer.Columns.Add("Estado", 62);
					detailViewer.Columns.Add("FS", 30);
					detailViewer.Columns.Add("Cmp", 39);
					detailViewer.Columns.Add("V1", 28);
					detailViewer.Columns.Add("V2", 28);
					detailViewer.Columns.Add("V3", 28);
					detailViewer.Columns.Add("Bomba", 50);
					break;
			case MacModel.A80TR:
					archiveViewer.Columns.Add("#", 43);
					archiveViewer.Columns.Add("Fecha/Hora", 145);
					archiveViewer.Columns.Add("Codigo", 55);
					archiveViewer.Columns.Add("Temp Ent/ Parametro", 120);
					archiveViewer.Columns.Add("Temp Sal.", 65);
					archiveViewer.Columns.Add("Temp Evap.", 70);
					archiveViewer.Columns.Add("Temp Amb.", 65);
					archiveViewer.Columns.Add("P. A Alta", 60);
					archiveViewer.Columns.Add("P. A Baja", 60);
					archiveViewer.Columns.Add("P. A Dif", 65);
					archiveViewer.Columns.Add("P. B Alta", 60);
					archiveViewer.Columns.Add("P. B Baja", 60);
					archiveViewer.Columns.Add("P. B Dif", 65);

					detailViewer.Columns.Add("Modo", 62);
					detailViewer.Columns.Add("Estado", 62);
					detailViewer.Columns.Add("FS", 30);
					detailViewer.Columns.Add("Cmp.A", 45);
					detailViewer.Columns.Add("Cmp.B", 45);
					detailViewer.Columns.Add("V.A1", 38);
					detailViewer.Columns.Add("V.A2", 38);
					detailViewer.Columns.Add("V.A3", 38);
					detailViewer.Columns.Add("V.B1", 38);
					detailViewer.Columns.Add("V.B2", 38);
					detailViewer.Columns.Add("V.B3", 38);
					detailViewer.Columns.Add("Bomba", 50);
					break;
		}
	}

				
	public bool ProcessEntry(byte[] entryData, EntryType type, ListViewItem entry)
	{
		entryCount++;
		entry.Text = entryCount.ToString();    // Save Entry number
		Int32 ValueInteger;
		float ValueFloat;
		bool retVal = true;
	 
		if (type == EntryType.OP_HISTORY)
		{
			entry.BackColor = OP_ENTRY_COLOR;

			// Get Timestamp
			ValueInteger = BitConverter.ToInt32(entryData, 0);
			if (ValueInteger < 0)
			{
					retVal = false;
					entry.ForeColor = INVALID_ENTRY_FORECOLOR;
					entry.SubItems.Add(ValueInteger.ToString());    // Save Timestamp
			}
			else
			{
					DateTime date = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(ValueInteger);
					entry.SubItems.Add(date.ToString());        // Save Timestamp
			}   
						  
			// Get Digital Cell
			ValueInteger = BitConverter.ToInt32(entryData, 4);
			ValueInteger &= ~(3 << 30); // Clear mask
			entry.SubItems.Add(ValueInteger.ToString("X4"));    // Save Digital Cell
	 
			// Get analog values
			for (int j = 0; j < (ArchiveInfo.opEntrySize - 2); j++)  // Save sensor values
			{
					ValueFloat = BitConverter.ToSingle(entryData, 8 + j * 4);
					entry.SubItems.Add(ValueFloat.ToString("F2"));
			}
		}
		else if (type == EntryType.ERROR_DATA)
		{
			entry.BackColor = ERROR_ENTRY_COLOR;

			// Get Timestamp
			ValueInteger = BitConverter.ToInt32(entryData, 0); 
			if (ValueInteger < 0)
			{
					retVal = false;
					entry.ForeColor = INVALID_ENTRY_FORECOLOR;
					entry.SubItems.Add(ValueInteger.ToString());    // Save Timestamp
			}
			else
			{
					DateTime date = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(ValueInteger);
					entry.SubItems.Add(date.ToString());        // Save Timestamp
			}
	 
			// Get error code
			ValueInteger = BitConverter.ToInt32(entryData, 4);
			ValueInteger &= ~(3 << 30); // Clear mask
			entry.SubItems.Add(ValueInteger.ToString("X2"));    // Save error Code
					 
			// Get error parameter
			ValueFloat = BitConverter.ToSingle(entryData, 8);
			entry.SubItems.Add(ValueFloat.ToString("F2"));      // Save Error Value
		}
		return retVal;
	}
	public String GetErrorString(String code, String param)
	{
		String errorString = "Codigo: " + code + Environment.NewLine;
		if (model == MacModel.A40TR)
		{
			#region GWF-40TR ErrorMessages
			switch (code)
			{
			case "00":          //ERR_SENSOR
			{
				try
				{
					float vinPin = float.Parse(param);
					errorString += "Valor de sensores no coherentes. Entrada VIN" + vinPin.ToString("f0");
				}
				catch (FormatException)
				{
					errorString += "Valor de sensores no coherentes. Entrada VIN" + param;
				}
				break;
			}
			case "01":      // ERR_TEMP_SENSOR
			{
				errorString += "Valor improbable en sensor de temperatura de entrada. Valor: " + param + " C";
				break;
			}
			case "02":      // ERR_TEMP_SENSOR
			{
				errorString += "Valor improbable en sensor de temperatura de salida. Valor: " + param + " C";
				break;
			}
			case "03":      // ERR_TEMP_SENSOR
			{
				errorString += "Valor improbable en sensor de temperatura del evaporador. Valor: " + param + " C";
				break;
			}
			case "04":      // ERR_TEMP_SENSOR
			{
				errorString += "Valor improbable en sensor de temperatura ambiente. Valor: " + param + " C";
				break;
			}
			case "05":      // ERR_EEPROM_READ
			{
				errorString += "Falla lectura EEPROM. Direccion: " + param;
				break;
			}
			case "06":      // ERR_EEPROM_WRITE
			{
				errorString += "Falla escritura EEPROM. Direccion: " + param;
				break;
			}
			case "07":      // ERR_OP_VALUES
			{
				errorString += "SetPoint recuperado de memoria no coherente. \n Se configuraron los valores por defecto.";
				break;
			}
			case "08":      // ERR_CREATE_TIMEOUT
			{
				errorString += "Error al crear Timeout " + param;
				break;
			}
			case "09":      // ERR_SYS_WATCHDOG
			{
				errorString += "System Watchdog. Se continuó la operacion normalmente.";
				break;
			}
			case "0A":      // ERR_FATAL_ERROR
			{
				errorString += "Excepcion no manejada. Imposible operar.";
				break;
			}
			case "0B":      // ERR_ARCHIVE_INIT
			{
				errorString += "Error iniciando historial. Se perdieron los datos pasados.";
				break;
			}
			case "0C":      // ERR_RTC_FAIL
			{
				errorString += "Fecha y hora invalida.";
				break;
			}
			case "0D":      // ERR_MAIL_SENDING
			{
				errorString += "Error al intentar envío de email. Verifique conexión a internet.";
				break;
			}
			case "0E":      // ERR_MAIL_SYSTEM
			{
				errorString += "Falla durante el envío de mail.";
				break;
			}
			case "0F":      // ERR_FLOW
			{
				errorString += "FlowSwitch Evaporador.";
				break;
			}
			case "10":      // ERR_P_HIGH
			{
				errorString += "Presion alta fuera de rango: " + param + " PSI";
				break;
			}
			case "11":      // ERR_P_LOW
			{
				errorString += "Presion baja fuera de rango: " + param + " PSI";
				break;
			}
			case "12":      // ERR_P_DIF
			{
				errorString += "Presion diferencial fuera de rango: " + param + " PSI";
				break;
			}
			case "13":      // ERR_T_CRIT
			{
				errorString += "Temp. evaporador subcritica: " + param + " C";
				break;
			}
			case "14":      // ERR_T_MAX
			{
				errorString += "Temperatura por encima de máxima: " + param + " C";
				break;
			}
			case "15":      // ERR_COOLDOWN
			{
				errorString += "Imposible disminuir P. alta en etapa de arranque." + Environment.NewLine + "Presion alta: " + param + " PSI";
				break;
			}
			case "16":      // ERR_COMP_OL
			{
				errorString += "Consumo compresor.";
				break;
			}
			case "17":      // ERR_CMP_WATCHDOG
			{
				errorString += "Compressor Watchdog.";
				break;
			}
			case "18":      // ERR_VENT1_OL
			{
				errorString += "Consumo ventilador 1.";
				break;
			}
			case "19":      // ERR_VENT2_OL
			{
				errorString += "Consumo ventilador 2.";
				break;
			}
			case "1A":      // ERR_VENT3_OL
			{
				errorString += "Consumo ventilador 3.";
				break;
			}
			case "1B":      // ERR_PUMP_OL
			{
				errorString += "Consumo bomba de agua.";
				break;
			}
			}
			#endregion
		}
		else if(model == MacModel.A80TR)
		{
			#region GWF-80TR ErrorMessages
			switch (code)
			{
				case "00":          //ERR_SENSOR
				{
					try
					{
						float vinPin = float.Parse(param);
						errorString += "Valor de sensores no coherentes. Entrada VIN" + vinPin.ToString("f0");
					}
					catch(FormatException)
					{
						errorString += "Valor de sensores no coherentes. Entrada VIN" + param;
					}
					break;
				}
				case "01":      // ERR_TEMP_IN_SENSOR
				{
					errorString += "Valor improbable en sensor de temperatura de entrada. Valor: " + param + " C";
					break;
				}
				case "02":      // ERR_TEMP_OUT_SENSOR
				{
					errorString += "Valor improbable en sensor de temperatura de salida. Valor: " + param + " C";
					break;
				}
				case "03":      // ERR_TEMP_EV_SENSOR
				{
					errorString += "Valor improbable en sensor de temperatura del evaporador. Valor: " + param + " C";
					break;
				}
				case "04":      // ERR_TEMP_AMB_SENSOR
				{
					errorString += "Valor improbable en sensor de temperatura ambiente. Valor: " + param + " C";
					break;
				}
				case "05":      // ERR_EEPROM_READ
				{
					errorString += "Falla lectura EEPROM. Direccion: " + param;
					break;
				}
				case "06":      // ERR_EEPROM_WRITE
				{
					errorString += "Falla escritura EEPROM. Direccion: " + param;
					break;
				}
				case "07":      // ERR_OP_VALUES
				{
					errorString += "SetPoint recuperado de memoria no coherente. \n Se configuraron los valores por defecto.";
					break;
				}
				case "08":      // ERR_CREATE_TIMEOUT
				{
					errorString += "Error al crear Timeout " + param;
					break;
				}
				case "09":      // ERR_CREATE_TIMER
				{
					errorString += "Error al crear Timer " + param;
					break;
				}
				case "0A":      // ERR_SYS_WATCHDOG
				{
					errorString += "System Watchdog. Se continuó la operacion normalmente.";
					break;
				}
				case "0B":      // ERR_REG_A_COUNT
				{
					errorString += "Error operando el registro A. Chequear conexiones del multiplexor.";
					break;
				}
				case "0C":      // ERR_FATAL_ERROR
				{
					errorString += "Excepcion no manejada. Imposible operar.";
					break;
				}
				case "0D":      // ERR_ARCHIVE_INIT
				{
					errorString += "Error iniciando historial. Se perdieron los datos pasados.";
					break;
				}
				case "0E":      // ERR_RTC_FAIL
				{
					errorString += "Fecha y hora invalida.";
					break;
				}
				case "0F":      // ERR_MAIL_SENDING
				{
					errorString += "Error al intentar envío de email. Verifique conexión a internet.";
					break;
				}
				case "10":      // ERR_MAIL_SYSTEM
				{
					errorString += "Falla durante el envío de mail.";
					break;
				}
				case "11":      // ERR_FLOW
				{
					errorString += "FlowSwitch Evaporador.";
					break;
				}
				case "12":      // ERR_P_HIGH_A
				{
					errorString += "Presion alta circuito A fuera de rango: " + param + " PSI";
					break;
				}
				case "13":      // ERR_P_HIGH_B
				{
					errorString += "Presion alta circuito B fuera de rango: " + param + " PSI";
					break;
				}
				case "14":      // ERR_P_LOW_A
				{
					errorString += "Presion baja circuito A fuera de rango: " + param + " PSI";
					break;
				}
				case "15":      // ERR_P_LOW_B
				{
					errorString += "Presion baja circuito B fuera de rango: " + param + " PSI";
					break;
				}
				case "16":      // ERR_P_DIF_A
				{
					errorString += "Presion diferencial circuito A fuera de rango: " + param + " PSI";
					break;
				}
				case "17":      // ERR_P_DIF_B
				{
					errorString += "Presion diferencial circuito B fuera de rango: " + param + " PSI";
					break;
				}
				case "18":      // ERR_T_CRIT
				{
					errorString += "Temp. evaporador subcritica: " + param + " C";
					break;
				}
				case "19":      // ERR_T_MAX
				{
					errorString += "Temperatura por encima de máxima: " + param + " C";
					break;
				}
				case "1A":      // ERR_COOLDOWN
				{
					errorString += "Imposible disminuir P. alta en etapa de arranque." + Environment.NewLine + "Presion alta: " + param + " PSI";
					break;
				}
				case "1B":      // ERR_COMP_OL_A
				{
					errorString += "Consumo compresor A.";
					break;
				}
				case "1C":      // ERR_COMP_OL_B
				{
					errorString += "Consumo compresor B.";
					break;
				}
				case "1D":      // ERR_CMP_WATCHDOG
				{
					errorString += "Compressor Watchdog.";
					break;
				}
				case "1E":      // ERR_FAN_A1_OL
				{
					errorString += "Consumo ventilador A1.";
					break;
				}
				case "1F":      // ERR_FAN_A2_OL
				{
					errorString += "Consumo ventilador A2.";
					break;
				}
				case "20":      // ERR_FAN_A3_OL
				{
					errorString += "Consumo ventilador A3.";
					break;
				}
				case "21":      // ERR_FAN_B1_OL
				{
					errorString += "Consumo ventilador B1.";
					break;
				}
				case "22":      // ERR_FAN_B2_OL
				{
					errorString += "Consumo ventilador B2.";
					break;
				}
				case "23":      // ERR_FAN_B3_OL
				{
					errorString += "Consumo ventilador B3.";
					break;
				}
				case "24":      // ERR_PUMP_OL
				{
					errorString += "Consumo bomba de agua.";
					break;
				}
			}
			#endregion
		}
		return errorString;
	}
	public EntryType GetEntryType(ListViewItem item)
	{

		EntryType retVal = EntryType.NO_DATA;
		if (item.BackColor == ERROR_ENTRY_COLOR && item.ForeColor != INVALID_ENTRY_FORECOLOR)  // If else, item.ForeColor == FORE_COLOR_BAD_DATA, return NO_DATA
			retVal = EntryType.ERROR_DATA;
		else if (item.BackColor == OP_ENTRY_COLOR)
			retVal = EntryType.OP_HISTORY;

		return retVal;
	}
				
	public void DecodeDigitalCell(ListViewItem item, Int32 code)
	{
		switch (model)
		{
			case MacModel.A40TR:
					A40TR_DecodeDigitalCell(item, code);
					break;
			case MacModel.A80TR:
					A80TR_DecodeDigitalCell(item, code);
					break;
		}

	}
	void A40TR_DecodeDigitalCell(ListViewItem item, Int32 digitalCell)
	{
		String text;
		OpStatus status = (OpStatus)((digitalCell >> 10) & 0x3);
		switch (status)
		{
			case OpStatus.IDLE:
					text = "LISTO";
					break;
			case OpStatus.RUNNING:
					text = "OPERANDO";
					break;
			case OpStatus.POSTOP:
					text = "POSTOP";
					break;
			case OpStatus.DISABLED_STATE:
					text = "DESHAB.";
					break;
			default:
					text = "...";
					break;
		}

		item.Text = text;
		item.SubItems.Add((digitalCell & 0x1).ToString());  // Flowswitch
		String tempStr = (digitalCell >> 5 & 0x1) == 0 ? (digitalCell >> 1 & 0x1).ToString() : "-1";   // -1 if Consumo Cmp = 1, else store Cmp ON/OFF value
		item.SubItems.Add(tempStr);  // Compressor 

		tempStr = (digitalCell >> 6 & 0x1) == 0 ? (digitalCell >> 2 & 0x1).ToString() : "-1";   // -1 if Consumo Fan 1 = 1, else store Fan 1 ON/OFF value
		item.SubItems.Add(tempStr);  // Fan 1 

		tempStr = (digitalCell >> 7 & 0x1) == 0 ? (digitalCell >> 3 & 0x1).ToString() : "-1";   // -1 if Consumo Fan 2 = 1, else store Fan 2 ON/OFF value
		item.SubItems.Add(tempStr);  // Fan 2 

		tempStr = (digitalCell >> 8 & 0x1) == 0 ? (digitalCell >> 4 & 0x1).ToString() : "-1";   // -1 if Consumo Fan 3 = 1, else store Fan 3 ON/OFF value
		item.SubItems.Add(tempStr);  // Fan 3 

		item.SubItems.Add( (digitalCell >> 9 & 0x1) == 0 ? "0" : "-1" );  // Consumo Bomba
	}
		  
	void A80TR_DecodeDigitalCell(ListViewItem item, Int32 digitalCell)
	{
		String text;
		Byte opMode = (Byte)((digitalCell >> 20) & 0x3);
		switch(opMode)
		{
			case 0x00:
					text = "80TR";
					break;
			case 0x01:
					text = "40TR-BAL";
					break;
			case 0x02:
					text = "40TR-A";
					break;
			case 0x03:
					text = "40TR-B";
					break;
			default:
					text = "...";
					break;
		}
		item.Text = text;   // Save machine Operation Mode

		Byte status = (Byte)((digitalCell >> 18) & 0x3);
		switch (status)
		{
			case 0x00:
					text = "LISTO";
					break;
			case 0x01:
					text = "COOLING";
					break;
			case 0x02:
					text = "OPERANDO";
					break;
			case 0x03:
					text = "POSTOP";
					break;
			default:
					text = "...";
					break;
		}
		item.SubItems.Add(text);    // Save machine status

		item.SubItems.Add((digitalCell & 0x1).ToString());  // Flowswitch

		String tempStr = (digitalCell >> 9 & 0x1) == 0 ? (digitalCell >> 1 & 0x1).ToString() : "-1";   // -1 if Consumo Cmp A = 1, else store Cmp A ON/OFF value
		item.SubItems.Add(tempStr);  // Compressor A

		tempStr = (digitalCell >> 10 & 0x1) == 0 ? (digitalCell >> 2 & 0x1).ToString() : "-1";   // -1 if Consumo Cmp B = 1, else store Cmp B ON/OFF value
		item.SubItems.Add(tempStr);  // Compressor B

		tempStr = (digitalCell >> 11 & 0x1) == 0 ? (digitalCell >> 3 & 0x1).ToString() : "-1";   // -1 if Consumo Vent A1 = 1, else store Vent A1 ON/OFF value
		item.SubItems.Add(tempStr);  // Vent A1

		tempStr = (digitalCell >> 12 & 0x1) == 0 ? (digitalCell >> 4 & 0x1).ToString() : "-1";   // -1 if Consumo Vent A2 = 1, else store Vent A2 ON/OFF value
		item.SubItems.Add(tempStr);  // Vent A2

		tempStr = (digitalCell >> 13 & 0x1) == 0 ? (digitalCell >> 5 & 0x1).ToString() : "-1";   // -1 if Consumo Vent A3 = 1, else store Vent A3 ON/OFF value
		item.SubItems.Add(tempStr);  // Vent A3

		tempStr = (digitalCell >> 14 & 0x1) == 0 ? (digitalCell >> 6 & 0x1).ToString() : "-1";   // -1 if Consumo Vent B1 = 1, else store Vent B1 ON/OFF value
		item.SubItems.Add(tempStr);  // Vent B1

		tempStr = (digitalCell >> 15 & 0x1) == 0 ? (digitalCell >> 7 & 0x1).ToString() : "-1";   // -1 if Consumo Vent B2 = 1, else store Vent B2 ON/OFF value
		item.SubItems.Add(tempStr);  // Vent B2

		tempStr = (digitalCell >> 16 & 0x1) == 0 ? (digitalCell >> 8 & 0x1).ToString() : "-1";   // -1 if Consumo Vent B1 = 1, else store Vent B1 ON/OFF value
		item.SubItems.Add(tempStr);  // Vent B3

		item.SubItems.Add( (digitalCell >> 17 & 0x1)==0 ? "0" : "-1" );    // Consumo Bomba agua
	}
}

public Stx8xxx PioBoard;
public ArchiveInterpreter arch1;
OpenFileDialog openFileDialog;

public MainForm()
{
	InitializeComponent();

	// Inicializar objeto PioBoard con dirección IP del PLC.
	// Recuerde especificar contraseña y tipo de dispositivo.
	PioBoard = new Stx8xxx("192.168.1.81", 0, Stx8xxxId.STX8091);
	String[] arguments = Environment.GetCommandLineArgs();
	if(arguments.GetLength(0) > 1)
	{
		AskModelPopup askForModel = new AskModelPopup();
		if(askForModel.ShowDialog() == DialogResult.OK)
		{
			modelSelected = true;
			MacModel myModel = (MacModel) askForModel.selectedModel;
			modelSelector.Text = modelSelector.Items[askForModel.selectedModel].ToString();
			LoadMachineModelParameters(myModel);
			OpenFileFromPath(arguments[1]);	
		}
		askForModel.Dispose();
		
	}
//	MessageBox.Show("argc= " + arguments.GetLength(0) + Environment.NewLine + "GetCommandLineArgs: " + String.Join(" - ", arguments));
}

#region LOAD ARCHIVE DATA
private void butReadEeprom_Click(object sender, EventArgs e)
{
	if (modelSelected == false)
	{
		MessageBox.Show("Por favor, seleccione un modelo de máquina.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		return;
	}
	// Request entire archive from memory

	// Crear un array donde se almacenarán los bytes recibidos de la memoria EEPROM.
	// Tamaño inicial igual al solicitado desde ventana.
	byte[] EepromBytes = new byte[ ArchiveInfo.archiveSize ];

	// Dirección inicial donde comenzar a leer los bytes de EEPROM.
	int EepromStartAddress = ArchiveInfo.metadataAddress;

	// Número de bytes leidos.
	int BytesRead = 0;

	// Número de bytes a leer en una sola petición al PLC (máximo 100).
	int BytesToRetrieve = 0;

	// Número de bytes que deben leerse de memoria EEPROM.
	int BytesToRead = ArchiveInfo.archiveSize;

	// Comprobar cantidad de bytes a leer.
	if (BytesToRead > 100)
		BytesToRetrieve = 100;  // Mayor a 100, limitar peticion incial.
	else
		BytesToRetrieve = BytesToRead;  // Menor o igual a 100, pedir todos los bytes inicialmente.

	//
	// Leer todos los bytes de EEPROM en pedidos no mayores de 100 bytes.
	//
	while (BytesRead < BytesToRead)
	{
		byte[] EepromTempBytes;

		// Enviar petición para leer memoria en PLC.
		if (SendReadEeprom((UInt16)(EepromStartAddress + BytesRead), (byte)BytesToRetrieve) == true)
		{
			// Error en transmision, retornar.
			return;
		}

		// Esperar respuesta del PLC y guardar bytes recibidos en EepromTempBytes[].
		if (WaitPlcResponse(out EepromTempBytes) == true)
		{
			// Error en recepción, retornar.
			return;
		}

		//
		// Copiar bytes recibidos en última petición al array EepromBytes[].
		//
		try
		{
			EepromTempBytes.CopyTo(EepromBytes, BytesRead);
		}
		catch (System.ArgumentException)
		{
			MessageBox.Show("Error en datos recibidos. Intente reiniciar la aplicación.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		//
		// Actualizar cantidad de bytes leidos.
		//
		BytesRead += BytesToRetrieve;

		// Comprobar cantidad de bytes a leer.
		if ((BytesToRead - BytesRead) > 100)
		{
			// Mayor a 100, limitar peticion.
			BytesToRetrieve = 100;
		}
		else
		{
			// Menor o igual a 100, pedir bytes restantes.
			BytesToRetrieve = BytesToRead - BytesRead;
		}
	}

	ProcessData(EepromBytes);
}
private void butLoadFile_Click(object sender, EventArgs e)
{
	if (modelSelected == false)
	{
		MessageBox.Show("Por favor, seleccione un modelo de máquina.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		return;
	}
	var fileContent = string.Empty;
	var filePath = string.Empty;

	using (openFileDialog = new OpenFileDialog())
	{
		openFileDialog.InitialDirectory = workingPath;
		openFileDialog.Filter = "binarios (*.bin)|*.bin|All files (*.*)|*.*";
		openFileDialog.FilterIndex = 1;
		//openFileDialog.RestoreDirectory = true;

		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			OpenFileFromPath(openFileDialog.FileName);
			//Stream fileStream = openFileDialog.OpenFile();
		}
	}
}

void OpenFileFromPath(String path)
{
	//Get the path of specified file
	statStripDataPath.Text = path;
	try{
		workingFileName = Path.GetFileNameWithoutExtension(path);
		workingPath = Path.GetDirectoryName(path); // Save the path for future operations
	}
	catch{
		workingFileName = "example";
		workingPath = path; // Save the path for future operations
	}
	
	//Read the contents of the file into a stream
	Stream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

	if (fileStream.Length == ArchiveInfo.archiveSize)
	{
		byte[] fileData = new byte[ArchiveInfo.archiveSize];
		fileStream.Read(fileData, 0, ArchiveInfo.archiveSize);
		ProcessData(fileData);
	}
	else
		MessageBox.Show("Error en el tamaño de archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
	fileStream.Close();
}
#endregion

#region RAW DATA PROCESSING
private void ProcessData(byte[] data)
{
	arch1.entryCount = 0;
	byte[] metadataBytes = new byte[ArchiveInfo.metadataSize];
	Array.Copy(data, 0, metadataBytes, 0, ArchiveInfo.metadataSize);
	GetMetadataFromEeprom(metadataBytes);

	byte[] mapBytes = new byte[ArchiveInfo.mapSize];
	Array.Copy(data, ArchiveInfo.mapAddress - ArchiveInfo.metadataAddress, mapBytes, 0, ArchiveInfo.mapSize);
	GetMemoryMapFromEeprom(mapBytes);

	byte[] archiveBytes = new byte[ArchiveInfo.regFileSize];
	Array.Copy(data,  ArchiveInfo.regFileAddress - ArchiveInfo.metadataAddress, archiveBytes, 0, ArchiveInfo.regFileSize);
	GetEntries(archiveBytes);
}
private void GetMetadataFromEeprom(byte[] metadataBytes)
{
	UInt32 tail = BitConverter.ToUInt32(metadataBytes, 0);
	UInt32 head = tail >> 16;
	tail = tail & 0x0000FFFF;
	UInt32 count = BitConverter.ToUInt32(metadataBytes, 4);

	txtTail.Text = tail.ToString();
	txtHead.Text = head.ToString();
	txtCount.Text = count.ToString();
}
private void GetMemoryMapFromEeprom(byte[] mapBytes)
{
	MapViewer.Items.Clear();
	// Fill Map Viewer
	ListViewItem row = new ListViewItem("0");
	for (int i = 0; i < mapBytes.Length; i++)
	{
		row.SubItems.Add(mapBytes[i].ToString());
		if ((i + 1) % 4 == 0 && i > 0)
		{
			MapViewer.Items.Add(row);
			row = new ListViewItem((i + 1).ToString());
		}
	}
	// Add last row 
	MapViewer.Items.Add(row);
}
private void GetEntries(byte[] EepromBytes)
{
	ClearFilter();
	ArchiveViewer.Items.Clear();

	if (DEBUGGING)
		Debug_AddColumns();

	if ((EepromBytes.Length % 4) != 0)
	{
		MessageBox.Show("La cantidad de bytes recibidos deben ser múltiplo de 4. Modifique valor.", "Error en bytes recibidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
		return;
	}

	bool done = false, dataError = false;
	int dataSize = 0;
	ListViewItem entry;
	EntryType type = EntryType.NO_DATA;
	int i = Convert.ToInt32(txtTail.Text);
	byte[] entryArray = new byte[ArchiveInfo.MaxEntrySize() * 4];   // Temp array to save one complete entry
	while (done == false)
	{
		#region GET ENTRY FROM BUFFER
		// First identify entry type
		byte id = (byte)(EepromBytes[(i * 4 + 7) % EepromBytes.Length] & (0xC0));
		if (id == OP_HISTORY_MASK)
		{
			dataSize = ArchiveInfo.opEntrySize;
			type = EntryType.OP_HISTORY;
		}
		else if (id == ERROR_MASK)
		{
			dataSize = ArchiveInfo.errorEntrySize;
			type = EntryType.ERROR_DATA;
		}
		else
		{
			MessageBox.Show(String.Format("Error reconociendo tipo de entrada. (Indice: {0}).", i), "Error en datos de registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			done = true;
			break;
		}

		// Copy entry to a temporary array to simplify access
		if ((i + dataSize) <= EepromBytes.Length / 4)  
			// If entire entry available:
			Array.Copy(EepromBytes, i * 4, entryArray, 0, dataSize * 4);    
		else
		{
			// Else, must wrap around buffer:
			int dataSize2 = (i + dataSize) % (EepromBytes.Length / 4);
			int dataSize1 = dataSize - dataSize2;
			Array.Copy(EepromBytes, i * 4, entryArray, 0, dataSize1 * 4);
			Array.Copy(EepromBytes, 0, entryArray, dataSize1 * 4, dataSize2 * 4);
		}
		#endregion GET ENTRY FROM BUFFER

		entry = new ListViewItem("");
		if (arch1.ProcessEntry(entryArray, type, entry) == false)
			dataError = true;

		if (DEBUGGING)
		{
			entry.SubItems.Add(i.ToString());                   // Save Index
			entry.SubItems.Add((ArchiveInfo.regFileAddress + i * 4).ToString());    // Save Address
		}

		// Push entry to list viewer
		ArchiveViewer.Items.Add(entry);

		i = (i + dataSize) % (EepromBytes.Length / 4);  // Advance index
		if (arch1.entryCount.ToString() == txtCount.Text)    // Check if done
			done = true;
	}


	UpdateDateFilterInput();
	if (dataError == true)
		MessageBox.Show("Algunas entradas presentan datos invalidos.", "Error en datos de registro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
	if (arch1.entryCount.ToString() != txtCount.Text)
	{
		MessageBox.Show("Algunas entradas no se pudieron mostrar", "Error en datos de registro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
		entry = new ListViewItem("...");
		ArchiveViewer.Items.Add(entry);
	}

}
#endregion
		  
private void Debug_AddColumns()
{
	this.columnHeaderDebug1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
	this.columnHeaderDebug1.Text = "Index";
	this.columnHeaderDebug1.Width = 42;
	ArchiveViewer.Columns.Insert(ArchiveViewer.Columns.Count - 1, columnHeaderDebug1);
	this.columnHeaderDebug2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
	this.columnHeaderDebug2.Text = "Address";
	this.columnHeaderDebug2.Width = 50;
	ArchiveViewer.Columns.Insert(ArchiveViewer.Columns.Count - 1, columnHeaderDebug2);
}

#region FILTER METHODS
private void ApplyTypeFilter()
{
	bool deleteItem = false;

	foreach (ListViewItem i in ArchiveViewer.Items)
	{
		deleteItem = false;
		EntryType type = arch1.GetEntryType(i);
		if (type == EntryType.NO_DATA && showInvalidEntries == false)
			deleteItem = true;
		else if (type == EntryType.ERROR_DATA && showErrorEntries == false)
			deleteItem = true;
		else if (type == EntryType.OP_HISTORY && showOpEntries == false)
			deleteItem = true;

		if (deleteItem == true)
		{
			DeleteEntry(i);
		}
	}
}
private void ApplyDateFilter()
{
	bool tryParseResult = false;
	DateTime entryDate = new DateTime();
	DateTime startDate = new DateTime();
	DateTime endDate = new DateTime();
	startDate = dateFilterStart.Value.Date;
	endDate = (dateFilterEnd.Value.AddDays(1)).Date;    // Compare with (endDate+1) @ 00:00:00 AM
	if (startDate.CompareTo(endDate) == 1)   // If startdate is after endDate, alert and return
	{
		MessageBox.Show("Rango de fechas invalido. No hay intersección.", "Error al aplicar el filtro", MessageBoxButtons.OK, MessageBoxIcon.Error);
		return;
	}

	foreach (ListViewItem i in ArchiveViewer.Items)
	{
		tryParseResult = DateTime.TryParse(i.SubItems[DATE].Text, out entryDate);
		if (tryParseResult == true && !(entryDate.CompareTo(startDate.Date) > -1 && entryDate.CompareTo(endDate.Date) == -1))
		{
			DeleteEntry(i);
		}
	}

}
private void ClearFilter()
{
	foreach (ListViewItem j in deletedEntries.Items)
	{
		ArchiveViewer.Items.Insert(Convert.ToInt16(j.Text)-1, (ListViewItem)j.Clone());
        j.Remove();
	}
}
private void DeleteEntry(ListViewItem i)
{
	if(deletedEntries.Items.Count == 0)
	{
		deletedEntries.Items.Insert(0, (ListViewItem)i.Clone());
		i.Remove();
	}
	else
	{
		for( int j = deletedEntries.Items.Count ; j >= 0 ; j--)
		{
			// Go through deleted entries (backwards) to find where to put new deleted entry
			if( j == 0 || Convert.ToInt16(deletedEntries.Items[j-1].Text) < Convert.ToInt16(i.Text))
			{
				deletedEntries.Items.Insert(j, (ListViewItem)i.Clone());  // Insert at the end of list
				i.Remove();
				break;
			}
		}
		
	}
	
}
private void SetDefaultFilterInput()
{
	chkboxShowErrors.Checked = true;
	chkboxShowOp.Checked = true;
	chkboxShowInvalid.Checked = true;
	UpdateDateFilterInput();
}
private void UpdateDateFilterInput()
{
	DateTime tempDate = new DateTime();
	if (ArchiveViewer.Items.Count == 0)
		return;
	try
	{
		tempDate = DateTime.Parse(ArchiveViewer.Items[0].SubItems[DATE].Text);
		dateFilterStart.Value = tempDate;
	}
	catch (System.FormatException)
	{
		dateFilterStart.Value = DateTime.Now;
	}

	try
	{
		tempDate = DateTime.Parse(ArchiveViewer.Items[ArchiveViewer.Items.Count - 1].SubItems[DATE].Text);
		if (tempDate.CompareTo(dateFilterStart.Value) == -1) // If endDate is previous than startDate
			dateFilterEnd.Value = DateTime.Now;
		else
			dateFilterEnd.Value = tempDate;
	}
	catch (System.FormatException)
	{
		dateFilterEnd.Value = DateTime.Now;
	}
}
#endregion

#region FORM INPUT ACTIONS
private void ArchiveViewer_SelectedIndexChanged(object sender, EventArgs e)
{
	ListView.SelectedListViewItemCollection selected = this.ArchiveViewer.SelectedItems;
	if (selected.Count > 0)
	{
		ListViewItem it = selected[0];
		if (arch1.GetEntryType(it) == EntryType.ERROR_DATA)   // If entry type ERROR
		{
			ErrorViewer.BringToFront();
			String errorCode = it.SubItems[CODE].Text;
			String param = it.SubItems[TEMPIN].Text;

			ErrorViewer.Text = arch1.GetErrorString(errorCode, param);
			ErrorViewer.Visible = true;
		}
		else if (arch1.GetEntryType(it) == EntryType.OP_HISTORY)
		{
			DetailViewer.Items.Clear();
			DetailViewer.BringToFront();
			// Get digital cell and decode it into a DetailViewer item.
			Int32 digital = Convert.ToInt32(it.SubItems[CODE].Text, 16);
			//        if (((digital >> 24) & 0xC0) == ERROR_MASK)
			//           return;
			ListViewItem digitalItem = new ListViewItem("");
			arch1.DecodeDigitalCell(digitalItem, digital);

			DetailViewer.Items.Add(digitalItem);
			DetailViewer.Visible = true;
		}
	}
	else
	{
		DetailViewer.Visible = false;
		ErrorViewer.Visible = false;
	}
}
private void modelSelector_SelectedIndexChanged(object sender, EventArgs e)
{
	MacModel model;
	if (modelSelected == false)
		modelSelected = true;
	switch (modelSelector.SelectedItem.ToString())
	{
		case "GWF-A-20/30/40TR":
			model = MacModel.A40TR;
			break;
		case "GWF-A-80TR":
			model = MacModel.A80TR;
			break;
		case "GWF-W-90TR":
			model = MacModel.W90TR;
			break;
		default:
			return;
	}
	LoadMachineModelParameters(model);
}
private void LoadMachineModelParameters(MacModel model)
{
	ArchiveInfo.archiveSize = ENTIRE_DATA_SIZE[(int)model];
	ArchiveInfo.metadataAddress = METADATA_ADDRESS[(int)model];
	ArchiveInfo.metadataSize = METADATA_SIZE[(int)model];
	ArchiveInfo.mapAddress = MAP_ADDRESS[(int)model];
	ArchiveInfo.mapSize = MAP_SIZE[(int)model];
	ArchiveInfo.regFileAddress = ARCHIVE_ADDRESS[(int)model];
	ArchiveInfo.regFileSize = ARCHIVE_SIZE[(int)model];
	ArchiveInfo.opEntrySize = OP_ENTRY_SIZE[(int)model];
	ArchiveInfo.errorEntrySize = ERROR_ENTRY_SIZE[(int)model];
	txtboxPlcVersion.Text = PLC_MATCHING_VERSION[(int)model];
	ArchiveViewer.Clear();
	DetailViewer.Clear();
	arch1 = new ArchiveInterpreter(model, ArchiveViewer, DetailViewer, MapViewer);
				
	ArchiveViewer_SelectedIndexChanged(null, null);  // Update detailed view
}
private void pictureBox1_Click(object sender, EventArgs e)
{
	HelpVisor helpVisor = new HelpVisor();
	helpVisor.ShowDialog();
}
private void butExport2CSV_Click(object sender, EventArgs e)
{
	SaveFileDialog sfd = new SaveFileDialog
	{
		Title = "Seleccione nombre de archivo a guardar",
		FileName = workingFileName,
		Filter = "CSV (*.csv)|*.csv",
		FilterIndex = 0,
		InitialDirectory = (csvPath == null ? workingPath : csvPath)
	};

	//show the dialog + display the results in a msgbox unless cancelled 
	if (sfd.ShowDialog() == DialogResult.OK)
	{ 
		try{
			csvPath = Path.GetDirectoryName(sfd.FileName); // Save the path for future operations
		}
		catch{
			csvPath = null;
		}
		string fileStr = "Tipo;";
		foreach (System.Windows.Forms.ColumnHeader i in ArchiveViewer.Columns)
		{
			fileStr += i.Text;
			if (i.Index < ArchiveViewer.Columns.Count - 1) // Add separator except for last column
					fileStr += ";";
		}
						  
		fileStr += Environment.NewLine;

		EntryType type = EntryType.NO_DATA;
		foreach(ListViewItem i in ArchiveViewer.Items)
		{
			type = arch1.GetEntryType(i);
			fileStr += type.ToString() + ";";
			for (int j = 0; j < i.SubItems.Count; j++)
			{
			/*		if (j == NUMBER)
					{
						fileStr += i.Index; // Save filtered index, not absolute index in the archive
					}*/
					if (j == CODE && type == EntryType.OP_HISTORY)  // Op entries save code as binary
						fileStr += Convert.ToString(Convert.ToInt32(i.SubItems[j].Text, 16), 2);    
					else
						fileStr += i.SubItems[j].Text;

					if (j < i.SubItems.Count - 1) // Add separator except for last column
						fileStr += ";";
			}
			if (i.Index < ArchiveViewer.Items.Count - 1)    // Unless its last item, add newline
					fileStr += Environment.NewLine;
		}

		DialogResult result = DialogResult.Retry;
		while(result == DialogResult.Retry)
		{
			try
			{
					System.IO.File.WriteAllText(sfd.FileName, fileStr);
					MessageBox.Show("Se guardaron los datos exitosamente.", "Operacion exitosa", MessageBoxButtons.OK, MessageBoxIcon.None);
					break;
			}
			catch (System.IO.IOException)
			{
					result = MessageBox.Show("Verificar que el archivo no se encuentre ya abierto.", "Error al guardar archivo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
			}
		}
	}
}
private void butExport2numCSV_Click(object sender, EventArgs e)
{
	SaveFileDialog sfd = new SaveFileDialog
	{
		Title = "Seleccione nombre de archivo a guardar",
		FileName = workingFileName,
		Filter = "CSV (*.csv)|*.csv",
		FilterIndex = 0,
		InitialDirectory = (numCsvPath == null ? workingPath : numCsvPath)
	};

	//show the dialog + display the results in a msgbox unless cancelled 
	if (sfd.ShowDialog() == DialogResult.OK)
	{ 
		try{
			numCsvPath = Path.GetDirectoryName(sfd.FileName); // Save the path for future operations
		}
		catch{
			numCsvPath = null;
		}
		string fileStr = "Tipo;";
		foreach (System.Windows.Forms.ColumnHeader i in ArchiveViewer.Columns)
		{
			fileStr += i.Text;
			if (i.Index < ArchiveViewer.Columns.Count - 1) // Add separator except for last column
					fileStr += ";";
		}
						  
		fileStr += Environment.NewLine;

		EntryType type = EntryType.NO_DATA;
		foreach(ListViewItem i in ArchiveViewer.Items)
		{
			type = arch1.GetEntryType(i);
			fileStr += (int) type + ";";
			for (int j = 0; j < i.SubItems.Count; j++)
			{
				if (j == NUMBER)
				{
					fileStr += i.Text; // Save filtered index, not absolute index in the archive
				}
				else if (j == CODE)  // Op entries save code as binary
					fileStr += Convert.ToString(Convert.ToInt32(i.SubItems[j].Text, 16));    
				else if(j == DATE)
				{
					DateTime t = Convert.ToDateTime(i.SubItems[DATE].Text);
					fileStr += t.ToString("yy;MM;dd;HH;mm;ss");
				}
				else
					fileStr += i.SubItems[j].Text.Replace(",", ".");

				if (j < i.SubItems.Count - 1) // Add separator except for last column
					fileStr += ";";
			}
			if (i.Index < ArchiveViewer.Items.Count - 1)    // Unless its last item, add newline
					fileStr += Environment.NewLine;
		}

		DialogResult result = DialogResult.Retry;
		while(result == DialogResult.Retry)
		{
			try
			{
					System.IO.File.WriteAllText(sfd.FileName, fileStr);
					MessageBox.Show("Se guardaron los datos exitosamente.", "Operacion exitosa", MessageBoxButtons.OK, MessageBoxIcon.None);
					break;
			}
			catch (System.IO.IOException)
			{
					result = MessageBox.Show("Verificar que el archivo no se encuentre ya abierto.", "Error al guardar archivo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
			}
		}
	}
}
private void butSetFilter_Click(object sender, EventArgs e)
{
	ClearFilter();    // First clear the filter in order to apply new filter 

	//Apply filter from fastest to slowest algorithm
	ApplyTypeFilter();
	ApplyDateFilter();
}
private void butClearFilter_Click(object sender, EventArgs e)
{
	ClearFilter();
}
private void chkboxShowInvalid_CheckedChanged(object sender, EventArgs e)
{
	showInvalidEntries = chkboxShowInvalid.Checked;
}
private void chkboxShowOp_CheckedChanged(object sender, EventArgs e)
{
	showOpEntries = chkboxShowOp.Checked;
}
private void chkboxShowErrors_CheckedChanged(object sender, EventArgs e)
{
	showErrorEntries = chkboxShowErrors.Checked;
}
#endregion


#region PLC COMMS
/// <summary>
/// Envia una petición por UDP al PLC para leer la memoria EEPROM.
/// </summary>
/// <param name="StartAddress">Dirección inicial a leer en EEPROM.</param>
/// <param name="Quantity">Cantidad de bytes a leer (máximo 100)</param>
/// <returns>Retorna "true" en caso de error.</returns>
private bool SendReadEeprom(UInt16 StartAddress, byte Quantity)
{
	// Inicializar retorno.
	SendStat CmdStat = SendStat.Success;
	UdpRxCmdStat OnUdpRxStat = UdpRxCmdStat.OK;
	bool ErrorsFound = false;

	// Limitar valor de Quantity a 100 si es mayor.
	if (Quantity > 100) Quantity = 100;

	// Crear array de bytes a enviar (seis bytes).
	byte[] DataBytes = new byte[6];

	// Especificar el primer byte identificador de paquete (RX_TYPE_READ_EEPROM), ver definiciones 
	// en globals.inc en proyecto StxLadder del PLC.
	DataBytes[0] = (byte)0xB0;

	// Especificar dirección de memoria a leer (al ser un valor mayor de 8-bits, lo dividimos en dos bytes).
	DataBytes[1] = (byte)(StartAddress >> 8);  // Valor más significativo.
	DataBytes[2] = (byte)(StartAddress); // Valor menos significativo.

	// Especificar cantidad de bytes a leer.
	DataBytes[3] = (byte)(Quantity);

	// Enviar bytes UDP al PLC con el metodo "Send".
	try
	{
		CmdStat = PioBoard.Cmd.Udp.Send(DataBytes, 6, out OnUdpRxStat);
	}
	catch (System.IndexOutOfRangeException)
	{
		MessageBox.Show("Fallo peticion de datos al PLC. Intente reiniciar la aplicación.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
		ErrorsFound = true;
	}

	// Comprobar respuesta del comando.
	if (CmdStat != SendStat.Success)
	{
		MessageBox.Show(String.Format("Error de comunicación con PLC (Código: {0}).", CmdStat), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
		ErrorsFound = true;
	}

	// Comprobar respuesta de operación "UdpRx".
	if (OnUdpRxStat != UdpRxCmdStat.OK)
	{
		MessageBox.Show(String.Format("Error al ejecutar operación en PLC. Evento UdpRx (Código: {0}).", OnUdpRxStat), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
		ErrorsFound = true;
	}

	// Retorno de método.
	return ErrorsFound;
}

/// <summary>
/// Espera respuesta del PLC. Si se recibieron datos desde EEPROM, se guardan en archivo.
/// </summary>
/// <param name="EepromBytes">Bytes recibidos de memoria EEPROM.</param>
/// <returns>Retorna "true" en caso de error.</returns>
private bool WaitPlcResponse(out byte[] EepromBytes)
{
	bool ErrorsFound = false;
	byte[] Packet;
	EepromBytes = null;

	// Esperar 5 segundos por un paquete UDP.
	// Comprobar que fue recibido.
	if (PioBoard.Cmd.Udp.Receive(4980, 5, out Packet) == UdpReceiveStat.Success)
	{
		// PAQUETE RECIBIDO!!!

		// Comprobar si tipo de paquete es TX_TYPE_EEPROM_DATA (0xAA).
		// Ver formato de paquete en globals.inc en proyecto de PLC en StxLadder.

		if (Packet[0] == 0xAA)
		{
			// Comprobar estado (STAT) de lectura en EEPROM.
			if (Packet[1] != 0)
			{
					MessageBox.Show(String.Format("Código de error: {0}", Packet[1]), "Error en memoria EEPROM del PLC", MessageBoxButtons.OK, MessageBoxIcon.Error);

					// Salir.
					ErrorsFound = true;
					return ErrorsFound;
			}

			// Obtener cantidad de bytes recibidos (SIZE).
			int Size = Packet[2];

			// Comprobar que tamaño de bytes recibidos concuerde con valor SIZE enviado por PLC.
			if ((Packet.Length - 3) != Size)
			{
					MessageBox.Show("Error, transmisión incompleta ...", String.Format("Bytes recibidos {0} vs esperados {1}", Size, Packet.Length - 3), MessageBoxButtons.OK, MessageBoxIcon.Error);

					// Salir.
					ErrorsFound = true;
					return ErrorsFound;
			}

			//
			// Guardar bytes leidos de EEPROM en array de retorno.
			//

			EepromBytes = new byte[Size];

			for (int i = 0; i < Size; i++)
			{
					// Copiar byte recibido en nuevo array.
					EepromBytes[i] = Packet[i + 3];
			}
		}
	}
	else
	{
		// NO SE RECIBIO NINGUN PAQUETE.
		MessageBox.Show("Error...", "No se recibieron datos", MessageBoxButtons.OK, MessageBoxIcon.Error);

		ErrorsFound = true;
	}

	// Retorno de método.
	return ErrorsFound;
}
		#endregion


	}
/*
class ListViewItemComparer : IComparer
{
		public int CompareDate_LQT(object x, object y)
		{
			Int32 entryNumberA = Convert.ToInt32(((ListViewItem)x).SubItems[0].Text);
			Int32 entryNumberB = Convert.ToInt32(((ListViewItem)y).SubItems[0].Text);
			if (entryNumberA <= entryNumberB)
				return -1;
			else
				return 1;
		}
		public int CompareIndex_LQT(object x, object y)
		{
			Int32 entryNumberA = Convert.ToInt32(((ListViewItem)x).SubItems[0].Text);
			Int32 entryNumberB = Convert.ToInt32(((ListViewItem)y).SubItems[0].Text);
			if (entryNumberA <= entryNumberB)
				return -1;
			else
				return 1;
		}
	}*/
}
