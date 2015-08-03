using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace JETIApp
{
	class LSCalibration : CalibrationCore
	{
		private class PortSettingsStruct
		{
			public string portname;
			public Handshake handshake;
			public StopBits stopbits;
			public int databits;
			public Parity parity;
			public int baud;
		}

		private SerialPort _serial;
		private string _data=null; // string which contains data from the serial port

		private PortSettingsStruct portsettings;

		//private void ReceiveData(object sender, SerialDataReceivedEventArgs e)
		//{
			
		//    System.Diagnostics.Debug.WriteLine("In Receive Data");

		//    // this runs in a different thread so we can't access much from the existing thread
		//    if (e.EventType == SerialData.Chars)
		//    {

		//        int ch = 0;
		//        StringBuilder sb = new StringBuilder();
		//        while ((char)ch != '\r')
		//        {
		//            ch = ((SerialPort)sender).ReadChar();
		//            sb.Append((char)ch);
		//        }

		//        _data = sb.ToString();
		//        System.Diagnostics.Debug.WriteLine("Data: " + _data);
		//    }

		
			
		//}

		public override bool Start(ref string result)
		{

			if (base.Start(ref result) == false)
				return false;

			Abort = false;

			InitOutput();

            long time;
            bool ret;
            ret = TakeReading(ref result, out time, true);
			return true;
		}

		public override bool ConfigDevice()
		{
			if (_serial != null) // reset serial port
			{
				_serial.Close();
				_serial.Dispose();
				_serial = null;
			}

			portsettings = null;

			string[] Ports = SerialPort.GetPortNames();
			Stopwatch sw = new Stopwatch();

			// clear down serial ports
			for (int i = 0; i < Ports.Length; i++)
			{
				SerialPort sp = new SerialPort(Ports[i], 4800, Parity.Even, 7, StopBits.Two);
				sp.Open();
				sp.ReadExisting();
				sp.DiscardInBuffer();
				sp.DiscardOutBuffer();
				sp.Close();
			}

			for (int i = 0; i < Ports.Length; i++)
			{
				// try each serial port in turn to see if we have the LS100
				// this also should clear out any reading still in the device which is not relevant
				SerialPort sp = new SerialPort(Ports[i], 4800, Parity.Even, 7, StopBits.Two);
				int ch = 0;
				bool end = false;
				StringBuilder sb = new StringBuilder();
				try
				{
					_data = null;
					sp.Handshake = Handshake.RequestToSend;
					sp.DtrEnable = true;
					sw.Reset();
					sp.Open();
					sp.ReadTimeout = 500;
					sp.ReadExisting();
					sp.DiscardInBuffer();
					sp.DiscardOutBuffer();
					sw.Start();
					sp.DtrEnable = false;
					// give it some time to read from the port
					sb.Remove(0, sb.ToString().Length);
					while (sw.ElapsedMilliseconds < 10000 && end == false)
					{
						if (sp.BytesToRead > 0)
						{
							while ((char)ch != '\r')
							{
								ch = sp.ReadChar();
								sb.Append((char)ch);
							}
							_data = sb.ToString();
							end = true;
						}

						Application.DoEvents();
					}
					sw.Stop();
					sw.Reset();
					if (_data == null)
						continue;
					else
					{
						portsettings = new PortSettingsStruct();
						portsettings.baud = sp.BaudRate;
						portsettings.databits = sp.DataBits;
						portsettings.portname = sp.PortName;
						portsettings.parity = sp.Parity;
						portsettings.stopbits = sp.StopBits;
						portsettings.handshake = sp.Handshake;

						break;
					}


				}
				catch (TimeoutException)
				{
					continue;
				}

				finally
				{
					sp.DtrEnable = true;
					sp.Close();
					sp.Dispose();
					sp = null;
				}
			}
			_data = null;
			if (portsettings == null)
			{
				string result = "LS100 not found or an error occurred searching for device.";
				MessageBox.Show("LS100 Error: " + result, "LS100 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				
				return false;
			}

			_serial = new SerialPort(portsettings.portname, portsettings.baud, portsettings.parity, portsettings.databits, portsettings.stopbits);
			_serial.Handshake = portsettings.handshake;
			_serial.Open();

			// try and flush any dodgy readings out of the device
			_serial.DtrEnable = true;
			_serial.DtrEnable = false;
			_serial.ReadExisting();
			_serial.DtrEnable = true;

			_serial.Close();

			return true;
		}
		
		public override bool CloseDevice()
		{
			if (_serial != null)
			{
				_serial.DtrEnable = true;
				_serial.Close();
				_serial.Dispose();
				_serial = null;
			}
			return true;
		}

		public override bool Stop()
		{
			CloseDevice();
			return base.Stop();
		}

		public override bool TakeReading(ref string result, out long time,bool ignore)
		{
			float lum =-1.0f;
			time = 0;

			if (_serial == null)
			{
				result = "Communications cannot be established with LS100";
				return false;
			}

			Stopwatch sw = new Stopwatch();
			Reading r = null;
			bool valid = true;
			StringBuilder sb = new StringBuilder();
			int ch = 0;
			//int nullcount=0;
			//int maxnullcount = 10;
			int maxretries = 3;
			int retries=0;

            try
			{

				// request a measurement from the LS100
				
				// initialise serial port etc. and make sure no data is still in the buffer
				_data = null;
				_serial.ReadTimeout = 10000;
                //_serial.Close();
				if (_serial.IsOpen==false)
                    _serial.Open();
                //_serial.Open();
				_serial.DtrEnable = true;
				_serial.DtrEnable = false;
                _serial.ReadExisting(); //flush buffer
                _serial.DiscardInBuffer();
                _serial.DiscardOutBuffer();

                sw.Reset();
				sw.Start();
                Stopwatch sw1 = new Stopwatch();
				// wait for initial character
				bool cont=false; // determine if we continue with read
				while (retries < maxretries)
                {
                    sw1.Stop();
                    sw1.Reset();
                    sw1.Start();
				    while (_serial.BytesToRead == 0 && sw.ElapsedMilliseconds < 1000) // wait for characters to come from device
				    {
					    Application.DoEvents();
				    }

				    if (sw1.ElapsedMilliseconds >= 1000 && _serial.BytesToRead==0) // timeout if nothing after given amount of time elapsed
				    {
                        cont=false;
                       // _serial.Close();
                        //_serial.Open();
                        _serial.ReadExisting();
                        _serial.DiscardInBuffer();
                        _serial.DiscardOutBuffer();
                        _serial.DtrEnable=true;
                        _serial.DtrEnable=false;
                        retries++;
                        //sw.Reset();
                       // sw.Start();
                    }
                    else
                    {
                        retries=0;
                        cont = true;
                        break;
                    }
                }
                
				
				if (retries == maxretries && cont==false) // timeout
                {
					    result = "Timeout on waiting for initial character";
					    throw new LSException(result);
				}

                int charcount=0;
				// read the rest of the data
				while (_data == null && Abort == false)
				{
					System.Diagnostics.Debug.WriteLine("Taking reading");

					// wait for data
					if (_serial.BytesToRead > 0)
						System.Diagnostics.Debug.WriteLine("Something in the buffer...");
					else
					{
						while (_serial.BytesToRead == 0)
						{
							Application.DoEvents();
						}
					}
					
					// get data
                    Stopwatch sw2 = new Stopwatch();
                    sw2.Start();
					while ((char)ch != '\r')
					{
						
						retries=0; // re-init retries
                        bool timeout;

						// wait for next char
						while (retries < maxretries)
						{
                            timeout = false;
                            try
							{
								ch = _serial.ReadChar();
							}
							catch  (TimeoutException)
							{
                                Debug.WriteLine("Timeout");
                                //_serial.Close();
								//_serial.Open();
								_serial.ReadExisting();
								_serial.DtrEnable = true;
								_serial.DtrEnable = false;
								_serial.DiscardInBuffer();
								_serial.DiscardOutBuffer();
                                timeout = true;
								retries++;
								if (retries == maxretries)
								{
									result = "Timeout occurred during measurement";
									throw new LSException(result);
								}
                            }
                            if (ch != 0 && timeout==false) // don't get 0 terminator is it prevents us seeing the rest of the string
                            {
                                sb.Append((char)ch);
                                charcount++;
                                //Debug.WriteLine(sb.ToString());
                                break;
                            }

                            Application.DoEvents();
						}

						
						
								//else
								//{
								//    nullcount++;
								//    if (nullcount == maxnullcount) // just seem to be getting nulls, try opening and closing the serial port
								//    {
								//        _serial.Close();
								//        _serial.Open();
								//        _serial.ReadExisting();
								//        _serial.DtrEnable = true;
								//        _serial.DtrEnable = false;
								//        _serial.DiscardInBuffer();
								//        _serial.DiscardOutBuffer();

								//        retries++;
								//        if (retries == maxretries)
								//        {
								//            result = "Timeout occurred during measurement";
								//            throw new LSException(result);
								//        }
								//    }
								//}

						
					}

					_data = sb.ToString();
					System.Diagnostics.Debug.WriteLine("Data : " + _data);
                    sw2.Stop();
                    Debug.WriteLine(sw2.ElapsedMilliseconds);
				}
				
				time = sw.ElapsedMilliseconds; // so we can update GUI
				r = new Reading(GrayValues[Index].R, GrayValues[Index].G, GrayValues[Index].B, GrayValues[Index].index);
				r.timing = sw.ElapsedMilliseconds;

				if (Abort == true)
				{
					result = "Measurement aborted";
					throw new LSException(result);
				}

				if (_data == null || _data == "")
				{
					result = "No data received";
					throw new LSException(result);
				}
				else
				{
					// find first instance of a number
					int idx = _data.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });
					if (idx != -1)
					{
						string lumstr = _data.Substring(idx, _data.Length - idx); // read to the end of the data to get luminance value
						try
						{
							lum = float.Parse(lumstr);
						}
						catch
						{
							result = "Could not derive luminance value from received data -" + _data;
							throw new LSException(result);
						}

						if (lum != -1.0 && ignore==false) // don't write a dummy reading or an invalid one
						{
							r.luminance = lum;
							WriteReading(r);
						}
					}
					else
					{
						result = "No luminance value in received data: - " + _data;
						throw new LSException(result);
					}
				}
			}
			catch (LSException ex)
			{
                if (ignore==false)
				    WriteError(r);
				valid = false;
                throw new Exception(ex.Message);  // rethrow
			}
			finally
			{
				sw.Stop();
				_serial.DtrEnable = true;
				//_serial.Close();
			}


#region commented code for new LS100 protocol - keeping this in case we get a new LS100
			//if (_data.Length >= 4)
			//{
			//    string code = _data.Substring(0, 4);
			//    switch (code)
			//    {
			//    case "LSOK":
			//         extract the luminance value from the returned string
			//         format of result is first four characters define result code
			//        int idx = _data.IndexOf('H'); // H should always be present
			//        if (idx == -1)
			//        {
			//            valid = false;
			//            WriteError(r);
			//        }
			//        else
			//        {
			//            string lumstr = _data.Substring(idx, 6); // next 6 characters are the luminance value
			//            lum = float.Parse(lumstr);
			//            r.luminance = lum;
			//            WriteReading(r);
			//        }
			//        break;


			//    default: // some error occurred so we should disregard the reading
			//        sw.Stop();
			//        r = new Reading(GrayValues[Index].R, GrayValues[Index].G, GrayValues[Index].B, 0.0, time, GrayValues[Index].index);
			//        WriteError(r);
			//        valid = false;
			//        break;
			//    }
			//}
			//else
			//{
			//    valid = false;
			//    WriteError(r);
			//}
#endregion
			return valid;
		}

		public LSCalibration(uint scrwidth, uint scrheight) 
			: base(scrwidth, scrheight)
		{
		}
	}

	class LSException : ApplicationException
	{
        public LSException(string message): base(message)
        {
        }
	}
}
