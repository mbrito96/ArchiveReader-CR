using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;

namespace ArchiveReader
{
	public partial class ArchivePlotter : Form
	{
		public class TempData
		{
			public string name;
			public int id;
			public List<double> values;
			public bool plot;	// Plot data in graph or not
			public ScottPlot.PlottableSignalXY signalName;
			public Color? color;
			public TEMP_STATISTICS stats;
			public TempData(int id, string name, bool plt, Color? colorVal = null)
			{
				this.id = id;
				this.name = name;
				this.plot = plt;
				this.color = colorVal;
				this.values = new List<double>();
			}
		}
		public class PressureData
		{
			public string name;
			public int id;
			public List<double> values;
			public bool plot;	// Plot data in graph or not
			public ScottPlot.PlottableSignalXY signalName;
			public Color? color;
			public PRESS_STATISTICS stats;

			public PressureData(int id, string n, bool plt, Color? colorVal = null)
			{
				this.id = id;
				this.name = n;
				this.plot = plt;
				this.color = colorVal;
				this.values = new List<double>();
			}
		}

		public class ArchiveSeries
		{
			public MacModel model;
			public double[] xs;	// Array to store X-axis values
			public List<DateTime> time;
			public List<int> digital;
			public List<TempData> temps;
			public List<PressureData> pressures;
			public List<int> coolingPower;
			public ScottPlot.PlottablePolygon coolingPlotTemps;
			public ScottPlot.PlottablePolygon coolingPlotPress;
		}

		struct linkXaxis_st {
			public bool linked;
			public bool rendered;
		};

		Version plotterVersion;
		ArchiveSeries seriesData;
		ListView source;
		FormWindowState? LastWindowState = null;
		static linkXaxis_st linkXaxis;
		GWF_DECODER decoder;
		bool showThermalJump = false;
		PlottablePolygon thermalJumpPolygon;

		public ArchivePlotter(ListView srcListView, MainForm.MachineId macId)
		{
			InitializeComponent();

			this.WindowState = FormWindowState.Maximized;

			source = srcListView;
			
			plotterVersion = new Version(PLOTTER_VERSION_NUMBER[0], PLOTTER_VERSION_NUMBER[1], PLOTTER_VERSION_NUMBER[2]);
			this.Text += " - V" + plotterVersion.ToString();
			
			seriesData = new ArchiveSeries();
			LoadDataFromMainForm(seriesData, macId);
			InitArchivePlotterForm(seriesData, macId);

			DrawTemperatures(seriesData);
			DrawPressures(seriesData);
		}

		#region DRAW SIGNALS
		/// <summary>
		/// Draws temperature curves into plotter_Temps plot, configures plot and sets the drawn curves in
		/// the checklistBox_Temps.
		/// </summary>
		/// <param name="serie"></param>
		void DrawTemperatures(ArchiveSeries serie, double? xLim_1 = null, double? xLim_2 = null, double? yLim_1 = null, double? yLim_2 = null)
		{
			for(int i = 0 ; i < serie.temps.Count ; i++)
			{
				TempData signal = seriesData.temps[i];
				if(signal.plot == true)		// If set, plot this temp
					signal.signalName = plotter_Temps.plt.PlotSignalXY(seriesData.xs, signal.values.ToArray(), label: signal.name, color: signal.color);
				
				chkListBox_Temps.SetItemChecked(i, serie.temps[i].plot); // Set checklist 
			}
			plotter_Temps.Configure(lowQualityWhileDragging: true, enableDoubleClickBenchmark: false, lockVerticalAxis: !chkbox_Temps_ScrollV.Checked, lockHorizontalAxis: !chkbox_Temps_ScrollH.Checked, showCoordinatesTooltip: btnShowCursorValue.Checked);
			plotter_Temps.plt.Ticks(dateTimeX: true);
			plotter_Temps.plt.YLabel("Temperatura (°C)");
			plotter_Temps.plt.Legend(location: legendLocation.upperRight, fontSize:10);

			plotter_Temps.plt.Axis(x1: xLim_1, x2: xLim_2, y1: yLim_1, y2: yLim_2);
			plotter_Temps.plt.Grid(ySpacing: 1);
			UpdateThermalJumpFill();
			plotter_Temps.Render();
		}

