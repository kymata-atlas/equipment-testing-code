using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using JETILib;

namespace JETIApp
{
	class CRSCalibration : CalibrationCore
	{
		private static int _Device = -1;

		private static bool _Laser;

		public CRSCalibration(uint scrwidth, uint scrheight)
			: base(scrwidth, scrheight)
		{
		}

		public override bool Stop()
		{
			CloseDevice();
			return base.Stop();
		}

		public override bool CloseDevice()
		{
			int ret;
			ret = JETILib.JETIRadio.JETI_MeasureBreak(_Device);
			ret = JETILib.JETIRadio.JETI_CloseRadio(_Device);
			return true;

		}

		public override bool TakeReading(ref string result, out long time,bool ignore)
		{
			time = 0;
			if (_Device == -1)
			{
				result = "SpectroCAL not initialised";
				return false;
			}

			else
			{
				int ret;
				float lum = 0.0f;
				Stopwatch sw = new Stopwatch();
				try
				{
					CloseDevice();
					ret = JETILib.JETIRadio.JETI_OpenRadio(0, ref _Device);
					if (EvalJETIResult(ret, ref result) == false)
					{
						throw new JETIException();
					}
					sw.Start();
					ret = JETILib.JETIRadio.JETI_Measure(_Device);
					if (EvalJETIResult(ret, ref result) == false)
					{
						throw new JETIException();
					}

					bool busy = true;
					while (busy && Abort == false)
					{
						ret = JETILib.JETIRadio.JETI_MeasureStatus(_Device, ref busy);
						if (EvalJETIResult(ret, ref result) == false)
						{
							ret = JETILib.JETIRadio.JETI_MeasureBreak(_Device);
							throw new JETIException();
						}
						Application.DoEvents();

					}

					if (Abort == true)
					{
						ret = JETILib.JETIRadio.JETI_MeasureBreak(_Device);
						result = "Aborting measurement";
						throw new JETIException();
					}

					ret = JETILib.JETIRadio.JETI_Photo(_Device, ref lum);
					if (EvalJETIResult(ret, ref result) == false)
						throw new JETIException();
					sw.Stop();
					time = sw.ElapsedMilliseconds;

				}
				catch (JETIException)
				{
					sw.Stop();
					time = sw.ElapsedMilliseconds;
					CloseDevice();
					return false;
				}

				CloseDevice();

				Reading r = new Reading(GrayValues[Index].R, GrayValues[Index].G, GrayValues[Index].B, lum, time, GrayValues[Index].index);

				return WriteReading(r);
				
			}

		}

		public static bool SetLaser(bool On, ref string result)
		{
			if (_Device == -1)
			{
				if (FindSpectroCal(ref result) == false)
					return false;

				if (RegisterDevice(ref result) == false)
					return false;
			}

			int ret = JETILib.JETICore.JETI_SetLaserStat(_Device, On);
			if (EvalJETIResult(ret, ref result) == false)
				return false;
			else
				return true;


		}

