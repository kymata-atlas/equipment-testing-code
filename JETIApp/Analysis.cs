using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Diagnostics;
using NPlot;
using WinNPlot = NPlot.Windows;
using StereolabFX;
using System.Linq;
using System.Windows.Forms;

namespace JETIApp
{
	public partial class frmAnalysis : Form
	{

        private List<Reading> _FinalReadings; // once we've processed repeat readings and removed errors this contains the list of all final readings
		private List<List<Reading>> _RawReadings; // raw readings
		private List<Reading> _GammaTable; // processed gamma table
        private List<double> NormR, NormG, NormB;


		private MarkerItem _MaxMarker = null;
		private MarkerItem _MinMarker = null;

		private static Marker bluecross = new Marker(Marker.MarkerType.Cross1, 6, new Pen(Color.Blue, 2.0F));
		private static Marker redcircle = new Marker(Marker.MarkerType.Circle, 12, new Pen(Color.Red, 2.0F));
        private static Marker redcirclecustom = new Marker(Marker.MarkerType.FilledCircle, 12, new Pen(Color.Red, 2.0F));
		private static Marker lightbluecross = new Marker(Marker.MarkerType.Cross1, 4, new Pen(Color.LightBlue, 1.0F));

		private double _Maxlum = -99999; // maximum luminance
		private double _Minlum = 99999; // minimum luminance
		private double _OldMinLum = -1;
		private double _OldMaxLum = -1;
		private int _OldMinLumIndex = -1;
		private int _OldMaxLumIndex = -1;

		private int _Maxlumindex = -1;
		private int _Minlumindex = -1;

		private DateTime _StartedAt;
		private DateTime _EndedAt;
		private int _NoOfReadingsPerLevel;
		private bool _ReadTimings;
		private int _NoOfGrayLevels;
		private bool _RandomizeRepeats;

		public enum ProcessDataEnum
		{
			Min, // minimum value
			Max, // maximum value
			Mean, // mean value
			Cancel, // cancel
			First // first value - defaults to this

		}


		public frmAnalysis()
		{
            InitializeComponent();
			_FinalReadings = new List<Reading>();
			btnSaveGamma.Enabled = false;
			btnTimings.Enabled = false;

		}



