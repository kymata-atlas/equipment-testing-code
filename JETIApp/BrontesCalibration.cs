using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.VisaNS;
using System.Windows.Forms;
using System.Diagnostics;


namespace JETIApp
{
	public class BrontesCalibration : CalibrationCore
	{

		private string BrontesID="";

		private MessageBasedSession Session;

		public BrontesCalibration(uint scrwidth, uint scrheight)
			: base(scrwidth, scrheight)
		{
		}

		public override bool CloseDevice()
		{
			if (Session != null)
				Session.Dispose();

			return true;
		}

		public override bool Start(ref string result)
		{

		
			if (base.Start(ref result) == false)
				return false;

			if(BrontesID=="")
			{
				if (ConfigDevice()==false)
					return false;
			}
			//try
			{

				Session = (MessageBasedSession)ResourceManager.GetLocalManager().Open(BrontesID);
				Session.Timeout = 10000;
				Session.Write(":*RST"); // reset command
				Session.Write(":*CLS"); // clear the system status
				Session.Write(string.Format(":SENSE:GAIN {0}",(int)Config.Gain)); // set maximum gain
				Session.Write(string.Format(":SENSE:AVERAGE {0}",Config.Samples)); // set samples to average over
				Session.Write(string.Format(":SENSE:INT {0}",Config.IntegrationTime));
				Session.Write(":SENSE:SBW small"); // set calibration matrix
				
			}
			//catch (VisaException ex)
			//{
			//	result = ex.Message;
			//	return false;
			//}
			InitOutput();
			Abort = false;
			return true;
		}

		public override bool Stop()
		{
			CloseDevice();
			return base.Stop();
		}

		public override bool TakeReading(ref string result, out long time, bool ignore)
		{
			time=0;
			string output;

			if (Session == null) {
				// Run start
				if (Start(ref result) == false) {
					return false;
				}
			}

			Stopwatch sw=new Stopwatch();
			sw.Start();

			try
			{
				output = Session.Query(":MEAS:XYZ");
			}
			catch (VisaException ex)
			{
				result = ex.Message;
				return false;
			}

			sw.Stop();
			time = sw.ElapsedMilliseconds;

            // wikipedia says that XYZ is designed so that Y is a measure of brightness or luminance
            // I think the following comment is wrong, and it's actually giving X,Y,Z,clip,noise, from which we want Y ([1])
			// result is Y,x,y and a measurement of clipping and noise
			string[] values = output.Split(new Char[] { ',' }, 5);
			double lum = double.Parse(values[1]);

			Reading r = new Reading(GrayValues[Index].R, GrayValues[Index].G, GrayValues[Index].B, lum, time, GrayValues[Index].index);

			return WriteReading(r);


		}

		public override bool ConfigDevice()
		{
			// firstly find the device
			string[] Resources;
			// get the local resource manager
			try
			{
				ResourceManager rm = ResourceManager.GetLocalManager();

				// get USB attached resources
				// 0x1781 is the USB Vendor ID for Admesy who manufacture the Brontes
				// 0x0E98 is the model number for Brontes LL instrument
                Resources = rm.FindResources("/USB[0-9]::0x1781::0x0E98::?*INSTR");
				if (Resources.Length == 0)
				{
					MessageBox.Show("Brontes Error: unable to find Brontes device on USB port", "Brontes Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Brontes Error (VISA): " + ex.Message, "Brontes Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			BrontesID = Resources[0];

			return true;

		}


	}
}
