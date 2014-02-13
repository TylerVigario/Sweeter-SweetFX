using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SweetFX_Configurator
{
    public partial class MainForm : Form
    {
        private List<Message> MessagePump = new List<Message>();
        private delegate void SweetFX_SaveSettingsFinishedD();
        InstallManagerForm install_manager_form;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Settings.Load();
            WindowGeometry.GeometryFromString(Settings.Main_Window_Geometry, this);
            //
            SweetFX.SaveSettingsFinished += SweetFX_SaveSettingsFinished;
            InstallManager.GameLoaded += InstallManager_GameLoaded;
            if (Settings.LastGame.isSweetFXInstalled)
            {
                InstallManager.LoadGame(Settings.LastGame);
            }
            //
            List<Game> _gms = Settings.GetGames();
            foreach (Game _game in _gms)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = _game.Name;
                item.Click += item_Click;
                gamesToolStripMenuItem1.DropDownItems.Add(item);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            InstallManager.LoadGame(((ToolStripMenuItem)sender).Text);
        }

        private delegate void InstallManager_GameLoadedD();

        void InstallManager_GameLoaded()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InstallManager_GameLoadedD(InstallManager_GameLoaded));
                return;
            }
            LoadSFXConfig();
        }

        private void LoadSFXConfig()
        {
            StopFormCapture();
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
            // FXAA
            checkBox6.Checked = SweetFX.FXAA.Enabled;
            numericUpDown8.Value = SweetFX.FXAA.Quality_Preset;
            trackBar8.Value = SweetFX.FXAA.Quality_Preset;
            numericUpDown7.Value = SweetFX.FXAA.Subpix;
            trackBar7.Value = Convert.ToInt32(SweetFX.FXAA.Subpix * (decimal)1000.00);
            numericUpDown6.Value = SweetFX.FXAA.Edge_Threshold;
            trackBar6.Value = Convert.ToInt32(SweetFX.FXAA.Edge_Threshold * (decimal)1000.00);
            numericUpDown5.Value = SweetFX.FXAA.Edge_Threshold_Min;
            trackBar5.Value = Convert.ToInt32(SweetFX.FXAA.Edge_Threshold_Min * (decimal)1000.00);
            // Explosion
            checkBox4.Checked = SweetFX.Explosion.Enabled;
            numericUpDown12.Value = SweetFX.Explosion.Radius;
            trackBar12.Value = Convert.ToInt32(SweetFX.Explosion.Radius * (decimal)10.00);
            // Cartoon
            checkBox5.Checked = SweetFX.Cartoon.Enabled;
            numericUpDown11.Value = SweetFX.Cartoon.Power;
            trackBar9.Value = Convert.ToInt32(SweetFX.Cartoon.Power * (decimal)10.00);
            numericUpDown9.Value = SweetFX.Cartoon.Edge_Slope;
            trackBar11.Value = Convert.ToInt32(SweetFX.Cartoon.Edge_Slope * (decimal)10.00);
            // CRT
            checkBox8.Checked = SweetFX.CRT.Enabled;
            checkBox12.Checked = SweetFX.CRT.Enabled;
            numericUpDown19.Value = SweetFX.CRT.Amount;
            trackBar19.Value = Convert.ToInt32(SweetFX.CRT.Amount * (decimal)100.00);
            numericUpDown18.Value = SweetFX.CRT.Resolution;
            trackBar18.Value = Convert.ToInt32(SweetFX.CRT.Resolution * (decimal)10.00);
            numericUpDown17.Value = SweetFX.CRT.Gamma;
            trackBar17.Value = Convert.ToInt32(SweetFX.CRT.Gamma * (decimal)10.00);
            numericUpDown16.Value = SweetFX.CRT.Monitor_Gamma;
            trackBar16.Value = Convert.ToInt32(SweetFX.CRT.Monitor_Gamma * (decimal)10.00);
            numericUpDown10.Value = SweetFX.CRT.Brightness;
            trackBar10.Value = Convert.ToInt32(SweetFX.CRT.Brightness * (decimal)10.00);
            numericUpDown13.Value = SweetFX.CRT.Scanline_Intensity;
            trackBar13.Value = Convert.ToInt32(SweetFX.CRT.Scanline_Intensity * (decimal)10.00);
            checkBox10.Checked = SweetFX.CRT.Scanline_Gaussian;
            numericUpDown23.Value = SweetFX.CRT.Corner_Size;
            trackBar23.Value = Convert.ToInt32(SweetFX.CRT.Corner_Size * (decimal)10000.00);
            numericUpDown22.Value = SweetFX.CRT.Distance;
            trackBar22.Value = Convert.ToInt32(SweetFX.CRT.Distance * (decimal)100.00);
            numericUpDown21.Value = SweetFX.CRT.AngleX;
            trackBar21.Value = Convert.ToInt32(SweetFX.CRT.AngleX * (decimal)100.00);
            numericUpDown20.Value = SweetFX.CRT.AngleY;
            trackBar20.Value = Convert.ToInt32(SweetFX.CRT.AngleY * (decimal)100.00);
            numericUpDown15.Value = SweetFX.CRT.Curvature_Radius;
            trackBar15.Value = Convert.ToInt32(SweetFX.CRT.Curvature_Radius * (decimal)10.00);
            numericUpDown14.Value = SweetFX.CRT.Overscan;
            trackBar14.Value = Convert.ToInt32(SweetFX.CRT.Overscan * (decimal)100.00);
            checkBox7.Checked = SweetFX.CRT.Curvature;
            checkBox14.Checked = SweetFX.CRT.Oversample;
            // Bloom
            checkBox16.Checked = SweetFX.Bloom.Enabled;
            numericUpDown26.Value = SweetFX.Bloom.Threshold;
            trackBar26.Value = Convert.ToInt32(SweetFX.Bloom.Threshold * (decimal)100.00);
            numericUpDown28.Value = SweetFX.Bloom.Power;
            trackBar27.Value = Convert.ToInt32(SweetFX.Bloom.Power * (decimal)1000.00);
            numericUpDown27.Value = SweetFX.Bloom.Width;
            trackBar28.Value = Convert.ToInt32(SweetFX.Bloom.Width * (decimal)10000.00);
            // HDR
            checkBox15.Checked = SweetFX.HDR.Enabled;
            numericUpDown25.Value = SweetFX.HDR.Power;
            trackBar24.Value = Convert.ToInt32(SweetFX.HDR.Power * (decimal)100.00);
            numericUpDown24.Value = SweetFX.HDR.Radius;
            trackBar25.Value = Convert.ToInt32(SweetFX.HDR.Radius * (decimal)100.00);
            // LumaSharpen
            checkBox20.Checked = SweetFX.LumaSharpen.Enabled;
            numericUpDown32.Value = SweetFX.LumaSharpen.Strength;
            trackBar32.Value = Convert.ToInt32(SweetFX.LumaSharpen.Strength * (decimal)100.00);
            numericUpDown31.Value = SweetFX.LumaSharpen.Clamp;
            trackBar31.Value = Convert.ToInt32(SweetFX.LumaSharpen.Clamp * (decimal)1000.00);
            numericUpDown30.Value = SweetFX.LumaSharpen.Pattern;
            trackBar30.Value = SweetFX.LumaSharpen.Pattern;
            numericUpDown29.Value = SweetFX.LumaSharpen.Offset_Bias;
            trackBar29.Value = Convert.ToInt32(SweetFX.LumaSharpen.Offset_Bias * (decimal)10.00);
            checkBox19.Checked = SweetFX.LumaSharpen.Show;
            // Levels
            checkBox21.Checked = SweetFX.Levels.Enabled;
            numericUpDown35.Value = SweetFX.Levels.Black_Point;
            trackBar48.Value = SweetFX.Levels.Black_Point;
            numericUpDown33.Value = SweetFX.Levels.White_Point;
            trackBar33.Value = SweetFX.Levels.White_Point;
            // Technicolor
            checkBox23.Checked = SweetFX.Technicolor.Enabled;
            numericUpDown52.Value = SweetFX.Technicolor.Amount;
            trackBar52.Value = Convert.ToInt32(SweetFX.Technicolor.Amount * (decimal)100.00);
            numericUpDown51.Value = SweetFX.Technicolor.Power;
            trackBar51.Value = Convert.ToInt32(SweetFX.Technicolor.Power * (decimal)100.00);
            numericUpDown50.Value = SweetFX.Technicolor.Red_Negative_Amount;
            trackBar50.Value = Convert.ToInt32(SweetFX.Technicolor.Red_Negative_Amount * (decimal)100.00);
            numericUpDown49.Value = SweetFX.Technicolor.Green_Negative_Amount;
            trackBar49.Value = Convert.ToInt32(SweetFX.Technicolor.Green_Negative_Amount * (decimal)100.00);
            numericUpDown48.Value = SweetFX.Technicolor.Blue_Negative_Amount;
            trackBar35.Value = Convert.ToInt32(SweetFX.Technicolor.Blue_Negative_Amount * (decimal)100.00);
            //
            StartFormCapture();
        }

        void SweetFX_SaveSettingsFinished()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SweetFX_SaveSettingsFinishedD(SweetFX_SaveSettingsFinished));
                return;
            }
            SetMessage("SweetFX configuration saved");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Main_Window_Geometry = WindowGeometry.GeometryToString(this);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SweetFX.Dispose();
            Environment.Exit(0);
        }

        private void StartFormCapture()
        {
            // SMAA
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
            // FXAA
            checkBox6.CheckedChanged += checkBox6_CheckedChanged;
            numericUpDown8.ValueChanged += numericUpDown8_ValueChanged;
            trackBar8.Scroll += trackBar8_Scroll;
            numericUpDown7.ValueChanged += numericUpDown7_ValueChanged;
            trackBar7.Scroll += trackBar7_Scroll;
            numericUpDown6.ValueChanged += numericUpDown6_ValueChanged;
            trackBar6.Scroll += trackBar6_Scroll;
            numericUpDown5.ValueChanged += numericUpDown5_ValueChanged;
            trackBar5.Scroll += trackBar5_Scroll;
            // Explosion
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            numericUpDown12.ValueChanged += numericUpDown12_ValueChanged;
            trackBar12.Scroll += trackBar12_Scroll;
            // Cartoon
            checkBox5.CheckedChanged += checkBox5_CheckedChanged;
            numericUpDown11.ValueChanged += numericUpDown11_ValueChanged;
            trackBar9.Scroll += trackBar9_Scroll;
            numericUpDown9.ValueChanged += numericUpDown9_ValueChanged;
            trackBar11.Scroll += trackBar11_Scroll;
            // CRT
            checkBox8.CheckedChanged += checkBox8_CheckedChanged;
            checkBox12.CheckedChanged += checkBox12_CheckedChanged;
            numericUpDown19.ValueChanged += numericUpDown19_ValueChanged;
            trackBar19.Scroll += trackBar19_Scroll;
            numericUpDown18.ValueChanged += numericUpDown18_ValueChanged;
            trackBar18.Value = Convert.ToInt32(SweetFX.CRT.Resolution * (decimal)10.00);
            numericUpDown17.Value = SweetFX.CRT.Gamma;
            trackBar17.Value = Convert.ToInt32(SweetFX.CRT.Gamma * (decimal)10.00);
            numericUpDown16.Value = SweetFX.CRT.Monitor_Gamma;
            trackBar16.Value = Convert.ToInt32(SweetFX.CRT.Monitor_Gamma * (decimal)10.00);
            numericUpDown10.Value = SweetFX.CRT.Brightness;
            trackBar10.Value = Convert.ToInt32(SweetFX.CRT.Brightness * (decimal)10.00);
            numericUpDown13.Value = SweetFX.CRT.Scanline_Intensity;
            trackBar13.Value = Convert.ToInt32(SweetFX.CRT.Scanline_Intensity * (decimal)10.00);
            checkBox10.Checked = SweetFX.CRT.Scanline_Gaussian;
            numericUpDown23.Value = SweetFX.CRT.Corner_Size;
            trackBar23.Value = Convert.ToInt32(SweetFX.CRT.Corner_Size * (decimal)10000.00);
            numericUpDown22.Value = SweetFX.CRT.Distance;
            trackBar22.Value = Convert.ToInt32(SweetFX.CRT.Distance * (decimal)100.00);
            numericUpDown21.Value = SweetFX.CRT.AngleX;
            trackBar21.Value = Convert.ToInt32(SweetFX.CRT.AngleX * (decimal)100.00);
            numericUpDown20.Value = SweetFX.CRT.AngleY;
            trackBar20.Value = Convert.ToInt32(SweetFX.CRT.AngleY * (decimal)100.00);
            numericUpDown15.Value = SweetFX.CRT.Curvature_Radius;
            trackBar15.Value = Convert.ToInt32(SweetFX.CRT.Curvature_Radius * (decimal)10.00);
            numericUpDown14.Value = SweetFX.CRT.Overscan;
            trackBar14.Value = Convert.ToInt32(SweetFX.CRT.Overscan * (decimal)100.00);
            checkBox7.Checked = SweetFX.CRT.Curvature;
            checkBox14.Checked = SweetFX.CRT.Oversample;
            // Bloom
            checkBox16.Checked = SweetFX.Bloom.Enabled;
            numericUpDown26.Value = SweetFX.Bloom.Threshold;
            trackBar26.Value = Convert.ToInt32(SweetFX.Bloom.Threshold * (decimal)100.00);
            numericUpDown28.Value = SweetFX.Bloom.Power;
            trackBar27.Value = Convert.ToInt32(SweetFX.Bloom.Power * (decimal)1000.00);
            numericUpDown27.Value = SweetFX.Bloom.Width;
            trackBar28.Value = Convert.ToInt32(SweetFX.Bloom.Width * (decimal)10000.00);
            // HDR
            checkBox15.Checked = SweetFX.HDR.Enabled;
            numericUpDown25.Value = SweetFX.HDR.Power;
            trackBar24.Value = Convert.ToInt32(SweetFX.HDR.Power * (decimal)100.00);
            numericUpDown24.Value = SweetFX.HDR.Radius;
            trackBar25.Value = Convert.ToInt32(SweetFX.HDR.Radius * (decimal)100.00);
            // LumaSharpen
            checkBox20.Checked = SweetFX.LumaSharpen.Enabled;
            numericUpDown32.Value = SweetFX.LumaSharpen.Strength;
            trackBar32.Value = Convert.ToInt32(SweetFX.LumaSharpen.Strength * (decimal)100.00);
            numericUpDown31.Value = SweetFX.LumaSharpen.Clamp;
            trackBar31.Value = Convert.ToInt32(SweetFX.LumaSharpen.Clamp * (decimal)1000.00);
            numericUpDown30.Value = SweetFX.LumaSharpen.Pattern;
            trackBar30.Value = SweetFX.LumaSharpen.Pattern;
            numericUpDown29.Value = SweetFX.LumaSharpen.Offset_Bias;
            trackBar29.Value = Convert.ToInt32(SweetFX.LumaSharpen.Offset_Bias * (decimal)10.00);
            checkBox19.Checked = SweetFX.LumaSharpen.Show;
            // Levels
            checkBox21.Checked = SweetFX.Levels.Enabled;
            numericUpDown35.Value = SweetFX.Levels.Black_Point;
            trackBar48.Value = SweetFX.Levels.Black_Point;
            numericUpDown33.Value = SweetFX.Levels.White_Point;
            trackBar33.Value = SweetFX.Levels.White_Point;
            // Technicolor
            checkBox23.Checked = SweetFX.Technicolor.Enabled;
            numericUpDown52.Value = SweetFX.Technicolor.Amount;
            trackBar52.Value = Convert.ToInt32(SweetFX.Technicolor.Amount * (decimal)100.00);
            numericUpDown51.Value = SweetFX.Technicolor.Power;
            trackBar51.Value = Convert.ToInt32(SweetFX.Technicolor.Power * (decimal)100.00);
            numericUpDown50.Value = SweetFX.Technicolor.Red_Negative_Amount;
            trackBar50.Value = Convert.ToInt32(SweetFX.Technicolor.Red_Negative_Amount * (decimal)100.00);
            numericUpDown49.Value = SweetFX.Technicolor.Green_Negative_Amount;
            trackBar49.Value = Convert.ToInt32(SweetFX.Technicolor.Green_Negative_Amount * (decimal)100.00);
            numericUpDown48.Value = SweetFX.Technicolor.Blue_Negative_Amount;
            trackBar35.Value = Convert.ToInt32(SweetFX.Technicolor.Blue_Negative_Amount * (decimal)100.00);
        }

        void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }

        void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar19_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {

        }

        private void StopFormCapture()
        {
            // SMAA
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
            // FXAA

        }

        private void SetMessage(string _message)
        {
            SetMessage(new Message(_message));
        }

        private void SetMessage(Message _message)
        {
            if (!timer1.Enabled)
            {
                toolStripStatusLabel1.Text = _message.ToString();
                timer1.Interval = _message.Timeout;
                timer1.Enabled = true;
                timer1.Start();
            }
            else { MessagePump.Add(_message); }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MessagePump.Count > 0)
            {
                toolStripStatusLabel1.Text = MessagePump[0].ToString();
                timer1.Interval = MessagePump[0].Timeout;
                MessagePump.Remove(MessagePump[0]);
            }
            else
            {
                toolStripStatusLabel1.Text = "";
                timer1.Enabled = false;
                timer1.Stop();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!InstallManagerForm.isOpen)
            {
                install_manager_form = new InstallManagerForm();
                install_manager_form.FormClosed += install_manager_form_FormClosed;
                install_manager_form.Show();
            }
            else { install_manager_form.BringToFront(); }
        }

        void install_manager_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            install_manager_form.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //about
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
            this.numericUpDown1.ValueChanged -= new System.EventHandler(this.numericUpDown1_ValueChanged);
            numericUpDown1.Value = (decimal)trackBar1.Value / (decimal)100.00;
            SweetFX.SMAA.Threshold = numericUpDown1.Value;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar1.Scroll -= new System.EventHandler(this.trackBar1_ValueChanged);
            SweetFX.SMAA.Threshold = numericUpDown1.Value * (decimal)100.00;
            trackBar1.Value = Convert.ToInt32(SweetFX.SMAA.Threshold);
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_ValueChanged);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown2.ValueChanged -= new System.EventHandler(this.numericUpDown2_ValueChanged);
            numericUpDown2.Value = trackBar2.Value;
            SweetFX.SMAA.Max_Search_Steps = trackBar2.Value;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
            trackBar2.Value = Convert.ToInt32(numericUpDown2.Value);
            SweetFX.SMAA.Max_Search_Steps = trackBar2.Value;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown3.ValueChanged -= new System.EventHandler(this.numericUpDown3_ValueChanged);
            numericUpDown3.Value = trackBar3.Value;
            SweetFX.SMAA.Max_Search_Steps_Diag = trackBar3.Value;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar3.Scroll -= new System.EventHandler(this.trackBar3_ValueChanged);
            trackBar3.Value = Convert.ToInt32(numericUpDown3.Value);
            SweetFX.SMAA.Max_Search_Steps_Diag = trackBar3.Value;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_ValueChanged);
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown4.ValueChanged -= new System.EventHandler(this.numericUpDown4_ValueChanged);
            numericUpDown4.Value = trackBar4.Value;
            SweetFX.SMAA.Corner_Rounding = trackBar4.Value;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar4.Scroll -= new System.EventHandler(this.trackBar4_ValueChanged);
            trackBar4.Value = Convert.ToInt32(numericUpDown4.Value);
            SweetFX.SMAA.Corner_Rounding = trackBar4.Value;
            this.trackBar4.Scroll += new System.EventHandler(this.trackBar4_ValueChanged);
        }

        #endregion

        #region FXAA

        void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar8_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar7_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar6_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar5_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Explosion

        void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar12_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Cartoon

        void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar9_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar11_Scroll(object sender, EventArgs e)
        {

        }

        #endregion
    }

    public class Message
    {
        private string _action;
        private string _text;
        private int _timeout;

        public Message(string tex)
        {
            _text = tex;
            _action = "";
            _timeout = 5;
        }

        public Message(string tex, string a)
        {
            _text = tex;
            _action = a;
            _timeout = 5;
        }

        public Message(string tex, int t)
        {
            _text = tex;
            _action = "";
            _timeout = t;
        }

        public Message(string tex, string a, int t)
        {
            _text = tex;
            _action = a;
            _timeout = t;
        }

        public string Action { get { return _action; } }

        public string Text { get { return _text; } }

        public int Timeout { get { return _timeout * 1000; } }

        public override string ToString()
        {
            string _final = "";
            if (!String.IsNullOrEmpty(_action)) { _final = _action + ": "; }
            return _final + _text;
        }
    }
}