		public bool OpenDataFile(string filename, ref string result)
		{
			// open data file

			TextReader datafile = null;

#region Read Header
			if (!File.Exists(filename))
			{
				result = "Data file not found";
				return false;
			}

			bool ret = true;
			try
			{
				datafile = File.OpenText(filename);

				string data = "";

				data = datafile.ReadLine();
				if (data != Constants.FileIdentifier)
				{
					result = "File does not contain data in the correct format";
					return false;
				}

				// read the rest of the data
				string[] dataitems;

				// read data from header
				int index = 0;

				for (int i = 0; i < Constants.NoHeaderLines; i++)
				{
					data = datafile.ReadLine();
					dataitems = data.Split(new char[] { (char)Keys.Tab });

					if (dataitems.Length != 2)
					{
						result = "Invalid data in file";
						throw new Exception();

					}
					else
					{
						switch (index)
						{
							case 0: // get date and time
								_StartedAt = DateTime.Parse(dataitems[1]);
								break;
							case 1: // get no. of gray levels
								_NoOfGrayLevels = int.Parse(dataitems[1]);
								break;
							case 2: // get no. of readings per level
								_NoOfReadingsPerLevel = int.Parse(dataitems[1]);
								break;
							case 3: // get whether timings were output
								_ReadTimings = bool.Parse(dataitems[1]);
								break;
							case 4: // determine if repeats are randomized rather than consecutive
								_RandomizeRepeats = bool.Parse(dataitems[1]);
								break;
						}
					}
					index++;
				}
#endregion
#region Read Data
				// read each data line

				// firstly for multiple readings ask how to process the data

				ProcessDataEnum pd;
				pd = ProcessDataEnum.First;

				if (_NoOfReadingsPerLevel != 1)
				{
					frmWhichReadings w = new frmWhichReadings(pd);
					w.ShowDialog();

					pd = w.Result;

					if (pd == ProcessDataEnum.Cancel)
					{
						result = "Data analysis cancelled";
						ret = false;
						throw new Exception();
					}

				}

				// list of lists for readings - each reading will contain values which might correspond to repeat readings as well
				
				
				_RawReadings = new List<List<Reading>>(_NoOfGrayLevels);
				
				_FinalReadings=new List<Reading>(_NoOfGrayLevels);

				int invalidreadings=0;

				int step = 1;
				if (_ReadTimings == true)
					step = 2; // read every other value as luminance data

				// initialise readings lists
				for (int i = 0; i < _NoOfGrayLevels; i++)
				{
					_RawReadings.Add(null);
					_FinalReadings.Add(null);
				}


				int NoOfDataLines;
				if (_RandomizeRepeats == false)
					NoOfDataLines = _NoOfGrayLevels;
				else
					NoOfDataLines = _NoOfGrayLevels * _NoOfReadingsPerLevel;
				// should be 1 line for each gray level
				for (int i = 0; i < NoOfDataLines; i++)
				{
					data = datafile.ReadLine();

					dataitems = data.Split(new char[] { (char)Keys.Tab });

					// depending on whether we have timings or not we have data in a different format

					uint r, g, b;
					int grayindex;
					grayindex = int.Parse(dataitems[0]);
					r = uint.Parse(dataitems[1]);
					g = uint.Parse(dataitems[2]);
					b = uint.Parse(dataitems[3]);

					double lum = -1.0;
					double timing = -1.0;
					
					int ReadingsPerLine;
					
					if (_RandomizeRepeats == true)
						ReadingsPerLine = 1;
					else
						ReadingsPerLine = _NoOfReadingsPerLevel;

					// each value is a reading or a timing
					for (int j = 0; j < ReadingsPerLine * step; j+=step)
					{
						bool valid = true;
						int start = 4; // index to start reading luminance values

						if (dataitems[start + j] != "!!!!") // !!!! denotes an error in reading
						{
							try
							{
								lum = double.Parse(dataitems[start+j]);
								if (_ReadTimings == true)
									timing = double.Parse(dataitems[start + j + 1]);
								valid = true;
							}
							catch
							{
								lum = -999.0; // error
								valid = false;
								invalidreadings++;
							}
						}
						else
						{
							lum = -999.0;
							valid = false;
							invalidreadings++;
						}


						// add reading to list
						if (valid == true)
						{
							Reading reading;
							if (_RawReadings[grayindex] == null)
								_RawReadings[grayindex] = new List<Reading>();
							List<Reading> temp = _RawReadings[grayindex];
							if (_ReadTimings == true)
								reading = new Reading(r, g, b, lum, timing, grayindex);
							else
								reading = new Reading(r, g, b, lum, grayindex);

							reading.index = grayindex;
							temp.Add(reading);
						}
					}
				}
					

				// at this point _RawReadings should be a list of all readings per gray index

				// depending on what processing we need to do we derive _FinalReadings from this list
				for (int i = 0; i < _NoOfGrayLevels; i++)
				{
					List<Reading> readings = _RawReadings[i];

					double lum;
					double templum;
					double timing = -1.0;
					double temptiming;

					if (pd==ProcessDataEnum.Min)
					{
						lum=999.0;
					}
					else if (pd==ProcessDataEnum.Max)
					{
						lum=-999.0;
					}
					else
					{
						lum=0.0;
					}


					uint r;
					uint g;
					uint b;
					int grayindex=-1;

					if (readings!=null && readings.Count > 0)
					{
						r = readings[0].R;
						g = readings[0].G;
						b = readings[0].B;
						grayindex = readings[0].index;

						for (int j = 0; j < readings.Count; j++)
						{
							Reading tempreading = readings[j];
							templum = tempreading.luminance;
							temptiming = tempreading.timing;

							switch (pd)
							{
								case ProcessDataEnum.First:
									lum = templum;
									timing = temptiming;
									break;
								case ProcessDataEnum.Max:
									if (templum > lum)
									{
										lum = templum;
										timing = temptiming;
									}

									break;
								case ProcessDataEnum.Min:
									if (templum < lum)
									{
										lum = templum;
										timing = temptiming;
									}

									break;
								case ProcessDataEnum.Mean:
									lum = lum + templum;
									timing = timing + temptiming;
									break;
							}

							if (pd == ProcessDataEnum.First) // don't need to process any more readings
								break;

						}
						if (pd == ProcessDataEnum.Mean)
						{
							{
								lum = lum / _RawReadings[i].Count;
								timing = timing / _RawReadings[i].Count;
							}
						}

						Reading finalreading;
						if (lum != -999.0 && grayindex!=-1)
						{
							if (_ReadTimings == true)
								finalreading = new Reading(r, g, b, lum, timing, grayindex);
							else
								finalreading = new Reading(r, g, b, lum, grayindex);

							_FinalReadings[grayindex] = finalreading;
						}
						else
							throw new Exception("Luminance value not set");
					}
				}

                // remove all null readings
                List<Reading> nulls = new List<Reading>();

                foreach (Reading r in _FinalReadings)
                {
                    if (r==null)
                    {
                        nulls.Add(r);
                    }
                }

                for (int i=0;i<nulls.Count;i++)
                {
                    _FinalReadings.Remove(nulls[i]);
                }
                
#endregion

				data = datafile.ReadLine();
				dataitems = data.Split(new char[] { (char)Keys.Tab });

				if (dataitems.Length != 2)
				{
					result = "Invalid data in file";
					throw new Exception();

				}
				else
					_EndedAt = DateTime.Parse(dataitems[1]);
			}


			catch (Exception e)
			{
				if (result == "")
					result = e.Message;

				ret = false;
			}
			finally
			{
				if (datafile != null)
				{
					datafile.Close();
					datafile.Dispose();
				}
			}
			return ret;


		}