		/// <summary>
		/// Draws pressure curves into plotter_Press plot, configures plot and sets the drawn curves in
		/// the checklistBox_Press.
		/// </summary>
		/// <param name="serie"></param>
		void DrawPressures(ArchiveSeries serie, double? xLim_1 = null, double? xLim_2 = null, double? yLim_1 = null, double? yLim_2 = null)
		{
			for(int i = 0 ; i < serie.pressures.Count ; i++)
			{
				if(serie.pressures[i].plot == true)		// If set, plot this temp
					seriesData.pressures[i].signalName = plotter_Press.plt.PlotSignalXY(seriesData.xs, seriesData.pressures[i].values.ToArray(), label: seriesData.pressures[i].name, color: seriesData.pressures[i].color);
				
				chkListBox_Press.SetItemChecked(i, serie.pressures[i].plot); // Set checklist 
			}
			plotter_Press.Configure(lowQualityWhileDragging: true, enableDoubleClickBenchmark: false, lockVerticalAxis: !chkbox_Press_ScrollV.Checked, lockHorizontalAxis: !chkbox_Press_ScrollH.Checked, showCoordinatesTooltip: btnShowCursorValue.Checked);
			plotter_Press.plt.Ticks(dateTimeX: true);
			plotter_Press.plt.YLabel("Presion (PSI)");
			plotter_Press.plt.Legend(location: legendLocation.upperRight, fontSize:10);


			
			plotter_Press.plt.Axis(x1: xLim_1, x2: xLim_2, y1: yLim_1, y2: yLim_2);
			plotter_Press.plt.Grid(ySpacing: 10);

			plotter_Press.Render();
		}
		#endregion

		/// <summary>
		/// Sets the plots title and adds the necessary items for interacting with the graphs.
		/// Must be called after loading serie param with data.
		/// </summary>
		/// <param name="serie"></param>
		/// <param name="macId"></param>
		void InitArchivePlotterForm(ArchiveSeries serie, MainForm.MachineId macId)
		{
			#region INIT TEMPERATURE PLOT SECTION
			plotter_Temps.plt.Title(macId.model.ToString() + "-" + macId.intern.ToString("D2") + ": Registro de temperaturas");	// Plot title
			
			// Init checkListBox with temp names
			foreach(TempData i in serie.temps)
				chkListBox_Temps.Items.Add(i.name);
			#endregion 

			#region INIT PRESSURE PLOT SECTION
			plotter_Press.plt.Title(macId.model.ToString() + "-" + macId.intern.ToString("D2") + " : Registro de presiones");	// Plot title
			
			// Init checkListBox with pressure names
			foreach(PressureData i in serie.pressures)
				chkListBox_Press.Items.Add(i.name);
			#endregion 
			
			linkXaxis.linked = btnLinkXaxis.Checked;
			showThermalJump = false;
		}

