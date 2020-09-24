namespace ArchiveReader
{
	partial class ArchivePlotter
	{
		static int[] PLOTTER_VERSION_NUMBER = { 1, 0, 0 };
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchivePlotter));
			this.tabControl3 = new System.Windows.Forms.TabControl();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.btnShowCursorValue = new System.Windows.Forms.CheckBox();
			this.btnShowCmp = new System.Windows.Forms.CheckBox();
			this.btnLinkXaxis = new System.Windows.Forms.CheckBox();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.btn_SaveImgPress = new System.Windows.Forms.Button();
			this.btn_SaveImgTemps = new System.Windows.Forms.Button();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.grpbox_Bottom = new System.Windows.Forms.GroupBox();
			this.grpboxPres = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.chkbox_Press_ScrollH = new System.Windows.Forms.CheckBox();
			this.chkbox_Press_ScrollV = new System.Windows.Forms.CheckBox();
			this.tabs_Press = new System.Windows.Forms.TabControl();
			this.tab_Press_Stats = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label14 = new System.Windows.Forms.Label();
			this.txt_StatsPressMaxOn = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.txt_StatsPressMinOn = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txt_StatsPressMeanOn = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.txt_StatsPressStdOn = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.selBox_Press_Stats = new System.Windows.Forms.ComboBox();
			this.tab_Press_Config = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.selBox_Press_Configs = new System.Windows.Forms.ComboBox();
			this.chkListBox_Press = new System.Windows.Forms.CheckedListBox();
			this.plotter_Press = new ScottPlot.FormsPlot();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.grpboxTemps = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkbox_Temps_ScrollH = new System.Windows.Forms.CheckBox();
			this.chkbox_Temps_ScrollV = new System.Windows.Forms.CheckBox();
			this.tabs_Temps = new System.Windows.Forms.TabControl();
			this.tab_Temps_Stats = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txt_StatsTempsMax = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txt_StatsTempsPerf = new System.Windows.Forms.TextBox();
			this.txt_StatsTempsMean = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txt_StatsTempsSTD = new System.Windows.Forms.TextBox();
			this.numUpDown_setpointError = new System.Windows.Forms.NumericUpDown();
			this.numUpDown_setpoint = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.selBox_Temps_Stats = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tab_Temps_Ref = new System.Windows.Forms.TabPage();
			this.but_FillTempDif = new System.Windows.Forms.Button();
			this.chkListBox_Temps = new System.Windows.Forms.CheckedListBox();
			this.plotter_Temps = new ScottPlot.FormsPlot();
			this.tabControl3.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.tabPage6.SuspendLayout();
			this.grpbox_Bottom.SuspendLayout();
			this.grpboxPres.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabs_Press.SuspendLayout();
			this.tab_Press_Stats.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tab_Press_Config.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.grpboxTemps.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabs_Temps.SuspendLayout();
			this.tab_Temps_Stats.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUpDown_setpointError)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUpDown_setpoint)).BeginInit();
			this.tab_Temps_Ref.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl3
			// 
			this.tabControl3.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl3.Controls.Add(this.tabPage5);
			this.tabControl3.Controls.Add(this.tabPage6);
			this.tabControl3.Location = new System.Drawing.Point(6, 19);
			this.tabControl3.Name = "tabControl3";
			this.tabControl3.SelectedIndex = 0;
			this.tabControl3.Size = new System.Drawing.Size(1337, 91);
			this.tabControl3.TabIndex = 4;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.btnShowCursorValue);
			this.tabPage5.Controls.Add(this.btnShowCmp);
			this.tabPage5.Controls.Add(this.btnLinkXaxis);
			this.tabPage5.Location = new System.Drawing.Point(4, 4);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage5.Size = new System.Drawing.Size(1329, 65);
			this.tabPage5.TabIndex = 0;
			this.tabPage5.Text = "Graficos";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// btnShowCursorValue
			// 
			this.btnShowCursorValue.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnShowCursorValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnShowCursorValue.Location = new System.Drawing.Point(287, 16);
			this.btnShowCursorValue.Name = "btnShowCursorValue";
			this.btnShowCursorValue.Size = new System.Drawing.Size(180, 34);
			this.btnShowCursorValue.TabIndex = 2;
			this.btnShowCursorValue.Text = "Mostrar valores en puntero";
			this.btnShowCursorValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnShowCursorValue.UseVisualStyleBackColor = true;
			this.btnShowCursorValue.CheckedChanged += new System.EventHandler(this.btnShowCursorValue_CheckedChanged);
			// 
			// btnShowCmp
			// 
			this.btnShowCmp.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnShowCmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnShowCmp.Location = new System.Drawing.Point(122, 16);
			this.btnShowCmp.Name = "btnShowCmp";
			this.btnShowCmp.Size = new System.Drawing.Size(159, 34);
			this.btnShowCmp.TabIndex = 1;
			this.btnShowCmp.Text = "Mostrar compresores";
			this.btnShowCmp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnShowCmp.UseVisualStyleBackColor = true;
			this.btnShowCmp.CheckedChanged += new System.EventHandler(this.btnShowCmp_CheckedChanged);
			// 
			// btnLinkXaxis
			// 
			this.btnLinkXaxis.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnLinkXaxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnLinkXaxis.Location = new System.Drawing.Point(6, 16);
			this.btnLinkXaxis.Name = "btnLinkXaxis";
			this.btnLinkXaxis.Size = new System.Drawing.Size(110, 34);
			this.btnLinkXaxis.TabIndex = 0;
			this.btnLinkXaxis.Text = "Vincular eje X";
			this.btnLinkXaxis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnLinkXaxis.UseVisualStyleBackColor = true;
			this.btnLinkXaxis.CheckedChanged += new System.EventHandler(this.btnLinkXaxis_CheckedChanged);
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add(this.btn_SaveImgPress);
			this.tabPage6.Controls.Add(this.btn_SaveImgTemps);
			this.tabPage6.Controls.Add(this.checkBox6);
			this.tabPage6.Location = new System.Drawing.Point(4, 4);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage6.Size = new System.Drawing.Size(1329, 65);
			this.tabPage6.TabIndex = 1;
			this.tabPage6.Text = "Reporte";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// btn_SaveImgPress
			// 
			this.btn_SaveImgPress.Location = new System.Drawing.Point(148, 11);
			this.btn_SaveImgPress.Name = "btn_SaveImgPress";
			this.btn_SaveImgPress.Size = new System.Drawing.Size(135, 41);
			this.btn_SaveImgPress.TabIndex = 6;
			this.btn_SaveImgPress.Text = "Guardar grafico de presiones";
			this.btn_SaveImgPress.UseVisualStyleBackColor = true;
			this.btn_SaveImgPress.Click += new System.EventHandler(this.btn_SaveImgPress_Click);
			// 
			// btn_SaveImgTemps
			// 
			this.btn_SaveImgTemps.Location = new System.Drawing.Point(7, 11);
			this.btn_SaveImgTemps.Name = "btn_SaveImgTemps";
			this.btn_SaveImgTemps.Size = new System.Drawing.Size(135, 41);
			this.btn_SaveImgTemps.TabIndex = 5;
			this.btn_SaveImgTemps.Text = "Guardar grafico de temperaturas";
			this.btn_SaveImgTemps.UseVisualStyleBackColor = true;
			this.btn_SaveImgTemps.Click += new System.EventHandler(this.btn_SaveImgTemps_Click);
			// 
			// checkBox6
			// 
			this.checkBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox6.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBox6.Enabled = false;
			this.checkBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBox6.Location = new System.Drawing.Point(1152, 11);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(144, 41);
			this.checkBox6.TabIndex = 4;
			this.checkBox6.Text = "Generar reporte completo";
			this.checkBox6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBox6.UseVisualStyleBackColor = true;
			// 
			// grpbox_Bottom
			// 
			this.grpbox_Bottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpbox_Bottom.Controls.Add(this.tabControl3);
			this.grpbox_Bottom.Location = new System.Drawing.Point(3, 615);
			this.grpbox_Bottom.Name = "grpbox_Bottom";
			this.grpbox_Bottom.Size = new System.Drawing.Size(1343, 121);
			this.grpbox_Bottom.TabIndex = 5;
			this.grpbox_Bottom.TabStop = false;
			this.grpbox_Bottom.Text = "Controles Generales";
			// 
			// grpboxPres
			// 
			this.grpboxPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.grpboxPres.Controls.Add(this.groupBox4);
			this.grpboxPres.Controls.Add(this.tabs_Press);
			this.grpboxPres.Controls.Add(this.chkListBox_Press);
			this.grpboxPres.Controls.Add(this.plotter_Press);
			this.grpboxPres.Location = new System.Drawing.Point(3, 309);
			this.grpboxPres.Name = "grpboxPres";
			this.grpboxPres.Size = new System.Drawing.Size(1343, 300);
			this.grpboxPres.TabIndex = 6;
			this.grpboxPres.TabStop = false;
			this.grpboxPres.Text = "Presiones";
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.chkbox_Press_ScrollH);
			this.groupBox4.Controls.Add(this.chkbox_Press_ScrollV);
			this.groupBox4.Location = new System.Drawing.Point(965, 223);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(131, 67);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Controles";
			// 
			// chkbox_Press_ScrollH
			// 
			this.chkbox_Press_ScrollH.AutoSize = true;
			this.chkbox_Press_ScrollH.Checked = true;
			this.chkbox_Press_ScrollH.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkbox_Press_ScrollH.Location = new System.Drawing.Point(6, 19);
			this.chkbox_Press_ScrollH.Name = "chkbox_Press_ScrollH";
			this.chkbox_Press_ScrollH.Size = new System.Drawing.Size(102, 17);
			this.chkbox_Press_ScrollH.TabIndex = 5;
			this.chkbox_Press_ScrollH.Text = "Scroll Horizontal";
			this.chkbox_Press_ScrollH.UseVisualStyleBackColor = true;
			this.chkbox_Press_ScrollH.CheckedChanged += new System.EventHandler(this.chkbox_Press_ScrollH_CheckedChanged);
			// 
			// chkbox_Press_ScrollV
			// 
			this.chkbox_Press_ScrollV.AutoSize = true;
			this.chkbox_Press_ScrollV.Location = new System.Drawing.Point(6, 42);
			this.chkbox_Press_ScrollV.Name = "chkbox_Press_ScrollV";
			this.chkbox_Press_ScrollV.Size = new System.Drawing.Size(90, 17);
			this.chkbox_Press_ScrollV.TabIndex = 6;
			this.chkbox_Press_ScrollV.Text = "Scroll Vertical";
			this.chkbox_Press_ScrollV.UseVisualStyleBackColor = true;
			this.chkbox_Press_ScrollV.CheckedChanged += new System.EventHandler(this.chkbox_Press_ScrollV_CheckedChanged);
			// 
			// tabs_Press
			// 
			this.tabs_Press.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabs_Press.Controls.Add(this.tab_Press_Stats);
			this.tabs_Press.Controls.Add(this.tab_Press_Config);
			this.tabs_Press.Location = new System.Drawing.Point(1112, 19);
			this.tabs_Press.Name = "tabs_Press";
			this.tabs_Press.SelectedIndex = 0;
			this.tabs_Press.Size = new System.Drawing.Size(225, 275);
			this.tabs_Press.TabIndex = 2;
			// 
			// tab_Press_Stats
			// 
			this.tab_Press_Stats.AutoScroll = true;
			this.tab_Press_Stats.Controls.Add(this.groupBox3);
			this.tab_Press_Stats.Controls.Add(this.label2);
			this.tab_Press_Stats.Controls.Add(this.selBox_Press_Stats);
			this.tab_Press_Stats.Location = new System.Drawing.Point(4, 22);
			this.tab_Press_Stats.Name = "tab_Press_Stats";
			this.tab_Press_Stats.Padding = new System.Windows.Forms.Padding(3);
			this.tab_Press_Stats.Size = new System.Drawing.Size(217, 249);
			this.tab_Press_Stats.TabIndex = 1;
			this.tab_Press_Stats.Text = "Estadísticas";
			this.tab_Press_Stats.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.txt_StatsPressMaxOn);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.txt_StatsPressMinOn);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.txt_StatsPressMeanOn);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.txt_StatsPressStdOn);
			this.groupBox3.Location = new System.Drawing.Point(17, 44);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(189, 143);
			this.groupBox3.TabIndex = 16;
			this.groupBox3.TabStop = false;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(5, 67);
			this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(71, 13);
			this.label14.TabIndex = 17;
			this.label14.Text = "Máximo ON =";
			// 
			// txt_StatsPressMaxOn
			// 
			this.txt_StatsPressMaxOn.Location = new System.Drawing.Point(84, 64);
			this.txt_StatsPressMaxOn.Margin = new System.Windows.Forms.Padding(2);
			this.txt_StatsPressMaxOn.Name = "txt_StatsPressMaxOn";
			this.txt_StatsPressMaxOn.ReadOnly = true;
			this.txt_StatsPressMaxOn.Size = new System.Drawing.Size(76, 20);
			this.txt_StatsPressMaxOn.TabIndex = 18;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(5, 43);
			this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(70, 13);
			this.label12.TabIndex = 15;
			this.label12.Text = "Mínimo ON =";
			// 
			// txt_StatsPressMinOn
			// 
			this.txt_StatsPressMinOn.Location = new System.Drawing.Point(84, 40);
			this.txt_StatsPressMinOn.Margin = new System.Windows.Forms.Padding(2);
			this.txt_StatsPressMinOn.Name = "txt_StatsPressMinOn";
			this.txt_StatsPressMinOn.ReadOnly = true;
			this.txt_StatsPressMinOn.Size = new System.Drawing.Size(76, 20);
			this.txt_StatsPressMinOn.TabIndex = 16;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(5, 19);
			this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(79, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "Promedio ON =";
			// 
			// txt_StatsPressMeanOn
			// 
			this.txt_StatsPressMeanOn.Location = new System.Drawing.Point(84, 16);
			this.txt_StatsPressMeanOn.Margin = new System.Windows.Forms.Padding(2);
			this.txt_StatsPressMeanOn.Name = "txt_StatsPressMeanOn";
			this.txt_StatsPressMeanOn.ReadOnly = true;
			this.txt_StatsPressMeanOn.Size = new System.Drawing.Size(76, 20);
			this.txt_StatsPressMeanOn.TabIndex = 10;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(5, 91);
			this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(57, 13);
			this.label11.TabIndex = 11;
			this.label11.Text = "STD ON =";
			// 
			// txt_StatsPressStdOn
			// 
			this.txt_StatsPressStdOn.Location = new System.Drawing.Point(84, 88);
			this.txt_StatsPressStdOn.Margin = new System.Windows.Forms.Padding(2);
			this.txt_StatsPressStdOn.Name = "txt_StatsPressStdOn";
			this.txt_StatsPressStdOn.ReadOnly = true;
			this.txt_StatsPressStdOn.Size = new System.Drawing.Size(76, 20);
			this.txt_StatsPressStdOn.TabIndex = 12;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(6, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Variable";
			// 
			// selBox_Press_Stats
			// 
			this.selBox_Press_Stats.FormattingEnabled = true;
			this.selBox_Press_Stats.Location = new System.Drawing.Point(85, 17);
			this.selBox_Press_Stats.Name = "selBox_Press_Stats";
			this.selBox_Press_Stats.Size = new System.Drawing.Size(121, 21);
			this.selBox_Press_Stats.TabIndex = 0;
			this.selBox_Press_Stats.SelectedIndexChanged += new System.EventHandler(this.selBox_Press_Stats_SelectedIndexChanged);
			// 
			// tab_Press_Config
			// 
			this.tab_Press_Config.Controls.Add(this.label5);
			this.tab_Press_Config.Controls.Add(this.selBox_Press_Configs);
			this.tab_Press_Config.Location = new System.Drawing.Point(4, 22);
			this.tab_Press_Config.Name = "tab_Press_Config";
			this.tab_Press_Config.Size = new System.Drawing.Size(217, 249);
			this.tab_Press_Config.TabIndex = 2;
			this.tab_Press_Config.Text = "Configuracion";
			this.tab_Press_Config.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(10, 13);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(198, 16);
			this.label5.TabIndex = 5;
			this.label5.Text = "Configuracion de rango colores:";
			// 
			// selBox_Press_Configs
			// 
			this.selBox_Press_Configs.FormattingEnabled = true;
			this.selBox_Press_Configs.Items.AddRange(new object[] {
            "Rangos normales"});
			this.selBox_Press_Configs.Location = new System.Drawing.Point(81, 34);
			this.selBox_Press_Configs.Name = "selBox_Press_Configs";
			this.selBox_Press_Configs.Size = new System.Drawing.Size(125, 21);
			this.selBox_Press_Configs.TabIndex = 4;
			this.selBox_Press_Configs.Text = "Rangos normales";
			this.selBox_Press_Configs.SelectedIndexChanged += new System.EventHandler(this.selBox_Press_Configs_SelectedIndexChanged);
			// 
			// chkListBox_Press
			// 
			this.chkListBox_Press.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkListBox_Press.CheckOnClick = true;
			this.chkListBox_Press.FormattingEnabled = true;
			this.chkListBox_Press.Location = new System.Drawing.Point(965, 19);
			this.chkListBox_Press.Name = "chkListBox_Press";
			this.chkListBox_Press.Size = new System.Drawing.Size(131, 139);
			this.chkListBox_Press.TabIndex = 1;
			this.chkListBox_Press.SelectedIndexChanged += new System.EventHandler(this.chkListBox_Press_SelectedIndexChanged);
			// 
			// plotter_Press
			// 
			this.plotter_Press.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.plotter_Press.Location = new System.Drawing.Point(6, 19);
			this.plotter_Press.Margin = new System.Windows.Forms.Padding(0);
			this.plotter_Press.Name = "plotter_Press";
			this.plotter_Press.Size = new System.Drawing.Size(953, 275);
			this.plotter_Press.TabIndex = 0;
			this.plotter_Press.AxesChanged += new System.EventHandler(this.plotters_AxesChanged);
			this.plotter_Press.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plotters_MouseUp);
			this.plotter_Press.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.plotters_MouseWheel);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Controls.Add(this.grpboxTemps);
			this.flowLayoutPanel1.Controls.Add(this.grpboxPres);
			this.flowLayoutPanel1.Controls.Add(this.grpbox_Bottom);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1346, 725);
			this.flowLayoutPanel1.TabIndex = 7;
			// 
			// grpboxTemps
			// 
			this.grpboxTemps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpboxTemps.Controls.Add(this.groupBox1);
			this.grpboxTemps.Controls.Add(this.tabs_Temps);
			this.grpboxTemps.Controls.Add(this.chkListBox_Temps);
			this.grpboxTemps.Controls.Add(this.plotter_Temps);
			this.grpboxTemps.Location = new System.Drawing.Point(3, 3);
			this.grpboxTemps.Name = "grpboxTemps";
			this.grpboxTemps.Size = new System.Drawing.Size(1343, 300);
			this.grpboxTemps.TabIndex = 2;
			this.grpboxTemps.TabStop = false;
			this.grpboxTemps.Text = "Temperaturas";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.chkbox_Temps_ScrollH);
			this.groupBox1.Controls.Add(this.chkbox_Temps_ScrollV);
			this.groupBox1.Location = new System.Drawing.Point(965, 223);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(131, 67);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Controles";
			// 
			// chkbox_Temps_ScrollH
			// 
			this.chkbox_Temps_ScrollH.AutoSize = true;
			this.chkbox_Temps_ScrollH.Checked = true;
			this.chkbox_Temps_ScrollH.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkbox_Temps_ScrollH.Location = new System.Drawing.Point(6, 19);
			this.chkbox_Temps_ScrollH.Name = "chkbox_Temps_ScrollH";
			this.chkbox_Temps_ScrollH.Size = new System.Drawing.Size(102, 17);
			this.chkbox_Temps_ScrollH.TabIndex = 5;
			this.chkbox_Temps_ScrollH.Text = "Scroll Horizontal";
			this.chkbox_Temps_ScrollH.UseVisualStyleBackColor = true;
			this.chkbox_Temps_ScrollH.CheckedChanged += new System.EventHandler(this.chkbox_Temps_ScrollH_CheckedChanged);
			// 
			// chkbox_Temps_ScrollV
			// 
			this.chkbox_Temps_ScrollV.AutoSize = true;
			this.chkbox_Temps_ScrollV.Location = new System.Drawing.Point(6, 42);
			this.chkbox_Temps_ScrollV.Name = "chkbox_Temps_ScrollV";
			this.chkbox_Temps_ScrollV.Size = new System.Drawing.Size(90, 17);
			this.chkbox_Temps_ScrollV.TabIndex = 6;
			this.chkbox_Temps_ScrollV.Text = "Scroll Vertical";
			this.chkbox_Temps_ScrollV.UseVisualStyleBackColor = true;
			this.chkbox_Temps_ScrollV.CheckedChanged += new System.EventHandler(this.chkbox_Temps_ScrollV_CheckedChanged);
			// 
			// tabs_Temps
			// 
			this.tabs_Temps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabs_Temps.Controls.Add(this.tab_Temps_Stats);
			this.tabs_Temps.Controls.Add(this.tab_Temps_Ref);
			this.tabs_Temps.Location = new System.Drawing.Point(1112, 19);
			this.tabs_Temps.Name = "tabs_Temps";
			this.tabs_Temps.SelectedIndex = 0;
			this.tabs_Temps.Size = new System.Drawing.Size(225, 275);
			this.tabs_Temps.TabIndex = 2;
			// 
			// tab_Temps_Stats
			// 
			this.tab_Temps_Stats.AutoScroll = true;
			this.tab_Temps_Stats.Controls.Add(this.groupBox2);
			this.tab_Temps_Stats.Controls.Add(this.numUpDown_setpointError);
			this.tab_Temps_Stats.Controls.Add(this.numUpDown_setpoint);
			this.tab_Temps_Stats.Controls.Add(this.label3);
			this.tab_Temps_Stats.Controls.Add(this.label1);
			this.tab_Temps_Stats.Controls.Add(this.selBox_Temps_Stats);
			this.tab_Temps_Stats.Controls.Add(this.label4);
			this.tab_Temps_Stats.Location = new System.Drawing.Point(4, 22);
			this.tab_Temps_Stats.Name = "tab_Temps_Stats";
			this.tab_Temps_Stats.Padding = new System.Windows.Forms.Padding(3);
			this.tab_Temps_Stats.Size = new System.Drawing.Size(217, 249);
			this.tab_Temps_Stats.TabIndex = 1;
			this.tab_Temps_Stats.Text = "Estadísticas";
			this.tab_Temps_Stats.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txt_StatsTempsMax);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.txt_StatsTempsPerf);
			this.groupBox2.Controls.Add(this.txt_StatsTempsMean);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.txt_StatsTempsSTD);
			this.groupBox2.Location = new System.Drawing.Point(8, 73);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(189, 123);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			// 
			// txt_StatsTempsMax
			// 
			this.txt_StatsTempsMax.Location = new System.Drawing.Point(84, 40);
			this.txt_StatsTempsMax.Margin = new System.Windows.Forms.Padding(2);
			this.txt_StatsTempsMax.Name = "txt_StatsTempsMax";
			this.txt_StatsTempsMax.ReadOnly = true;
			this.txt_StatsTempsMax.Size = new System.Drawing.Size(76, 20);
			this.txt_StatsTempsMax.TabIndex = 16;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(5, 43);
			this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(55, 13);
			this.label13.TabIndex = 15;
			this.label13.Text = "Máximo = ";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(5, 19);
			this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(60, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "Promedio =";
			// 
			// txt_StatsTempsPerf
			// 
			this.txt_StatsTempsPerf.Location = new System.Drawing.Point(84, 88);
			this.txt_StatsTempsPerf.Margin = new System.Windows.Forms.Padding(2);
			this.txt_StatsTempsPerf.Name = "txt_StatsTempsPerf";
			this.txt_StatsTempsPerf.ReadOnly = true;
			this.txt_StatsTempsPerf.Size = new System.Drawing.Size(76, 20);
			this.txt_StatsTempsPerf.TabIndex = 14;
			// 
			// txt_StatsTempsMean
			// 
			this.txt_StatsTempsMean.Location = new System.Drawing.Point(84, 16);
			this.txt_StatsTempsMean.Margin = new System.Windows.Forms.Padding(2);
			this.txt_StatsTempsMean.Name = "txt_StatsTempsMean";
			this.txt_StatsTempsMean.ReadOnly = true;
			this.txt_StatsTempsMean.Size = new System.Drawing.Size(76, 20);
			this.txt_StatsTempsMean.TabIndex = 10;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(5, 91);
			this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(75, 13);
			this.label8.TabIndex = 13;
			this.label8.Text = "Rendimiento =";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(5, 67);
			this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(38, 13);
			this.label7.TabIndex = 11;
			this.label7.Text = "STD =";
			// 
			// txt_StatsTempsSTD
			// 
			this.txt_StatsTempsSTD.Location = new System.Drawing.Point(84, 64);
			this.txt_StatsTempsSTD.Margin = new System.Windows.Forms.Padding(2);
			this.txt_StatsTempsSTD.Name = "txt_StatsTempsSTD";
			this.txt_StatsTempsSTD.ReadOnly = true;
			this.txt_StatsTempsSTD.Size = new System.Drawing.Size(76, 20);
			this.txt_StatsTempsSTD.TabIndex = 12;
			// 
			// numUpDown_setpointError
			// 
			this.numUpDown_setpointError.DecimalPlaces = 1;
			this.numUpDown_setpointError.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numUpDown_setpointError.Location = new System.Drawing.Point(139, 16);
			this.numUpDown_setpointError.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numUpDown_setpointError.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
			this.numUpDown_setpointError.Name = "numUpDown_setpointError";
			this.numUpDown_setpointError.Size = new System.Drawing.Size(58, 20);
			this.numUpDown_setpointError.TabIndex = 6;
			this.numUpDown_setpointError.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numUpDown_setpointError.ValueChanged += new System.EventHandler(this.numUpDown_setpoint_ValueChanged);
			// 
			// numUpDown_setpoint
			// 
			this.numUpDown_setpoint.DecimalPlaces = 1;
			this.numUpDown_setpoint.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numUpDown_setpoint.Location = new System.Drawing.Point(68, 17);
			this.numUpDown_setpoint.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numUpDown_setpoint.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
			this.numUpDown_setpoint.Name = "numUpDown_setpoint";
			this.numUpDown_setpoint.Size = new System.Drawing.Size(56, 20);
			this.numUpDown_setpoint.TabIndex = 4;
			this.numUpDown_setpoint.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
			this.numUpDown_setpoint.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
			this.numUpDown_setpoint.ValueChanged += new System.EventHandler(this.numUpDown_setpoint_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(5, 19);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 15);
			this.label3.TabIndex = 3;
			this.label3.Text = "Setpoint =";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Variable:";
			// 
			// selBox_Temps_Stats
			// 
			this.selBox_Temps_Stats.FormattingEnabled = true;
			this.selBox_Temps_Stats.Location = new System.Drawing.Point(71, 46);
			this.selBox_Temps_Stats.Name = "selBox_Temps_Stats";
			this.selBox_Temps_Stats.Size = new System.Drawing.Size(126, 21);
			this.selBox_Temps_Stats.TabIndex = 0;
			this.selBox_Temps_Stats.SelectedIndexChanged += new System.EventHandler(this.selBox_Temps_Stats_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(123, 17);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(18, 20);
			this.label4.TabIndex = 5;
			this.label4.Text = "±";
			// 
			// tab_Temps_Ref
			// 
			this.tab_Temps_Ref.Controls.Add(this.but_FillTempDif);
			this.tab_Temps_Ref.Location = new System.Drawing.Point(4, 22);
			this.tab_Temps_Ref.Name = "tab_Temps_Ref";
			this.tab_Temps_Ref.Padding = new System.Windows.Forms.Padding(3);
			this.tab_Temps_Ref.Size = new System.Drawing.Size(217, 249);
			this.tab_Temps_Ref.TabIndex = 0;
			this.tab_Temps_Ref.Text = "Referencias";
			this.tab_Temps_Ref.UseVisualStyleBackColor = true;
			// 
			// but_FillTempDif
			// 
			this.but_FillTempDif.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.but_FillTempDif.Location = new System.Drawing.Point(9, 15);
			this.but_FillTempDif.Name = "but_FillTempDif";
			this.but_FillTempDif.Size = new System.Drawing.Size(197, 37);
			this.but_FillTempDif.TabIndex = 0;
			this.but_FillTempDif.Text = "Ver salto térmico";
			this.but_FillTempDif.UseVisualStyleBackColor = true;
			this.but_FillTempDif.Click += new System.EventHandler(this.but_FillTempDif_Click);
			// 
			// chkListBox_Temps
			// 
			this.chkListBox_Temps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkListBox_Temps.CheckOnClick = true;
			this.chkListBox_Temps.FormattingEnabled = true;
			this.chkListBox_Temps.Location = new System.Drawing.Point(965, 19);
			this.chkListBox_Temps.Name = "chkListBox_Temps";
			this.chkListBox_Temps.Size = new System.Drawing.Size(131, 139);
			this.chkListBox_Temps.TabIndex = 1;
			this.chkListBox_Temps.SelectedIndexChanged += new System.EventHandler(this.chkListBox_Temps_SelectedIndexChanged);
			// 
			// plotter_Temps
			// 
			this.plotter_Temps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.plotter_Temps.Location = new System.Drawing.Point(6, 19);
			this.plotter_Temps.Margin = new System.Windows.Forms.Padding(0);
			this.plotter_Temps.Name = "plotter_Temps";
			this.plotter_Temps.Size = new System.Drawing.Size(953, 275);
			this.plotter_Temps.TabIndex = 0;
			this.plotter_Temps.AxesChanged += new System.EventHandler(this.plotters_AxesChanged);
			this.plotter_Temps.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plotters_MouseUp);
			this.plotter_Temps.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.plotters_MouseWheel);
			// 
			// ArchivePlotter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1370, 749);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ArchivePlotter";
			this.Text = "ArchivePlotter";
			this.Resize += new System.EventHandler(this.ArchivePlotter_Resize);
			this.tabControl3.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.tabPage6.ResumeLayout(false);
			this.grpbox_Bottom.ResumeLayout(false);
			this.grpboxPres.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.tabs_Press.ResumeLayout(false);
			this.tab_Press_Stats.ResumeLayout(false);
			this.tab_Press_Stats.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tab_Press_Config.ResumeLayout(false);
			this.tab_Press_Config.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.grpboxTemps.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabs_Temps.ResumeLayout(false);
			this.tab_Temps_Stats.ResumeLayout(false);
			this.tab_Temps_Stats.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUpDown_setpointError)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUpDown_setpoint)).EndInit();
			this.tab_Temps_Ref.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl3;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.CheckBox btnLinkXaxis;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.CheckBox btnShowCursorValue;
		private System.Windows.Forms.CheckBox btnShowCmp;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.GroupBox grpbox_Bottom;
		private System.Windows.Forms.GroupBox grpboxPres;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox chkbox_Press_ScrollH;
		private System.Windows.Forms.CheckBox chkbox_Press_ScrollV;
		private System.Windows.Forms.TabControl tabs_Press;
		private System.Windows.Forms.TabPage tab_Press_Stats;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox selBox_Press_Stats;
		private System.Windows.Forms.CheckedListBox chkListBox_Press;
		private ScottPlot.FormsPlot plotter_Press;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.GroupBox grpboxTemps;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkbox_Temps_ScrollH;
		private System.Windows.Forms.CheckBox chkbox_Temps_ScrollV;
		private System.Windows.Forms.TabControl tabs_Temps;
		private System.Windows.Forms.TabPage tab_Temps_Ref;
		private System.Windows.Forms.TabPage tab_Temps_Stats;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox selBox_Temps_Stats;
		private System.Windows.Forms.CheckedListBox chkListBox_Temps;
		private ScottPlot.FormsPlot plotter_Temps;
		private System.Windows.Forms.Button btn_SaveImgPress;
		private System.Windows.Forms.Button btn_SaveImgTemps;
		private System.Windows.Forms.NumericUpDown numUpDown_setpointError;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numUpDown_setpoint;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txt_StatsTempsMean;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txt_StatsTempsPerf;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txt_StatsTempsSTD;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txt_StatsPressMeanOn;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txt_StatsPressStdOn;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox txt_StatsPressMinOn;
		private System.Windows.Forms.Button but_FillTempDif;
		private System.Windows.Forms.TextBox txt_StatsTempsMax;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox txt_StatsPressMaxOn;
		private System.Windows.Forms.TabPage tab_Press_Config;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox selBox_Press_Configs;
	}
}