		public bool SaveGamma(string filename, ref string result)
		{
			// save gamma table
			return true;
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			// open raw data file
			dlgOpenData.InitialDirectory = @"c:\"; //TODO: change this
			dlgOpenData.CheckFileExists = true;
			dlgOpenData.DefaultExt = "lum";
			dlgOpenData.Filter = "Gamma calibration luminance results files (*.lum,*.txt) |*.lum;*.txt";
			dlgOpenData.FilterIndex = 1;
			dlgOpenData.RestoreDirectory = true;
			dlgOpenData.Title = "Open results file";
			dlgOpenData.ValidateNames = true;

			DialogResult dlg = dlgOpenData.ShowDialog();
			if (dlg == DialogResult.Cancel)
				return;
			bool ret;
			string result = "";
			ret = OpenDataFile(dlgOpenData.FileName, ref result);

			if (ret == false)
			{
				MessageBox.Show("File error: " + result, "Open file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			SetDefaultMaxMin();
			PlotRawData();
			CalcGammaTable();
			PlotGamma();
			btnSaveGamma.Enabled = true;
			pnlModGammaInternal.Visible = true;
			if (_ReadTimings==true)
			{
				btnTimings.Enabled=true;
			}
			else
			{
				btnTimings.Enabled=false;
			}

		}

		private void SetDefaultMaxMin()
		{

			// determine max and min luminances for default

			Reading max = null;
			Reading min = null;

            bool SetMax=true;
            bool SetMin=true;


            if (chkForceMin.Checked == true)
            {
                SetMin = false;
                _Minlum = _FinalReadings[0].luminance;
                _Minlumindex = 0;
            }

            if (chkForceMax.Checked == true)
            {
                SetMax = false;
                _Maxlum = _FinalReadings[_FinalReadings.Count - 1].luminance;
                _Maxlumindex = _FinalReadings.Count - 1;
            }

            
            // MPD added when we have very low range of luminance values
            bool ResetForceMinMax = false;
            if (SetMax==false || SetMin==false)
            {
                if (_Maxlum == _Minlum)
                {
                    SetMax = true;
                    SetMin = true;
                    ResetForceMinMax = true; // if true we uncheck the force min/max luminance check boxes
                }
            }

            if (SetMax == false && SetMin == false)
            { }
            else
            {

                for (int i = 0; i < _FinalReadings.Count; i++)
                {
                    if (_FinalReadings[i].luminance > _Maxlum && SetMax == true)
                    {
                        _Maxlum = _FinalReadings[i].luminance;
                        _Maxlumindex = _FinalReadings.IndexOf(_FinalReadings[i]);
                        max = _FinalReadings[i];

                    }
                    if (_FinalReadings[i].luminance < _Minlum && SetMin == true)
                    {
                        _Minlum = _FinalReadings[i].luminance;
                        _Minlumindex = _FinalReadings.IndexOf(_FinalReadings[i]);
                        min = _FinalReadings[i];
                    }
                }
            }

			_OldMaxLumIndex = _Maxlumindex;
			_OldMinLumIndex = _Minlumindex;
			_OldMaxLum = _Maxlum;
			_OldMinLum = _Minlum;

            if (ResetForceMinMax == true)
            {
                chkForceMin.Checked = false;
                chkForceMax.Checked = false;
            }

		}

		private void SetMax(double maxlum, int maxlumindex)
		{
			_Maxlum = maxlum;
			_Maxlumindex = maxlumindex;

			_OldMaxLumIndex = _Maxlumindex;
			_OldMaxLum = _Maxlum;

			uint baselevel = 0;
			uint score = 0;

			score = ScoreReading(_FinalReadings[maxlumindex], ref baselevel);

			if (_MaxMarker != null)
			{
				pltRawData.Remove(_MaxMarker, false);
			}


			_MaxMarker = null;
			_MaxMarker = new MarkerItem(redcircle, baselevel+maxlumindex, _Maxlum);
			pltRawData.Add(_MaxMarker);
			txtMaxlum.Text = maxlum.ToString();

		}

		private void SetCustomMax(double maxlum)
		{
			_Maxlumindex = -1;
			if (_MaxMarker != null)
			{
				pltRawData.Remove(_MaxMarker, false);
			}

			_MaxMarker = null;
			_MaxMarker = new MarkerItem(redcirclecustom, 255, maxlum);
			pltRawData.Add(_MaxMarker);
			


		}

		private void SetCustomMin(double minlum)
		{
			_Minlumindex = -1;
			if (_MinMarker != null)
			{
				pltRawData.Remove(_MinMarker, false);
			}

			_MinMarker = null;
			_MinMarker = new MarkerItem(redcirclecustom, 0, _Minlum);
			pltRawData.Add(_MinMarker);
			
		}

		private void SetMin(double minlum, int minlumindex)
		{

			_Minlum = minlum;
			_Minlumindex = minlumindex;

			_OldMinLumIndex = _Minlumindex;
			_OldMinLum = _Minlum;

			uint baselevel = 0;
			uint score = 0;

			score = ScoreReading(_FinalReadings[minlumindex], ref baselevel);

			if (_MinMarker != null)
			{
				pltRawData.Remove(_MinMarker, false);
			}

			_MinMarker = null;
			_MinMarker = new MarkerItem(redcircle, baselevel+minlumindex, _Minlum);
			pltRawData.Add(_MinMarker);
			txtMinlum.Text = minlum.ToString();

		}

		private uint ScoreReading(Reading r, ref uint baselevel)
		{
			// determine based on luminance values what the current RGB value should be scored as

			// RGB luminances increase using the following table

			// 000
			// 001
			// 100
			// 010
			// 101
			// 011

			// so we score the RGB value accordingly

			uint level = r.R;

			if (r.G < level)
				level = r.G;

			if (r.B < level)
				level = r.B;

			baselevel = level;

			if (r.R == r.G && r.R == r.B) // 0,0,0
				return 0;

			if (r.B > level && (r.R == r.G) && r.R == level)  //0,0,1
				return 1;

			if (r.R > level && (r.G == r.B) && (r.G == level)) // 1,0,0
				return 1;

			if (r.G > level && (r.R == r.B) && (r.R == level)) // 0,1,0
				return 1;

			if (r.R > level && r.B > level && r.G == level) // 1,0,1
				return 1;

			if (r.R == level && r.G > level && r.B > level) // 0,1,1
				return 1;

			if (r.R > level && (r.G == r.R) && (r.B == level)) //1,1,0
				return 1;

			// anything other than these values is an error
			throw new ArgumentException("Invalid RGB value");
		}

		private void PlotRawData()
		{

			//plot readings

			pltRawData.Clear();
			pltRawData.RightMenu = WinNPlot.PlotSurface2D.DefaultContextMenu;

			pltRawData.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.VerticalGuideline());
			pltRawData.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.HorizontalRangeSelection(1));
			pltRawData.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.AxisDrag(true));
			
