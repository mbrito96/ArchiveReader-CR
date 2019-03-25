using stx8xxx;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

// Incluir en Namespace la Librería STX8XXX de Slicetex.
/// <summary>
/// ----------------------------------------------------------------------------------------------
/// File        : MainForm.cs
/// Title       : Ejemplo para acceder a datos de memoria EEPROM en PLC desde Visual C#
/// 
/// Revised     : 07/Feb/2018
/// Created     : 07/Feb/2018
/// Version     : N/A
/// 
/// Author      : Boris Estudiez <devel@slicetex.com>
/// Website     : www.slicetex.com
/// 
/// License     : (C) Slicetex Electronics 2018. 
///             : Marcos Brito
/// 
/// ----------------------------------------------------------------------------------------------
/// </summary>


namespace EepromVisualAccess
{
    public partial class MainForm : Form
    {
        public Stx8xxx PioBoard;
        OpenFileDialog openFileDialog;

        public MainForm()
        {
            InitializeComponent();

            // Inicializar objeto PioBoard con dirección IP del PLC.
            // Recuerde especificar contraseña y tipo de dispositivo.
            PioBoard = new Stx8xxx("192.168.1.81", 0, Stx8xxxId.STX8091);
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
            byte[] EepromBytes = new byte[(int)(DEFAULT_DATA_SIZE)];

            // Dirección inicial donde comenzar a leer los bytes de EEPROM.
            int EepromStartAddress = (int)METADATA_ADDRESS;

            // Número de bytes leidos.
            int BytesRead = 0;

            // Número de bytes a leer en una sola petición al PLC (máximo 100).
            int BytesToRetrieve = 0;

            // Número de bytes que deben leerse de memoria EEPROM.
            int BytesToRead = (int)(DEFAULT_DATA_SIZE);

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
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    workingPath = openFileDialog.FileName; // Save the path for future operations

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    if (fileStream.Length == DEFAULT_DATA_SIZE)
                    {
                        byte[] fileData = new byte[DEFAULT_DATA_SIZE];
                        fileStream.Read(fileData, 0, DEFAULT_DATA_SIZE);
                        ProcessData(fileData);
                    }
                    else
                        MessageBox.Show("Error en el tamaño de archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fileStream.Close();
                }
            }
        }
    #endregion

