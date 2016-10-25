namespace JETIApp
{
    partial class frmConfig
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
			this.propGrid = new System.Windows.Forms.PropertyGrid();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnToggleLaser = new System.Windows.Forms.Button();
			this.btnDisplay = new System.Windows.Forms.Button();
			this.btnTest = new System.Windows.Forms.Button();
			this.btnAnalysis = new System.Windows.Forms.Button();
			this.btnDebug = new System.Windows.Forms.Button();
			//this.MsgBoxManager = new MessageBoxManager();
			this.dlgLoadConfig = new System.Windows.Forms.OpenFileDialog();
			this.dlgSaveConfig = new System.Windows.Forms.SaveFileDialog();
			this.chkHideConfig = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// propGrid
			// 
			this.propGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.propGrid.Location = new System.Drawing.Point(12, 40);
			this.propGrid.Name = "propGrid";
			this.propGrid.Size = new System.Drawing.Size(514, 464);
			this.propGrid.TabIndex = 0;
			this.propGrid.ToolbarVisible = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(370, 510);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "Start";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(451, 510);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Exit";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(450, 11);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "Save Config";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoad.Location = new System.Drawing.Point(369, 11);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(75, 23);
			this.btnLoad.TabIndex = 4;
			this.btnLoad.Text = "Load Config";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnToggleLaser
			// 
			this.btnToggleLaser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnToggleLaser.Location = new System.Drawing.Point(13, 510);
			this.btnToggleLaser.Name = "btnToggleLaser";
			this.btnToggleLaser.Size = new System.Drawing.Size(75, 23);
			this.btnToggleLaser.TabIndex = 5;
			this.btnToggleLaser.Text = "Laser On";
			this.btnToggleLaser.UseVisualStyleBackColor = true;
			this.btnToggleLaser.Click += new System.EventHandler(this.btnToggleLaser_Click);
			// 
			// btnDisplay
			// 
			this.btnDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDisplay.Location = new System.Drawing.Point(289, 510);
			this.btnDisplay.Name = "btnDisplay";
			this.btnDisplay.Size = new System.Drawing.Size(75, 23);
			this.btnDisplay.TabIndex = 6;
			this.btnDisplay.Text = "Set Display";
			this.btnDisplay.UseVisualStyleBackColor = true;
			this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
			// 
			// btnTest
			// 
			this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTest.Location = new System.Drawing.Point(208, 510);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 7;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// btnAnalysis
			// 
			this.btnAnalysis.Location = new System.Drawing.Point(12, 11);
			this.btnAnalysis.Name = "btnAnalysis";
			this.btnAnalysis.Size = new System.Drawing.Size(75, 23);
			this.btnAnalysis.TabIndex = 8;
			this.btnAnalysis.Text = "Analysis...";
			this.btnAnalysis.UseVisualStyleBackColor = true;
			this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
			// 
			// btnDebug
			// 
			this.btnDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDebug.Location = new System.Drawing.Point(127, 510);
			this.btnDebug.Name = "btnDebug";
			this.btnDebug.Size = new System.Drawing.Size(75, 23);
			this.btnDebug.TabIndex = 9;
			this.btnDebug.Text = "Debug";
			this.btnDebug.UseVisualStyleBackColor = true;
			this.btnDebug.Visible = false;
			// 
			// chkHideConfig
			// 
			this.chkHideConfig.AutoSize = true;
			this.chkHideConfig.Location = new System.Drawing.Point(253, 539);
			this.chkHideConfig.Name = "chkHideConfig";
			this.chkHideConfig.Size = new System.Drawing.Size(273, 17);
			this.chkHideConfig.TabIndex = 10;
			this.chkHideConfig.Text = "Hide Config on Display (not required for Haploscope)";
			this.chkHideConfig.UseVisualStyleBackColor = true;
			// 
			// frmConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(540, 568);
			this.Controls.Add(this.chkHideConfig);
			this.Controls.Add(this.btnDebug);
			this.Controls.Add(this.btnAnalysis);
			this.Controls.Add(this.btnTest);
			this.Controls.Add(this.btnDisplay);
			this.Controls.Add(this.btnToggleLaser);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.propGrid);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmConfig";
			this.Text = "Gamma Calibration ";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnToggleLaser;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnAnalysis;
		private System.Windows.Forms.Button btnDebug;
		//private MessageBoxManager MsgBoxManager;
		private System.Windows.Forms.OpenFileDialog dlgLoadConfig;
		private System.Windows.Forms.SaveFileDialog dlgSaveConfig;
		private System.Windows.Forms.CheckBox chkHideConfig;
    }
}