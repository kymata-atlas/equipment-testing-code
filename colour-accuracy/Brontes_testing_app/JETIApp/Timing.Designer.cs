namespace JETIApp
{
	partial class frmTiming
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pltTiming = new NPlot.Windows.PlotSurface2D();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnClose = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pltTiming
			// 
			this.pltTiming.AutoScaleAutoGeneratedAxes = false;
			this.pltTiming.AutoScaleTitle = false;
			this.pltTiming.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.pltTiming.DateTimeToolTip = false;
			this.pltTiming.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pltTiming.Legend = null;
			this.pltTiming.LegendZOrder = -1;
			this.pltTiming.Location = new System.Drawing.Point(0, 0);
			this.pltTiming.Name = "pltTiming";
			this.pltTiming.RightMenu = null;
			this.pltTiming.ShowCoordinates = true;
			this.pltTiming.Size = new System.Drawing.Size(753, 469);
			this.pltTiming.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
			this.pltTiming.TabIndex = 0;
			this.pltTiming.Title = "";
			this.pltTiming.TitleFont = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.pltTiming.XAxis1 = null;
			this.pltTiming.XAxis2 = null;
			this.pltTiming.YAxis1 = null;
			this.pltTiming.YAxis2 = null;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(753, 30);
			this.panel1.TabIndex = 1;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(675, 3);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// frmTiming
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(753, 469);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pltTiming);
			this.Name = "frmTiming";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Timing";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private NPlot.Windows.PlotSurface2D pltTiming;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnClose;
	}
}