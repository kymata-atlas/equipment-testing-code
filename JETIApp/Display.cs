using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;
using System.Net;
using JETILib;
using StereolabFX;
using Tao.OpenGl;
using System.Diagnostics;

namespace JETIApp
{
    public partial class frmDisplay : Form
    {
        private CalibrationCore _Core;
        private bool InitGl = true;
        private DisplayParams disp;
		private GrayItem grayitem;
        bool DoPatch=false;
		private bool abort;

		private DateTime _EstFinishTime; // estimated time when readings will finish
		private TimeSpan _TimeLeft;

		private frmConfig ConfigForm;

		public delegate void AbortDelegate(object sender, EventArgs e);

		public event AbortDelegate Abort;

		public CalibrationCore Core
		{
			get
			{
				return _Core;
			}
			set
			{
				_Core = value;
				this.Abort += new AbortDelegate(_Core.AbortMeasurement);

			}
		}
		
		public virtual void OnAbort(object sender, EventArgs e)
		{
			if (Abort != null)
				Abort(sender, e);
		}

        public frmDisplay()
        {
            InitializeComponent();

            var glControl = new Tao.Platform.Windows.SimpleOpenGlControl();
            
            disp = new DisplayParams();
            disp.Width = glControl.Width;
            disp.Height = glControl.Height;
            disp.Pix_Width = glControl.Width;
            disp.Pix_Height = glControl.Height;
            disp.Distance = 1.0f;
            glControl.InitializeLifetimeService();
            //glControl.Draw();
            this.KeyPreview = true;
			abort = false;
			grayitem = new GrayItem(127,127,127);
        }


        public void Init(CalibConfig Config,frmConfig ConfigForm)
        {
            //this._Core = core;
			InitGl = true;
            PopulateFromConfig(Config);
			this.ConfigForm = ConfigForm;
			abort = false;
        }

        public void Test()
        {
            this.Text = "Testing patch position, press escape to continue";
			DoPatch = true;
			grayitem.R = 127;
			grayitem.G = 127;
			grayitem.B = 127;

            glControl.Draw();
        /*    ret = _Core.FindSpectroCal(ref result);
            if (ret == false)
                MessageBox.Show(" SpectroCAL error: " + result);
            else
                MessageBox.Show("SpectroCal detected successfully");
		*/
        }

        private void PopulateFromConfig(CalibConfig Config)
        {
            // set position of the GL control
            int Y = (int)(Config.DisplayHeight - Config.PatchHeight) / 2;
            int X = (int)(Config.DisplayWidth - Config.PatchWidth) / 2;

            if (Config.SpanScreens == true)
            {
                if (Config.SpanSide == SpanSideEnum.Full)
                    X = (int)(Config.DisplayWidth - Config.PatchWidth) / 2;
                else if (Config.SpanSide == SpanSideEnum.Left)
                {
                    uint DisplayWidth = Config.DisplayWidth / 2;
                    X = (int)(DisplayWidth - Config.PatchWidth) / 2;
                }
                else if (Config.SpanSide == SpanSideEnum.Right)
                {
                    uint DisplayWidth = Config.DisplayWidth / 2;
                    X = (int)(DisplayWidth - Config.PatchWidth) / 2;
                    X = (int)(X + DisplayWidth); // shift to the right hand side of the screen
                }

            }
            glControl.Location = new Point(X, Y);
            glControl.Width = (int)Config.PatchWidth;
            glControl.Height = (int)Config.PatchHeight;

        }

        private void SetStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Status: ");
            if (DoPatch ==true)
            {
                sb.AppendFormat("Graylevel: {0:D} {1:D} {2:D}",grayitem.R,grayitem.B,grayitem.G);
            }
            sb.Append(" Total Readings: " + _Core.Config.TotalReadings.ToString());
            sb.Append(" Readings Done: " + _Core.Config.TotalReadingsDone.ToString());
            sb.Append(" Readings To Go: " + _Core.Config.ReadingsToGo.ToString());
			if (ConfigForm != null)
			{
				if (ConfigForm.Visible == true)
				{
					ConfigForm.Text = sb.ToString();
				}
			}

			if (_Core.Config.RandomizeRepeats == false)
			{
				if (_Core.Config.ReadingsPerLevel > 1)
				{
					sb.Append(" Readings left for gray level: " + ((int)(_Core.Config.ReadingsPerLevel - _Core.Config.ReadingsDonePerLevel)).ToString());
				}
			}

