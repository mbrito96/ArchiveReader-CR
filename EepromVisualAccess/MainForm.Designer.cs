
namespace EepromVisualAccess
{
    partial class MainForm
    {
        enum EntryType : byte{
            NO_DATA,
            ERROR_DATA,
            OP_HISTORY,
            MSG_DATA
        }
        enum OpStatus : byte
        {
            IDLE,
            RUNNING,
            POSTOP,
            DISABLED_STATE
        }
        enum MacModel : byte
        {
            A40TR,
            A80TR,
            W90TR
        }
        private const bool DEBUGGING = false;

        private const int DEFAULT_DATA_SIZE = 32588;
        private const int METADATA_ADDRESS = 180;
        private const int METADATA_SIZE = 8;
        private const int MAP_ADDRESS = 191;
        private const int MAP_SIZE = 97;
        private const int ARCHIVE_ADDRESS = 288;
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

        private byte[] BACK_COLOR_OP = new byte[3] { 135, 206, 235 };
        private byte[] BACK_COLOR_ERROR = new byte[3] { 255, 165, 119 };
        private byte[] FORE_COLOR_BAD_DATA = new byte[3] { 229, 5, 5 };

        private string workingPath = "C:\\Users\\mdevo\\Desktop";


        private MacModel model;
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
            this.butReadEeprom = new System.Windows.Forms.Button();
            this.numArchiveSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.modelSelector = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.butLoadFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.deletedEntries = new System.Windows.Forms.ListView();
            this.ArchiveViewer = new System.Windows.Forms.ListView();
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.MapViewer = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.DetailViewer = new System.Windows.Forms.ListView();
            this.columnHeader27 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader28 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.butExport2CSV = new System.Windows.Forms.Button();
            this.chkboxShowOp = new System.Windows.Forms.CheckBox();
            this.chkboxShowErrors = new System.Windows.Forms.CheckBox();
            this.chkboxShowInvalid = new System.Windows.Forms.CheckBox();
            this.butClearFilter = new System.Windows.Forms.Button();
            this.butSetFilter = new System.Windows.Forms.Button();
            this.dateFilterEnd = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numArchiveSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // butReadEeprom
            // 
            this.butReadEeprom.Location = new System.Drawing.Point(208, 100);
            this.butReadEeprom.Margin = new System.Windows.Forms.Padding(4);
            this.butReadEeprom.Name = "butReadEeprom";
            this.butReadEeprom.Size = new System.Drawing.Size(171, 43);
            this.butReadEeprom.TabIndex = 0;
            this.butReadEeprom.Text = "Leer memoria";
            this.butReadEeprom.UseVisualStyleBackColor = true;
            this.butReadEeprom.Click += new System.EventHandler(this.butReadEeprom_Click);
            // 
            // numArchiveSize
            // 
            this.numArchiveSize.Enabled = false;
            this.numArchiveSize.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numArchiveSize.Location = new System.Drawing.Point(106, 110);
            this.numArchiveSize.Margin = new System.Windows.Forms.Padding(4);
            this.numArchiveSize.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.numArchiveSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numArchiveSize.Name = "numArchiveSize";
            this.numArchiveSize.Size = new System.Drawing.Size(84, 22);
            this.numArchiveSize.TabIndex = 2;
            this.numArchiveSize.Value = new decimal(new int[] {
            32480,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 112);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tamaño (By)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.modelSelector);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.butLoadFile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.butReadEeprom);
            this.groupBox1.Controls.Add(this.numArchiveSize);
            this.groupBox1.Location = new System.Drawing.Point(19, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(441, 165);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parámetros de lectura en memoria EEPROM";
            // 
            // modelSelector
            // 
            this.modelSelector.CausesValidation = false;
            this.modelSelector.FormattingEnabled = true;
            this.modelSelector.Items.AddRange(new object[] {
            "GWF-A-20/30/40TR"});
            this.modelSelector.Location = new System.Drawing.Point(11, 49);
            this.modelSelector.Name = "modelSelector";
            this.modelSelector.Size = new System.Drawing.Size(179, 24);
            this.modelSelector.TabIndex = 9;
            this.modelSelector.Text = "Seleccione modelo";
            this.modelSelector.SelectedIndexChanged += new System.EventHandler(this.modelSelector_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EepromVisualAccess.Properties.Resources.info_img;
            this.pictureBox1.Location = new System.Drawing.Point(393, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 41);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // butLoadFile
            // 
            this.butLoadFile.Location = new System.Drawing.Point(208, 39);
            this.butLoadFile.Name = "butLoadFile";
            this.butLoadFile.Size = new System.Drawing.Size(171, 43);
            this.butLoadFile.TabIndex = 6;
            this.butLoadFile.Text = "Cargar archivo";
            this.butLoadFile.UseVisualStyleBackColor = true;
            this.butLoadFile.Click += new System.EventHandler(this.butLoadFile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.deletedEntries);
            this.groupBox2.Controls.Add(this.ArchiveViewer);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(20, 181);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1059, 415);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Visor de historial";
            // 
            // deletedEntries
            // 
            this.deletedEntries.Location = new System.Drawing.Point(838, 29);
            this.deletedEntries.Name = "deletedEntries";
            this.deletedEntries.Size = new System.Drawing.Size(203, 108);
            this.deletedEntries.TabIndex = 9;
            this.deletedEntries.UseCompatibleStateImageBehavior = false;
            this.deletedEntries.Visible = false;
            // 
            // ArchiveViewer
            // 
            this.ArchiveViewer.AutoArrange = false;
            this.ArchiveViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader16,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.ArchiveViewer.FullRowSelect = true;
            this.ArchiveViewer.GridLines = true;
            this.ArchiveViewer.Location = new System.Drawing.Point(17, 29);
            this.ArchiveViewer.MultiSelect = false;
            this.ArchiveViewer.Name = "ArchiveViewer";
            this.ArchiveViewer.Size = new System.Drawing.Size(1024, 380);
            this.ArchiveViewer.TabIndex = 0;
            this.ArchiveViewer.UseCompatibleStateImageBehavior = false;
            this.ArchiveViewer.View = System.Windows.Forms.View.Details;
            this.ArchiveViewer.SelectedIndexChanged += new System.EventHandler(this.ArchiveViewer_SelectedIndexChanged);
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "#";
            this.columnHeader16.Width = 43;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Fecha/Hora";
            this.columnHeader2.Width = 145;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Codigo";
            this.columnHeader3.Width = 55;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Temp Ent/Parametro";
            this.columnHeader4.Width = 120;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Temp Sal.";
            this.columnHeader5.Width = 65;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Temp Evap";
            this.columnHeader6.Width = 70;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Temp Amb";
            this.columnHeader7.Width = 65;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "P. Alta";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "P. Baja";
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "P. Dif";
            this.columnHeader10.Width = 65;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.MapViewer);
            this.groupBox3.Location = new System.Drawing.Point(17, 29);
            this.groupBox3.Name = "groupBox3";
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
            this.MapViewer.Location = new System.Drawing.Point(6, 35);
            this.MapViewer.Name = "MapViewer";
            this.MapViewer.Size = new System.Drawing.Size(239, 71);
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
            this.groupBox5.Controls.Add(this.ErrorViewer);
            this.groupBox5.Controls.Add(this.DetailViewer);
            this.groupBox5.Location = new System.Drawing.Point(496, 75);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(583, 103);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Detalles";
            // 
            // DetailViewer
            // 
            this.DetailViewer.BackColor = System.Drawing.SystemColors.Window;
            this.DetailViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader27,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader21,
            this.columnHeader22,
            this.columnHeader23,
            this.columnHeader24,
            this.columnHeader28,
            this.columnHeader25,
            this.columnHeader26});
            this.DetailViewer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.DetailViewer.Location = new System.Drawing.Point(6, 21);
            this.DetailViewer.MultiSelect = false;
            this.DetailViewer.Name = "DetailViewer";
            this.DetailViewer.Size = new System.Drawing.Size(571, 76);
            this.DetailViewer.TabIndex = 0;
            this.DetailViewer.UseCompatibleStateImageBehavior = false;
            this.DetailViewer.View = System.Windows.Forms.View.Details;
            this.DetailViewer.Visible = false;
            // 
            // columnHeader27
            // 
            this.columnHeader27.Text = "Estado";
            this.columnHeader27.Width = 62;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "FS";
            this.columnHeader18.Width = 30;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Cmp";
            this.columnHeader19.Width = 39;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "V1";
            this.columnHeader20.Width = 28;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "V2";
            this.columnHeader21.Width = 28;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "V3";
            this.columnHeader22.Width = 28;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Cmp";
            this.columnHeader23.Width = 39;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "V1";
            this.columnHeader24.Width = 28;
            // 
            // columnHeader28
            // 
            this.columnHeader28.Text = "V2";
            this.columnHeader28.Width = 28;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "V3";
            this.columnHeader25.Width = 28;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "Bomba";
            this.columnHeader26.Width = 50;
            // 
            // ErrorViewer
            // 
            this.ErrorViewer.BackColor = System.Drawing.SystemColors.Window;
            this.ErrorViewer.Location = new System.Drawing.Point(6, 21);
            this.ErrorViewer.Multiline = true;
            this.ErrorViewer.Name = "ErrorViewer";
            this.ErrorViewer.ReadOnly = true;
            this.ErrorViewer.Size = new System.Drawing.Size(571, 76);
            this.ErrorViewer.TabIndex = 1;
            this.ErrorViewer.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Cantidad";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(85, 24);
            this.txtCount.Name = "txtCount";
            this.txtCount.ReadOnly = true;
            this.txtCount.Size = new System.Drawing.Size(100, 22);
            this.txtCount.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(206, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Tail";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(366, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Head";
            // 
            // txtTail
            // 
            this.txtTail.Location = new System.Drawing.Point(243, 24);
            this.txtTail.Name = "txtTail";
            this.txtTail.ReadOnly = true;
            this.txtTail.Size = new System.Drawing.Size(100, 22);
            this.txtTail.TabIndex = 4;
            // 
            // txtHead
            // 
            this.txtHead.Location = new System.Drawing.Point(412, 24);
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
            this.groupBox4.Location = new System.Drawing.Point(496, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(584, 56);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Metadatos";
            // 
            // dateFilterStart
            // 
            this.dateFilterStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFilterStart.Location = new System.Drawing.Point(81, 34);
            this.dateFilterStart.Name = "dateFilterStart";
            this.dateFilterStart.Size = new System.Drawing.Size(110, 22);
            this.dateFilterStart.TabIndex = 11;
            // 
            // groupBox6
            // 
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
            this.groupBox6.Location = new System.Drawing.Point(1092, 13);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(203, 583);
            this.groupBox6.TabIndex = 12;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Filtros";
            // 
            // butExport2CSV
            // 
            this.butExport2CSV.Location = new System.Drawing.Point(13, 519);
            this.butExport2CSV.Name = "butExport2CSV";
            this.butExport2CSV.Size = new System.Drawing.Size(178, 42);
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
            this.chkboxShowErrors.Location = new System.Drawing.Point(13, 200);
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
            this.chkboxShowInvalid.Location = new System.Drawing.Point(13, 120);
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
            this.butClearFilter.Location = new System.Drawing.Point(26, 324);
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
            this.dateFilterEnd.Location = new System.Drawing.Point(81, 72);
            this.dateFilterEnd.Name = "dateFilterEnd";
            this.dateFilterEnd.Size = new System.Drawing.Size(110, 22);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 606);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Historial de operaciones";
            ((System.ComponentModel.ISupportInitialize)(this.numArchiveSize)).EndInit();
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butReadEeprom;
        private System.Windows.Forms.NumericUpDown numArchiveSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView ArchiveViewer;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ListView MapViewer;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeaderDebug2;
        private System.Windows.Forms.ColumnHeader columnHeaderDebug1;
        private System.Windows.Forms.Button butLoadFile;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView DetailViewer;
        private System.Windows.Forms.ColumnHeader columnHeader27;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.ColumnHeader columnHeader23;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.ColumnHeader columnHeader26;
        private System.Windows.Forms.ColumnHeader columnHeader28;
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
    }
}

