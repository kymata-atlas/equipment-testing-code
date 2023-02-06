using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NPlot;
using NPlot.Windows;

namespace JETIApp
{
	public partial class frmTiming : Form
	{
		Marker redcross = new Marker(Marker.MarkerType.Cross1, 6, new Pen(Color.Red, 2.0F));

		public void PlotTimingData(List<Reading> readings)
		{
			//plot readings

			pltTiming.Clear();
			NPlot.Grid grid = new Grid();

			grid.VerticalGridType = Grid.GridType.Coarse;
			grid.HorizontalGridType = Grid.GridType.Coarse;

			grid.MajorGridPen = new Pen(Color.LightGray, 1.0f);

			// add grid to chart
			pltTiming.Add(grid);

			// create a point plot using the marker to plot values
			PointPlot pp = new PointPlot(redcross);

			List<uint> indices=new List<uint>();
			List<double>values=new List<double>();
			for (int i=0;i<readings.Count;i++)
			{
				
				indices.Add(readings[i].R);
				values.Add(readings[i].timing);
			}

			pp.AbscissaData = indices;
			pp.OrdinateData = values;
			pp.Label = "Timing data";

			pltTiming.Add(pp);

			pltTiming.Title = "Timing";
			pltTiming.XAxis1.Label = "Index";
			pltTiming.YAxis1.Label = "Time/ms";

			pltTiming.XAxis1.WorldMin = 0;
			pltTiming.XAxis1.WorldMax = 270;


			Legend legend = new Legend();
			legend.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Top, NPlot.PlotSurface2D.YAxisPosition.Right);
			legend.VerticalEdgePlacement = Legend.Placement.Inside;
			legend.HorizontalEdgePlacement = Legend.Placement.Inside;
			legend.XOffset = 10;
			legend.YOffset = 10;

			pltTiming.Legend = legend;
			pltTiming.LegendZOrder = 1;

			pltTiming.Refresh();
		}
		public frmTiming()
		{
			InitializeComponent();
		}


		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Hide();
		}
	}
}