		public static bool ToggleLaser(ref string result)
		{
			if (_Laser == false)
			{
				if (CRSCalibration.SetLaser(true, ref result) == true)
				{
					result=  "Laser Off";
					_Laser = true;
					return true;
				}
				else
				{
					MessageBox.Show("SpectroCAL error: " + result);
					return false;
				}
			}
			else
			{
				if (CRSCalibration.SetLaser(false, ref result) == true)
				{
					result = "Laser on";
					_Laser = false;
					return true;
				}
				else
				{
					MessageBox.Show("SpectroCAL error: " + result);
					return false;
				}

			}
		}
		public override bool ConfigDevice()
		{
			string result = "";
			if (Config.TargetDevice == TargetDeviceEnum.CRS_Spectrocal)
			{
				if (CRSCalibration.SetLaser(false, ref result) == true)
				{
					_Laser = false;
					result = "Laser On";
				}

				if (CRSCalibration.FindSpectroCal(ref result) == false)
				{
					MessageBox.Show("SpectroCAL Error: " + result, "SpectroCAL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else
				{
					if (CRSCalibration.RegisterDevice(ref result) == false)
					{
						MessageBox.Show("SpectroCAL Error: " + result, "SpectroCAL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
				}

				if (CRSCalibration.SetLaser(false, ref result) == false)
				{
					MessageBox.Show("SpectroCAL Error: " + result, "SpectroCAL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
			return true;
		}
		public static bool RegisterDevice(ref string result)
		{
			// open spectroCAL to get device number

			int ret;

			if (FindSpectroCal(ref result) == false)
				return false;

			if (_Device != -1)
				ret = JETILib.JETIRadio.JETI_CloseRadio(_Device);

			ret = JETILib.JETIRadio.JETI_OpenRadio(0, ref _Device);

			if (EvalJETIResult(ret, ref result) == false)
				return false;
			else
				return true;
		}

		public static bool FindSpectroCal(ref string result)
		{

			if (_Device == -1)
			{
				int _NumRadios = 0;
				int ret = JETILib.JETIRadio.JETI_GetNumRadio(ref _NumRadios);
				if (EvalJETIResult(ret, ref result) == false)
					return false;

				if (_NumRadios != 1)
				{
					result = "No SpectroCAL detected";
					return false;
				}
				else
				{
					result = "Success!";
					return true;
				}

			}
			else
			{
				return true;
			}
		}

		public override bool Start(ref string result)
		{

			if (base.Start(ref result) == false)
				return false;


			// find spectroCAL

			if (FindSpectroCal(ref result) == false)
				return false;

			if (RegisterDevice(ref result) == false)
			{
				// try closing the device and trying again
				if (CloseDevice() == false)
					return false;
				else
					if (RegisterDevice(ref result) == false)
						return false;
			}

			InitOutput();
			Abort = false;
			return true;
		}

		private static bool EvalJETIResult(int JETIResult, ref string result)
		{
			switch (JETIResult)
			{
				case JETICore.JETI_SUCCESS: // No error occurred
					result = "";
					break;
				case JETICore.JETI_TIMEOUT: // timeout error
					result = "Timeout error";
					break;
				case JETICore.JETI_INVALID_HANDLE: // invalid device handle
					result = "Invalid device handle";
					break;
				case JETICore.JETI_INVALID_NUMBER: // invalid device number
					result = " Invalid device number";
					break;
				case JETICore.JETI_INVALID_STEPWIDTH: // invalid step width
					result = "Invalid step width";
					break;
				case JETICore.JETI_NOT_CONNECTED: // device not connected
					result = "Device not connected";
					break;
				case JETICore.JETI_CHECKSUM_ERROR: // invalid checksum on received data
					result = "Invalid checksum on received data";
					break;
				case JETICore.JETI_ERROR_BUFFER_SIZE: // Could not set buffer size for comms
					result = "Could not set buffer size for communications";
					break;
				case JETICore.JETI_ERROR_CONVERT: // could not convert received data
					result = "Could not convert received data";
					break;
				case JETICore.JETI_ERROR_NAK: // command or argument invalid
					result = "Command is not supported or invalid argument specified";
					break;
				case JETICore.JETI_ERROR_OPEN_PORT: // Could not open comms
					result = "Could not open communications with device";
					break;
				case JETICore.JETI_ERROR_PARAMETER: // invalid argument
					result = "Invalid argument specified";
					break;
				case JETICore.JETI_ERROR_PORT_SETTING: // comms invalid
					result = "Communications settings invalid";
					break;
				case JETICore.JETI_ERROR_PURGE:
					result = "Could not purge comms buffers"; // Could not purge comms buffers
					break;
				case JETICore.JETI_ERROR_RECEIVE: // could not receive from device
					result = "Could not receive from device";
					break;
				case JETICore.JETI_ERROR_SEND: // could not send to device
					result = "Could not send to device";
					break;
				case JETICore.JETI_ERROR_TIMEOUT_SETTING: // could not set comms timeout
					result = "Could not set communications timeout";
					break;
				case JETICore.JETI_BUSY: // device busy
					result = "Device busy";
					break;
				default:
					result = "Unknown error : " + JETIResult.ToString();
					break;
			}

			if (JETIResult != JETICore.JETI_SUCCESS)
			{
				return false;
			}
			else
				return true;

		}

	}
}