			pltRawData.ShowCoordinates = false;

			NPlot.Grid grid = new Grid();

			grid.VerticalGridType = Grid.GridType.Coarse;
			grid.HorizontalGridType = Grid.GridType.Coarse;

			grid.MajorGridPen = new Pen(Color.LightGray, 1.0f);

			// add grid to chart

            pltRawData.Add(grid);


			List<uint> indices = new List<uint>();
			List<double> values = new List<double>();

			List<double> ext_values = new List<double>();
			List<uint> ext_indices=new List<uint>();

			uint baselevel = 0;

            for (int i = 0; i < _FinalReadings.Count; i++)
			{

				if (_FinalReadings[i].R == _FinalReadings[i].G && (_FinalReadings[i].G == _FinalReadings[i].B)) // grayscale
				{
					indices.Add(_FinalReadings[i].R);
					values.Add(_FinalReadings[i].luminance);
				}
                else // MPD added 29-11-11 determine if we have only one channel 
                {
                    // get sum of all readings for R,G,B channels

                    var R_sum=(from f in _FinalReadings select (int)f.R).Sum();
                    var G_sum=(from f in _FinalReadings select (int)f.G).Sum();
                    var B_sum=(from f in _FinalReadings select (int)f.B).Sum();

                    StringBuilder Channels=new StringBuilder();
                    
                    if (R_sum!=0) // we have red channel
                        Channels.Append("R");
                    
                    if (G_sum!=0) // we have green channel
                        Channels.Append("G");

                    if (B_sum!=0) // we have blue channel
                        Channels.Append("B");

                    if (Channels.ToString().Length==1) // only one channel present so we are not using extended range
                    {
                        switch (Channels.ToString())
                        {
                            case "R":
                                indices.Add(_FinalReadings[i].R);
                                break;
                            case "G":
                                indices.Add(_FinalReadings[i].G);
                                break;
                            case "B":
                                indices.Add(_FinalReadings[i].G);
                                break;
                        }
                        values.Add(_FinalReadings[i].luminance);
                    }
                    else // using extended 
				    {
					    ScoreReading(_FinalReadings[i],ref baselevel);
					    ext_indices.Add(baselevel);
					    ext_values.Add(_FinalReadings[i].luminance);
                    }
				}

			}

