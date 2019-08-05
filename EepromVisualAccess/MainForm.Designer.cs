namespace EepromVisualAccess
{
    partial class MainForm
    {
        const bool DEBUGGING = false;

        public enum EntryType : byte
        {
            NO_DATA,
            ERROR_DATA,
            OP_HISTORY,
            MSG_DATA
        }
        public enum OpStatus : byte
        {
            IDLE,
            RUNNING,
            POSTOP,
            DISABLED_STATE
        }
        public enum MacModel : byte
        {
            A40TR,
            A80TR,
            W90TR
        }

        static class ArchiveInfo
        {
            public static int archiveSize,
            metadataAddress,
            metadataSize,
            mapAddress,
            mapSize,
            regFileAddress,
            regFileSize;

            public static int opEntrySize, errorEntrySize;

            public static int MaxEntrySize()
            {
                if (opEntrySize > errorEntrySize)
                    return opEntrySize;
                else
                    return errorEntrySize;
            }
        }

        static string[] PLC_MATCHING_VERSION = { "V2.2.8", "V0.0.0" };
        // Defined Archive parameters for: { A40TR, A80TR, W90TR } 
        static int[] ENTIRE_DATA_SIZE = { 32588, 32616 };
        static int[] METADATA_ADDRESS = { 180, 152 };
        static int[] METADATA_SIZE = { 8, 8 };
        static int[] MAP_ADDRESS = { 191, 160 };
        static int[] MAP_SIZE = { 97, 128 };
        static int[] ARCHIVE_ADDRESS = { 288, 288 };
        static int[] ARCHIVE_SIZE = { 32480, 32480 };
        static int[] OP_ENTRY_SIZE = { 9, 12 };
        static int[] ERROR_ENTRY_SIZE = { 3, 3 };

        static byte[] BACK_COLOR_OP = new byte[3] { 135, 206, 235 };
        static byte[] BACK_COLOR_ERROR = new byte[3] { 255, 165, 119 };
        static byte[] FORE_COLOR_BAD_DATA = new byte[3] { 229, 5, 5 };

        private const byte OP_HISTORY_MASK = (0x80);
        private const byte ERROR_MASK = (0x40);
        // Defines for accessing entry subItems
        private const int NUMBER = 0;
        private const int DATE = 1;
        private const int CODE = 2;
        private const int TEMPIN = 3;
        private const int TEMPOUT = 4;
        private const int TEMPEV = 5;
        private const int TEMPAMB = 6;
        private const int PHIGH = 7;
        private const int PLOW = 8;
        private const int PDIF = 9;
        private const int INDEX = 10;
        private const int ADDRESS = 11;


        private string workingPath = "C:\\Users\\mdevo\\Desktop";
		private string workingFileName = null;
		private string csvPath = null;
		private string numCsvPath = null;

        
        private bool modelSelected = false;
        private bool showInvalidEntries = true;
        private bool showOpEntries = true;
        private bool showErrorEntries = true;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtboxPlcVersion = new System.Windows.Forms.TextBox();
			this.modelSelector = new System.Windows.Forms.ComboBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.butLoadFile = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.deletedEntries = new System.Windows.Forms.ListView();
			this.ArchiveViewer = new System.Windows.Forms.ListView();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.MapViewer = new System.Windows.Forms.ListView();
			this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.DetailViewer = new System.Windows.Forms.ListView();
			this.ErrorViewer = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtCount = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtTail = new System.Windows.Forms.TextBox();
			this.txtHead = new System.Windows.Forms.TextBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.dateFilterStart = new System.Windows.Forms.DateTimePicker();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.butExport2numCSV = new System.Windows.Forms.Button();
			this.butExport2CSV = new System.Windows.Forms.Button();
			this.chkboxShowOp = new System.Windows.Forms.CheckBox();
			this.chkboxShowErrors = new System.Windows.Forms.CheckBox();
			this.chkboxShowInvalid = new System.Windows.Forms.CheckBox();
			this.butClearFilter = new System.Windows.Forms.Button();
			this.butSetFilter = new System.Windows.Forms.Button();
			this.dateFilterEnd = new System.Windows.Forms.DateTimePicker();
			this.label6 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.statstrFilePath = new System.Windows.Forms.StatusStrip();
			this.statStripDataPath = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.statstrFilePath.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtboxPlcVersion);
			this.groupBox1.Controls.Add(this.modelSelector);
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Controls.Add(this.butLoadFile);
			this.groupBox1.Location = new System.Drawing.Point(19, 14);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Size = new System.Drawing.Size(441, 165);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Parámetros de lectura en memoria EEPROM";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 91);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(151, 17);
			this.label2.TabIndex = 11;
			this.label2.Text = "Lector compatible con:";
			// 
			// txtboxPlcVersion
			// 
			this.txtboxPlcVersion.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtboxPlcVersion.Location = new System.Drawing.Point(11, 116);
			this.txtboxPlcVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtboxPlcVersion.Name = "txtboxPlcVersion";
			this.txtboxPlcVersion.ReadOnly = true;
			this.txtboxPlcVersion.Size = new System.Drawing.Size(179, 25);
			this.txtboxPlcVersion.TabIndex = 10;
			this.txtboxPlcVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// modelSelector
			// 
			this.modelSelector.CausesValidation = false;
			this.modelSelector.FormattingEnabled = true;
			this.modelSelector.Items.AddRange(new object[] {
            "GWF-A-20/30/40TR",
            "GWF-A-80TR"});
			this.modelSelector.Location = new System.Drawing.Point(11, 49);
			this.modelSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.modelSelector.Name = "modelSelector";
			this.modelSelector.Size = new System.Drawing.Size(179, 24);
			this.modelSelector.TabIndex = 9;
			this.modelSelector.Text = "Seleccione modelo";
			this.modelSelector.SelectedIndexChanged += new System.EventHandler(this.modelSelector_SelectedIndexChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::ArchiveReader.Properties.Resources.info_img;
			this.pictureBox1.Location = new System.Drawing.Point(393, 11);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(43, 41);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 7;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// butLoadFile
			// 
			this.butLoadFile.Location = new System.Drawing.Point(241, 98);
			this.butLoadFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.butLoadFile.Name = "butLoadFile";
			this.butLoadFile.Size = new System.Drawing.Size(171, 43);
			this.butLoadFile.TabIndex = 6;
			this.butLoadFile.Text = "Cargar archivo";
			this.butLoadFile.UseVisualStyleBackColor = true;
			this.butLoadFile.Click += new System.EventHandler(this.butLoadFile_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.deletedEntries);
			this.groupBox2.Controls.Add(this.ArchiveViewer);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Location = new System.Drawing.Point(20, 181);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox2.Size = new System.Drawing.Size(1059, 415);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Visor de historial";
			// 
			// deletedEntries
			// 
			this.deletedEntries.Location = new System.Drawing.Point(837, 30);
			this.deletedEntries.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.deletedEntries.Name = "deletedEntries";
			this.deletedEntries.Size = new System.Drawing.Size(203, 107);
			this.deletedEntries.TabIndex = 9;
			this.deletedEntries.UseCompatibleStateImageBehavior = false;
			this.deletedEntries.Visible = false;
			// 
			// ArchiveViewer
			// 
			this.ArchiveViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ArchiveViewer.AutoArrange = false;
			this.ArchiveViewer.FullRowSelect = true;
			this.ArchiveViewer.GridLines = true;
			this.ArchiveViewer.Location = new System.Drawing.Point(17, 30);
			this.ArchiveViewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.ArchiveViewer.MultiSelect = false;
			this.ArchiveViewer.Name = "ArchiveViewer";
			this.ArchiveViewer.Size = new System.Drawing.Size(1024, 381);
			this.ArchiveViewer.TabIndex = 0;
			this.ArchiveViewer.UseCompatibleStateImageBehavior = false;
			this.ArchiveViewer.View = System.Windows.Forms.View.Details;
			this.ArchiveViewer.SelectedIndexChanged += new System.EventHandler(this.ArchiveViewer_SelectedIndexChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.MapViewer);
			this.groupBox3.Location = new System.Drawing.Point(17, 30);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox3.Size = new System.Drawing.Size(251, 116);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Mapa de memoria";
			this.groupBox3.Visible = false;
			// 
			// MapViewer
			// 
			this.MapViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15});
			this.MapViewer.Location = new System.Drawing.Point(5, 34);
			this.MapViewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MapViewer.Name = "MapViewer";
			this.MapViewer.Size = new System.Drawing.Size(239, 70);
			this.MapViewer.TabIndex = 7;
			this.MapViewer.UseCompatibleStateImageBehavior = false;
			this.MapViewer.View = System.Windows.Forms.View.Details;
			this.MapViewer.Visible = false;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "I";
			this.columnHeader11.Width = 31;
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "0";
			this.columnHeader12.Width = 35;
			// 
			// columnHeader13
			// 
			this.columnHeader13.Text = "1";
			this.columnHeader13.Width = 31;
			// 
			// columnHeader14
			// 
			this.columnHeader14.Text = "2";
			this.columnHeader14.Width = 31;
			// 
			// columnHeader15
			// 
			this.columnHeader15.Text = "3";
			this.columnHeader15.Width = 31;
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.DetailViewer);
			this.groupBox5.Controls.Add(this.ErrorViewer);
			this.groupBox5.Location = new System.Drawing.Point(496, 75);
			this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox5.Size = new System.Drawing.Size(583, 103);
			this.groupBox5.TabIndex = 10;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Detalles";
			// 
			// DetailViewer
			// 
			this.DetailViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DetailViewer.BackColor = System.Drawing.SystemColors.Window;
			this.DetailViewer.ForeColor = System.Drawing.SystemColors.WindowText;
			this.DetailViewer.Location = new System.Drawing.Point(5, 21);
			this.DetailViewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.DetailViewer.MultiSelect = false;
			this.DetailViewer.Name = "DetailViewer";
			this.DetailViewer.Size = new System.Drawing.Size(571, 75);
			this.DetailViewer.TabIndex = 0;
			this.DetailViewer.UseCompatibleStateImageBehavior = false;
			this.DetailViewer.View = System.Windows.Forms.View.Details;
			this.DetailViewer.Visible = false;
			// 
			// ErrorViewer
			// 
			this.ErrorViewer.BackColor = System.Drawing.SystemColors.Window;
			this.ErrorViewer.Location = new System.Drawing.Point(5, 21);
			this.ErrorViewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.ErrorViewer.Multiline = true;
			this.ErrorViewer.Name = "ErrorViewer";
			this.ErrorViewer.ReadOnly = true;
			this.ErrorViewer.Size = new System.Drawing.Size(571, 75);
			this.ErrorViewer.TabIndex = 1;
			this.ErrorViewer.Visible = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 17);
			this.label3.TabIndex = 0;
			this.label3.Text = "Cantidad";
			// 
			// txtCount
			// 
			this.txtCount.Location = new System.Drawing.Point(85, 21);
			this.txtCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtCount.Name = "txtCount";
			this.txtCount.ReadOnly = true;
			this.txtCount.Size = new System.Drawing.Size(100, 22);
			this.txtCount.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(229, 25);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 17);
			this.label4.TabIndex = 1;
			this.label4.Text = "Tail";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(413, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 17);
			this.label5.TabIndex = 2;
			this.label5.Text = "Head";
			// 
			// txtTail
			// 
			this.txtTail.Location = new System.Drawing.Point(267, 21);
			this.txtTail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtTail.Name = "txtTail";
			this.txtTail.ReadOnly = true;
			this.txtTail.Size = new System.Drawing.Size(100, 22);
			this.txtTail.TabIndex = 4;
			// 
			// txtHead
			// 
			this.txtHead.Location = new System.Drawing.Point(460, 21);
			this.txtHead.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtHead.Name = "txtHead";
			this.txtHead.ReadOnly = true;
			this.txtHead.Size = new System.Drawing.Size(100, 22);
			this.txtHead.TabIndex = 5;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.txtHead);
			this.groupBox4.Controls.Add(this.txtTail);
			this.groupBox4.Controls.Add(this.txtCount);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Location = new System.Drawing.Point(496, 14);
			this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox4.Size = new System.Drawing.Size(584, 57);
			this.groupBox4.TabIndex = 9;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Metadatos";
			// 
			// dateFilterStart
			// 
			this.dateFilterStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateFilterStart.Location = new System.Drawing.Point(81, 34);
			this.dateFilterStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.dateFilterStart.Name = "dateFilterStart";
			this.dateFilterStart.Size = new System.Drawing.Size(111, 22);
			this.dateFilterStart.TabIndex = 11;
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox6.Controls.Add(this.butExport2numCSV);
			this.groupBox6.Controls.Add(this.butExport2CSV);
			this.groupBox6.Controls.Add(this.chkboxShowOp);
			this.groupBox6.Controls.Add(this.chkboxShowErrors);
			this.groupBox6.Controls.Add(this.chkboxShowInvalid);
			this.groupBox6.Controls.Add(this.butClearFilter);
			this.groupBox6.Controls.Add(this.butSetFilter);
			this.groupBox6.Controls.Add(this.dateFilterEnd);
			this.groupBox6.Controls.Add(this.label6);
			this.groupBox6.Controls.Add(this.label1);
			this.groupBox6.Controls.Add(this.dateFilterStart);
			this.groupBox6.Location = new System.Drawing.Point(1092, 14);
			this.groupBox6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBox6.Size = new System.Drawing.Size(203, 583);
			this.groupBox6.TabIndex = 12;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Filtros";
			// 
			// butExport2numCSV
			// 
			this.butExport2numCSV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.butExport2numCSV.Location = new System.Drawing.Point(13, 453);
			this.butExport2numCSV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.butExport2numCSV.Name = "butExport2numCSV";
			this.butExport2numCSV.Size = new System.Drawing.Size(179, 42);
			this.butExport2numCSV.TabIndex = 20;
			this.butExport2numCSV.Text = "Exportar a numCSV";
			this.butExport2numCSV.UseVisualStyleBackColor = true;
			this.butExport2numCSV.Click += new System.EventHandler(this.butExport2numCSV_Click);
			// 
			// butExport2CSV
			// 
			this.butExport2CSV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.butExport2CSV.Location = new System.Drawing.Point(13, 519);
			this.butExport2CSV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.butExport2CSV.Name = "butExport2CSV";
			this.butExport2CSV.Size = new System.Drawing.Size(179, 42);
			this.butExport2CSV.TabIndex = 10;
			this.butExport2CSV.Text = "Exportar a CSV";
			this.butExport2CSV.UseVisualStyleBackColor = true;
			this.butExport2CSV.Click += new System.EventHandler(this.butExport2CSV_Click);
			// 
			// chkboxShowOp
			// 
			this.chkboxShowOp.AutoSize = true;
			this.chkboxShowOp.Checked = true;
			this.chkboxShowOp.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkboxShowOp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkboxShowOp.Location = new System.Drawing.Point(13, 160);
			this.chkboxShowOp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chkboxShowOp.Name = "chkboxShowOp";
			this.chkboxShowOp.Size = new System.Drawing.Size(153, 24);
			this.chkboxShowOp.TabIndex = 19;
			this.chkboxShowOp.Text = "Mostrar estados";
			this.chkboxShowOp.UseVisualStyleBackColor = true;
			this.chkboxShowOp.CheckedChanged += new System.EventHandler(this.chkboxShowOp_CheckedChanged);
			// 
			// chkboxShowErrors
			// 
			this.chkboxShowErrors.AutoSize = true;
			this.chkboxShowErrors.Checked = true;
			this.chkboxShowErrors.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkboxShowErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkboxShowErrors.Location = new System.Drawing.Point(13, 199);
			this.chkboxShowErrors.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chkboxShowErrors.Name = "chkboxShowErrors";
			this.chkboxShowErrors.Size = new System.Drawing.Size(148, 24);
			this.chkboxShowErrors.TabIndex = 18;
			this.chkboxShowErrors.Text = "Mostrar errores";
			this.chkboxShowErrors.UseVisualStyleBackColor = true;
			this.chkboxShowErrors.CheckedChanged += new System.EventHandler(this.chkboxShowErrors_CheckedChanged);
			// 
			// chkboxShowInvalid
			// 
			this.chkboxShowInvalid.AutoSize = true;
			this.chkboxShowInvalid.Checked = true;
			this.chkboxShowInvalid.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkboxShowInvalid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkboxShowInvalid.Location = new System.Drawing.Point(13, 121);
			this.chkboxShowInvalid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chkboxShowInvalid.Name = "chkboxShowInvalid";
			this.chkboxShowInvalid.Size = new System.Drawing.Size(159, 24);
			this.chkboxShowInvalid.TabIndex = 17;
			this.chkboxShowInvalid.Text = "Mostrar invalidos";
			this.chkboxShowInvalid.UseVisualStyleBackColor = true;
			this.chkboxShowInvalid.CheckedChanged += new System.EventHandler(this.chkboxShowInvalid_CheckedChanged);
			// 
			// butClearFilter
			// 
			this.butClearFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butClearFilter.Location = new System.Drawing.Point(27, 324);
			this.butClearFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.butClearFilter.Name = "butClearFilter";
			this.butClearFilter.Size = new System.Drawing.Size(145, 42);
			this.butClearFilter.TabIndex = 16;
			this.butClearFilter.Text = "Limpiar";
			this.butClearFilter.UseVisualStyleBackColor = true;
			this.butClearFilter.Click += new System.EventHandler(this.butClearFilter_Click);
			// 
			// butSetFilter
			// 
			this.butSetFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butSetFilter.Location = new System.Drawing.Point(27, 261);
			this.butSetFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.butSetFilter.Name = "butSetFilter";
			this.butSetFilter.Size = new System.Drawing.Size(145, 42);
			this.butSetFilter.TabIndex = 15;
			this.butSetFilter.Text = "Aplicar";
			this.butSetFilter.UseVisualStyleBackColor = true;
			this.butSetFilter.Click += new System.EventHandler(this.butSetFilter_Click);
			// 
			// dateFilterEnd
			// 
			this.dateFilterEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateFilterEnd.Location = new System.Drawing.Point(81, 71);
			this.dateFilterEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.dateFilterEnd.Name = "dateFilterEnd";
			this.dateFilterEnd.Size = new System.Drawing.Size(111, 22);
			this.dateFilterEnd.TabIndex = 14;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(4, 73);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(54, 20);
			this.label6.TabIndex = 13;
			this.label6.Text = "Hasta";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(4, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 20);
			this.label1.TabIndex = 12;
			this.label1.Text = "Desde";
			// 
			// statstrFilePath
			// 
			this.statstrFilePath.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statstrFilePath.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statStripDataPath});
			this.statstrFilePath.Location = new System.Drawing.Point(0, 600);
			this.statstrFilePath.Name = "statstrFilePath";
			this.statstrFilePath.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
			this.statstrFilePath.Size = new System.Drawing.Size(1311, 22);
			this.statstrFilePath.TabIndex = 13;
			this.statstrFilePath.Text = "Ningun archivo elegido.";
			// 
			// statStripDataPath
			// 
			this.statStripDataPath.Name = "statStripDataPath";
			this.statStripDataPath.Size = new System.Drawing.Size(0, 17);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1311, 622);
			this.Controls.Add(this.statstrFilePath);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "MainForm";
			this.Text = "Historial de operaciones";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.statstrFilePath.ResumeLayout(false);
			this.statstrFilePath.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView ArchiveViewer;
        private System.Windows.Forms.ListView MapViewer;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ColumnHeader columnHeaderDebug2;
        private System.Windows.Forms.ColumnHeader columnHeaderDebug1;
        private System.Windows.Forms.Button butLoadFile;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView DetailViewer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox ErrorViewer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTail;
        private System.Windows.Forms.TextBox txtHead;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox modelSelector;
        private System.Windows.Forms.DateTimePicker dateFilterStart;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button butClearFilter;
        private System.Windows.Forms.Button butSetFilter;
        private System.Windows.Forms.DateTimePicker dateFilterEnd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView deletedEntries;
        private System.Windows.Forms.CheckBox chkboxShowInvalid;
        private System.Windows.Forms.CheckBox chkboxShowOp;
        private System.Windows.Forms.CheckBox chkboxShowErrors;
        private System.Windows.Forms.Button butExport2CSV;
        private System.Windows.Forms.StatusStrip statstrFilePath;
        private System.Windows.Forms.ToolStripStatusLabel statStripDataPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtboxPlcVersion;
		private System.Windows.Forms.Button butExport2numCSV;
	}
}

