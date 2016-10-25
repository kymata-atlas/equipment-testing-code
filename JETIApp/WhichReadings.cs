using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JETIApp
{
	public partial class frmWhichReadings : Form
	{
		private frmAnalysis.ProcessDataEnum _Result;

		public frmWhichReadings()
		{
			InitializeComponent();
			pictureBox1.Image = SystemIcons.Question.ToBitmap();

		}

		public frmWhichReadings(frmAnalysis.ProcessDataEnum result)
		{

			InitializeComponent();
			pictureBox1.Image = SystemIcons.Question.ToBitmap();

			this._Result=result;
		}

		public frmAnalysis.ProcessDataEnum Result
		{
			get
			{
				return _Result;
			}
		}

		private void btnMin_Click(object sender, EventArgs e)
		{
			_Result=frmAnalysis.ProcessDataEnum.Min;
			this.Hide();
		}

		private void btnMax_Click(object sender, EventArgs e)
		{
			_Result=frmAnalysis.ProcessDataEnum.Max;
			this.Hide();
		}

		private void btnMean_Click(object sender, EventArgs e)
		{
			_Result=frmAnalysis.ProcessDataEnum.Mean;
			this.Hide();
		
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			_Result = frmAnalysis.ProcessDataEnum.Cancel;
			this.Hide();

		}

		private void btnFirst_Click(object sender, EventArgs e)
		{
			_Result = frmAnalysis.ProcessDataEnum.First;
			this.Hide();
		}


	}
}