namespace JETIApp
{
	partial class frmWhichReadings
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
			this.btnMin = new System.Windows.Forms.Button();
			this.btnMax = new System.Windows.Forms.Button();
			this.btnMean = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnFirst = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnMin
			// 
			this.btnMin.Location = new System.Drawing.Point(141, 80);
			this.btnMin.Name = "btnMin";
			this.btnMin.Size = new System.Drawing.Size(75, 23);
			this.btnMin.TabIndex = 0;
			this.btnMin.Text = "Minimum";
			this.btnMin.UseVisualStyleBackColor = true;
			this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
			// 
			// btnMax
			// 
			this.btnMax.Location = new System.Drawing.Point(235, 80);
			this.btnMax.Name = "btnMax";
			this.btnMax.Size = new System.Drawing.Size(75, 23);
			this.btnMax.TabIndex = 1;
			this.btnMax.Text = "Maximum";
			this.btnMax.UseVisualStyleBackColor = true;
			this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
			// 
			// btnMean
			// 
			this.btnMean.Location = new System.Drawing.Point(327, 80);
			this.btnMean.Name = "btnMean";
			this.btnMean.Size = new System.Drawing.Size(75, 23);
			this.btnMean.TabIndex = 2;
			this.btnMean.Text = "Mean";
			this.btnMean.UseVisualStyleBackColor = true;
			this.btnMean.Click += new System.EventHandler(this.btnMean_Click);
			// 
			// textBox2
			// 
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Location = new System.Drawing.Point(114, 12);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(261, 37);
			this.textBox2.TabIndex = 6;
			this.textBox2.Text = "Data file contains multiple readings for each gray level.\r\nHow do you want to pro" +
				"cess the data?";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(421, 80);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(37, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(50, 50);
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// btnFirst
			// 
			this.btnFirst.Location = new System.Drawing.Point(49, 80);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(75, 23);
			this.btnFirst.TabIndex = 9;
			this.btnFirst.Text = "First";
			this.btnFirst.UseVisualStyleBackColor = true;
			this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
			// 
			// frmWhichReadings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(589, 115);
			this.ControlBox = false;
			this.Controls.Add(this.btnFirst);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.btnMean);
			this.Controls.Add(this.btnMax);
			this.Controls.Add(this.btnMin);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmWhichReadings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WhichReadings";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnMin;
		private System.Windows.Forms.Button btnMax;
		private System.Windows.Forms.Button btnMean;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnFirst;
	}
}