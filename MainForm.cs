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
            SweetFX.GameLoaded += SweetFX_GameLoaded;
            if (Settings.LastGame != null && Settings.LastGame.isSweetFXInstalled)
            {
                SweetFX.Load(Settings.LastGame);
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
            //
            Settings.GameAdded += Settings_GameAdded;
        }

        private delegate void InstallManager_GameLoadedD();

        void SweetFX_GameLoaded()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InstallManager_GameLoadedD(SweetFX_GameLoaded));
                return;
            }
            LoadSFXConfig();
            this.Text = "The Unknown - Game loaded: " + Settings.LastGame.Name;
        }

        void Settings_GameAdded(Game _g)
        {
            gamesToolStripMenuItem1.DropDownItems.Add(_g.Name);
        }

        void item_Click(object sender, EventArgs e)
        {
            SweetFX.Load(Settings.GetGame(((ToolStripMenuItem)sender).Text));
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
            SetMessage("Last SweetFX configuration saved on: " + DateTime.Now.ToShortTimeString());
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
            trackBar18.Scroll += trackBar18_Scroll;
            numericUpDown17.ValueChanged += numericUpDown17_ValueChanged;
            trackBar17.Scroll += trackBar17_Scroll;
            numericUpDown16.ValueChanged += numericUpDown16_ValueChanged;
            trackBar16.Scroll += trackBar16_Scroll;
            numericUpDown10.ValueChanged += numericUpDown10_ValueChanged;
            trackBar10.Scroll += trackBar10_Scroll;
            numericUpDown13.ValueChanged += numericUpDown13_ValueChanged;
            trackBar13.Scroll += trackBar13_Scroll;
            checkBox10.CheckedChanged += checkBox10_CheckedChanged;
            numericUpDown23.ValueChanged += numericUpDown23_ValueChanged;
            trackBar23.Scroll += trackBar23_Scroll;
            numericUpDown22.ValueChanged += numericUpDown22_ValueChanged;
            trackBar22.Scroll += trackBar22_Scroll;
            numericUpDown21.ValueChanged += numericUpDown21_ValueChanged;
            trackBar21.Scroll += trackBar21_Scroll;
            numericUpDown20.ValueChanged += numericUpDown20_ValueChanged;
            trackBar20.Scroll += trackBar20_Scroll;
            numericUpDown15.ValueChanged += numericUpDown15_ValueChanged;
            trackBar15.Scroll += trackBar15_Scroll;
            numericUpDown14.ValueChanged += numericUpDown14_ValueChanged;
            trackBar14.Scroll += trackBar14_Scroll;
            checkBox7.CheckedChanged += checkBox7_CheckedChanged;
            checkBox14.CheckedChanged += checkBox14_CheckedChanged;
            // Bloom
            checkBox16.CheckedChanged += checkBox16_CheckedChanged;
            numericUpDown26.ValueChanged += numericUpDown26_ValueChanged;
            trackBar26.Scroll += trackBar26_Scroll;
            numericUpDown28.ValueChanged += numericUpDown28_ValueChanged;
            trackBar27.Scroll += trackBar27_Scroll;
            numericUpDown27.ValueChanged += numericUpDown27_ValueChanged;
            trackBar28.Scroll += trackBar28_Scroll;
            // HDR
            checkBox15.CheckedChanged += checkBox15_CheckedChanged;
            numericUpDown25.ValueChanged += numericUpDown25_ValueChanged;
            trackBar24.Scroll += trackBar24_Scroll;
            numericUpDown24.ValueChanged += numericUpDown24_ValueChanged;
            trackBar25.Scroll += trackBar25_Scroll;
            // LumaSharpen
            checkBox20.CheckedChanged += checkBox20_CheckedChanged;
            numericUpDown32.ValueChanged += numericUpDown32_ValueChanged;
            trackBar32.Scroll += trackBar32_Scroll;
            numericUpDown31.ValueChanged += numericUpDown31_ValueChanged;
            trackBar31.Scroll += trackBar31_Scroll;
            numericUpDown30.ValueChanged += numericUpDown30_ValueChanged;
            trackBar30.Scroll += trackBar30_Scroll;
            numericUpDown29.ValueChanged += numericUpDown29_ValueChanged;
            trackBar29.ValueChanged += trackBar29_ValueChanged;
            checkBox19.CheckedChanged += checkBox19_CheckedChanged;
            // Levels
            checkBox21.CheckedChanged += checkBox21_CheckedChanged;
            numericUpDown35.ValueChanged += numericUpDown35_ValueChanged;
            trackBar48.Scroll += trackBar48_Scroll;
            numericUpDown33.ValueChanged += numericUpDown33_ValueChanged;
            trackBar33.Scroll += trackBar33_Scroll;
            // Technicolor
            checkBox23.CheckedChanged += checkBox23_CheckedChanged;
            numericUpDown52.ValueChanged += numericUpDown52_ValueChanged;
            trackBar52.Scroll += trackBar52_Scroll;
            numericUpDown51.ValueChanged += numericUpDown51_ValueChanged;
            trackBar51.Scroll += trackBar51_Scroll;
            numericUpDown50.ValueChanged += numericUpDown50_ValueChanged;
            trackBar50.Scroll += trackBar50_Scroll;
            numericUpDown49.ValueChanged += numericUpDown49_ValueChanged;
            trackBar49.ValueChanged += trackBar49_ValueChanged;
            numericUpDown48.ValueChanged += numericUpDown48_ValueChanged;
            trackBar35.Scroll += trackBar35_Scroll;
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
            checkBox6.CheckedChanged -= checkBox6_CheckedChanged;
            numericUpDown8.ValueChanged -= numericUpDown8_ValueChanged;
            trackBar8.Scroll -= trackBar8_Scroll;
            numericUpDown7.ValueChanged -= numericUpDown7_ValueChanged;
            trackBar7.Scroll -= trackBar7_Scroll;
            numericUpDown6.ValueChanged -= numericUpDown6_ValueChanged;
            trackBar6.Scroll -= trackBar6_Scroll;
            numericUpDown5.ValueChanged -= numericUpDown5_ValueChanged;
            trackBar5.Scroll -= trackBar5_Scroll;
            // Explosion
            checkBox4.CheckedChanged -= checkBox4_CheckedChanged;
            numericUpDown12.ValueChanged -= numericUpDown12_ValueChanged;
            trackBar12.Scroll -= trackBar12_Scroll;
            // Cartoon
            checkBox5.CheckedChanged -= checkBox5_CheckedChanged;
            numericUpDown11.ValueChanged -= numericUpDown11_ValueChanged;
            trackBar9.Scroll -= trackBar9_Scroll;
            numericUpDown9.ValueChanged -= numericUpDown9_ValueChanged;
            trackBar11.Scroll -= trackBar11_Scroll;
            // CRT
            checkBox8.CheckedChanged -= checkBox8_CheckedChanged;
            checkBox12.CheckedChanged -= checkBox12_CheckedChanged;
            numericUpDown19.ValueChanged -= numericUpDown19_ValueChanged;
            trackBar19.Scroll -= trackBar19_Scroll;
            numericUpDown18.ValueChanged -= numericUpDown18_ValueChanged;
            trackBar18.Scroll -= trackBar18_Scroll;
            numericUpDown17.ValueChanged -= numericUpDown17_ValueChanged;
            trackBar17.Scroll -= trackBar17_Scroll;
            numericUpDown16.ValueChanged -= numericUpDown16_ValueChanged;
            trackBar16.Scroll -= trackBar16_Scroll;
            numericUpDown10.ValueChanged -= numericUpDown10_ValueChanged;
            trackBar10.Scroll -= trackBar10_Scroll;
            numericUpDown13.ValueChanged -= numericUpDown13_ValueChanged;
            trackBar13.Scroll -= trackBar13_Scroll;
            checkBox10.CheckedChanged -= checkBox10_CheckedChanged;
            numericUpDown23.ValueChanged -= numericUpDown23_ValueChanged;
            trackBar23.Scroll -= trackBar23_Scroll;
            numericUpDown22.ValueChanged -= numericUpDown22_ValueChanged;
            trackBar22.Scroll -= trackBar22_Scroll;
            numericUpDown21.ValueChanged -= numericUpDown21_ValueChanged;
            trackBar21.Scroll -= trackBar21_Scroll;
            numericUpDown20.ValueChanged -= numericUpDown20_ValueChanged;
            trackBar20.Scroll -= trackBar20_Scroll;
            numericUpDown15.ValueChanged -= numericUpDown15_ValueChanged;
            trackBar15.Scroll -= trackBar15_Scroll;
            numericUpDown14.ValueChanged -= numericUpDown14_ValueChanged;
            trackBar14.Scroll -= trackBar14_Scroll;
            checkBox7.CheckedChanged -= checkBox7_CheckedChanged;
            checkBox14.CheckedChanged -= checkBox14_CheckedChanged;
            // Bloom
            checkBox16.CheckedChanged -= checkBox16_CheckedChanged;
            numericUpDown26.ValueChanged -= numericUpDown26_ValueChanged;
            trackBar26.Scroll -= trackBar26_Scroll;
            numericUpDown28.ValueChanged -= numericUpDown28_ValueChanged;
            trackBar27.Scroll -= trackBar27_Scroll;
            numericUpDown27.ValueChanged -= numericUpDown27_ValueChanged;
            trackBar28.Scroll -= trackBar28_Scroll;
            // HDR
            checkBox15.CheckedChanged -= checkBox15_CheckedChanged;
            numericUpDown25.ValueChanged -= numericUpDown25_ValueChanged;
            trackBar24.Scroll -= trackBar24_Scroll;
            numericUpDown24.ValueChanged -= numericUpDown24_ValueChanged;
            trackBar25.Scroll -= trackBar25_Scroll;
            // LumaSharpen
            checkBox20.CheckedChanged -= checkBox20_CheckedChanged;
            numericUpDown32.ValueChanged -= numericUpDown32_ValueChanged;
            trackBar32.Scroll -= trackBar32_Scroll;
            numericUpDown31.ValueChanged -= numericUpDown31_ValueChanged;
            trackBar31.Scroll -= trackBar31_Scroll;
            numericUpDown30.ValueChanged -= numericUpDown30_ValueChanged;
            trackBar30.Scroll -= trackBar30_Scroll;
            numericUpDown29.ValueChanged -= numericUpDown29_ValueChanged;
            trackBar29.ValueChanged -= trackBar29_ValueChanged;
            checkBox19.CheckedChanged -= checkBox19_CheckedChanged;
            // Levels
            checkBox21.CheckedChanged -= checkBox21_CheckedChanged;
            numericUpDown35.ValueChanged -= numericUpDown35_ValueChanged;
            trackBar48.Scroll -= trackBar48_Scroll;
            numericUpDown33.ValueChanged -= numericUpDown33_ValueChanged;
            trackBar33.Scroll -= trackBar33_Scroll;
            // Technicolor
            checkBox23.CheckedChanged -= checkBox23_CheckedChanged;
            numericUpDown52.ValueChanged -= numericUpDown52_ValueChanged;
            trackBar52.Scroll -= trackBar52_Scroll;
            numericUpDown51.ValueChanged -= numericUpDown51_ValueChanged;
            trackBar51.Scroll -= trackBar51_Scroll;
            numericUpDown50.ValueChanged -= numericUpDown50_ValueChanged;
            trackBar50.Scroll -= trackBar50_Scroll;
            numericUpDown49.ValueChanged -= numericUpDown49_ValueChanged;
            trackBar49.ValueChanged -= trackBar49_ValueChanged;
            numericUpDown48.ValueChanged -= numericUpDown48_ValueChanged;
            trackBar35.Scroll -= trackBar35_Scroll;
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
            SweetFX.FXAA.Enabled = checkBox6.Checked;
        }

        void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar8.Scroll -= new System.EventHandler(this.trackBar8_Scroll);
            SweetFX.FXAA.Quality_Preset = Convert.ToInt32(numericUpDown8.Value);
            trackBar8.Value = SweetFX.FXAA.Quality_Preset;
            this.trackBar8.Scroll += new System.EventHandler(this.trackBar8_Scroll);
        }

        void trackBar8_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown8.ValueChanged -= new System.EventHandler(this.numericUpDown8_ValueChanged);
            numericUpDown8.Value = trackBar8.Value;
            SweetFX.FXAA.Quality_Preset = trackBar8.Value;
            this.numericUpDown8.ValueChanged += new System.EventHandler(this.numericUpDown8_ValueChanged);
        }

        void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar8.Scroll -= new System.EventHandler(this.trackBar8_Scroll);
            SweetFX.FXAA.Subpix = numericUpDown7.Value;
            trackBar8.Value = Convert.ToInt32(numericUpDown7.Value * (decimal)1000);
            this.trackBar8.Scroll += new System.EventHandler(this.trackBar8_Scroll);
        }

        void trackBar7_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown7.ValueChanged -= new System.EventHandler(this.numericUpDown7_ValueChanged);
            numericUpDown7.Value = (decimal)trackBar7.Value / (decimal)1000;
            SweetFX.FXAA.Subpix = numericUpDown7.Value;
            this.numericUpDown7.ValueChanged += new System.EventHandler(this.numericUpDown7_ValueChanged);
        }

        void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar6.Scroll -= new System.EventHandler(this.trackBar6_Scroll);
            SweetFX.FXAA.Edge_Threshold = numericUpDown6.Value;
            trackBar6.Value = Convert.ToInt32(numericUpDown6.Value * (decimal)1000);
            this.trackBar6.Scroll += new System.EventHandler(this.trackBar6_Scroll);
        }

        void trackBar6_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown6.ValueChanged -= new System.EventHandler(this.numericUpDown6_ValueChanged);
            numericUpDown6.Value = (decimal)trackBar6.Value / (decimal)1000;
            SweetFX.FXAA.Edge_Threshold = numericUpDown6.Value;
            this.numericUpDown6.ValueChanged += new System.EventHandler(this.numericUpDown6_ValueChanged);
        }

        void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar5.Scroll -= new System.EventHandler(this.trackBar5_Scroll);
            SweetFX.FXAA.Edge_Threshold_Min = numericUpDown5.Value;
            trackBar5.Value = Convert.ToInt32(numericUpDown5.Value * (decimal)1000);
            this.trackBar5.Scroll += new System.EventHandler(this.trackBar5_Scroll);
        }

        void trackBar5_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown5.ValueChanged -= new System.EventHandler(this.numericUpDown5_ValueChanged);
            numericUpDown5.Value = (decimal)trackBar5.Value / (decimal)1000;
            SweetFX.FXAA.Edge_Threshold_Min = numericUpDown5.Value;
            this.numericUpDown5.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
        }

        #endregion

        #region Explosion

        void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.Explosion.Enabled = checkBox4.Checked;
        }

        void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown12.ValueChanged -= new System.EventHandler(this.numericUpDown12_ValueChanged);
            trackBar12.Value = Convert.ToInt32(numericUpDown12.Value / (decimal)10);
            SweetFX.Explosion.Radius = numericUpDown12.Value;
            this.numericUpDown12.ValueChanged += new System.EventHandler(this.numericUpDown12_ValueChanged);
        }

        void trackBar12_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown12.ValueChanged -= new System.EventHandler(this.numericUpDown12_ValueChanged);
            numericUpDown12.Value = (decimal)trackBar12.Value / (decimal)10;
            SweetFX.Explosion.Radius = numericUpDown12.Value;
            this.numericUpDown12.ValueChanged += new System.EventHandler(this.numericUpDown12_ValueChanged);
        }

        #endregion

        #region Cartoon

        void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.Cartoon.Enabled = checkBox5.Checked;
        }

        void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown11.ValueChanged -= new System.EventHandler(this.numericUpDown11_ValueChanged);
            trackBar9.Value = Convert.ToInt32(numericUpDown11.Value / (decimal)10);
            SweetFX.Cartoon.Power = numericUpDown11.Value;
            this.numericUpDown11.ValueChanged += new System.EventHandler(this.numericUpDown11_ValueChanged);
        }

        void trackBar9_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown11.ValueChanged -= new System.EventHandler(this.numericUpDown11_ValueChanged);
            numericUpDown11.Value = (decimal)trackBar9.Value / (decimal)10;
            SweetFX.Cartoon.Power = numericUpDown11.Value;
            this.numericUpDown11.ValueChanged += new System.EventHandler(this.numericUpDown11_ValueChanged);
        }

        void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown9.ValueChanged -= new System.EventHandler(this.numericUpDown9_ValueChanged);
            trackBar11.Value = Convert.ToInt32(numericUpDown9.Value / (decimal)10);
            SweetFX.Cartoon.Edge_Slope = numericUpDown9.Value;
            this.numericUpDown9.ValueChanged += new System.EventHandler(this.numericUpDown9_ValueChanged);
        }

        void trackBar11_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown9.ValueChanged -= new System.EventHandler(this.numericUpDown9_ValueChanged);
            numericUpDown9.Value = (decimal)trackBar11.Value / (decimal)10;
            SweetFX.Cartoon.Edge_Slope = numericUpDown9.Value;
            this.numericUpDown9.ValueChanged += new System.EventHandler(this.numericUpDown9_ValueChanged);
        }

        #endregion

        #region CRT

        void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.CRT.Enabled = checkBox8.Checked;
            checkBox8.Checked = checkBox12.Checked;
        }

        void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.CRT.Enabled = checkBox12.Checked;
            checkBox12.Checked = checkBox8.Checked;
        }

        void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown19.ValueChanged -= new System.EventHandler(this.numericUpDown19_ValueChanged);
            trackBar19.Value = Convert.ToInt32(numericUpDown19.Value / (decimal)100);
            SweetFX.CRT.Amount = numericUpDown19.Value;
            this.numericUpDown9.ValueChanged += new System.EventHandler(this.numericUpDown19_ValueChanged);
        }

        void trackBar19_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown9.ValueChanged -= new System.EventHandler(this.numericUpDown19_ValueChanged);
            numericUpDown19.Value = (decimal)trackBar19.Value / (decimal)100;
            SweetFX.CRT.Amount = numericUpDown19.Value;
            this.numericUpDown19.ValueChanged += new System.EventHandler(this.numericUpDown19_ValueChanged);
        }

        void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown18.ValueChanged -= new System.EventHandler(this.numericUpDown18_ValueChanged);
            trackBar18.Value = Convert.ToInt32(numericUpDown18.Value / (decimal)10);
            SweetFX.CRT.Resolution = numericUpDown18.Value;
            this.numericUpDown18.ValueChanged += new System.EventHandler(this.numericUpDown18_ValueChanged);
        }

        void trackBar18_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown18.ValueChanged -= new System.EventHandler(this.numericUpDown18_ValueChanged);
            numericUpDown18.Value = (decimal)trackBar18.Value / (decimal)10;
            SweetFX.CRT.Resolution = numericUpDown18.Value;
            this.numericUpDown18.ValueChanged += new System.EventHandler(this.numericUpDown18_ValueChanged);
        }

        void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown17.ValueChanged -= new System.EventHandler(this.numericUpDown17_ValueChanged);
            trackBar17.Value = Convert.ToInt32(numericUpDown17.Value / (decimal)10);
            SweetFX.CRT.Gamma = numericUpDown17.Value;
            this.numericUpDown17.ValueChanged += new System.EventHandler(this.numericUpDown17_ValueChanged);
        }

        void trackBar17_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown17.ValueChanged -= new System.EventHandler(this.numericUpDown17_ValueChanged);
            numericUpDown17.Value = (decimal)trackBar17.Value / (decimal)10;
            SweetFX.CRT.Gamma = numericUpDown17.Value;
            this.numericUpDown17.ValueChanged += new System.EventHandler(this.numericUpDown17_ValueChanged);
        }

        void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown16.ValueChanged -= new System.EventHandler(this.numericUpDown16_ValueChanged);
            trackBar16.Value = Convert.ToInt32(numericUpDown16.Value / (decimal)10);
            SweetFX.CRT.Monitor_Gamma = numericUpDown16.Value;
            this.numericUpDown16.ValueChanged += new System.EventHandler(this.numericUpDown16_ValueChanged);
        }

        void trackBar16_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown16.ValueChanged -= new System.EventHandler(this.numericUpDown16_ValueChanged);
            numericUpDown16.Value = (decimal)trackBar16.Value / (decimal)10;
            SweetFX.CRT.Monitor_Gamma = numericUpDown16.Value;
            this.numericUpDown16.ValueChanged += new System.EventHandler(this.numericUpDown16_ValueChanged);
        }

        void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown10.ValueChanged -= new System.EventHandler(this.numericUpDown10_ValueChanged);
            trackBar10.Value = Convert.ToInt32(numericUpDown10.Value / (decimal)10);
            SweetFX.CRT.Brightness = numericUpDown10.Value;
            this.numericUpDown10.ValueChanged += new System.EventHandler(this.numericUpDown10_ValueChanged);
        }

        void trackBar10_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown10.ValueChanged -= new System.EventHandler(this.numericUpDown10_ValueChanged);
            numericUpDown10.Value = (decimal)trackBar10.Value / (decimal)10;
            SweetFX.CRT.Brightness = numericUpDown10.Value;
            this.numericUpDown10.ValueChanged += new System.EventHandler(this.numericUpDown10_ValueChanged);
        }

        void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown13.ValueChanged -= new System.EventHandler(this.numericUpDown13_ValueChanged);
            trackBar13.Value = Convert.ToInt32(numericUpDown13.Value / (decimal)10);
            SweetFX.CRT.Scanline_Intensity = numericUpDown13.Value;
            this.numericUpDown13.ValueChanged += new System.EventHandler(this.numericUpDown13_ValueChanged);
        }

        void trackBar13_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown13.ValueChanged -= new System.EventHandler(this.numericUpDown13_ValueChanged);
            numericUpDown13.Value = (decimal)trackBar13.Value / (decimal)10;
            SweetFX.CRT.Scanline_Intensity = numericUpDown13.Value;
            this.numericUpDown13.ValueChanged += new System.EventHandler(this.numericUpDown13_ValueChanged);
        }

        void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.CRT.Scanline_Gaussian = checkBox10.Checked;
        }

        void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown23.ValueChanged -= new System.EventHandler(this.numericUpDown23_ValueChanged);
            trackBar23.Value = Convert.ToInt32(numericUpDown23.Value / (decimal)1000);
            SweetFX.CRT.Corner_Size = numericUpDown23.Value;
            this.numericUpDown23.ValueChanged += new System.EventHandler(this.numericUpDown23_ValueChanged);
        }

        void trackBar23_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown23.ValueChanged -= new System.EventHandler(this.numericUpDown23_ValueChanged);
            numericUpDown23.Value = (decimal)trackBar23.Value / (decimal)10000;
            SweetFX.CRT.Corner_Size = numericUpDown23.Value;
            this.numericUpDown23.ValueChanged += new System.EventHandler(this.numericUpDown23_ValueChanged);
        }

        void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown22.ValueChanged -= new System.EventHandler(this.numericUpDown22_ValueChanged);
            trackBar22.Value = Convert.ToInt32(numericUpDown22.Value / (decimal)100);
            SweetFX.CRT.Distance = numericUpDown22.Value;
            this.numericUpDown22.ValueChanged += new System.EventHandler(this.numericUpDown22_ValueChanged);
        }

        void trackBar22_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown22.ValueChanged -= new System.EventHandler(this.numericUpDown22_ValueChanged);
            numericUpDown22.Value = (decimal)trackBar22.Value / (decimal)100;
            SweetFX.CRT.Distance = numericUpDown22.Value;
            this.numericUpDown22.ValueChanged += new System.EventHandler(this.numericUpDown22_ValueChanged);
        }

        void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown21.ValueChanged -= new System.EventHandler(this.numericUpDown21_ValueChanged);
            trackBar21.Value = Convert.ToInt32(numericUpDown21.Value / (decimal)100);
            SweetFX.CRT.AngleX = numericUpDown21.Value;
            this.numericUpDown21.ValueChanged += new System.EventHandler(this.numericUpDown21_ValueChanged);
        }

        void trackBar21_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown21.ValueChanged -= new System.EventHandler(this.numericUpDown21_ValueChanged);
            numericUpDown21.Value = (decimal)trackBar21.Value / (decimal)100;
            SweetFX.CRT.AngleX = numericUpDown21.Value;
            this.numericUpDown21.ValueChanged += new System.EventHandler(this.numericUpDown21_ValueChanged);
        }

        void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown20.ValueChanged -= new System.EventHandler(this.numericUpDown20_ValueChanged);
            trackBar20.Value = Convert.ToInt32(numericUpDown20.Value / (decimal)100);
            SweetFX.CRT.AngleX = numericUpDown20.Value;
            this.numericUpDown20.ValueChanged += new System.EventHandler(this.numericUpDown20_ValueChanged);
        }

        void trackBar20_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown20.ValueChanged -= new System.EventHandler(this.numericUpDown20_ValueChanged);
            numericUpDown20.Value = (decimal)trackBar20.Value / (decimal)100;
            SweetFX.CRT.AngleX = numericUpDown20.Value;
            this.numericUpDown20.ValueChanged += new System.EventHandler(this.numericUpDown20_ValueChanged);
        }

        void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown15.ValueChanged -= new System.EventHandler(this.numericUpDown15_ValueChanged);
            trackBar15.Value = Convert.ToInt32(numericUpDown15.Value / (decimal)10);
            SweetFX.CRT.Curvature_Radius = numericUpDown15.Value;
            this.numericUpDown15.ValueChanged += new System.EventHandler(this.numericUpDown15_ValueChanged);
        }

        void trackBar15_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown15.ValueChanged -= new System.EventHandler(this.numericUpDown15_ValueChanged);
            numericUpDown15.Value = (decimal)trackBar15.Value / (decimal)10;
            SweetFX.CRT.Curvature_Radius = numericUpDown15.Value;
            this.numericUpDown15.ValueChanged += new System.EventHandler(this.numericUpDown15_ValueChanged);
        }

        void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown14.ValueChanged -= new System.EventHandler(this.numericUpDown14_ValueChanged);
            trackBar14.Value = Convert.ToInt32(numericUpDown14.Value / (decimal)100);
            SweetFX.CRT.Overscan = numericUpDown14.Value;
            this.numericUpDown14.ValueChanged += new System.EventHandler(this.numericUpDown14_ValueChanged);
        }

        void trackBar14_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown14.ValueChanged -= new System.EventHandler(this.numericUpDown14_ValueChanged);
            numericUpDown14.Value = (decimal)trackBar14.Value / (decimal)100;
            SweetFX.CRT.Overscan = numericUpDown14.Value;
            this.numericUpDown14.ValueChanged += new System.EventHandler(this.numericUpDown14_ValueChanged);
        }

        void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.CRT.Curvature = checkBox7.Checked;
        }

        void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.CRT.Oversample = checkBox14.Checked;
        }

        #endregion

        #region Bloom

        void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.Bloom.Enabled = checkBox16.Checked;
        }

        void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown26.ValueChanged -= new System.EventHandler(this.numericUpDown26_ValueChanged);
            trackBar26.Value = Convert.ToInt32(numericUpDown26.Value / (decimal)100);
            SweetFX.Bloom.Threshold = numericUpDown26.Value;
            this.numericUpDown26.ValueChanged += new System.EventHandler(this.numericUpDown26_ValueChanged);
        }

        void trackBar26_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown26.ValueChanged -= new System.EventHandler(this.numericUpDown26_ValueChanged);
            numericUpDown26.Value = (decimal)trackBar26.Value / (decimal)100;
            SweetFX.Bloom.Threshold = numericUpDown26.Value;
            this.numericUpDown26.ValueChanged += new System.EventHandler(this.numericUpDown26_ValueChanged);
        }

        void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown28.ValueChanged -= new System.EventHandler(this.numericUpDown28_ValueChanged);
            trackBar27.Value = Convert.ToInt32(numericUpDown28.Value / (decimal)1000);
            SweetFX.Bloom.Power = numericUpDown28.Value;
            this.numericUpDown28.ValueChanged += new System.EventHandler(this.numericUpDown28_ValueChanged);
        }

        void trackBar27_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown28.ValueChanged -= new System.EventHandler(this.numericUpDown28_ValueChanged);
            numericUpDown28.Value = (decimal)trackBar27.Value / (decimal)1000;
            SweetFX.Bloom.Power = numericUpDown28.Value;
            this.numericUpDown28.ValueChanged += new System.EventHandler(this.numericUpDown28_ValueChanged);
        }

        void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown27.ValueChanged -= new System.EventHandler(this.numericUpDown27_ValueChanged);
            trackBar28.Value = Convert.ToInt32(numericUpDown27.Value / (decimal)10000);
            SweetFX.Bloom.Width = numericUpDown27.Value;
            this.numericUpDown27.ValueChanged += new System.EventHandler(this.numericUpDown27_ValueChanged);
        }

        void trackBar28_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown27.ValueChanged -= new System.EventHandler(this.numericUpDown27_ValueChanged);
            numericUpDown27.Value = (decimal)trackBar28.Value / (decimal)10000;
            SweetFX.Bloom.Width = numericUpDown27.Value;
            this.numericUpDown27.ValueChanged += new System.EventHandler(this.numericUpDown27_ValueChanged);
        }

        #endregion

        #region HDR

        void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.HDR.Enabled = checkBox15.Checked;
        }

        void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown25.ValueChanged -= new System.EventHandler(this.numericUpDown25_ValueChanged);
            trackBar24.Value = Convert.ToInt32(numericUpDown25.Value / (decimal)100);
            SweetFX.HDR.Power = numericUpDown25.Value;
            this.numericUpDown25.ValueChanged += new System.EventHandler(this.numericUpDown25_ValueChanged);
        }

        void trackBar24_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown25.ValueChanged -= new System.EventHandler(this.numericUpDown25_ValueChanged);
            numericUpDown25.Value = (decimal)trackBar24.Value / (decimal)100;
            SweetFX.HDR.Power = numericUpDown25.Value;
            this.numericUpDown25.ValueChanged += new System.EventHandler(this.numericUpDown25_ValueChanged);
        }

        void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown24.ValueChanged -= new System.EventHandler(this.numericUpDown24_ValueChanged);
            trackBar25.Value = Convert.ToInt32(numericUpDown24.Value / (decimal)100);
            SweetFX.HDR.Radius = numericUpDown24.Value;
            this.numericUpDown24.ValueChanged += new System.EventHandler(this.numericUpDown24_ValueChanged);
        }

        void trackBar25_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown24.ValueChanged -= new System.EventHandler(this.numericUpDown24_ValueChanged);
            numericUpDown24.Value = (decimal)trackBar25.Value / (decimal)100;
            SweetFX.HDR.Radius = numericUpDown24.Value;
            this.numericUpDown24.ValueChanged += new System.EventHandler(this.numericUpDown24_ValueChanged);
        }

        #endregion

        #region LumaSharpen

        void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.LumaSharpen.Enabled = checkBox20.Checked;
        }

        void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown32.ValueChanged -= new System.EventHandler(this.numericUpDown32_ValueChanged);
            trackBar32.Value = Convert.ToInt32(numericUpDown32.Value / (decimal)100);
            SweetFX.LumaSharpen.Strength = numericUpDown32.Value;
            this.numericUpDown32.ValueChanged += new System.EventHandler(this.numericUpDown32_ValueChanged);
        }

        void trackBar32_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown32.ValueChanged -= new System.EventHandler(this.numericUpDown32_ValueChanged);
            numericUpDown32.Value = (decimal)trackBar32.Value / (decimal)100;
            SweetFX.LumaSharpen.Strength = numericUpDown32.Value;
            this.numericUpDown32.ValueChanged += new System.EventHandler(this.numericUpDown32_ValueChanged);
        }

        void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown31.ValueChanged -= new System.EventHandler(this.numericUpDown31_ValueChanged);
            trackBar31.Value = Convert.ToInt32(numericUpDown31.Value / (decimal)1000);
            SweetFX.LumaSharpen.Clamp = numericUpDown31.Value;
            this.numericUpDown31.ValueChanged += new System.EventHandler(this.numericUpDown31_ValueChanged);
        }

        void trackBar31_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown31.ValueChanged -= new System.EventHandler(this.numericUpDown31_ValueChanged);
            numericUpDown31.Value = (decimal)trackBar31.Value / (decimal)1000;
            SweetFX.LumaSharpen.Clamp = numericUpDown31.Value;
            this.numericUpDown31.ValueChanged += new System.EventHandler(this.numericUpDown31_ValueChanged);
        }

        void numericUpDown30_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown30.ValueChanged -= new System.EventHandler(this.numericUpDown30_ValueChanged);
            trackBar30.Value = Convert.ToInt32(numericUpDown30.Value);
            SweetFX.LumaSharpen.Pattern = trackBar30.Value;
            this.numericUpDown30.ValueChanged += new System.EventHandler(this.numericUpDown30_ValueChanged);
        }

        void trackBar30_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown30.ValueChanged -= new System.EventHandler(this.numericUpDown30_ValueChanged);
            numericUpDown30.Value = (decimal)trackBar30.Value;
            SweetFX.LumaSharpen.Pattern = trackBar30.Value;
            this.numericUpDown30.ValueChanged += new System.EventHandler(this.numericUpDown30_ValueChanged);
        }

        void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown29.ValueChanged -= new System.EventHandler(this.numericUpDown29_ValueChanged);
            trackBar29.Value = Convert.ToInt32(numericUpDown29.Value / (decimal)10);
            SweetFX.LumaSharpen.Offset_Bias = numericUpDown29.Value;
            this.numericUpDown29.ValueChanged += new System.EventHandler(this.numericUpDown29_ValueChanged);
        }

        void trackBar29_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown29.ValueChanged -= new System.EventHandler(this.numericUpDown29_ValueChanged);
            numericUpDown29.Value = (decimal)trackBar29.Value / (decimal)10;
            SweetFX.LumaSharpen.Offset_Bias = numericUpDown29.Value;
            this.numericUpDown29.ValueChanged += new System.EventHandler(this.numericUpDown29_ValueChanged);
        }

        void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.LumaSharpen.Show = checkBox19.Checked;
        }

        #endregion

        #region Levels

        void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.Levels.Enabled = checkBox21.Checked;
        }

        void numericUpDown35_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown35.ValueChanged -= new System.EventHandler(this.numericUpDown35_ValueChanged);
            trackBar48.Value = Convert.ToInt32(numericUpDown35.Value);
            SweetFX.Levels.Black_Point = trackBar48.Value;
            this.numericUpDown35.ValueChanged += new System.EventHandler(this.numericUpDown35_ValueChanged);
        }

        void trackBar48_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown35.ValueChanged -= new System.EventHandler(this.numericUpDown35_ValueChanged);
            numericUpDown35.Value = (decimal)trackBar48.Value;
            SweetFX.Levels.Black_Point = trackBar48.Value;
            this.numericUpDown35.ValueChanged += new System.EventHandler(this.numericUpDown35_ValueChanged);
        }

        void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown33.ValueChanged -= new System.EventHandler(this.numericUpDown33_ValueChanged);
            trackBar33.Value = Convert.ToInt32(numericUpDown33.Value);
            SweetFX.Levels.White_Point = trackBar33.Value;
            this.numericUpDown33.ValueChanged += new System.EventHandler(this.numericUpDown33_ValueChanged);
        }

        void trackBar33_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown33.ValueChanged -= new System.EventHandler(this.numericUpDown33_ValueChanged);
            numericUpDown33.Value = (decimal)trackBar33.Value;
            SweetFX.Levels.White_Point = trackBar33.Value;
            this.numericUpDown33.ValueChanged += new System.EventHandler(this.numericUpDown33_ValueChanged);
        }

        #endregion

        #region Technicolor

        void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.Technicolor.Enabled = checkBox23.Checked;
        }

        void numericUpDown52_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown52.ValueChanged -= new System.EventHandler(this.numericUpDown52_ValueChanged);
            trackBar52.Value = Convert.ToInt32(numericUpDown52.Value / (decimal)100);
            SweetFX.Technicolor.Amount = numericUpDown52.Value;
            this.numericUpDown52.ValueChanged += new System.EventHandler(this.numericUpDown52_ValueChanged);
        }

        void trackBar52_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown52.ValueChanged -= new System.EventHandler(this.numericUpDown52_ValueChanged);
            numericUpDown52.Value = (decimal)trackBar52.Value / (decimal)100;
            SweetFX.Technicolor.Amount = numericUpDown52.Value;
            this.numericUpDown52.ValueChanged += new System.EventHandler(this.numericUpDown52_ValueChanged);
        }

        void numericUpDown51_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown51.ValueChanged -= new System.EventHandler(this.numericUpDown51_ValueChanged);
            trackBar51.Value = Convert.ToInt32(numericUpDown51.Value / (decimal)100);
            SweetFX.Technicolor.Power = numericUpDown51.Value;
            this.numericUpDown51.ValueChanged += new System.EventHandler(this.numericUpDown51_ValueChanged);
        }

        void trackBar51_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown51.ValueChanged -= new System.EventHandler(this.numericUpDown51_ValueChanged);
            numericUpDown51.Value = (decimal)trackBar51.Value / (decimal)100;
            SweetFX.Technicolor.Power= numericUpDown51.Value;
            this.numericUpDown51.ValueChanged += new System.EventHandler(this.numericUpDown51_ValueChanged);
        }

        void numericUpDown50_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown50.ValueChanged -= new System.EventHandler(this.numericUpDown50_ValueChanged);
            trackBar50.Value = Convert.ToInt32(numericUpDown50.Value / (decimal)100);
            SweetFX.Technicolor.Red_Negative_Amount = numericUpDown50.Value;
            this.numericUpDown50.ValueChanged += new System.EventHandler(this.numericUpDown50_ValueChanged);
        }

        void trackBar50_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown50.ValueChanged -= new System.EventHandler(this.numericUpDown50_ValueChanged);
            numericUpDown50.Value = (decimal)trackBar50.Value / (decimal)100;
            SweetFX.Technicolor.Red_Negative_Amount = numericUpDown50.Value;
            this.numericUpDown50.ValueChanged += new System.EventHandler(this.numericUpDown50_ValueChanged);
        }

        void numericUpDown49_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown49.ValueChanged -= new System.EventHandler(this.numericUpDown49_ValueChanged);
            trackBar49.Value = Convert.ToInt32(numericUpDown49.Value / (decimal)100);
            SweetFX.Technicolor.Green_Negative_Amount = numericUpDown49.Value;
            this.numericUpDown49.ValueChanged += new System.EventHandler(this.numericUpDown49_ValueChanged);
        }

        void trackBar49_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown49.ValueChanged -= new System.EventHandler(this.numericUpDown49_ValueChanged);
            numericUpDown49.Value = (decimal)trackBar49.Value / (decimal)100;
            SweetFX.Technicolor.Green_Negative_Amount = numericUpDown49.Value;
            this.numericUpDown49.ValueChanged += new System.EventHandler(this.numericUpDown49_ValueChanged);
        }

        void numericUpDown48_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown48.ValueChanged -= new System.EventHandler(this.numericUpDown48_ValueChanged);
            trackBar35.Value = Convert.ToInt32(numericUpDown48.Value / (decimal)100);
            SweetFX.Technicolor.Blue_Negative_Amount = numericUpDown49.Value;
            this.numericUpDown48.ValueChanged += new System.EventHandler(this.numericUpDown48_ValueChanged);
        }

        void trackBar35_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown48.ValueChanged -= new System.EventHandler(this.numericUpDown48_ValueChanged);
            numericUpDown48.Value = (decimal)trackBar35.Value / (decimal)100;
            SweetFX.Technicolor.Blue_Negative_Amount = numericUpDown49.Value;
            this.numericUpDown48.ValueChanged += new System.EventHandler(this.numericUpDown48_ValueChanged);
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
