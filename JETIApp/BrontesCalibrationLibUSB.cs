using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using LibUsbDotNet;
using LibUsbDotNet.Main;


namespace JETIApp
{
    public class BrontesCalibrationLibUSB : CalibrationCore
	{
		public UsbDevice BrontesDevice;
		public static UsbDeviceFinder BrontesFinder = new UsbDeviceFinder(0x1781, 0x0E98);

		UsbEndpointReader BrontesReader; 
		UsbEndpointWriter BrontesWriter; 


		public BrontesCalibrationLibUSB(uint scrwidth, uint scrheight)
			: base(scrwidth, scrheight)
		{
		}

		public override bool CloseDevice()
		{
			BrontesReader.Abort();
			BrontesReader.Dispose();

			if (BrontesDevice != null)
			{
				if (BrontesDevice.IsOpen)
				{
					IUsbDevice UsbDevice = BrontesDevice as IUsbDevice;
					if (!ReferenceEquals(UsbDevice, null))
					{
						UsbDevice.ReleaseInterface(0);
					}
				}
				BrontesDevice = null;
			}
			BrontesDevice.Close();
			BrontesDevice = null;
			return true;
				
		}

		private string Execute(string Cmd)
		{
			int length = 0;
			ErrorCode ec=BrontesWriter.Write(Encoding.Default.GetBytes(Cmd), 5000, out length);
			if (ec != ErrorCode.None)
			{
				throw new Exception(UsbDevice.LastErrorString);
			}

			byte[] Buffer=new byte[1024];
			int BytesRead = 0;
			while (ec == ErrorCode.None)
			{
				ec = BrontesReader.Read(Buffer, 10000, out BytesRead);
			}

			if (ec != ErrorCode.Ok)
				throw new Exception(UsbDevice.LastErrorString);

			return Encoding.Default.GetString(Buffer, 0, BytesRead);
		}

		public override bool Start(ref string result)
		{


			if (base.Start(ref result) == false)
				return false;

			try
			{

				Execute(":*RST");
				Execute(":*CLS"); // clear the system status
				Execute(":SENSE:GAIN 1"); // set maximum gain
				Execute(":SENSE:AVERAGE 500"); // set samples to average over
			}
			catch (Exception ex)
			{
				Abort = true;
				result = ex.Message;
				return false;
			}
				
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
			string data;

			Stopwatch sw=new Stopwatch();
			sw.Start();
			try
			{
				data=Execute(":MEASURE:Lab");
			}
			catch (Exception ex)
			{
				Abort=true;
				result=ex.Message;
				return false;
			}
			sw.Stop();

			time = sw.ElapsedMilliseconds;

			// result is Y,x,y and a measurement of clipping and noise
			string[] values = data.Split(new Char[] { ',' }, 5);
			double lum = double.Parse(values[0]);

			Reading r = new Reading(GrayValues[Index].R, GrayValues[Index].G, GrayValues[Index].B, lum, time, GrayValues[Index].index);

			return WriteReading(r);


		}

		public override bool ConfigDevice()
		{

			BrontesDevice = LibUsbDotNet.UsbDevice.OpenUsbDevice(BrontesCalibrationLibUSB.BrontesFinder);

			if (BrontesDevice==null)
			{
				MessageBox.Show("Brontes Error: unable to find Brontes device on USB port", "Brontes Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			IUsbDevice UsbDevice = BrontesDevice as IUsbDevice;

			if (!ReferenceEquals(UsbDevice, null))
			{
				UsbDevice.SetConfiguration(1);
				UsbDevice.ClaimInterface(0);

				UsbEndpointReader BrontesReader = UsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
				UsbEndpointWriter BrontesWriter = UsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);

			}

			return true;

		}


	}
}