			if (_EstFinishTime.Year==DateTime.Now.Year)
			{
				sb.Append(" Time now : " + DateTime.Now.ToString());
				sb.Append(" Estimated finish: " + _EstFinishTime.ToString());
				sb.Append(" Time remaining: " + _TimeLeft.Hours.ToString() + " hrs " + _TimeLeft.Minutes.ToString() + " mins " + _TimeLeft.Seconds.ToString() +" secs ");
			}

            this.Text = sb.ToString();
			
        }

        private void DrawPatch()
        {
//			Debug.WriteLine("Drawpatch:");
			if (DoPatch)
			{
				int border = 2;

                var glControl = new Tao.Platform.Windows.SimpleOpenGlControl();
                
				Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
				Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_ACCUM_BUFFER_BIT);
				Gl.glMatrixMode(Gl.GL_MODELVIEW);
				Gl.glDisable(Gl.GL_DEPTH_TEST);
				Gl.glDisable(Gl.GL_POINT_SMOOTH);
				Gl.glPushMatrix();
				Gl.glLoadIdentity();
				Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
				Gl.glLineWidth(1.0f);
				StereolabFX.DisplaySetupFunctions.DO_ORTHO_PIXEL(disp, 0, 0, StereolabFX.DisplaySetupFunctions.FlipAxisEnum.NORMAL, StereolabFX.DisplaySetupFunctions.FlipAxisEnum.NORMAL);
				Gl.glViewport(0, 0, disp.Pix_Width, disp.Pix_Height);
				Gl.glColor3ub((byte)grayitem.R, (byte)grayitem.G, (byte)grayitem.B); //unsigned byte corresponds to 8-bit RGB values
				Gl.glRecti(-disp.Pix_Width / 2 + border, -disp.Pix_Height / 2 + border, disp.Pix_Width / 2 - border, disp.Pix_Height / 2 - border);
				Gl.glPopMatrix();

                glControl.SwapBuffers();
			}
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
			//Debug.WriteLine("Painting gl control");
            if (InitGl == true)
            {
                InitGl = false;

                var glControl = new Tao.Platform.Windows.SimpleOpenGlControl();
                
                disp.Width = glControl.Width;
                disp.Height = glControl.Height;
                disp.Pix_Width = glControl.Width;
                disp.Pix_Height = glControl.Height;

                DisplaySetupFunctions.SetStandardGLConfig(disp, true);
                Gl.glDisable(Gl.GL_POLYGON_SMOOTH); // required when drawing in 2D
            }
            DrawPatch();
        }

        public bool TakeReadings(frmConfig c)
        {

            var glControl = new Tao.Platform.Windows.SimpleOpenGlControl();

            bool ScreenSaver=true;
            bool ret;
			DoPatch = true;
			grayitem.R = 127;
			grayitem.B = 127;
			grayitem.G = 127;
			this.Focus();
            glControl.Draw();
            string result = "";

			ret = StereolabFX.DisplaySetupFunctions.GetScreenSaverActive(ref ScreenSaver);
			ret = StereolabFX.DisplaySetupFunctions.SetScreenSaverInActive();

            _Core.Config.TotalReadingsDone = 0; // reset number readings
			Cursor.Position = new Point(-1,-1);
			Cursor.Hide();
			this.Focus();

			System.Media.SystemSounds.Exclamation.Play();
			Application.DoEvents();
            
			Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < _Core.Config.StartDelay)
            {
				this.Text = "Waiting...";
            }
            sw.Stop();

			Application.DoEvents();
			
			bool start = true;
			long ReadingTime=0;
			long TotalTime = 0;
			double MeanTime = 0;
			double EstTotalTime = 0;
			double TimeLeft = 0;

			uint ConsecutiveReadingsToTake;

			if (_Core.Config.RandomizeRepeats == true)
				ConsecutiveReadingsToTake = 1;
			else
				ConsecutiveReadingsToTake = _Core.Config.ReadingsPerLevel;


			DateTime StartTime=DateTime.Now;
			SetStatus();
			try
			{

				while (_Core.Config.ReadingsToGo > 0 && abort == false)
				{
					if (_Core.Config.ReadingsDonePerLevel >= ConsecutiveReadingsToTake || start == true)
					{
						Debug.WriteLine("Getting next gray level");
						grayitem=_Core.GetGrayLevel();
						_Core.Config.ReadingsDonePerLevel = 0;
						glControl.Refresh();

						sw.Reset();
						sw.Start();
						while (sw.ElapsedMilliseconds < _Core.Config.DelayBeforeMeasurement)
						{
							Application.DoEvents();
						}
						sw.Stop();
					}

					if (start == true)
						start = false;

					SetStatus();
					ReadingTime=0;
					if (_Core.TakeReading(ref result,out ReadingTime,false) == false) // take reading
					{
						Cursor.Show();
						MessageBox.Show("Gamma calibration error: " + result, "Gamma calibration error",MessageBoxButtons.OK,MessageBoxIcon.Error);
						if (abort==false)
							throw new Exception(); // throw exception unless we've already aborted
					}
					else
					{
						_Core.Config.TotalReadingsDone++;
						_Core.Config.ReadingsDonePerLevel++;
						if (_Core.Config.ReadingsDonePerLevel >= ConsecutiveReadingsToTake)
						{
							_Core.GrayLevelIndex++;
						}
						Debug.WriteLine("Time for reading " + ReadingTime.ToString());


						// update time estimates
						TotalTime += ReadingTime; // total time taken so far in milliseconds;
						Debug.WriteLine("Total time so far " + TotalTime.ToString());


						MeanTime = TotalTime / _Core.Config.TotalReadingsDone; // mean amount of time taken over all readings done so far
						Debug.WriteLine("Mean time per reading " + MeanTime.ToString());

						TimeLeft = MeanTime * (_Core.Config.TotalReadings - _Core.Config.TotalReadingsDone);

						EstTotalTime = MeanTime * _Core.Config.TotalReadings; // estimate of how long it'll take to finish based on mean time in milliseconds

						Debug.WriteLine("Estimated total time based on mean " + EstTotalTime.ToString());

						_EstFinishTime = DateTime.Now.AddMilliseconds(TimeLeft);
                        _TimeLeft = _EstFinishTime - DateTime.Now;

						Debug.WriteLine("Estimated finish time " + _EstFinishTime.ToString());

						
						Debug.WriteLine("Time left " + _TimeLeft.ToString());


					}

				}
			}
			catch (Exception e)
			{
				abort=true;
				OnAbort(this, new EventArgs());
				if (e.Message != "")
//					MessageBox.Show("Gamma Calibration Error: " + e.Message, "Gamma Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				result=e.Message;

			}
			finally
			{
				_Core.Stop();
                SetStatus();
				Cursor.Show();
				timer.Enabled = true;

				

				if (_Core.Config.EmailWhenComplete == true)
				{
					MailMessage mail = new MailMessage(new MailAddress("gamma_results@bham.ac.uk","Gamma Calibration Notification"),new MailAddress(_Core.Config.EmailTo));
					mail.Subject = "Gamma Calibration complete";
						
					mail.Body = "Gamma calibration is now complete";
					if (result != "")
					{
						mail.Subject += " (with errors)";
						mail.Body += "\r\n" + result;
					}

					SmtpClient sc = new SmtpClient(_Core.Config.SMTPServer);
                    sc.Port = _Core.Config.SMTPPort;
                    if (!string.IsNullOrEmpty(_Core.Config.SMTPUser))
                    {
                        NetworkCredential Creds = new NetworkCredential(_Core.Config.SMTPUser, _Core.Config.GetSMTPPassword());
                        sc.Credentials = Creds;
                    }
                    sc.Timeout = 5; // timeout if not completed in 5 seconds
                    if (_Core.Config.SMTPServer=="auth-smtp.bham.ac.uk")
                        sc.EnableSsl=true;

					sc.Send(mail);

					if (result != "")
						MessageBox.Show("Gamma Calibration Error: " + result, "Gamma Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				}

                if (abort == true)
                    MessageBox.Show("Calibration aborted, press OK to continue", "Gamma Calibration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Press OK to finish!", "Gamma Calibration", MessageBoxButtons.OK, MessageBoxIcon.Information);

				timer.Enabled = false;
				this.Hide();
				if (ScreenSaver)
					ret = StereolabFX.DisplaySetupFunctions.SetScreenSaverActive();
				c.ShowMeAgain(this);
			}

			if (abort == true)
				return false;
			else
				return true;

        }

		private void timer_Tick(object sender, EventArgs e)
		{
			System.Media.SystemSounds.Exclamation.Play();
		}

		private void frmDisplay_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Escape) // abort
			{
				Debug.WriteLine("Escape!");
				abort = true;
				OnAbort(this, new EventArgs());
			}

		}

		private void frmDisplay_FormClosing(object sender, FormClosingEventArgs e)
		{
			Cursor.Show();
			if (_Core != null)
			{
				_Core.CloseDevice();
			}
		}


    }
}

        
       
       