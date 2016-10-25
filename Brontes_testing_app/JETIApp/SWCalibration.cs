using System;
using System.Collections.Generic;
using System.Text;

namespace JETIApp
{
	public class SWCalibration : CalibrationCore
	{
		private const double _gamma=2.2;

		public SWCalibration(uint scrwidth, uint scrheight)
			: base(scrwidth, scrheight)
		{
		}

		public override void AbortMeasurement(object source, EventArgs e)
		{
			base.AbortMeasurement(source, e);
		}

		public override bool ConfigDevice()
		{
			return true;
		}


		public override bool CloseDevice()
		{
			return base.CloseDevice();
		}

		public override bool Start(ref string result)
		{
			if (base.Start(ref result)==false)
				return false;

			InitOutput();
			Abort = false;
			return true;


		}

		public override bool Stop()
		{
			return base.Stop();
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
				return 2;

			if (r.G > level && (r.R == r.B) && (r.R == level)) // 0,1,0
				return 3;

			if (r.R > level && r.B > level && r.G == level) // 1,0,1
				return 4;

			if (r.R == level && r.G > level && r.B > level) // 0,1,1
				return 5;

			if (r.R > level && (r.G == r.R) && (r.B == level)) //1,1,0
				return 6;

			// anything other than these values is an error
			throw new ArgumentException("Invalid RGB value");
		}

		public override bool TakeReading(ref string result, out long time,bool ignore)
		{
			Reading r;
			uint baselevel=0;
			r = new Reading(GrayValues[Index].R, GrayValues[Index].G, GrayValues[Index].B, GrayValues[Index].index);

			//if (sg.NextDouble() < 0.05)
			//	WriteError(r);
			//else
			{
				r.luminance = Math.Pow(GrayValues[Index].R / (double)Constants.MaxLevel, _gamma) * 100;
				r.luminance = r.luminance + ( ScoreReading(r, ref baselevel));
				WriteReading(r);
			}


			time = 0;
			return true;

		}

	}

}