			spnMax.Maximum = _FinalReadings.Count - 1;
			spnMax.Minimum = 0;
			spnMax.Increment = 1;

			spnMin.Maximum = _FinalReadings.Count - 1;
			spnMin.Minimum = 0;
			spnMin.Increment = 1;

			// create a point plot using the marker to plot values
			PointPlot pp = new PointPlot(bluecross);
			PointPlot ext_pp = new PointPlot(lightbluecross);

			pp.AbscissaData = indices;
			pp.OrdinateData = values;
			pp.Label = "Luminance values";

			ext_pp.AbscissaData = ext_indices;
			ext_pp.OrdinateData = ext_values;
			ext_pp.Label = "Extended range luminance values";

			pltRawData.Add(pp);
			pltRawData.Add(ext_pp);


			pltRawData.Title = "Raw luminance values";
			pltRawData.XAxis1.Label = "Index";
			pltRawData.YAxis1.Label = "Luminance";

			pltRawData.XAxis1.WorldMin = -10; // allow the max and min circles to be more easily seen
			pltRawData.XAxis1.WorldMax = 270;


			Legend legend = new Legend();
			legend.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Top, NPlot.PlotSurface2D.YAxisPosition.Left);
			legend.VerticalEdgePlacement = Legend.Placement.Inside;
			legend.HorizontalEdgePlacement = Legend.Placement.Inside;
			legend.XOffset = 10;
			legend.YOffset = 10;

