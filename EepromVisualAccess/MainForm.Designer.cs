﻿namespace ArchiveReader
{
	using System;
	public enum EntryType : byte
	{
		NO_DATA,
		ERROR_DATA,
		OP_HISTORY,
		EVENT_DATA
	}
	public enum OpStatus : byte
	{
		IDLE,
		COOLDOWN,
		RUNNING,
		POSTOP,
		NONE
	}
	public enum MacModel : byte
	{
		A20TR,
		A80TR,
		W90TR,
		A100TR,
		A30TR,
		A40TR,
		GAF20TR,
		GAF30TR,
		NONE
	}
	public partial class MainForm
	{
		const bool DEBUGGING = false;


		static class ArchiveInfo
		{
			public static int archiveSize,
			machineIdAddress,
			machineIdSize,
			metadataAddress,
			metadataSize,
			mapAddress,
			mapSize,
			regFileAddress,
			regFileSize;

			public static int opEntrySize, errorEntrySize, eventEntrySize;

			public static int MaxEntrySize()
			{
				return opEntrySize;
			}
		}
		public struct Version
		{
			public uint major;
			public uint minor;
			public uint patch;
			public Version(uint versionMajor, uint versionMinor, uint versionPatch)
			{
				major = versionMajor;
				minor = versionMinor;
				patch = versionPatch;
			}
			/// <summary>
			/// Checks if object version is retrocompatible with parameter version.
			/// </summary>
			/// <param name="v">Parameter version to check compotibility with.</param>
			/// <returns>True if object version is greater and compatible with given version. False otherwise.</returns>
			public bool IsCompatibleWith(Version v)
			{
				bool retVal = false;
				if (this.major == v.major && this.minor >= v.minor && this.patch >= v.patch)
					retVal = true;

				return retVal;
			}
			public override string ToString() => $"V{major}.{minor}.{patch}";
		}
		public struct MachineId
		{
			public uint ID;
			public MacModel model;
			public uint intern;
			public Version softwareVersion;
			public MachineId(byte[] storedId)
			{
				if (storedId.Length == 4)
				{
					this.ID = BitConverter.ToUInt32(storedId, 0);
					uint modelCode = (ID >> 4) & 0xF;
					switch (modelCode)
					{
						case 1:
							this.model = MacModel.A20TR;
							break;
						case 2:
							this.model = MacModel.A80TR;
							break;
						case 3:
							this.model = MacModel.W90TR;
							break;
						case 4:
							this.model = MacModel.A100TR;
							break;
						case 5:
							this.model = MacModel.A30TR;
							break;
						case 6:
							this.model = MacModel.A40TR;
							break;
						case 7:
							this.model = MacModel.GAF20TR;
							break;
						case 8:
							this.model = MacModel.GAF30TR;
							break;
						default:
							this.model = MacModel.NONE;
							this.ID = 0;            // ID = 0 means an error. 
							break;
					}
					this.softwareVersion = new Version((ID >> 24) & 0xFF, (ID >> 16) & 0xFF, (ID >> 8) & 0xF);
					this.intern = (ID) & 0xF;
				}
				else
				{
					this.ID = 0;
					this.model = MacModel.NONE;
					this.intern = 0;
					this.softwareVersion = new Version(0, 0, 0);
				}
			}
			public bool InitOk()
			{
				return (this.ID == 0 ? false : true);
			}
		}
		
		public Version ARCHIVE_VERSION;
		public MachineId macID;

		static uint[] ARCHIVE_VERSION_NUMBER = { 3, 4, 2 };
        // Defined Archive parameters for: { A40TR, A80TR, W90TR, A100TR, GWF30A, GWF40A, GAF20A, GAF30A } 
        static int[] ENTIRE_DATA_SIZE = { 32492, 32492, 32492, 32580, 32492, 32492, 32492, 32492 };
        static int[] MACID_ADDRESS = { 276, 276, 276, 188, 276, 276, 276, 276 };
        static int[] MACID_SIZE = { 4, 4, 4, 4, 4, 4, 4, 4 };       // Only first value is used. All sizes should be equal to the first value for the program to identify machine.
        static int[] METADATA_ADDRESS = { 280, 280, 280, 192, 280, 280, 280, 280 };
        static int[] METADATA_SIZE = { 8, 8, 8, 8, 8, 8, 8, 8 };
        static int[] MAP_ADDRESS = { 191, 160, 144, 204, 191, 191, 191, 191 };
        static int[] MAP_SIZE = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static int[] ARCHIVE_ADDRESS = { 288, 288, 288, 200, 288, 288, 288, 288 };
        static int[] ARCHIVE_SIZE = { 32480, 32480, 32480, 32564, 32480, 32480, 32480, 32480 };
        static int[] OP_ENTRY_SIZE = { 9, 12, 13, 15, 9, 9, 8, 10 };
        static int[] ERROR_ENTRY_SIZE = { 3, 3, 3, 3, 3, 3, 3, 3 };
        static int[] EVENT_ENTRY_SIZE = { 2, 2, 2, 2, 2, 2, 2, 2 };

        static byte[] BACK_COLOR_OP = new byte[3] { 135, 206, 235 };
		static byte[] BACK_COLOR_OP_READY = new byte[3] { 145, 236, 244 };
		static byte[] BACK_COLOR_OP_POSTOP = new byte[3] { 147, 178, 241 };
		static byte[] BACK_COLOR_ERROR = new byte[3] { 255, 165, 119 };
		static byte[] BACK_COLOR_EVENT = new byte[3] { 255, 220, 23 };
		static byte[] FORE_COLOR_BAD_DATA = new byte[3] { 229, 5, 5 };

		private const byte OP_HISTORY_MASK = (0x80);
		private const byte ERROR_MASK = (0x40);
		private const byte EVENT_MASK = (0xC0);
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

		public static string workingPath = "C:\\Users\\mdevo\\Desktop";
		private string workingFileName = null;
		private string csvPath = null;
		private string numCsvPath = null;

		private bool modelSelected = false;
		private bool showInvalidEntries = true;
		private bool showOpEntries = true;
		private bool showErrorEntries = true;
		private bool showEventEntries = false;


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.modelSelector = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.butLoadFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ArchiveViewer = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.MapViewer = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deletedEntries = new System.Windows.Forms.ListView();
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
            this.butPlotter = new System.Windows.Forms.Button();
            this.chkboxShowEvents = new System.Windows.Forms.CheckBox();
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
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtMacVersion = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMacIntern = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMacModel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.statstrFilePath.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.modelSelector);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.butLoadFile);
            this.groupBox1.Location = new System.Drawing.Point(14, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 76);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parámetros de lectura en memoria EEPROM";
            // 
            // modelSelector
            // 
            this.modelSelector.CausesValidation = false;
            this.modelSelector.FormattingEnabled = true;
            this.modelSelector.Items.AddRange(new object[] {
            "GWF-A-20/30/40TR",
            "GWF-A-80TR",
            "GWF-W-90TR",
            "GWF-A-100TR",
            "GAF-A-20TR",
            "GAF-A-30TR"});
            this.modelSelector.Location = new System.Drawing.Point(5, 31);
            this.modelSelector.Margin = new System.Windows.Forms.Padding(2);
            this.modelSelector.Name = "modelSelector";
            this.modelSelector.Size = new System.Drawing.Size(135, 21);
            this.modelSelector.TabIndex = 9;
            this.modelSelector.Text = "Seleccione modelo";
            this.modelSelector.SelectedIndexChanged += new System.EventHandler(this.modelSelector_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ArchiveReader.Properties.Resources.info_img;
            this.pictureBox1.Location = new System.Drawing.Point(294, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // butLoadFile
            // 
            this.butLoadFile.Location = new System.Drawing.Point(154, 23);
            this.butLoadFile.Margin = new System.Windows.Forms.Padding(2);
            this.butLoadFile.Name = "butLoadFile";
            this.butLoadFile.Size = new System.Drawing.Size(128, 35);
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
            this.groupBox2.Controls.Add(this.ArchiveViewer);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.deletedEntries);
            this.groupBox2.Location = new System.Drawing.Point(15, 173);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(895, 311);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Visor de historial";
            // 
            // ArchiveViewer
            // 
            this.ArchiveViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArchiveViewer.AutoArrange = false;
            this.ArchiveViewer.FullRowSelect = true;
            this.ArchiveViewer.GridLines = true;
            this.ArchiveViewer.HideSelection = false;
            this.ArchiveViewer.Location = new System.Drawing.Point(13, 24);
            this.ArchiveViewer.Margin = new System.Windows.Forms.Padding(2);
            this.ArchiveViewer.MultiSelect = false;
            this.ArchiveViewer.Name = "ArchiveViewer";
            this.ArchiveViewer.Size = new System.Drawing.Size(870, 284);
            this.ArchiveViewer.TabIndex = 0;
            this.ArchiveViewer.UseCompatibleStateImageBehavior = false;
            this.ArchiveViewer.View = System.Windows.Forms.View.Details;
            this.ArchiveViewer.SelectedIndexChanged += new System.EventHandler(this.ArchiveViewer_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.MapViewer);
            this.groupBox3.Location = new System.Drawing.Point(13, 24);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(272, 309);
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
            this.MapViewer.HideSelection = false;
            this.MapViewer.Location = new System.Drawing.Point(4, 28);
            this.MapViewer.Margin = new System.Windows.Forms.Padding(2);
            this.MapViewer.Name = "MapViewer";
            this.MapViewer.Size = new System.Drawing.Size(264, 268);
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
            // deletedEntries
            // 
            this.deletedEntries.HideSelection = false;
            this.deletedEntries.Location = new System.Drawing.Point(730, 26);
            this.deletedEntries.Margin = new System.Windows.Forms.Padding(2);
            this.deletedEntries.Name = "deletedEntries";
            this.deletedEntries.Size = new System.Drawing.Size(153, 88);
            this.deletedEntries.TabIndex = 9;
            this.deletedEntries.UseCompatibleStateImageBehavior = false;
            this.deletedEntries.Visible = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.DetailViewer);
            this.groupBox5.Controls.Add(this.ErrorViewer);
            this.groupBox5.Location = new System.Drawing.Point(372, 67);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(538, 101);
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
            this.DetailViewer.HideSelection = false;
            this.DetailViewer.Location = new System.Drawing.Point(4, 17);
            this.DetailViewer.Margin = new System.Windows.Forms.Padding(2);
            this.DetailViewer.MultiSelect = false;
            this.DetailViewer.Name = "DetailViewer";
            this.DetailViewer.Size = new System.Drawing.Size(530, 80);
            this.DetailViewer.TabIndex = 0;
            this.DetailViewer.UseCompatibleStateImageBehavior = false;
            this.DetailViewer.View = System.Windows.Forms.View.Details;
            this.DetailViewer.Visible = false;
            // 
            // ErrorViewer
            // 
            this.ErrorViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorViewer.BackColor = System.Drawing.SystemColors.Window;
            this.ErrorViewer.Location = new System.Drawing.Point(4, 17);
            this.ErrorViewer.Margin = new System.Windows.Forms.Padding(2);
            this.ErrorViewer.Multiline = true;
            this.ErrorViewer.Name = "ErrorViewer";
            this.ErrorViewer.ReadOnly = true;
            this.ErrorViewer.Size = new System.Drawing.Size(530, 80);
            this.ErrorViewer.TabIndex = 1;
            this.ErrorViewer.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Cantidad";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(64, 17);
            this.txtCount.Margin = new System.Windows.Forms.Padding(2);
            this.txtCount.Name = "txtCount";
            this.txtCount.ReadOnly = true;
            this.txtCount.Size = new System.Drawing.Size(76, 20);
            this.txtCount.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Tail";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(310, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Head";
            // 
            // txtTail
            // 
            this.txtTail.Location = new System.Drawing.Point(200, 17);
            this.txtTail.Margin = new System.Windows.Forms.Padding(2);
            this.txtTail.Name = "txtTail";
            this.txtTail.ReadOnly = true;
            this.txtTail.Size = new System.Drawing.Size(76, 20);
            this.txtTail.TabIndex = 4;
            // 
            // txtHead
            // 
            this.txtHead.Location = new System.Drawing.Point(345, 17);
            this.txtHead.Margin = new System.Windows.Forms.Padding(2);
            this.txtHead.Name = "txtHead";
            this.txtHead.ReadOnly = true;
            this.txtHead.Size = new System.Drawing.Size(76, 20);
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
            this.groupBox4.Location = new System.Drawing.Point(372, 11);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(437, 48);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Metadatos";
            // 
            // dateFilterStart
            // 
            this.dateFilterStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFilterStart.Location = new System.Drawing.Point(61, 28);
            this.dateFilterStart.Margin = new System.Windows.Forms.Padding(2);
            this.dateFilterStart.Name = "dateFilterStart";
            this.dateFilterStart.Size = new System.Drawing.Size(84, 20);
            this.dateFilterStart.TabIndex = 11;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.butPlotter);
            this.groupBox6.Controls.Add(this.chkboxShowEvents);
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
            this.groupBox6.Location = new System.Drawing.Point(920, 11);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(152, 474);
            this.groupBox6.TabIndex = 12;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Filtros";
            // 
            // butPlotter
            // 
            this.butPlotter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.butPlotter.Location = new System.Drawing.Point(10, 345);
            this.butPlotter.Margin = new System.Windows.Forms.Padding(2);
            this.butPlotter.Name = "butPlotter";
            this.butPlotter.Size = new System.Drawing.Size(134, 34);
            this.butPlotter.TabIndex = 22;
            this.butPlotter.Text = "Graficar datos";
            this.butPlotter.UseVisualStyleBackColor = true;
            this.butPlotter.Click += new System.EventHandler(this.butPlotter_Click);
            // 
            // chkboxShowEvents
            // 
            this.chkboxShowEvents.AutoSize = true;
            this.chkboxShowEvents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxShowEvents.Location = new System.Drawing.Point(10, 194);
            this.chkboxShowEvents.Margin = new System.Windows.Forms.Padding(2);
            this.chkboxShowEvents.Name = "chkboxShowEvents";
            this.chkboxShowEvents.Size = new System.Drawing.Size(129, 21);
            this.chkboxShowEvents.TabIndex = 21;
            this.chkboxShowEvents.Text = "Mostrar eventos";
            this.chkboxShowEvents.UseVisualStyleBackColor = true;
            this.chkboxShowEvents.CheckedChanged += new System.EventHandler(this.chkboxShowEvents_CheckedChanged);
            // 
            // butExport2numCSV
            // 
            this.butExport2numCSV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.butExport2numCSV.Location = new System.Drawing.Point(10, 390);
            this.butExport2numCSV.Margin = new System.Windows.Forms.Padding(2);
            this.butExport2numCSV.Name = "butExport2numCSV";
            this.butExport2numCSV.Size = new System.Drawing.Size(134, 34);
            this.butExport2numCSV.TabIndex = 20;
            this.butExport2numCSV.Text = "Exportar a numCSV";
            this.butExport2numCSV.UseVisualStyleBackColor = true;
            this.butExport2numCSV.Click += new System.EventHandler(this.butExport2numCSV_Click);
            // 
            // butExport2CSV
            // 
            this.butExport2CSV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.butExport2CSV.Location = new System.Drawing.Point(10, 436);
            this.butExport2CSV.Margin = new System.Windows.Forms.Padding(2);
            this.butExport2CSV.Name = "butExport2CSV";
            this.butExport2CSV.Size = new System.Drawing.Size(134, 34);
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
            this.chkboxShowOp.Location = new System.Drawing.Point(10, 130);
            this.chkboxShowOp.Margin = new System.Windows.Forms.Padding(2);
            this.chkboxShowOp.Name = "chkboxShowOp";
            this.chkboxShowOp.Size = new System.Drawing.Size(129, 21);
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
            this.chkboxShowErrors.Location = new System.Drawing.Point(10, 162);
            this.chkboxShowErrors.Margin = new System.Windows.Forms.Padding(2);
            this.chkboxShowErrors.Name = "chkboxShowErrors";
            this.chkboxShowErrors.Size = new System.Drawing.Size(125, 21);
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
            this.chkboxShowInvalid.Location = new System.Drawing.Point(10, 98);
            this.chkboxShowInvalid.Margin = new System.Windows.Forms.Padding(2);
            this.chkboxShowInvalid.Name = "chkboxShowInvalid";
            this.chkboxShowInvalid.Size = new System.Drawing.Size(134, 21);
            this.chkboxShowInvalid.TabIndex = 17;
            this.chkboxShowInvalid.Text = "Mostrar invalidos";
            this.chkboxShowInvalid.UseVisualStyleBackColor = true;
            this.chkboxShowInvalid.CheckedChanged += new System.EventHandler(this.chkboxShowInvalid_CheckedChanged);
            // 
            // butClearFilter
            // 
            this.butClearFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butClearFilter.Location = new System.Drawing.Point(26, 293);
            this.butClearFilter.Margin = new System.Windows.Forms.Padding(2);
            this.butClearFilter.Name = "butClearFilter";
            this.butClearFilter.Size = new System.Drawing.Size(109, 34);
            this.butClearFilter.TabIndex = 16;
            this.butClearFilter.Text = "Limpiar";
            this.butClearFilter.UseVisualStyleBackColor = true;
            this.butClearFilter.Click += new System.EventHandler(this.butClearFilter_Click);
            // 
            // butSetFilter
            // 
            this.butSetFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butSetFilter.Location = new System.Drawing.Point(26, 242);
            this.butSetFilter.Margin = new System.Windows.Forms.Padding(2);
            this.butSetFilter.Name = "butSetFilter";
            this.butSetFilter.Size = new System.Drawing.Size(109, 34);
            this.butSetFilter.TabIndex = 15;
            this.butSetFilter.Text = "Aplicar";
            this.butSetFilter.UseVisualStyleBackColor = true;
            this.butSetFilter.Click += new System.EventHandler(this.butSetFilter_Click);
            // 
            // dateFilterEnd
            // 
            this.dateFilterEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFilterEnd.Location = new System.Drawing.Point(61, 58);
            this.dateFilterEnd.Margin = new System.Windows.Forms.Padding(2);
            this.dateFilterEnd.Name = "dateFilterEnd";
            this.dateFilterEnd.Size = new System.Drawing.Size(84, 20);
            this.dateFilterEnd.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 59);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Hasta";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Desde";
            // 
            // statstrFilePath
            // 
            this.statstrFilePath.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statstrFilePath.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statStripDataPath});
            this.statstrFilePath.Location = new System.Drawing.Point(0, 483);
            this.statstrFilePath.Name = "statstrFilePath";
            this.statstrFilePath.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statstrFilePath.Size = new System.Drawing.Size(1084, 22);
            this.statstrFilePath.TabIndex = 13;
            this.statstrFilePath.Text = "Ningun archivo elegido.";
            // 
            // statStripDataPath
            // 
            this.statStripDataPath.Name = "statStripDataPath";
            this.statStripDataPath.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtMacVersion);
            this.groupBox7.Controls.Add(this.label8);
            this.groupBox7.Controls.Add(this.txtMacIntern);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.txtMacModel);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Location = new System.Drawing.Point(13, 93);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(341, 75);
            this.groupBox7.TabIndex = 14;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Información de la máquina";
            // 
            // txtMacVersion
            // 
            this.txtMacVersion.Location = new System.Drawing.Point(116, 45);
            this.txtMacVersion.Margin = new System.Windows.Forms.Padding(2);
            this.txtMacVersion.Name = "txtMacVersion";
            this.txtMacVersion.ReadOnly = true;
            this.txtMacVersion.Size = new System.Drawing.Size(76, 20);
            this.txtMacVersion.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 48);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Version de software";
            // 
            // txtMacIntern
            // 
            this.txtMacIntern.Location = new System.Drawing.Point(207, 18);
            this.txtMacIntern.Margin = new System.Windows.Forms.Padding(2);
            this.txtMacIntern.Name = "txtMacIntern";
            this.txtMacIntern.ReadOnly = true;
            this.txtMacIntern.Size = new System.Drawing.Size(76, 20);
            this.txtMacIntern.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(161, 21);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Interno";
            // 
            // txtMacModel
            // 
            this.txtMacModel.Location = new System.Drawing.Point(58, 18);
            this.txtMacModel.Margin = new System.Windows.Forms.Padding(2);
            this.txtMacModel.Name = "txtMacModel";
            this.txtMacModel.ReadOnly = true;
            this.txtMacModel.Size = new System.Drawing.Size(76, 20);
            this.txtMacModel.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Modelo";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 505);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.statstrFilePath);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Archive Reader - Lector de historiales - ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
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
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.ListView ArchiveViewer; // Public so as to access it from plotter
		private System.Windows.Forms.ListView MapViewer;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ColumnHeader columnHeader14;
		private System.Windows.Forms.ColumnHeader columnHeader15;
		private System.Windows.Forms.GroupBox groupBox3;
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
		public System.Windows.Forms.DateTimePicker dateFilterStart;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Button butClearFilter;
		private System.Windows.Forms.Button butSetFilter;
		public System.Windows.Forms.DateTimePicker dateFilterEnd;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView deletedEntries;
		private System.Windows.Forms.CheckBox chkboxShowInvalid;
		private System.Windows.Forms.CheckBox chkboxShowOp;
		private System.Windows.Forms.CheckBox chkboxShowErrors;
		private System.Windows.Forms.Button butExport2CSV;
		private System.Windows.Forms.StatusStrip statstrFilePath;
		private System.Windows.Forms.ToolStripStatusLabel statStripDataPath;
		private System.Windows.Forms.Button butExport2numCSV;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.TextBox txtMacVersion;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtMacIntern;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtMacModel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkboxShowEvents;
		private System.Windows.Forms.Button butPlotter;
	}
}

