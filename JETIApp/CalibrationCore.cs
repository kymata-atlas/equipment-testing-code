using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace JETIApp
{
    public class GrayItem
	{
		public uint R;
		public uint G;
		public uint B;
		public int index;

		public GrayItem(uint r, uint g, uint b)
		{
			R = r;
			G = g;
			B = b;
		}
	}

	public class Reading : GrayItem
	{
		public double luminance;
		public double timing;

		public Reading(uint r, uint g, uint b,int idx)
			: base(r, g, b)
		{
			luminance = -1.0;
			timing = -1;
			index = idx;
		}


		public Reading(uint r, uint g, uint b, double lum,int idx)
			: base(r, g, b)
		{
			luminance = lum;
			timing = -1;
			index = idx;
		}

		public Reading(uint r, uint g,uint b, double lum,double time,int idx):
			base(r,g,b)
		{
			luminance = lum;
			timing = time;
			index = idx;
		}
		/*
		public Reading(uint r, uint g, uint b, double lum, double time, int idx)
			: base(r, g, b)
		{
			luminance = lum;
			timing = time;
			index = idx;
		}
*/
	}

  public class TripleReading : GrayItem
  {
    public double X;
    public double Y;
    public double Z;
    public double timing;

    public TripleReading(uint r, uint g, uint b, int idx)
      : base (r, g, b)
    {
      X = -1.0;
      Y = -1.0;
      Z = -1.0;
      timing = -1;
      index = idx;
    }

    public TripleReading(uint r, uint g, uint b, double x, double y, double z, int idx)
      : base (r, g, b)
    {
      X = x;
      Y = y;
      Z = Z;
      timing = -1;
      index = idx;
    }

    public TripleReading(uint r, uint g, uint b, double x, double y, double z, double time, int idx)
      : base (r, g, b)
    {
      X = x;
      Y = y;
      Z = z;
      timing = time;
      index = idx;
    }
  }



	public class CalibrationCore

	{

		private List<Reading> _Readings;
		private List<GrayItem> _GrayValues;
		private CalibConfig _Config;

		private StreamWriter _Output;

		private int _Index;

		private static bool _Abort = false;

		protected static bool Abort
		{
			get
			{
				return _Abort;
			}
			set
			{
				_Abort = value;
			}

		}

		protected List<GrayItem> GrayValues
		{
			get
			{
				return _GrayValues;
			}
		}

		protected int Index
		{
			get
			{
				return _Index;
			}
			set
			{
				_Index = value;
			}
		}

		protected StreamWriter Output
		{
			get
			{
				return _Output;
			}
			set
			{
				_Output = value;
			}
		}

		public List<Reading> Readings
		{
			get
			{
				return _Readings;
			}
		}

		public CalibConfig Config
		{
			get
			{
				return _Config;
			}
			set
			{
				_Config = value;
			}
		}

		public int GrayLevelIndex
		{
			get
			{
				return _Index;

			}
			set
			{
				_Index = value;
			}
		}

		public GrayItem GetGrayLevel()
		{
			return _GrayValues[_Index];
		}

		public virtual void AbortMeasurement(object source, EventArgs e)
		{
			_Abort = true;
		}

		protected void GenerateGrayValues()
		{
			uint R=0;
			uint G=0;
			uint B=0;

			_GrayValues=new List<GrayItem>();
			List<GrayItem> temp=new List<GrayItem>();

			GrayItem gi; // temporary item

			int index = -1;
			//			uint step = (Constants.MaxLevel+1)/Config.NoOfGrayLevels; // because levels go from zero there are MaxLevel+1 total levels

			uint RealGrayLevels = ((Constants.MaxLevel + 1) / Config.GrayLevelStepSize);
			if (Config.GrayLevelStepSize != 1)
				RealGrayLevels++;

			uint repeats;
			if (Config.RandomizeRepeats==false)
				repeats=1; // multiple readings are just taken consecutively so multiple items in the gray list are not needed
			else
				repeats=Config.ReadingsPerLevel;

			for (uint i=0;i<RealGrayLevels;i++)
			{
				uint level;

				if (i == 0)
					level = 0;
				else
					level = (i * Config.GrayLevelStepSize);

				if (level > Constants.MaxLevel)
					level = Constants.MaxLevel;

				index++;

				// add non extended items
				for (uint j=0;j<repeats;j++)
				{

					switch (Config.TargetColour)
					{
					case TargetColourEnum.Grayscale:
						R = level;
						G = level;
						B = level;
						break;
					case TargetColourEnum.Red:
						R = level;
						G = 0;
						B = 0;
						break;
					case TargetColourEnum.Green:
						R = 0;
						G = level;
						B = 0;
						break;
					case TargetColourEnum.Blue:
						R = 0;
						G = 0;
						B = level;
						break;
					}
					temp.Add(new GrayItem(R,G,B));
					gi = temp[temp.Count - 1];
					gi.index = index;
				}

				if (Config.ExtendRange==true)
				{
					// the order is important here - it should be in order of increasing luminosity as we use the index here to re-order the results for analysis
					if (level<Constants.MaxLevel)
					{

						if (Config.ExtraBits == (Config.ExtraBits | ExtraBitsEnum._001))
						{
							index++;
							for (uint j=0;j<repeats;j++)
							{
								temp.Add(new GrayItem(R, G, B + 1));
								gi = temp[temp.Count - 1];
								gi.index = index;
							}
						}


						if (Config.ExtraBits == (Config.ExtraBits | ExtraBitsEnum._100))
						{
							index++;
							for (uint j=0;j<repeats;j++)
							{
								temp.Add(new GrayItem(R+1, G , B ));
								gi = temp[temp.Count - 1];
								gi.index = index;
							}
						}

						if (Config.ExtraBits == (Config.ExtraBits | ExtraBitsEnum._010))
						{
							index++;
							for (uint j=0;j<repeats;j++)
							{
								temp.Add(new GrayItem(R , G+1, B));
								gi = temp[temp.Count - 1];
								gi.index = index;
							}
						}


						if (Config.ExtraBits == (Config.ExtraBits | ExtraBitsEnum._101))
						{
							index++;
							for (uint j=0;j<repeats;j++)
							{
								temp.Add(new GrayItem(R + 1, G , B+1));
								gi = temp[temp.Count - 1];
								gi.index = index;
							}
						}

						if (Config.ExtraBits == (Config.ExtraBits | ExtraBitsEnum._011))
						{
							index++;
							for (uint j=0;j<repeats;j++)
							{
								temp.Add(new GrayItem(R, G+1 , B+1));
								gi = temp[temp.Count - 1];
								gi.index = index;
							}
						}

						if (Config.ExtraBits == (Config.ExtraBits | ExtraBitsEnum._110))
						{
							index++;
							for (uint j = 0; j < repeats; j++)
							{
								temp.Add(new GrayItem(R+1, G + 1, B ));
								gi = temp[temp.Count - 1];
								gi.index = index;
							}
						}
					}

				}
			}

			// check same number of readings against total readings
			if (Config.RandomizeRepeats == false)
			{
				if (Config.TotalReadings != temp.Count * Config.ReadingsPerLevel)
					throw new ArgumentOutOfRangeException("Generated number of grey levels does not match expected number");
			}
			else // if we are randomizing repeats then total readings will just be the number of readings
			{
				if (Config.TotalReadings != temp.Count)
					throw new ArgumentOutOfRangeException("Generated number of grey levels does not match expected number");
			}

			//Randomize the gray values
            Random rng = new Random();
			int r;

			while (temp.Count>0)
			{
                r = rng.Next(0, temp.Count - 1);
				_GrayValues.Add(temp[r]);
				temp.RemoveAt(r);
			}

		}

		void LoadGrayValues () {

			// Check if the extended range is on or off
			Console.WriteLine (String.Format ("We are really hoping this is false: {0}.", Config.ExtendRange));

			_GrayValues = new List<GrayItem>();
			List<GrayItem> tempGrayItems = new List<GrayItem>();

			GrayItem gi; // temporary item

			// read a file
			List<ColourRGB> colours = new List<ColourRGB> ();

			using (StreamReader reader = new StreamReader("ResourceFiles/Kymata-visual-stimulus-rgb-dataset1.txt")) {

				while (!reader.EndOfStream) {

					string line = reader.ReadLine();

					string[] segments = line.Split(',');

					// Add to list of colours
					colours.Add(new ColourRGB(segments[0], segments[1], segments[2]));

				}

			}

			int index = -1;

			foreach (ColourRGB colour in colours) {

				index++;

				// set R, G, B

				tempGrayItems.Add(new GrayItem(colour.R, colour.G, colour.B));
				gi = tempGrayItems[tempGrayItems.Count - 1];
				gi.index = index;
			}

			// add to real list

            foreach(GrayItem tempGrayItem in tempGrayItems)
            {
                _GrayValues.Add(tempGrayItem);
            }
		}

		/*        public virtual bool CloseDevice()
        {
			//int ret;
			//ret = JETILib.JETIRadio.JETI_CloseRadio(_Device);
			//return true;
			throw new NotImplementedException();
        }
		*/
		public virtual bool TakeReading(ref string result, out long time,bool ignore)
		{
			throw new NotImplementedException();
		}


		protected bool ValidateConfig(ref string result)
		{
			if (Config.TargetColour == TargetColourEnum.Not_Set)
			{
				result = "Target colour not set";
				return false;
			}

			if (Config.GetOutputFile() == "")
			{
				result = "No output file specified";
				return false;
			}


			if (Config.PatchWidth <= 0 || Config.PatchHeight <= 0 || Config.PatchWidth > Config.DisplayWidth || Config.PatchHeight > Config.DisplayHeight)
			{
				result = "Patch size is invalid";
				return false;
			}


			if (Config.TotalReadings <=0)
			{
				result="Configuration is invalid";
				return false;
			}

			if (Config.EmailWhenComplete == true)
			{
				if (Config.SMTPServer == "" || Config.EmailTo == "")
				{
					result = "Email details are not valid - notification e-mail will not be sent";
					Config.EmailWhenComplete = false;
				}
			}

			return true;
		}

		/*public void Debug()
		{
			GenerateGrayValues();
			Config.OutputFile = @"c:\debug.txt";
			// open file for output
			_Output = File.CreateText(Config.OutputFile);
			_Output.WriteLine(Constants.FileIdentifier); // use this to identify the correct file type
			// colons here are important as we use them for delimiters when reading the file back in
			_Output.WriteLine("Calibration started\t" + DateTime.Now.ToString());
			_Output.WriteLine("No of gray levels\t" + Config.NoOfGrayLevels.ToString());
			_Output.WriteLine("No of repeat readings\t" + Config.ReadingsPerLevel.ToString());
			_Output.WriteLine("Write timings\t " + Config.WriteTimings.ToString());
			_Output.Flush();

			double lum;
			double time;

			double gamma = 2.2f; // standard gamma value for Windows
			double maxjitter = 0;
			StandardGenerator sg = new StandardGenerator();


			for (int i = 0; i < Config.NoOfGrayLevels; i++)

			{
				String s = "";

				Config.ReadingsDonePerLevel = 0;
				// write RGB levels
				if (_Config.ReadingsPerLevel == 1 || Config.ReadingsDonePerLevel == 0) // write out grey values
				{
					s = String.Format("{0:D}\t{1:D}\t{2:D}\t{3:D}", _GrayValues[i].index, _GrayValues[i].R, _GrayValues[i].G, _GrayValues[i].B);
					_Output.Write(s);
				}

				time = 10.0; // arbitrary here

				// fake gamma curve
				lum = Math.Pow(_GrayValues[i].R / (double)Constants.MaxLevel, gamma)*100;

				for (int j = 0; j < Config.ReadingsPerLevel; j++)
				{

					double jitter = sg.NextDouble(-maxjitter, maxjitter);
					lum += jitter;

					StringBuilder sb = new StringBuilder();
					// write the luminance value
					sb.AppendFormat("\t{0:N}", lum);

					if (_Config.WriteTimings == true) // write timings
						sb.AppendFormat("\t{0:N}", time);

					if (_Config.ReadingsPerLevel == 1 || (_Config.ReadingsDonePerLevel == _Config.ReadingsPerLevel-1))
					{
						_Output.WriteLine(sb.ToString());
					}
					else
					{
						_Output.Write(sb.ToString());
					}
					_Config.ReadingsDonePerLevel++;

				}

			}
			_Output.WriteLine("Calibration finished\t" + DateTime.Now.ToString());

			_Output.Flush();
			_Output.Close();

		}
		*/

		public virtual bool Start(ref string result)
		{
			// generate gray values
			if (ValidateConfig(ref result) == false)
				return false;

			if (result != "")
			{
				MessageBox.Show(result, "Gamma Calibration", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			//GenerateGrayValues();

			LoadGrayValues();

			return true;

		}

		protected bool WriteError(Reading reading)
		{

			if (reading != null)
			{
				Output.Write(GetRGB(reading));

				StringBuilder sb = new StringBuilder();
				sb.Append("\t!!!!");

				if (Config.WriteTimings == true)
				{
					sb.AppendFormat("\t{0:N}", reading.timing);
				}

				EndReading(sb);
				return true;

			}

			return false;
		}

		private void EndReading(StringBuilder sb)
		{

			if (Config.ReadingsPerLevel == 1 || (Config.ReadingsDonePerLevel == Config.ReadingsPerLevel - 1 || Config.RandomizeRepeats==true))
			{
				Output.WriteLine(sb.ToString());
			}
			else
			{
				Output.Write(sb.ToString());
			}

			Output.Flush();

        }

        private string GetRGB(Reading reading)
        {
            String s = "";
            // write RGB levels
            if (Config.ReadingsPerLevel == 1 || Config.ReadingsDonePerLevel == 0) // write out grey values
            {
                s = String.Format("{0:D}\t{1:D}\t{2:D}\t{3:D}", reading.index, reading.R, reading.G, reading.B);
            }
            return s;
        }

        private string GetRGB(TripleReading reading)
        {
            String s = "";
            // write RGB levels
            if (Config.ReadingsPerLevel == 1 || Config.ReadingsDonePerLevel == 0) // write out grey values
            {
                s = String.Format("{0:D}\t{1:D}\t{2:D}\t{3:D}", reading.index, reading.R, reading.G, reading.B);
            }
            return s;
        }

        protected bool WriteReading(Reading reading)
		{
			if (_Config.WriteOutput == true)
			{
				if (reading != null)
				{

					Output.Write(GetRGB(reading));

					StringBuilder sb = new StringBuilder();
					// write the luminance value
					sb.AppendFormat("\t{0:N}", reading.luminance);

					if (Config.WriteTimings == true) // write timings

						sb.AppendFormat("\t{0:N}", reading.timing);

					EndReading(sb);

					return true;
				}

				return false;
			}
			else
			{
				return true;
			}
		}

		protected bool WriteReading(TripleReading reading)
		{
			if (_Config.WriteOutput == true)
			{
				if (reading != null)
				{

					Output.Write(GetRGB(reading));

					StringBuilder sb = new StringBuilder();
					// write the X, Y and Z values
					sb.AppendFormat("\t{0:N}", reading.X);
					sb.AppendFormat("\t{0:N}", reading.Y);
					sb.AppendFormat("\t{0:N}", reading.Z);

					if (Config.WriteTimings == true) // write timings

						sb.AppendFormat("\t{0:N}", reading.timing);

					EndReading(sb);

					return true;
				}

				return false;
			}
			else
			{
				return true;
			}
		}

		protected void InitOutput()
		{
			// open file for output
			Output = File.CreateText(Config.GetOutputFile());
			Output.WriteLine(Constants.FileIdentifier); // use this to identify the correct file type
			// colons here are important as we use them for delimiters when reading the file back in
			Output.WriteLine("Calibration started\t" + DateTime.Now.ToString());
			Output.WriteLine("No of gray levels\t" + Config.NoOfGrayLevels.ToString());
			Output.WriteLine("No of repeat readings\t" + Config.ReadingsPerLevel.ToString());
			Output.WriteLine("Write timings\t " + Config.WriteTimings.ToString());
			Output.WriteLine("Repeats are randomized\t" + Config.RandomizeRepeats.ToString());
			Output.Flush();

		}


		public virtual bool CloseDevice()
		{
			return false;
		}

		public virtual bool ConfigDevice()
		{
			return false;
		}

		public virtual bool Stop()
		{
			_Output.WriteLine("Calibration finished\t" + DateTime.Now.ToString());
			_Output.Flush();
			_Output.Close();
			_Output.Dispose();

			return true;
		}

		private CalibrationCore()
		{

		}


		public CalibrationCore(uint scrwidth, uint scrheight)
		{
			_Config = new CalibConfig(scrwidth, scrheight);
			_Index = 0;
			_Readings = new List<Reading>();
		}

	}


	/// <summary>
	/// A Colour with RGB values.
	/// </summary>
	public struct ColourRGB {

		// TODO: is float the appropriate precision for this?

		/// <summary>
		/// Red.
		/// </summary>
		public uint R;
		/// <summary>
		/// Green
		/// </summary>
		public uint G;
		/// <summary>
		/// Blue.
		/// </summary>
		public uint B;

		/// <summary>
		/// String-based constructor.
		/// </summary>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
		public ColourRGB(string stringR, string stringG, string stringB) {

			float floatR = float.Parse(stringR);
			float floatG = float.Parse(stringG);
			float floatB = float.Parse(stringB);

			this.R = float2uint(floatR);
			this.G = float2uint(floatG);
			this.B = float2uint(floatB);
		}

		/// <summary>
		/// Converts floats (from Matlab) to ints (for the actual colour) in the way we need.
		/// </summary>
		/// <param name="inValue">In value.</param>
		private static uint float2uint(float inValue) {
			return (uint)Math.Floor (inValue);
		}
	}
}