			pltRawData.Legend = legend;
			pltRawData.LegendZOrder = 1;

			SetMax(_Maxlum, _Maxlumindex);
			SetMin(_Minlum, _Minlumindex);

			spnMax.Value = _Maxlumindex;
			spnMin.Value = _Minlumindex;

			pltRawData.Refresh();

		}

		private void CalcGammaTable()
		{

			if (_FinalReadings.Count == 0)
			{
				MessageBox.Show("No readings!");
				return;
			}

			// calculate gamma table

			// generate lookup table for desired luminances

			List<double> DesiredLum = new List<double>();
			double lumdiff = (_Maxlum - _Minlum) / 255; // 256 grey levels - 255 differences

			for (int i = 0; i < 256; i++)
			{
				DesiredLum.Add(_Minlum + (i * lumdiff));
			}

			_GammaTable = new List<Reading>();
			for (int i = 0; i < 256; i++)
				_GammaTable.Add(null);


			// find closest match to desired luminance in existing readings
            // MPD 29-11-11 changed to force readings to always choose the lowest gray level available and increment in grey levels otherwise PTB will choke
            // this is needed for noisy data which has a low luminance range
            int MinIndex = 0;
			for (int i = 0; i < DesiredLum.Count; i++)
			{
                List<int> indices = new List<int>();
				double diff = 999;
				uint baselevel=0;

				for (int j = MinIndex; j < _FinalReadings.Count; j++)
				{

					if (ScoreReading(_FinalReadings[j], ref baselevel) == 0 || chkUseExtended.Checked == true)
					{
						double thisdiff = Math.Abs(_FinalReadings[j].luminance - DesiredLum[i]);
						if (thisdiff < diff)
						{
							indices.Clear();
                            indices.Add( _FinalReadings.IndexOf(_FinalReadings[j]));
							diff = thisdiff;
						}
                        else if (thisdiff == diff)
                        {
                            indices.Add(_FinalReadings.IndexOf(_FinalReadings[j]));
                        }
					}
				}
                int index = -1;
                if (indices.Count == 0)
                { 
                }
                else if (indices.Count == 1)
                    index = indices[0]; // only one value available
                else
                    // get lowest available index
                    index = indices.Min();

                MinIndex = index;

				_GammaTable[i] = _FinalReadings[index];
			}

            // calculate normalised luminance values for Matlab
            List<double> R = (from Reading r in _GammaTable select (double)r.R).ToList();
            List<double> G = (from Reading r in _GammaTable select (double)r.G).ToList();
            List<double> B = (from Reading r in _GammaTable select (double)r.B).ToList();
            double MaxR = R.Max();
            double MaxG = G.Max();
            double MaxB = B.Max();

            NormR = (from  l in R select (l / MaxR)*(MaxR/255)).ToList();
            NormG = (from  l in G select (l / MaxG)*(MaxG/255)).ToList();
            NormB = (from  l in B select (l / MaxB)*(MaxB/255)).ToList(); 
            UpdateGammaGrid();

		}

		private void UpdateGammaGrid()
		{
			dataGamma.Rows.Clear();
			for (int i = 0; i < _GammaTable.Count; i++)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(dataGamma);
				row.SetValues(i, _GammaTable[i].R, _GammaTable[i].G, _GammaTable[i].B, string.Format("{0:#0.##}",_GammaTable[i].luminance),string.Format("{0:#0.##}",NormR[i]));
				dataGamma.Rows.Add(row);
			}
		}
		private void PlotGamma()
		{

			// plot luminance from generated gamma

			pltGamma.Clear();
			pltGamma.RightMenu = NPlot.Windows.PlotSurface2D.DefaultContextMenu;
			pltGamma.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.VerticalGuideline());
			pltGamma.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.HorizontalRangeSelection(3));
			pltGamma.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.AxisDrag(true));
			pltGamma.ShowCoordinates = false;

			NPlot.Grid grid = new Grid();

			grid.VerticalGridType = Grid.GridType.Coarse;
			grid.HorizontalGridType = Grid.GridType.Coarse;

			grid.MajorGridPen = new Pen(Color.LightGray, 1.0f);

			// add grid to chart
			pltGamma.Add(grid);

			// create a marker

			Marker m = new Marker(Marker.MarkerType.Cross1, 6, new Pen(Color.Red, 2.0F));

			// create a point plot using the marker to plot values
			PointPlot pp = new PointPlot(m);

			List<int> indices = new List<int>();
			List<double> values = new List<double>();

			for (int i = 0; i < _GammaTable.Count; i++)
			{
				indices.Add(i);
				values.Add(_GammaTable[i].luminance);
			}

			pp.AbscissaData = indices;
			pp.OrdinateData = values;
			pp.Label = "Luminance values";

			pltGamma.Add(pp);

			pltGamma.Title = "Luminance values for linearised gamma";
			pltGamma.XAxis1.Label = "Index";
			pltGamma.YAxis1.Label = "Luminance cd/m^2";
			pltGamma.XAxis1.WorldMin = -10;
			pltGamma.XAxis1.WorldMax = 270;

			Legend legend = new Legend();
			legend.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Top, NPlot.PlotSurface2D.YAxisPosition.Left);
			legend.VerticalEdgePlacement = Legend.Placement.Inside;
			legend.HorizontalEdgePlacement = Legend.Placement.Inside;
			legend.XOffset = 10;
			legend.YOffset = 10;

			pltGamma.Legend = legend;
			pltGamma.LegendZOrder = 1;

			pltGamma.Refresh();


		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void spnMin_ValueChanged(object sender, EventArgs e)
		{
			if (_Minlumindex == -1)
			{
				_Minlumindex = _OldMinLumIndex;
				spnMin.Value = _OldMinLumIndex;
				return;
			}

			SetMin(_FinalReadings[(int)spnMin.Value].luminance, (int)spnMin.Value);
			CalcGammaTable();
			PlotGamma();
			pltRawData.Refresh();
		}

		private void spnMax_ValueChanged(object sender, EventArgs e)
		{

			if (_Maxlumindex == -1)
			{
				_Maxlumindex = _OldMaxLumIndex;
				spnMax.Value = _OldMaxLumIndex;
				return;
			}

			SetMax(_FinalReadings[(int)spnMax.Value].luminance, (int)spnMax.Value);
			CalcGammaTable();
			PlotGamma();
			pltRawData.Refresh();

		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			SetDefaultMaxMin();
			SetMax(_Maxlum, _Maxlumindex);
			SetMin(_Minlum, _Minlumindex);
			pltRawData.Refresh();
		}

		private void btnSaveGamma_Click(object sender, EventArgs e)
		{
			// open raw data file
            dlgSaveGamma.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			dlgSaveGamma.CheckPathExists = true;
			dlgSaveGamma.DefaultExt = "gamma";
			dlgSaveGamma.Filter = "Gamma lookup table file (*.gamma)|*.gamma";
			dlgSaveGamma.RestoreDirectory = true;
			dlgSaveGamma.Title = "Save Gamma Table";
			dlgSaveGamma.ValidateNames = true;
			dlgSaveGamma.OverwritePrompt = true;

			if (dlgSaveGamma.ShowDialog() == DialogResult.OK)
			{
				TextWriter tw = File.CreateText(dlgSaveGamma.FileName);
				for (int i = 0; i < _GammaTable.Count; i++)
				{
					tw.WriteLine(_GammaTable[i].R);
					tw.WriteLine(_GammaTable[i].G);
					tw.WriteLine(_GammaTable[i].B);
				}
				tw.Flush();
				tw.Close();
				MessageBox.Show("Gamma table written");
			}

		}

		private void spnMin_Validating(object sender, CancelEventArgs e)
		{
			if (spnMin.Value >= spnMax.Value)
			{
				MessageBox.Show("Validation failed");
				e.Cancel = true;
			}
		}

		private void spnMax_Validating(object sender, CancelEventArgs e)
		{
			if (spnMin.Value >= spnMax.Value)
			{
				MessageBox.Show("validation failed");
				e.Cancel = true;
			}

		}

		private void txtMinlum_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				double lum = 0;
				if (double.TryParse(txtMinlum.Text, out lum) == true)
				{
					if (lum < _Maxlum)
					{
						_Minlum = lum;
						SetCustomMin(_Minlum);
						CalcGammaTable();
						PlotGamma();
						pltRawData.Refresh();
					}
				}
				else
				{
					MessageBox.Show("Invalid value specified");
					return;
				}
			}


		}

		private void txtMaxlum_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				double lum = 0;
				if (double.TryParse(txtMaxlum.Text, out lum) == true)
				{
					if (lum > _Minlum)
					{
						_Maxlum = lum;
						SetCustomMax(_Maxlum);
						CalcGammaTable();
						PlotGamma();
						pltRawData.Refresh();
					}
				}
				else
				{
					MessageBox.Show("Invalid value specified");
					return;
				}
			}


		}


		private void btnTimings_Click(object sender, EventArgs e)
		{
			frmTiming timing = new frmTiming();
			timing.PlotTimingData(_FinalReadings);
			timing.ShowDialog();
			timing.Close();
			timing.Dispose();

		}

        private void chkForceMax_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForceMax.Checked==true)
				SetDefaultMaxMin();
    
			CalcGammaTable();
            PlotGamma();
            pltRawData.Refresh();

        }

        private void chkForceMin_CheckedChanged(object sender, EventArgs e)
        {
			if (chkForceMin.Checked==true)
				SetDefaultMaxMin();
            
			CalcGammaTable();
            PlotGamma();
            pltRawData.Refresh();

        }

		private void chkUseExtended_CheckedChanged(object sender, EventArgs e)
		{
			CalcGammaTable();
			PlotGamma();
			pltRawData.Refresh();

		}

        private void btnSave_Norm_Click(object sender, EventArgs e)
        {
            // open raw data file
            dlgSaveGamma.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlgSaveGamma.CheckPathExists = true;
            dlgSaveGamma.DefaultExt = "gamma.csv";
            dlgSaveGamma.Filter = "Normalised gamma lookup table file (*.gamma.csv)|*.gamma.csv";
            dlgSaveGamma.RestoreDirectory = true;
            dlgSaveGamma.Title = "Save Normalised Gamma Table";
            dlgSaveGamma.ValidateNames = true;
            dlgSaveGamma.OverwritePrompt = true;

            if (dlgSaveGamma.ShowDialog() == DialogResult.OK)
            {
                // write out values as CSV 
                using (TextWriter tw = File.CreateText(dlgSaveGamma.FileName))
                {

                    for (int i = 0; i < NormR.Count; i++)
                    {

                        tw.WriteLine(string.Format("{0},{0},{0}", NormR[i], NormG[i], NormB[i]));
                    }
                    tw.Flush();
                    tw.Close();
                }
            }
        }
	}	
}