    #region RAW DATA PROCESSING
        private void ProcessData(byte[] data)
        {
            byte[] metadataBytes = new byte[METADATA_SIZE];
            Array.Copy(data, 0, metadataBytes, 0, METADATA_SIZE);
            GetMetadataFromEeprom(metadataBytes);

            byte[] mapBytes = new byte[MAP_SIZE];
            Array.Copy(data, MAP_ADDRESS - METADATA_ADDRESS, mapBytes, 0, MAP_SIZE);
            GetMemoryMapFromEeprom(mapBytes);


            byte[] archiveBytes = new byte[(int)numArchiveSize.Value];
            Array.Copy(data, ARCHIVE_ADDRESS - METADATA_ADDRESS, archiveBytes, 0, (int)numArchiveSize.Value);
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
            UInt16 entryNumber = 0;

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
            byte[] entryArray = new byte[9 * 4];   // Temp array to save one complete entry
            while (done == false)
            {
                // First grab ID
                if ((EepromBytes[(i * 4 + 7) % EepromBytes.Length] & (0xC0)) == OP_HISTORY_MASK)
                {
                    dataSize = 9;
                    type = EntryType.OP_HISTORY;
                }
                else if ((EepromBytes[(i * 4 + 7) % EepromBytes.Length] & (0xC0)) == ERROR_MASK)
                {
                    dataSize = 3;
                    type = EntryType.ERROR_DATA;
                }
                else
                {
                    MessageBox.Show(String.Format("Error reconociendo tipo de entrada. (Indice: {0}).", i), "Error en datos de registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    done = true;
                    break;
                }

                // Copy entry to a temporal array to simplify access
                if ((i + dataSize) <= EepromBytes.Length / 4)
                    Array.Copy(EepromBytes, i * 4, entryArray, 0, dataSize * 4);    // If entire entry available
                else
                {
                    // Else, must wrap around buffer
                    int dataSize2 = (i + dataSize) % (EepromBytes.Length / 4);
                    int dataSize1 = dataSize - dataSize2;
                    Array.Copy(EepromBytes, i * 4, entryArray, 0, dataSize1 * 4);
                    Array.Copy(EepromBytes, 0, entryArray, dataSize1 * 4, dataSize2 * 4);
                }

                entryNumber++;
                entry = new ListViewItem(entryNumber.ToString());    // Save Entry number

                if (ProcessEntry(entryArray, type, entry) == false)
                    dataError = true;

                if (DEBUGGING)
                {
                    entry.SubItems.Add(i.ToString());                   // Save Index
                    entry.SubItems.Add((ARCHIVE_ADDRESS + i * 4).ToString());    // Save Address
                }

                ArchiveViewer.Items.Add(entry);
                i = (i + dataSize) % (EepromBytes.Length / 4);


                if (entryNumber.ToString() == txtCount.Text)
                    done = true;
            }
            if (dataError == true)
                MessageBox.Show("Algunas entradas presentan datos invalidos.", "Error en datos de registro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateDateFilterInput();
            if (entryNumber.ToString() != txtCount.Text)
            {
                entry = new ListViewItem("...");
                ArchiveViewer.Items.Add(entry);
            }

        }
        private bool ProcessEntry(byte[] entryData, EntryType type, ListViewItem entry)
        {
            Int32 ValueInteger;
            float ValueFloat;
            bool retVal = true;

            if (type == EntryType.OP_HISTORY)
            {
                entry.BackColor = Color.FromArgb(BACK_COLOR_OP[0], BACK_COLOR_OP[1], BACK_COLOR_OP[2]);

                ValueInteger = BitConverter.ToInt32(entryData, 0);
                if (ValueInteger < 0)
                {
                    retVal = false;
                    entry.ForeColor = Color.FromArgb(FORE_COLOR_BAD_DATA[0], FORE_COLOR_BAD_DATA[1], FORE_COLOR_BAD_DATA[2]); ;
                    entry.SubItems.Add(ValueInteger.ToString());    // Save Timestamp
                }
                else
                {
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(ValueInteger);
                    entry.SubItems.Add(date.ToString());        // Save Timestamp
                }

                ValueInteger = BitConverter.ToInt32(entryData, 4);
                ValueInteger &= ~(3 << 30); // Clear mask
                entry.SubItems.Add(ValueInteger.ToString("X4"));    // Save Digital Cell

                for (int j = 0; j < 7; j++)  // Save 7 sensor values
                {
                    ValueFloat = BitConverter.ToSingle(entryData, 8 + j * 4);
                    entry.SubItems.Add(ValueFloat.ToString());
                }
            }
            else if (type == EntryType.ERROR_DATA)
            {
                entry.BackColor = Color.FromArgb(BACK_COLOR_ERROR[0], BACK_COLOR_ERROR[1], BACK_COLOR_ERROR[2]);

                ValueInteger = BitConverter.ToInt32(entryData, 0);
                if (ValueInteger < 0)
                {
                    retVal = false;
                    entry.ForeColor = Color.FromArgb(FORE_COLOR_BAD_DATA[0], FORE_COLOR_BAD_DATA[1], FORE_COLOR_BAD_DATA[2]);
                    entry.SubItems.Add(ValueInteger.ToString());    // Save Timestamp
                }
                else
                {
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(ValueInteger);
                    entry.SubItems.Add(date.ToString());        // Save Timestamp
                }

                ValueInteger = BitConverter.ToInt32(entryData, 4);
                ValueInteger &= ~(3 << 30); // Clear mask
                entry.SubItems.Add(ValueInteger.ToString("X2"));    // Save error Code

                ValueFloat = BitConverter.ToSingle(entryData, 8);
                entry.SubItems.Add(ValueFloat.ToString());      // Save Error Value
            }
            return retVal;
        }

    #endregion


        private EntryType GetEntryType(ListViewItem item)
        {
            Color errorColor = Color.FromArgb(BACK_COLOR_ERROR[0], BACK_COLOR_ERROR[1], BACK_COLOR_ERROR[2]);
            Color opColor = Color.FromArgb(BACK_COLOR_OP[0], BACK_COLOR_OP[1], BACK_COLOR_OP[2]);
            Color invalidForeColor = Color.FromArgb(FORE_COLOR_BAD_DATA[0], FORE_COLOR_BAD_DATA[1], FORE_COLOR_BAD_DATA[2]);

            EntryType retVal = EntryType.NO_DATA;
            if (item.BackColor == errorColor && item.ForeColor != invalidForeColor)  // If else, item.ForeColor == FORE_COLOR_BAD_DATA, return NO_DATA
                retVal = EntryType.ERROR_DATA;
            else if (item.BackColor == opColor)
                retVal = EntryType.OP_HISTORY;

            return retVal;
        }
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
        private String GetErrorString(String code, Int32 param)
        {
            String errorString = "Codigo: " + code + Environment.NewLine;
            if (model == MacModel.A40TR)
            {
                #region GWF-40TR ErrorMessages
                switch (code)
                {
                    case "00":          //ERR_SENSOR
                        {
                            errorString += "Valor de sensores no coherentes.";
                            break;
                        }
                    case "01":      // ERR_TEMP_SENSOR
                        {
                            errorString += "Valor improbable en sensor de temperatura. Valor: " + param.ToString();
                            break;
                        }
                    case "02":      // ERR_EEPROM_READ
                        {
                            errorString += "Falla lectura EEPROM. Direccion: " + param.ToString();
                            break;
                        }
                    case "03":      // ERR_EEPROM_WRITE
                        {
                            errorString += "Falla escritura EEPROM. Direccion: " + param.ToString();
                            break;
                        }
                    case "04":      // ERR_OP_VALUES
                        {
                            errorString += "SetPoint recuperado de memoria no coherente. \n Se configuraron los valores por defecto.";
                            break;
                        }
                    case "05":      // ERR_CREATE_TIMEOUT
                        {
                            errorString += "Error al crear Timeout " + param.ToString();
                            break;
                        }
                    case "06":      // ERR_SYS_WATCHDOG
                        {
                            errorString += "System Watchdog. Se continuó la operacion normalmente.";
                            break;
                        }
                    case "07":      // ERR_FATAL_ERROR
                        {
                            errorString += "Excepcion no manejada. Imposible operar.";
                            break;
                        }
                    case "08":      // ERR_ARCHIVE_INIT
                        {
                            errorString += "Error iniciando historial. Se perdieron los datos pasados.";
                            break;
                        }
                    case "09":      // ERR_RTC_FAIL
                        {
                            errorString += "Fecha y hora invalida.";
                            break;
                        }
                    case "0A":      // ERR_FLOW
                        {
                            errorString += "FlowSwitch Evaporador.";
                            break;
                        }
                    case "0B":      // ERR_P_HIGH
                        {
                            errorString += "Presion alta : " + param.ToString();
                            break;
                        }
                    case "0C":      // ERR_P_LOW
                        {
                            errorString += "Presion baja: " + param.ToString();
                            break;
                        }
                    case "0D":      // ERR_P_DIF
                        {
                            errorString += "Presion diferencial: " + param.ToString();
                            break;
                        }
                    case "0E":      // ERR_T_CRIT
                        {
                            errorString += "Temp. evaporador: " + param.ToString();
                            break;
                        }
                    case "0F":      // ERR_T_MAX
                        {
                            errorString += "Temperatura por encima de máxima: " + param.ToString();
                            break;
                        }
                    case "10":      // ERR_COOLDOWN
                        {
                            errorString += "Imposible disminuir P. alta en etapa de arranque. \n Presion alta: " + param.ToString();
                            break;
                        }
                    case "11":      // ERR_COMP_OL
                        {
                            errorString += "Consumo compresor.";
                            break;
                        }
                    case "12":      // ERR_CMP_WATCHDOG
                        {
                            errorString += "Compressor Watchdog.";
                            break;
                        }
                    case "13":      // ERR_VENT1_OL
                        {
                            errorString += "Consumo ventilador 1.";
                            break;
                        }
                    case "14":      // ERR_VENT2_OL
                        {
                            errorString += "Consumo ventilador 2.";
                            break;
                        }
                    case "15":      // ERR_VENT3_OL
                        {
                            errorString += "Consumo ventilador 3.";
                            break;
                        }
                    case "16":      // ERR_PUMP_OL
                        {
                            errorString += "Consumo bomba de agua.";
                            break;
                        }
                }
                #endregion
            }
            return errorString;
        }


        


    #region FILTER METHODS
        private void butSetFilter_Click(object sender, EventArgs e)
        {
            ClearFilter();    // First clear the filter in order to apply new filter 

            //Apply filter from fastest to slowest algorithm
            ApplyTypeFilter();
            ApplyDateFilter();
        }
        private void ApplyTypeFilter()
        {
            bool deleteItem = false;

            foreach (ListViewItem i in ArchiveViewer.Items)
            {
                deleteItem = false;
                EntryType type = GetEntryType(i);
                if (type == EntryType.NO_DATA && showInvalidEntries == false)
                    deleteItem = true;
                else if (type == EntryType.ERROR_DATA && showErrorEntries == false)
                    deleteItem = true;
                else if (type == EntryType.OP_HISTORY && showOpEntries == false)
                    deleteItem = true;

                if (deleteItem == true)
                {
                    deletedEntries.Items.Insert(deletedEntries.Items.Count, (ListViewItem)i.Clone());  // Insert at the end of list
                    i.Remove();
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
                    deletedEntries.Items.Insert(deletedEntries.Items.Count, (ListViewItem)i.Clone());  // Insert at the end of list
                    i.Remove();
                }
            }

        }
        private void butClearFilter_Click(object sender, EventArgs e)
        {
            ClearFilter();
        }
        private void ClearFilter()
        {
            foreach (ListViewItem j in deletedEntries.Items)
            {
                ArchiveViewer.Items.Insert(Convert.ToInt32(j.SubItems[NUMBER].Text) - 1, (ListViewItem)j.Clone());
                j.Remove();
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

    #region FORM INPUT ACTIONS
        private void ArchiveViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selected = this.ArchiveViewer.SelectedItems;
            if (selected.Count > 0)
            {
                if(GetEntryType(selected[0]) == EntryType.ERROR_DATA)   // If entry type ERROR
                {
                    ErrorViewer.BringToFront();
                    ErrorViewer.Visible = true;
                    String errorCode = selected[0].SubItems[CODE].Text;
                    Int32 param = Convert.ToInt32(selected[0].SubItems[TEMPIN].Text);

                    ErrorViewer.Text = GetErrorString(errorCode, param); 

                }
                else if(GetEntryType(selected[0]) == EntryType.OP_HISTORY)
                {
                    DetailViewer.Items.Clear();
                    DetailViewer.BringToFront();
                    DetailViewer.Visible = true;
                    String code = selected[0].SubItems[CODE].Text;
                    Int32 digital = Convert.ToInt32(code, 16);
                    if (((digital >> 24) & 0xC0) == ERROR_MASK)
                        return;


                    String text;
                    OpStatus status = (OpStatus)((digital >> 10) & 0x3);
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

                    ListViewItem item = new ListViewItem(text);
                    item.SubItems.Add((digital & 0x1).ToString());  // Flowswitch
                    item.SubItems.Add((digital >> 1 & 0x1).ToString());  // Compressor
                    item.SubItems.Add((digital >> 2 & 0x1).ToString());  // Fan 1
                    item.SubItems.Add((digital >> 3 & 0x1).ToString());  // Fan 2
                    item.SubItems.Add((digital >> 4 & 0x1).ToString());  // Fan 3
                    item.SubItems.Add((digital >> 5 & 0x1).ToString());  // Consumo Cmp
                    item.SubItems.Add((digital >> 6 & 0x1).ToString());  // Consumo Fan 1
                    item.SubItems.Add((digital >> 7 & 0x1).ToString());  // Consumo Fan 2
                    item.SubItems.Add((digital >> 8 & 0x1).ToString());  // Consumo Fan 3
                    item.SubItems.Add((digital >> 9 & 0x1).ToString());  // Consumo Bomba
                    DetailViewer.Items.Add(item);
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
            }
            ArchiveViewer_SelectedIndexChanged(sender, e);  // Update detailed view
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
                    FileName = "example.csv",
                    Filter = "CSV (*.csv)|*.csv",
                    FilterIndex = 0,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

            //show the dialog + display the results in a msgbox unless cancelled 
            if (sfd.ShowDialog() == DialogResult.OK)
            { 
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
                    type = GetEntryType(i);
                    fileStr += type.ToString() + ";";
                    for (int j = 0; j < i.SubItems.Count; j++)
                    {
                        if (j == NUMBER)
                        {
                            fileStr += i.Index; // Save filtered index, not absolute index in the archive
                        }
                        else if (j == CODE && type == EntryType.OP_HISTORY)  // Op entries save code as binary
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

    class ListViewItemComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            Int32 entryNumberA = Convert.ToInt32(((ListViewItem)x).SubItems[0].Text);
            Int32 entryNumberB = Convert.ToInt32(((ListViewItem)y).SubItems[0].Text);
            if (entryNumberA <= entryNumberB)
                return -1;
            else
                return 1;
        }
    }
}