		/// <summary>
		/// First, configures the serie according to machine model macId.
		/// Then, loads the data from MainForm into the serie object. 
		/// </summary>
		/// <param name="serie"></param>
		/// <param name="macId"></param>
		void LoadDataFromMainForm(ArchiveSeries serie, MainForm.MachineId macId)
		{
			// First, initialize structures
			serie.model = macId.model;
			serie.time = new List<DateTime>();
			serie.digital = new List<int>();
			serie.coolingPower = new List<int>();
			serie.temps = new List<TempData>();
			serie.pressures = new List<PressureData>();

			decoder = new GWF_DECODER(macId.model);
			
			// Instantiate one element in list per temperature signal
			for(int i = 0 ; i < decoder.temperatures.COLUMN_NAME.Length ; i++)
				serie.temps.Add(new TempData( decoder.temperatures.COLUMN_LUT[i], decoder.temperatures.COLUMN_NAME[i], true, plotter_Temps.plt.Colorset().GetColor(i)));

			// Instantiate one element in list per pressure signal
			for(int i = 0 ; i < decoder.pressures.COLUMN_NAME.Length ; i++)
				serie.pressures.Add(new PressureData( decoder.pressures.COLUMN_LUT[i], decoder.pressures.COLUMN_NAME[i], true, plotter_Temps.plt.Colorset().GetColor(i)));
			
			InitStatisticsFields(serie);

			// Now, obtain data from Archive Reader
			foreach(ListViewItem i in source.Items)
			{
				if(MainForm.arch1.GetEntryType(i) != EntryType.OP_HISTORY || MainForm.arch1.EntryIsInvalid(i))
					continue;
				// Get date
				DateTime entryDate = Convert.ToDateTime(i.SubItems[1].Text);
				serie.time.Add(entryDate);
				
				// Get digital code 
				serie.digital.Add(Convert.ToInt32(i.SubItems[2].Text, 16));

				// Get cooling power at that entry
				serie.coolingPower.Add(decoder.digital.GetCoolingPower(serie.digital.Last()));

				// Load temperature values
				for(int j = 0 ; j<serie.temps.Count ; j++)
					serie.temps[j].values.Add(double.Parse(i.SubItems[3+j].Text));
				
				// Load pressure values
				for(int j = 0 ; j<serie.pressures.Count ; j++)
					serie.pressures[j].values.Add(double.Parse(i.SubItems[7+j].Text));
			}

			// Transform datetimes into values for ScottPlot
			serie.xs = new double[serie.time.Count];
			for(int j =0 ; j < serie.time.Count ; j++)
			{
				serie.xs[j] = serie.time[j].ToOADate();
			}

			// Compute statistic data
			CalculateTempStat();
			CalculatePressStat();
			foreach(PressureData x in serie.pressures)
			{
				if(x.stats.onRatio == 0)
					x.plot = false;
			}
		}

		void InitStatisticsFields(ArchiveSeries serie)
		{
			switch(decoder.model)
			{
				case MacModel.A80TR:
					seriesData.temps[GWF80TR_TEMP_FIELDS.TEMP_OUT].stats = new TEMP_STATISTICS(GWF80TR_TEMP_FIELDS.TEMP_OUT, meanRange:KpiTempOut_GetMeanRanges(), performanceRange:KpiTempOut_GetPerformanceRanges());
					selBox_Temps_Stats.Items.Add(seriesData.temps[GWF80TR_TEMP_FIELDS.TEMP_OUT].name);

					seriesData.temps[GWF80TR_TEMP_FIELDS.TEMP_IN].stats = new TEMP_STATISTICS(GWF80TR_TEMP_FIELDS.TEMP_IN);
					selBox_Temps_Stats.Items.Add(seriesData.temps[GWF80TR_TEMP_FIELDS.TEMP_IN].name);

					seriesData.temps[GWF80TR_TEMP_FIELDS.TEMP_EVAP].stats = new TEMP_STATISTICS(GWF80TR_TEMP_FIELDS.TEMP_EVAP);
					selBox_Temps_Stats.Items.Add(seriesData.temps[GWF80TR_TEMP_FIELDS.TEMP_EVAP].name);

					seriesData.pressures[GWF80TR_PRESSURE_FIELDS.P_HIGH_A].stats = new PRESS_STATISTICS(GWF80TR_PRESSURE_FIELDS.P_HIGH_A, meanOnRange:KpiPresHigh_GetMeanOnRanges(), minOnRange:KpiPresHigh_GetMinOnRanges(), maxOnRange:KpiPresHigh_GetMaxOnRanges());
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF80TR_PRESSURE_FIELDS.P_HIGH_A].name);

