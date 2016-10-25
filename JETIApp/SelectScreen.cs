using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using StereolabFX;
using System.Linq;

namespace JETIApp
{
    public partial class frmSelectScreen : Form
    {
		private List<uint> _ScreenNumbers;
		private SpanSideEnum _Spanside;
		private bool _Spanscreens;
		private string _GammaFile;

		public string GammaFile
		{
			get
			{
				return _GammaFile;
			}
			set
			{
				_GammaFile=value;
			}

		}

		public bool SpanScreens
		{
			get
			{
				return _Spanscreens;
			}
			set
			{
				_Spanscreens = value;
			}
		}

		public SpanSideEnum SpanSide
		{
			get
			{
				return _Spanside;
			}
			set
			{
				_Spanside = value;
			}
		}

        public frmSelectScreen(List<uint> screens,bool spanscreens, SpanSideEnum side,string gammafile)
        {
            InitializeComponent();
			_ScreenNumbers = screens;
			_Spanscreens = spanscreens;
			_Spanside = side;
			_GammaFile = gammafile;
			txtGammaFile.Text = System.IO.Path.GetFileName(_GammaFile);

        }

		public List<uint> ScreenNumbers
		{
			get
			{
				return _ScreenNumbers;
			}
			set
			{
				_ScreenNumbers = value;
			}
		}

  
        private void chkSpan_CheckedChanged(object sender, EventArgs e)
        {
			if (chkSpan.Checked == true)
			{
				rdoLeft.Visible = true;
				rdoRight.Visible = true;
                rdoFull.Visible = true;
                rdoFull.Checked = true;
			}
			else
			{
				rdoLeft.Visible = false;
				rdoRight.Visible = false;
                rdoFull.Visible = false;
                // reset list of checked items
                if (lstMonitors.CheckedIndices.Count > 1)
                {
                    foreach (ListViewItem item in lstMonitors.Items)
                    {
                        item.Checked = false;
                    }
                    lstMonitors.Items[0].Checked = true;
                }
                
			}

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstMonitors.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please choose a screen");
                return;
            }
            else
            {


                _ScreenNumbers.Clear();
                foreach (int c in lstMonitors.CheckedIndices)
                {
                    _ScreenNumbers.Add((uint)c);
                }
				_Spanscreens = chkSpan.Checked;
				if (_Spanscreens)
				{
                    if (rdoLeft.Checked == true)
                        _Spanside = SpanSideEnum.Left;
                    if (rdoRight.Checked == true)
                        _Spanside = SpanSideEnum.Right;
                    if (rdoFull.Checked == true)
                        _Spanside = SpanSideEnum.Full;

				}

                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

		private void lstMonitors_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (e.NewValue == CheckState.Unchecked)
				return;
			else
				if (lstMonitors.CheckedIndices.Count > 0)
                    if (chkSpan.Checked==false)
					    lstMonitors.Items[lstMonitors.CheckedIndices[0]].Checked = false;

		}

		private void frmSelectScreen_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		private void frmSelectScreen_Load(object sender, EventArgs e)
		{
			lstMonitors.View = View.Details;
			for (int i = 0; i < Screen.AllScreens.Length; i++)
			{
				Screen s = Screen.AllScreens[i];
				ListViewItem lvi = new ListViewItem();
				lvi.Text = i.ToString();
				lvi.SubItems.Add(s.Bounds.Width.ToString());
				lvi.SubItems.Add(s.Bounds.Height.ToString());
				lstMonitors.Items.Add(lvi);
			}

            rdoLeft.Visible=false;
            rdoRight.Visible=false;
            rdoFull.Visible = false;
            chkSpan.Checked = false;
            
            if (_Spanscreens == true)
            {
                rdoLeft.Visible = true;
                rdoRight.Visible = true;
                rdoFull.Visible = true;
                chkSpan.Checked = true;
                if (_Spanside == SpanSideEnum.Left)
                    rdoLeft.Checked = true;
                if (_Spanside==SpanSideEnum.Right)
                    rdoRight.Checked = true;
                if (_Spanside == SpanSideEnum.Full)
                    rdoFull.Checked = true;
            }

			if (Screen.AllScreens.Length < 2)
			{
				lstMonitors.Items[0].Checked = true;
				lstMonitors.Enabled = false;
			}
			else
			{
                foreach (int s in _ScreenNumbers)
                {
                    lstMonitors.Items[s].Checked = true;
                }
			}

		}

		private void btnSelectGamma_Click(object sender, EventArgs e)
		{
			openFile.InitialDirectory = @"c:\";
			openFile.Filter = "Gamma lookup table files (*.gamma,*.txt)|*.gamma;*.txt";
			openFile.CheckFileExists = true;
			openFile.AddExtension = true;
			openFile.DefaultExt = "gamma";
			openFile.Title = "Select gamma lookup table file";
			openFile.ValidateNames = true;
			if (openFile.ShowDialog() == DialogResult.OK)
			{
				_GammaFile = openFile.FileName;
				txtGammaFile.Text = System.IO.Path.GetFileName(_GammaFile);
			}
		}
    }
}