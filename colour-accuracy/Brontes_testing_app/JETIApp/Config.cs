using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace JETIApp
{
    public partial class frmConfig : Form
    {
		private CalibConfig _Config;
		private CalibrationCore _Core;

        //private MessageBoxManager MsgBoxManager = new MessageBoxManager();
        public frmConfig()
        {
            InitializeComponent();
            /*
			MsgBoxManager.AutoClose = false;
			MsgBoxManager.AutoCloseResult = System.Windows.Forms.DialogResult.None;
			MsgBoxManager.CenterWindow = true;
			MsgBoxManager.DisableButtons = false;
			MsgBoxManager.DisableCancel = false;
			MsgBoxManager.HookEnabled = true;
			MsgBoxManager.LastCheckState = false;
			MsgBoxManager.ShowNextTimeCheck = false;
			MsgBoxManager.ShowTitleCountDown = false;
			MsgBoxManager.TimeOut = 0;
            */

			_Config = new CalibConfig((uint)Screen.AllScreens[0].Bounds.Width,(uint) Screen.AllScreens[0].Bounds.Height);
            this.propGrid.SelectedObject = _Config;
			_Config.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged);

			CheckConfig();
			
        }

		private void CheckConfig()
		{
			if (_Config.TargetDevice == TargetDeviceEnum.CRS_Spectrocal)
			{
				btnToggleLaser.Visible = true;
				btnToggleLaser.Enabled = true;
			}
			else
			{
				btnToggleLaser.Visible = false;
				btnToggleLaser.Enabled = false;
			}
		}

		public void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			
			propGrid.Refresh();

			// update display for device specific stuff
			if (e.PropertyName == "TargetDevice")
			{
				CheckConfig();
			}
				
		}

		private void SetGammaTable()
		{
			if (_Config.GammaFile != "")
			{
				bool valid = true;
				try
				{
					if (!File.Exists(_Config.GammaFile))
						valid = false;
				}
				catch
				{
					valid = false;
				}

				if (valid == false)
				{
					MessageBox.Show("Gamma table file is not valid, gamma table not set", "Gamma table not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					// load gamma lookup table

					List<StereolabFX.Monitor> Monitors;
					Monitors = StereolabFX.DisplaySetupFunctions.GetMonitors();
					foreach (StereolabFX.Monitor m in Monitors)
					{
                        if (_Config.ScreenNumbers.Contains((uint)m.Number))
						{
							StereolabFX.DisplaySetupFunctions.LOAD_GAMMA(_Config.GammaFile, m);
							break;
						}
					}
				}
			}
		}

        private void btnOK_Click(object sender, EventArgs e)
        {
            string result = "";

            if (_Config.TargetDevice==TargetDeviceEnum.Not_Set)
			{
				MessageBox.Show("Please select a target device first","Gamma Calibration",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			if (_Config.WriteOutput == true)
			{
				if (_Config.GetOutputFile() == "")
				{
					MessageBox.Show("No output file set");
					return;
				}

				if (!Directory.Exists(Path.GetPathRoot(_Config.GetOutputFile())))
				{
					MessageBox.Show("Invalid path for output file");
					return;
				}

				if (File.Exists(_Config.GetOutputFile()))
				{

					if (MessageBox.Show("File " + Path.GetFileName(_Config.GetOutputFile()) + " already exists.\nChoosing OK will overwrite the file", "Overwrite file", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == DialogResult.Cancel)
						return;
				}
			}
            
            if (_Config.EmailWhenComplete == true)
            {
                if (string.IsNullOrEmpty(_Config.GetSMTPPassword()))
                {
                    if (MessageBox.Show("SMTP Server Password is blank - is this correct", "Gamma Calibration", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                        return;
                }
            }

            switch (_Config.TargetDevice)
			{
				case TargetDeviceEnum.CRS_Spectrocal:
					_Core = new CRSCalibration(_Config.DisplayWidth, _Config.DisplayHeight);
					break;
				case TargetDeviceEnum.Minolta_LS110:
					_Core = new LSCalibration(_Config.DisplayWidth, _Config.DisplayHeight);
					break;
				case TargetDeviceEnum.Software_2_2: // "software" calibrator - generates curve with ideal gamma of 2.2
					_Core = new SWCalibration(_Config.DisplayWidth, _Config.DisplayHeight);
					break;
				case TargetDeviceEnum.Brontes:
					_Core = new BrontesCalibration(_Config.DisplayWidth, _Config.DisplayHeight);
					break;
				//case TargetDeviceEnum.BrontesLibUSB:
//					_Core = new BrontesCalibrationLibUSB(_Config.DisplayWidth, _Config.DisplayHeight);
//					break;

			}


            bool ret;
			_Core.Config = _Config;

			if (_Core.ConfigDevice() == false)
			{
				return;
			}

			int timeout = 0;
			if (Power.MonitorTimeout(ref result, ref timeout) == false)
			{
				MessageBox.Show(result, "Monitor timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			result = "";  // reset result so errors here don't get propagated into core code

			

			_Core.Config = _Config;

			if (_Core.Start(ref result) == false)
			{
				MessageBox.Show("Gamma Calibration Error: " + result, "Gamma Calibration Error");
				return;
			}


            frmDisplay d = new frmDisplay();
			d.Core = _Core;

			SetGammaTable();

			SetupDisplay(d);
            d.Show();
            d.Text = "Press space to start or escape to abort";
			 if (MessageBox.Show(this, "Press OK to start or cancel to abort (to abort mid measurement press escape).", "Start measurement", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == DialogResult.Cancel)
			{
				ResetGammaTable();
				d.Close();
				d.Dispose();
				Cursor.Show();
			}
			else
			{
				ret = d.TakeReadings(this);
			}

        }

		private void ResetGammaTable()
		{
			StereolabFX.DisplaySetupFunctions.SET_DEFAULT_GAMMA_ALL();
		}


        public void ShowMeAgain(frmDisplay d)
        {
			ResetGammaTable();
            d.Close();
            d.Dispose();
            this.Show();
			this.Focus();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
			ResetGammaTable();
			Application.Exit();
        }

		private void SetupDisplay(frmDisplay d)
		{

			d.WindowState = FormWindowState.Normal;
			d.StartPosition = FormStartPosition.Manual;
			if (chkHideConfig.Checked)
				this.Hide();
            d.Init(_Config, this);

            d.Show();
            // determine the size of the window to create
            Screen s=null;
            if (_Config.ScreenNumbers.Count == 1) // only one screen
            {
                s = Screen.AllScreens[_Config.ScreenNumbers[0]];
                d.Top = s.WorkingArea.Top + (s.WorkingArea.Height / 2);
                d.Left = s.WorkingArea.Left + (s.WorkingArea.Width / 2);
                d.TopMost = true;
                //d.WindowState = FormWindowState.Maximized;

                d.Top = s.Bounds.Top;
                d.Height = s.Bounds.Height;
                d.Width = s.Bounds.Width;
                d.Left = s.Bounds.Left;

                //if (_Config.SpanScreens == true)
                //{
                //    if (_Config.SpanSide == SpanSideEnum.Right)
                //        d.Left = s.Bounds.Left + (s.Bounds.Width / 2) + 10;
                //    else
                //        d.Left = s.Bounds.Left;

                //    d.Width = 1000;
                //}
                //else
                //{
                //    d.Left = s.Bounds.Left;
                //    d.Width = s.Bounds.Width;

                //}
            }
            else
            {
                // determine size of window spanning multiple screens
                var screens = (from uint n in _Config.ScreenNumbers select Screen.AllScreens[n]).ToList();

                int Width = 0;
                int Top = (from Screen scr in screens select scr.Bounds.Top).Min();
                int Left = (from Screen scr in screens select scr.Bounds.Left).Min();
                int Height = (from Screen scr in screens select scr.Bounds.Height).Max();

                foreach (Screen scr in screens)
                {
                    Width += scr.Bounds.Width;
                }
                d.Top = Top;
                d.Left = Left;
                d.Width = Width-Left;
                d.Height = Height - Top;
            }

          //  return;
			
			
            d.Show();

		}

        private void btnTest_Click(object sender, EventArgs e)
        {
			
			string result = "";
			if (_Config.TargetDevice == TargetDeviceEnum.CRS_Spectrocal)
			{
				CRSCalibration.SetLaser(true, ref result);
			}
			SetGammaTable();
			frmDisplay d = new frmDisplay();
			Application.DoEvents();
			SetupDisplay(d);
            d.Abort += delegate { d.Visible = false; };
			d.Test();
            while (d.Visible == true)
            {
                Application.DoEvents();
            }
			//MessageBox.Show(this,"Press OK to close test display.", "Test display", MessageBoxButtons.OK, MessageBoxIcon.Information);
			d.Close();
			d.Dispose();
			ResetGammaTable();
			this.Show();

			if (_Config.TargetDevice == TargetDeviceEnum.CRS_Spectrocal)
			{
				CRSCalibration.SetLaser(false, ref result);
			}

        }

       

        private void btnToggleLaser_Click(object sender, EventArgs e)
        {
			string result = "";

			if (CRSCalibration.ToggleLaser(ref result) == true)
				btnToggleLaser.Text = result;
                    
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            frmSelectScreen s = new frmSelectScreen(_Config.ScreenNumbers,_Config.SpanScreens,_Config.SpanSide,_Config.GammaFile);
            DialogResult ret;
			s.ScreenNumbers = _Config.ScreenNumbers;
            ret = s.ShowDialog();
			if (ret == DialogResult.OK)
			{
				_Config.ScreenNumbers = s.ScreenNumbers;
				_Config.SpanScreens = s.SpanScreens;
				_Config.SpanSide = s.SpanSide;
				_Config.GammaFile = s.GammaFile;

                if (s.ScreenNumbers.Count <= 1)
                {
                    _Config.DisplayWidth = ((uint)Screen.AllScreens[s.ScreenNumbers[0]].Bounds.Width);
                    _Config.DisplayHeight = ((uint)Screen.AllScreens[s.ScreenNumbers[0]].Bounds.Height);
                }
                else
                {
                    // determine size of window spanning multiple screens
                    var screens = (from uint n in _Config.ScreenNumbers select Screen.AllScreens[n]).ToList();

                    int Width=0;
                    int Top = (from Screen scr in screens select scr.Bounds.Top).Min();
                    int Left = (from Screen scr in screens select scr.Bounds.Left).Min();
                    int Height=(from Screen scr in screens select scr.Bounds.Height).Max();
 
                    foreach (Screen scr in screens)
                    {
                        Width+=scr.Bounds.Width;
                    }
                    _Config.DisplayWidth = (uint)(Width-Left);
                    _Config.DisplayHeight =(uint)( Height-Top);
                }
				propGrid.Refresh();
			}
			s.Close();
			s.Dispose();
			this.Focus();
        }

		private void btnAnalysis_Click(object sender, EventArgs e)
		{
			frmAnalysis analysis = new frmAnalysis();
			analysis.ShowDialog();
			analysis.Close();
			analysis.Dispose();
			this.Focus();

		}

		/*private void btnDebug_Click(object sender, EventArgs e)
		{
			CalibrationCore cc=new CalibrationCore(0,0); // just generated dummy information so screen dimensions don't really matter here
			cc.Debug();
			MessageBox.Show("Check output");
		}
		*/

		private void btnLoad_Click(object sender, EventArgs e)
		{
			// open raw data file
            dlgLoadConfig.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			dlgLoadConfig.CheckFileExists = true;
			dlgLoadConfig.DefaultExt = "gcfg";
			dlgLoadConfig.Filter = "Gamma calibration config Files(*.cfg,*.gcfg)|*.gcfg;*.cfg";
			dlgLoadConfig.FilterIndex = 1;
			dlgLoadConfig.RestoreDirectory = true;
			dlgLoadConfig.Title = "Load Gamma calibration configuration file";
			dlgLoadConfig.ValidateNames = true;

			DialogResult dlg = dlgLoadConfig.ShowDialog();
			if (dlg == DialogResult.OK)
			{

				CalibConfig c = new CalibConfig(0, 0);
				if (CalibConfig.Load(dlgLoadConfig.FileName, ref c) == true)
				{
					_Config = c;
					this.propGrid.SelectedObject = _Config;
					this.propGrid.Refresh();
					CheckConfig();
					MessageBox.Show("Configuration loaded","Gamma Calibration",MessageBoxButtons.OK,MessageBoxIcon.Information);
				}
			}
				
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
            dlgSaveConfig.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			dlgSaveConfig.CheckPathExists = true;
			dlgSaveConfig.DefaultExt = "gcfg";
			dlgSaveConfig.Filter = "Gamma calibration config files (*.gcfg)|*.gcfg";
			dlgSaveConfig.FilterIndex = 1;
			dlgSaveConfig.RestoreDirectory = true;
			dlgSaveConfig.Title = "Save Config";
			dlgSaveConfig.ValidateNames = true;
			dlgSaveConfig.OverwritePrompt = true;

			if (dlgSaveConfig.ShowDialog() == DialogResult.OK)
			{
				if (CalibConfig.Save(dlgSaveConfig.FileName, _Config) == true)
					MessageBox.Show("Configuration saved", "Gamma Calibration", MessageBoxButtons.OK, MessageBoxIcon.Information);


			}
		}
    }
}