					seriesData.pressures[GWF80TR_PRESSURE_FIELDS.P_HIGH_B].stats = new PRESS_STATISTICS(GWF80TR_PRESSURE_FIELDS.P_HIGH_B, meanOnRange:KpiPresHigh_GetMeanOnRanges(), minOnRange:KpiPresHigh_GetMinOnRanges(), maxOnRange:KpiPresHigh_GetMaxOnRanges());
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF80TR_PRESSURE_FIELDS.P_HIGH_B].name);
					break;
				case MacModel.W90TR:
					seriesData.temps[GWF90TR_TEMP_FIELDS.TEMP_OUT].stats = new TEMP_STATISTICS(GWF90TR_TEMP_FIELDS.TEMP_OUT, meanRange:KpiTempOut_GetMeanRanges(), performanceRange:KpiTempOut_GetPerformanceRanges());
					selBox_Temps_Stats.Items.Add(seriesData.temps[GWF90TR_TEMP_FIELDS.TEMP_OUT].name);

					seriesData.temps[GWF90TR_TEMP_FIELDS.TEMP_IN].stats = new TEMP_STATISTICS(GWF90TR_TEMP_FIELDS.TEMP_IN);
					selBox_Temps_Stats.Items.Add(seriesData.temps[GWF90TR_TEMP_FIELDS.TEMP_IN].name);

					seriesData.temps[GWF90TR_TEMP_FIELDS.TEMP_EVAP].stats = new TEMP_STATISTICS(GWF90TR_TEMP_FIELDS.TEMP_EVAP);
					selBox_Temps_Stats.Items.Add(seriesData.temps[GWF90TR_TEMP_FIELDS.TEMP_EVAP].name);

					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_HIGH_TA].stats = new PRESS_STATISTICS(GWF90TR_PRESSURE_FIELDS.P_HIGH_TA, meanOnRange:KpiPresHigh_GetMeanOnRanges(), minOnRange:KpiPresHigh_GetMinOnRanges(), maxOnRange:KpiPresHigh_GetMaxOnRanges());
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_HIGH_TA].name);

					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_LOW_TA].stats = new PRESS_STATISTICS(GWF90TR_PRESSURE_FIELDS.P_LOW_TA, meanOnRange:KpiPresLow_GetMeanOnRanges(), minOnRange:KpiPresLow_GetMinOnRanges(), maxOnRange:KpiPresLow_GetMaxOnRanges());
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_LOW_TA].name);

					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_TA].stats = new PRESS_STATISTICS(GWF90TR_PRESSURE_FIELDS.P_OIL_TA);
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_TA].name);

					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_TB].stats = new PRESS_STATISTICS(GWF90TR_PRESSURE_FIELDS.P_OIL_TB);
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_TB].name);

					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_HIGH_S].stats = new PRESS_STATISTICS(GWF90TR_PRESSURE_FIELDS.P_HIGH_S, meanOnRange:KpiPresHigh_GetMeanOnRanges(), minOnRange:KpiPresHigh_GetMinOnRanges(), maxOnRange:KpiPresHigh_GetMaxOnRanges());
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_HIGH_S].name);

					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_LOW_S].stats = new PRESS_STATISTICS(GWF90TR_PRESSURE_FIELDS.P_LOW_S, meanOnRange:KpiPresLow_GetMeanOnRanges(), minOnRange:KpiPresLow_GetMinOnRanges(), maxOnRange:KpiPresLow_GetMaxOnRanges());
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_LOW_S].name);

					seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_S].stats = new PRESS_STATISTICS(GWF90TR_PRESSURE_FIELDS.P_OIL_S);
					selBox_Press_Stats.Items.Add(seriesData.pressures[GWF90TR_PRESSURE_FIELDS.P_OIL_S].name);
					break;
			}
		}

		#region TEMPERATURE PLOT CONTROLS
		private void chkbox_Temps_ScrollH_CheckedChanged(object sender, EventArgs e)
		{
			plotter_Temps.Configure(lockHorizontalAxis:(!chkbox_Temps_ScrollH.Checked));
		}

		private void chkbox_Temps_ScrollV_CheckedChanged(object sender, EventArgs e)
		{
			plotter_Temps.Configure(lockVerticalAxis:(!chkbox_Temps_ScrollV.Checked));
		}

		private void chkListBox_Temps_SelectedIndexChanged(object sender, EventArgs e)
		{
			double[] currentAxis = plotter_Temps.plt.Axis();	// Save axis to mantain x axis after redrawing
			for(int i = 0 ; i < chkListBox_Temps.Items.Count ; i++)	
				seriesData.temps[i].plot = chkListBox_Temps.GetItemChecked(i);
			plotter_Temps.plt.Clear();

			DrawTemperatures(seriesData, currentAxis[0], currentAxis[1]);	// Draw with previous x axis limits
		}

		#endregion
		
		#region PRESSURE PLOT CONTROLS
		private void chkbox_Press_ScrollH_CheckedChanged(object sender, EventArgs e)
		{
			plotter_Press.Configure(lockHorizontalAxis:(!chkbox_Press_ScrollH.Checked));
		}
		private void chkbox_Press_ScrollV_CheckedChanged(object sender, EventArgs e)
		{
			plotter_Press.Configure(lockVerticalAxis:(!chkbox_Press_ScrollV.Checked));
		}
		private void chkListBox_Press_SelectedIndexChanged(object sender, EventArgs e)
		{
			double[] currentAxis = plotter_Press.plt.Axis();	// Save axis to mantain x axis after redrawing
			for(int i = 0 ; i < chkListBox_Press.Items.Count ; i++)
				seriesData.pressures[i].plot = chkListBox_Press.GetItemChecked(i);
			plotter_Press.plt.Clear();

			DrawPressures(seriesData, currentAxis[0], currentAxis[1]);	// Draw with previous x axis limits
		}
		#endregion
		private void ArchivePlotter_Resize(object sender, EventArgs e) 
		{
			// When window state changes
			if (WindowState != LastWindowState) 
			{
				grpboxTemps.Width = flowLayoutPanel1.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth - 4 * grpboxTemps.Margin.Left;
				grpboxPres.Width = flowLayoutPanel1.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth - 4 * grpboxPres.Margin.Left;
				grpbox_Bottom.Width = flowLayoutPanel1.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth - 4 * grpbox_Bottom.Margin.Left;
			
				int heightForPlots = flowLayoutPanel1.Height-1*20 - grpbox_Bottom.Height;
				grpboxTemps.Height = heightForPlots / 2;
				grpboxPres.Height = heightForPlots / 2;
				/*	if (WindowState == FormWindowState.Maximized) 
					if (WindowState == FormWindowState.Normal) */
				LastWindowState = WindowState;
			}
		}

		#region GRAPH AXES CHANGE HANDLERS
		/// <summary>
		/// Prevents mouse wheel from scrolling main form when zooming on graph. Also,
		/// renders graph if x axes is linked.
		/// </summary>
		void plotters_MouseWheel(object sender, EventArgs e)
		{
			HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
			ee.Handled = true;
			if(linkXaxis.linked == true && linkXaxis.rendered == false)
				Refresh_xAxesLink((ScottPlot.FormsPlot)sender);
		}
		void plotters_MouseUp(object sender, EventArgs e)
		{
			if(linkXaxis.linked == true && linkXaxis.rendered == false)
				Refresh_xAxesLink((ScottPlot.FormsPlot)sender);
		}
		ScottPlot.FormsPlot lastPlotUsed;
		/// <summary>
		/// Links x axis limits if linkXaxis is true.
		/// </summary>
		void plotters_AxesChanged(object sender, EventArgs e)
		{
			lastPlotUsed = (ScottPlot.FormsPlot) sender;
			if(linkXaxis.linked == true)
			{
				ScottPlot.FormsPlot activePlot = (ScottPlot.FormsPlot) sender;
				ScottPlot.FormsPlot otherPlot;
				if(activePlot.Name == "plotter_Temps")
					otherPlot = plotter_Press;
				else
					otherPlot = plotter_Temps;
				otherPlot.plt.MatchAxis(activePlot.plt, horizontal: true, vertical: false);
				linkXaxis.rendered = false;
			}
		}
		void Refresh_xAxesLink(ScottPlot.FormsPlot sender)
		{
			ScottPlot.FormsPlot otherPlot;
			if(sender.Name == "plotter_Temps")
				otherPlot = plotter_Press;
			else
				otherPlot = plotter_Temps;	
			otherPlot.Render();
		}
		private void btnLinkXaxis_CheckedChanged(object sender, EventArgs e)
		{
			linkXaxis.linked = btnLinkXaxis.Checked;
			linkXaxis.rendered = false;
			// Update all plotters according to last used plot.
			ScottPlot.FormsPlot otherPlot;
			if(lastPlotUsed.Name == plotter_Temps.Name)
				otherPlot = plotter_Press;
			else
				otherPlot = plotter_Temps;	

			otherPlot.plt.MatchAxis(lastPlotUsed.plt, horizontal: true, vertical: false);
			otherPlot.Render();
		}
		#endregion

		private void btnShowCmp_CheckedChanged(object sender, EventArgs e)
		{
			if(btnShowCmp.Checked)
			{
				double tempPowerHeight = (plotter_Temps.plt.Axis()[3]-plotter_Temps.plt.Axis()[2]) / 4;	// Cooling power height is represented as 20% of the total plot height
				double pressPowerHeight = (plotter_Press.plt.Axis()[3]-plotter_Press.plt.Axis()[2]) / 4;	// Cooling power height is represented as 20% of the total plot height
			
				double[] yTemps = seriesData.coolingPower.ConvertAll(x => ((double)x*tempPowerHeight + plotter_Temps.plt.Axis()[2])).ToArray();
				double[] yPress = seriesData.coolingPower.ConvertAll(x => ((double)x*pressPowerHeight + plotter_Press.plt.Axis()[2])).ToArray();

				seriesData.coolingPlotTemps = plotter_Temps.plt.PlotFill(seriesData.xs, yTemps, fillColor:Color.Red, fillAlpha: 0.5, baseline: plotter_Temps.plt.Axis()[2]);
				plotter_Temps.Render();
				seriesData.coolingPlotPress = plotter_Press.plt.PlotFill(seriesData.xs, yPress, fillColor:Color.Red, fillAlpha: 0.5, baseline: plotter_Press.plt.Axis()[2]);
				plotter_Press.Render();
			}
			else
			{
				// remove cmp curves
				plotter_Temps.plt.Remove(seriesData.coolingPlotTemps);
				plotter_Temps.Render();
				plotter_Press.plt.Remove(seriesData.coolingPlotPress);
				plotter_Press.Render();
			}
			
		}

		private void btnShowCursorValue_CheckedChanged(object sender, EventArgs e)
		{
			plotter_Temps.Configure(showCoordinatesTooltip: btnShowCursorValue.Checked);
			plotter_Press.Configure(showCoordinatesTooltip: btnShowCursorValue.Checked);
		}

		#region SAVE_IMAGES
		private void btn_SaveImgTemps_Click(object sender, EventArgs e)
		{
			String filePath = GetImgSavePathFromUser(seriesData.model.ToString() + " Registro de temperaturas - yymmdd");
			if(filePath != string.Empty)
				plotter_Temps.plt.SaveFig(filePath);
		}

		private void btn_SaveImgPress_Click(object sender, EventArgs e)
		{
			String filePath = GetImgSavePathFromUser(seriesData.model.ToString() + " Registro de presiones - yymmdd");
			if(filePath != string.Empty)
				plotter_Press.plt.SaveFig(filePath);
		}
		private string GetImgSavePathFromUser(String defaultFileName)
		{
			SaveFileDialog saveFileDialog;
			String fileContent = string.Empty;
			String filePath = string.Empty;
			
			using (saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.InitialDirectory = MainForm.workingPath;
				saveFileDialog.FileName = defaultFileName;
				saveFileDialog.Filter = "imagen (*.png)|*.png|All files (*.*)|*.*";;
				saveFileDialog.FilterIndex = 1;
				//openFileDialog.RestoreDirectory = true;

				if(saveFileDialog.ShowDialog() == DialogResult.OK)
					filePath = saveFileDialog.FileName;
			}
			return filePath;
		}
		#endregion	

		private void but_FillTempDif_Click(object sender, EventArgs e)
		{
			showThermalJump = !showThermalJump;
			UpdateThermalJumpFill();
			plotter_Temps.Render();
		}
		private void UpdateThermalJumpFill()
		{
			plotter_Temps.plt.Remove(thermalJumpPolygon);
			if(showThermalJump)
				thermalJumpPolygon = plotter_Temps.plt.PlotFill(seriesData.xs, seriesData.temps[decoder.temperatures.GetTempOut()].values.ToArray(), seriesData.xs, seriesData.temps[decoder.temperatures.GetTempIn()].values.ToArray(), fillAlpha: .3, fillColor: Color.Green);
		}

	}

}
