using System;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            SweetFX.Load(@"D:\Desktop\SweetFX_settings.txt");
            // SMAA
            checkBox1.Checked = SweetFX.SMAA.Enabled;
            numericUpDown1.Value = SweetFX.SMAA.Threshold;
            trackBar1.Value = Convert.ToInt32(SweetFX.SMAA.Threshold * (decimal)100.00);
            numericUpDown2.Value = SweetFX.SMAA.Max_Search_Steps;
            trackBar2.Value = SweetFX.SMAA.Max_Search_Steps;
            numericUpDown3.Value = SweetFX.SMAA.Max_Search_Steps_Diag;
            trackBar3.Value = SweetFX.SMAA.Max_Search_Steps_Diag;
            numericUpDown4.Value = SweetFX.SMAA.Corner_Rounding;
            trackBar4.Value = SweetFX.SMAA.Corner_Rounding;
            checkBox2.Checked = SweetFX.SMAA.Color_Edge_Detection;
            checkBox3.Checked = SweetFX.SMAA.DirectX9_Linear_Blend;
            //
            StartFormCapture();
        }

        private void StartFormCapture()
        {
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_ValueChanged);
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_ValueChanged);
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            this.trackBar4.Scroll += new System.EventHandler(this.trackBar4_ValueChanged);
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
        }

        private void StopFormCapture()
        {
            this.checkBox1.CheckedChanged -= new System.EventHandler(this.checkBox1_CheckedChanged);
            this.numericUpDown1.ValueChanged -= new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.trackBar1.Scroll -= new System.EventHandler(this.trackBar1_ValueChanged);
            this.numericUpDown2.ValueChanged -= new System.EventHandler(this.numericUpDown2_ValueChanged);
            this.trackBar2.Scroll -= new System.EventHandler(this.trackBar2_ValueChanged);
            this.numericUpDown3.ValueChanged -= new System.EventHandler(this.numericUpDown3_ValueChanged);
            this.trackBar3.Scroll -= new System.EventHandler(this.trackBar3_ValueChanged);
            this.numericUpDown4.ValueChanged -= new System.EventHandler(this.numericUpDown4_ValueChanged);
            this.trackBar4.Scroll -= new System.EventHandler(this.trackBar4_ValueChanged);
            this.checkBox2.CheckedChanged -= new System.EventHandler(this.checkBox2_CheckedChanged);
            this.checkBox3.CheckedChanged -= new System.EventHandler(this.checkBox3_CheckedChanged);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //
            Environment.Exit(0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // add game - create form
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // remove game - easy
        }

        #region SMAA

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.SMAA.Color_Edge_Detection = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.SMAA.DirectX9_Linear_Blend = checkBox3.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.SMAA.Enabled = checkBox1.Checked;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            // Remove only the event needed, sigh
            // sigh - bitch work
            StopFormCapture();
            numericUpDown1.Value = (decimal)trackBar1.Value / (decimal)100.00;
            SweetFX.SMAA.Threshold = numericUpDown1.Value;
            StartFormCapture();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            StopFormCapture();
            SweetFX.SMAA.Threshold = numericUpDown1.Value * (decimal)100.00;
            trackBar1.Value = Convert.ToInt32(SweetFX.SMAA.Threshold);
            StartFormCapture();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            StopFormCapture();
            numericUpDown2.Value = trackBar2.Value;
            SweetFX.SMAA.Max_Search_Steps = trackBar2.Value;
            StartFormCapture();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            StopFormCapture();
            trackBar2.Value = Convert.ToInt32(numericUpDown2.Value);
            SweetFX.SMAA.Max_Search_Steps = trackBar2.Value;
            StartFormCapture();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            StopFormCapture();
            numericUpDown3.Value = trackBar3.Value;
            SweetFX.SMAA.Max_Search_Steps_Diag = trackBar3.Value;
            StartFormCapture();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            StopFormCapture();
            trackBar3.Value = Convert.ToInt32(numericUpDown3.Value);
            SweetFX.SMAA.Max_Search_Steps_Diag = trackBar3.Value;
            StartFormCapture();
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            StopFormCapture();
            numericUpDown4.Value = trackBar4.Value;
            SweetFX.SMAA.Corner_Rounding = trackBar4.Value;
            StartFormCapture();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            StopFormCapture();
            trackBar4.Value = Convert.ToInt32(numericUpDown4.Value);
            SweetFX.SMAA.Corner_Rounding = trackBar4.Value;
            StartFormCapture();
        }

        #endregion
    }
}
