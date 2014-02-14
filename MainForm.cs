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
            // DPX
            checkBox9.Checked = SweetFX.Cineon_DPX.Enabled;
            numericUpDown39.Value = SweetFX.Cineon_DPX.Color_Gamma;
            trackBar39.Value = Convert.ToInt32(SweetFX.Cineon_DPX.Color_Gamma * (decimal)10.00);
            numericUpDown38.Value = SweetFX.Cineon_DPX.Saturation;
            trackBar38.Value = Convert.ToInt32(SweetFX.Cineon_DPX.Saturation * (decimal)10.00);
            numericUpDown36.Value = SweetFX.Cineon_DPX.Blend;
            trackBar36.Value = Convert.ToInt32(SweetFX.Cineon_DPX.Blend * (decimal)100.00);
            numericUpDown34.Value = SweetFX.Cineon_DPX.Red;
            trackBar34.Value = Convert.ToInt32(SweetFX.Cineon_DPX.Red * (decimal)100.00);
            numericUpDown37.Value = SweetFX.Cineon_DPX.Green;
            trackBar37.Value = Convert.ToInt32(SweetFX.Cineon_DPX.Green * (decimal)100.00);
            numericUpDown40.Value = SweetFX.Cineon_DPX.Blue;
            trackBar40.Value = Convert.ToInt32(SweetFX.Cineon_DPX.Blue * (decimal)100.00);
            numericUpDown41.Value = SweetFX.Cineon_DPX.RedC;
            trackBar41.Value = Convert.ToInt32(SweetFX.Cineon_DPX.RedC * (decimal)100.00);
            numericUpDown42.Value = SweetFX.Cineon_DPX.GreenC;
            trackBar42.Value = Convert.ToInt32(SweetFX.Cineon_DPX.GreenC * (decimal)100.00);
            numericUpDown43.Value = SweetFX.Cineon_DPX.BlueC;
            trackBar43.Value = Convert.ToInt32(SweetFX.Cineon_DPX.BlueC * (decimal)100.00);
            // Monochrome
            checkBox11.Checked = SweetFX.Monochrome.Enabled;
            numericUpDown54.Value = SweetFX.Monochrome.Conversion_Values.Red;
            trackBar54.Value = Convert.ToInt32(SweetFX.Monochrome.Conversion_Values.Red * (decimal)100.00);
            numericUpDown44.Value = SweetFX.Monochrome.Conversion_Values.Green;
            trackBar44.Value = Convert.ToInt32(SweetFX.Monochrome.Conversion_Values.Green * (decimal)100.00);
            numericUpDown45.Value = SweetFX.Monochrome.Conversion_Values.Blue;
            trackBar45.Value = Convert.ToInt32(SweetFX.Monochrome.Conversion_Values.Blue * (decimal)100.00);
            // Lift Gamma Gain
            checkBox13.Checked = SweetFX.Lift_Gamma_Gain.Enabled;
            numericUpDown53.Value = SweetFX.Lift_Gamma_Gain.Lift.Red;
            trackBar53.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Lift.Red * (decimal)1000);
            numericUpDown46.Value = SweetFX.Lift_Gamma_Gain.Lift.Green;
            trackBar46.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Lift.Green * (decimal)1000);
            numericUpDown47.Value = SweetFX.Lift_Gamma_Gain.Lift.Blue;
            trackBar47.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Lift.Blue * (decimal)1000);
            numericUpDown57.Value = SweetFX.Lift_Gamma_Gain.Gamma.Red;
            trackBar57.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Gamma.Red * (decimal)1000);
            numericUpDown56.Value = SweetFX.Lift_Gamma_Gain.Gamma.Green;
            trackBar56.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Gamma.Green * (decimal)1000);
            numericUpDown55.Value = SweetFX.Lift_Gamma_Gain.Gamma.Blue;
            trackBar55.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Gamma.Blue * (decimal)1000);
            numericUpDown60.Value = SweetFX.Lift_Gamma_Gain.Gain.Red;
            trackBar60.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Gain.Red * (decimal)1000);
            numericUpDown59.Value = SweetFX.Lift_Gamma_Gain.Gain.Green;
            trackBar59.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Gain.Green * (decimal)1000);
            numericUpDown58.Value = SweetFX.Lift_Gamma_Gain.Gain.Blue;
            trackBar58.Value = Convert.ToInt32(SweetFX.Lift_Gamma_Gain.Gain.Blue * (decimal)1000);
            // Tonemap
            checkBox17.Checked = SweetFX.Tonemap.Enabled;
            numericUpDown69.Value = SweetFX.Tonemap.Gamma;
            trackBar69.Value = Convert.ToInt32(SweetFX.Tonemap.Gamma * (decimal)1000);
            numericUpDown67.Value = SweetFX.Tonemap.Exposure;
            trackBar67.Value = Convert.ToInt32(SweetFX.Tonemap.Exposure * (decimal)1000);
            numericUpDown61.Value = SweetFX.Tonemap.Bleach;
            trackBar61.Value = Convert.ToInt32(SweetFX.Tonemap.Bleach * (decimal)1000);
            numericUpDown63.Value = SweetFX.Tonemap.Defog;
            trackBar63.Value = Convert.ToInt32(SweetFX.Tonemap.Defog * (decimal)1000);
            numericUpDown62.Value = SweetFX.Tonemap.Saturation;
            trackBar62.Value = Convert.ToInt32(SweetFX.Tonemap.Saturation * (decimal)1000);
            numericUpDown66.Value = SweetFX.Tonemap.Fog_Color.Red;
            trackBar66.Value = Convert.ToInt32(SweetFX.Tonemap.Fog_Color.Red * (decimal)100);
            numericUpDown64.Value = SweetFX.Tonemap.Fog_Color.Green;
            trackBar64.Value = Convert.ToInt32(SweetFX.Tonemap.Fog_Color.Green * (decimal)100);
            numericUpDown65.Value = SweetFX.Tonemap.Fog_Color.Blue;
            trackBar65.Value = Convert.ToInt32(SweetFX.Tonemap.Fog_Color.Blue * (decimal)100);
            // Vibrance
            checkBox18.Checked = SweetFX.Vibrance.Enabled;
            numericUpDown76.Value = SweetFX.Vibrance.Vibrance;
            trackBar76.Value = Convert.ToInt32(SweetFX.Vibrance.Vibrance * (decimal)1000);
            numericUpDown74.Value = SweetFX.Vibrance.RGB_Balance.Red;
            trackBar74.Value = Convert.ToInt32(SweetFX.Vibrance.RGB_Balance.Red * (decimal)1000);
            numericUpDown68.Value = SweetFX.Vibrance.RGB_Balance.Green;
            trackBar68.Value = Convert.ToInt32(SweetFX.Vibrance.RGB_Balance.Green * (decimal)1000);
            numericUpDown70.Value = SweetFX.Vibrance.RGB_Balance.Blue;
            trackBar70.Value = Convert.ToInt32(SweetFX.Vibrance.RGB_Balance.Blue * (decimal)1000);
            // Curves
            checkBox24.Checked = SweetFX.Curves.Enabled;
            comboBox1.SelectedIndex = SweetFX.Curves.Mode;
            numericUpDown72.Value = SweetFX.Curves.Formula;
            trackBar72.Value = Convert.ToInt32(SweetFX.Curves.Formula);
            numericUpDown73.Value = SweetFX.Curves.Contrast;
            trackBar73.Value = Convert.ToInt32(SweetFX.Curves.Contrast * (decimal)100);
            // Dither
            checkBox26.Checked = SweetFX.Dither.Enabled;
            comboBox3.SelectedIndex = SweetFX.Dither.Method - 1;
            // Sepia
            checkBox22.Checked = SweetFX.Sepia.Enabled;
            numericUpDown82.Value = SweetFX.Sepia.Grey_Power;
            trackBar82.Value = Convert.ToInt32(SweetFX.Sepia.Grey_Power * (decimal)100);
            numericUpDown78.Value = SweetFX.Sepia.Power;
            trackBar78.Value = Convert.ToInt32(SweetFX.Sepia.Power * (decimal)100);
            numericUpDown80.Value = SweetFX.Sepia.Color_Tone.Red;
            trackBar80.Value = Convert.ToInt32(SweetFX.Sepia.Color_Tone.Red * (decimal)100);
            numericUpDown75.Value = SweetFX.Sepia.Color_Tone.Green;
            trackBar75.Value = Convert.ToInt32(SweetFX.Sepia.Color_Tone.Green * (decimal)100);
            numericUpDown71.Value = SweetFX.Sepia.Color_Tone.Blue;
            trackBar71.Value = Convert.ToInt32(SweetFX.Sepia.Color_Tone.Blue * (decimal)100);
            // Vignette
            checkBox25.Checked = SweetFX.Vignette.Enabled;
            numericUpDown77.Value = SweetFX.Vignette.Ratio;
            trackBar77.Value = Convert.ToInt32(SweetFX.Vignette.Ratio * (decimal)100);
            numericUpDown79.Value = SweetFX.Vignette.Radius;
            trackBar79.Value = Convert.ToInt32(SweetFX.Vignette.Radius * (decimal)100);
            numericUpDown81.Value = SweetFX.Vignette.Amount;
            trackBar81.Value = Convert.ToInt32(SweetFX.Vignette.Amount * (decimal)100);
            numericUpDown83.Value = SweetFX.Vignette.Slope;
            trackBar83.Value = Convert.ToInt32(SweetFX.Vignette.Slope);
            comboBox2.SelectedIndex = SweetFX.Vignette.Type - 1;
            numericUpDown86.Value = SweetFX.Vignette.Center.Red;
            trackBar86.Value = Convert.ToInt32(SweetFX.Vignette.Center.Red * (decimal)100);
            numericUpDown85.Value = SweetFX.Vignette.Center.Green;
            trackBar85.Value = Convert.ToInt32(SweetFX.Vignette.Center.Green * (decimal)100);
            // Border
            checkBox28.Checked = SweetFX.Border.Enabled;
            numericUpDown90.Value = SweetFX.Border.Width.Red;
            trackBar90.Value = Convert.ToInt32(SweetFX.Border.Width.Red * (decimal)100);
            numericUpDown88.Value = SweetFX.Border.Width.Green;
            trackBar88.Value = Convert.ToInt32(SweetFX.Border.Width.Green * (decimal)100);
            numericUpDown89.Value = SweetFX.Border.Color.Red;
            trackBar89.Value = Convert.ToInt32(SweetFX.Border.Color.Red * (decimal)100);
            numericUpDown87.Value = SweetFX.Border.Color.Green;
            trackBar87.Value = Convert.ToInt32(SweetFX.Border.Color.Green * (decimal)100);
            numericUpDown84.Value = SweetFX.Border.Color.Blue;
            trackBar84.Value = Convert.ToInt32(SweetFX.Border.Color.Blue * (decimal)100);
            // Splitscreen
            checkBox27.Checked = SweetFX.Splitscreen.Enabled;
            comboBox4.SelectedIndex = SweetFX.Splitscreen.Mode - 1;
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
            // DPX
            checkBox9.CheckedChanged += checkBox9_CheckedChanged;
            numericUpDown39.ValueChanged += numericUpDown39_ValueChanged;
            trackBar39.Scroll += trackBar39_Scroll;
            numericUpDown38.ValueChanged += numericUpDown38_ValueChanged;
            trackBar38.Scroll += trackBar38_Scroll;
            numericUpDown36.ValueChanged += numericUpDown36_ValueChanged;
            trackBar36.Scroll += trackBar36_Scroll;
            numericUpDown34.ValueChanged += numericUpDown34_ValueChanged;
            trackBar34.Scroll += trackBar34_Scroll;
            numericUpDown37.ValueChanged += numericUpDown37_ValueChanged;
            trackBar37.Scroll += trackBar37_Scroll;
            numericUpDown40.ValueChanged += numericUpDown40_ValueChanged;
            trackBar40.Scroll += trackBar40_Scroll;
            numericUpDown41.ValueChanged += numericUpDown41_ValueChanged;
            trackBar41.Scroll += trackBar41_Scroll;
            numericUpDown42.ValueChanged += numericUpDown42_ValueChanged;
            trackBar42.Scroll += trackBar42_Scroll;
            numericUpDown43.ValueChanged += numericUpDown43_ValueChanged;
            trackBar43.Scroll += trackBar43_Scroll;
            // Monochrome
            checkBox11.CheckedChanged += checkBox11_CheckedChanged;
            numericUpDown54.ValueChanged += numericUpDown54_ValueChanged;
            trackBar54.Scroll += trackBar54_Scroll;
            numericUpDown44.ValueChanged += numericUpDown44_ValueChanged;
            trackBar44.Scroll += trackBar44_Scroll;
            numericUpDown45.ValueChanged += numericUpDown45_ValueChanged;
            trackBar45.Scroll += trackBar45_Scroll;
            // Lift Gamma Gain
            checkBox13.CheckedChanged += checkBox13_CheckedChanged;
            numericUpDown53.ValueChanged += numericUpDown53_ValueChanged;
            trackBar53.Scroll += trackBar53_Scroll;
            numericUpDown46.ValueChanged += numericUpDown46_ValueChanged;
            trackBar46.Scroll += trackBar46_Scroll;
            numericUpDown47.ValueChanged += numericUpDown47_ValueChanged;
            trackBar47.Scroll += trackBar47_Scroll;
            numericUpDown57.ValueChanged += numericUpDown57_ValueChanged;
            trackBar57.Scroll += trackBar57_Scroll;
            numericUpDown56.ValueChanged += numericUpDown56_ValueChanged;
            trackBar56.Scroll += trackBar56_Scroll;
            numericUpDown55.ValueChanged += numericUpDown55_ValueChanged;
            trackBar55.Scroll += trackBar55_Scroll;
            numericUpDown60.ValueChanged += numericUpDown60_ValueChanged;
            trackBar60.Scroll += trackBar60_Scroll;
            numericUpDown59.ValueChanged += numericUpDown59_ValueChanged;
            trackBar59.Scroll += trackBar59_Scroll;
            numericUpDown58.ValueChanged += numericUpDown58_ValueChanged;
            trackBar58.Scroll += trackBar58_Scroll;
            // Tonemap
            checkBox17.CheckedChanged += checkBox17_CheckedChanged;
            numericUpDown69.ValueChanged += numericUpDown69_ValueChanged;
            trackBar69.Scroll += trackBar69_Scroll;
            numericUpDown67.ValueChanged += numericUpDown67_ValueChanged;
            trackBar67.Scroll += trackBar67_Scroll;
            numericUpDown61.ValueChanged += numericUpDown61_ValueChanged;
            trackBar61.Scroll += trackBar61_Scroll;
            numericUpDown63.ValueChanged += numericUpDown63_ValueChanged;
            trackBar63.Scroll += trackBar63_Scroll;
            numericUpDown62.ValueChanged += numericUpDown62_ValueChanged;
            trackBar62.Scroll += trackBar62_Scroll;
            numericUpDown66.ValueChanged += numericUpDown66_ValueChanged;
            trackBar66.Scroll += trackBar66_Scroll;
            numericUpDown64.ValueChanged += numericUpDown64_ValueChanged;
            trackBar64.Scroll += trackBar64_Scroll;
            numericUpDown65.ValueChanged += numericUpDown65_ValueChanged;
            trackBar65.Scroll += trackBar65_Scroll;
            // Vibrance
            checkBox18.CheckedChanged += checkBox18_CheckedChanged;
            numericUpDown76.ValueChanged += numericUpDown76_ValueChanged;
            trackBar76.Scroll += trackBar76_Scroll;
            numericUpDown74.ValueChanged += numericUpDown74_ValueChanged;
            trackBar74.Scroll += trackBar74_Scroll;
            numericUpDown68.ValueChanged += numericUpDown68_ValueChanged;
            trackBar68.Scroll += trackBar68_Scroll;
            numericUpDown70.ValueChanged += numericUpDown70_ValueChanged;
            trackBar70.Scroll += trackBar70_Scroll;
            // Curves
            checkBox24.CheckedChanged += checkBox24_CheckedChanged;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            numericUpDown72.ValueChanged += numericUpDown72_ValueChanged;
            trackBar72.Scroll += trackBar72_Scroll;
            numericUpDown73.ValueChanged += numericUpDown73_ValueChanged;
            trackBar73.Scroll += trackBar73_Scroll;
            // Dither
            checkBox26.CheckedChanged += checkBox26_CheckedChanged;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // Sepia
            checkBox22.CheckedChanged += checkBox22_CheckedChanged;
            numericUpDown82.ValueChanged += numericUpDown82_ValueChanged;
            trackBar82.Scroll += trackBar82_Scroll;
            numericUpDown78.ValueChanged += numericUpDown78_ValueChanged;
            trackBar78.Scroll += trackBar78_Scroll;
            numericUpDown80.ValueChanged += numericUpDown80_ValueChanged;
            trackBar80.Scroll += trackBar80_Scroll;
            numericUpDown75.ValueChanged += numericUpDown75_ValueChanged;
            trackBar75.Scroll += trackBar75_Scroll;
            numericUpDown71.ValueChanged += numericUpDown71_ValueChanged;
            trackBar71.Scroll += trackBar71_Scroll;
            // Vignette
            checkBox25.CheckedChanged += checkBox25_CheckedChanged;
            numericUpDown77.Scroll += numericUpDown77_Scroll;
            trackBar77.ValueChanged += trackBar77_ValueChanged;
            numericUpDown79.ValueChanged += numericUpDown79_ValueChanged;
            trackBar79.Scroll += trackBar79_Scroll;
            numericUpDown81.ValueChanged += numericUpDown81_ValueChanged;
            trackBar81.Scroll += trackBar81_Scroll;
            numericUpDown83.ValueChanged += numericUpDown83_ValueChanged;
            trackBar83.Scroll += trackBar83_Scroll;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            numericUpDown86.ValueChanged += numericUpDown86_ValueChanged;
            trackBar86.Scroll += trackBar86_Scroll;
            numericUpDown85.ValueChanged += numericUpDown85_ValueChanged;
            trackBar85.Scroll += trackBar85_Scroll;
            // Border
            checkBox28.CheckedChanged += checkBox28_CheckedChanged;
            numericUpDown90.ValueChanged += numericUpDown90_ValueChanged;
            trackBar90.Scroll += trackBar90_Scroll;
            numericUpDown88.ValueChanged += numericUpDown88_ValueChanged;
            trackBar88.Scroll += trackBar88_Scroll;
            numericUpDown89.ValueChanged += numericUpDown89_ValueChanged;
            trackBar89.Scroll += trackBar89_Scroll;
            numericUpDown87.ValueChanged += numericUpDown87_ValueChanged;
            trackBar87.Scroll += trackBar87_Scroll;
            numericUpDown84.ValueChanged += numericUpDown84_ValueChanged;
            trackBar84.Scroll += trackBar84_Scroll;
            // Splitscreen
            checkBox27.CheckedChanged += checkBox27_CheckedChanged;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
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
            // DPX
            checkBox9.CheckedChanged -= checkBox9_CheckedChanged;
            numericUpDown39.ValueChanged -= numericUpDown39_ValueChanged;
            trackBar39.Scroll -= trackBar39_Scroll;
            numericUpDown38.ValueChanged -= numericUpDown38_ValueChanged;
            trackBar38.Scroll -= trackBar38_Scroll;
            numericUpDown36.ValueChanged -= numericUpDown36_ValueChanged;
            trackBar36.Scroll -= trackBar36_Scroll;
            numericUpDown34.ValueChanged -= numericUpDown34_ValueChanged;
            trackBar34.Scroll -= trackBar34_Scroll;
            numericUpDown37.ValueChanged -= numericUpDown37_ValueChanged;
            trackBar37.Scroll -= trackBar37_Scroll;
            numericUpDown40.ValueChanged -= numericUpDown40_ValueChanged;
            trackBar40.Scroll -= trackBar40_Scroll;
            numericUpDown41.ValueChanged -= numericUpDown41_ValueChanged;
            trackBar41.Scroll -= trackBar41_Scroll;
            numericUpDown42.ValueChanged -= numericUpDown42_ValueChanged;
            trackBar42.Scroll -= trackBar42_Scroll;
            numericUpDown43.ValueChanged -= numericUpDown43_ValueChanged;
            trackBar43.Scroll -= trackBar43_Scroll;
            // Monochrome
            checkBox11.CheckedChanged -= checkBox11_CheckedChanged;
            numericUpDown54.ValueChanged -= numericUpDown54_ValueChanged;
            trackBar54.Scroll -= trackBar54_Scroll;
            numericUpDown44.ValueChanged -= numericUpDown44_ValueChanged;
            trackBar44.Scroll -= trackBar44_Scroll;
            numericUpDown45.ValueChanged -= numericUpDown45_ValueChanged;
            trackBar45.Scroll -= trackBar45_Scroll;
            // Lift Gamma Gain
            checkBox13.CheckedChanged -= checkBox13_CheckedChanged;
            numericUpDown53.ValueChanged -= numericUpDown53_ValueChanged;
            trackBar53.Scroll -= trackBar53_Scroll;
            numericUpDown46.ValueChanged -= numericUpDown46_ValueChanged;
            trackBar46.Scroll -= trackBar46_Scroll;
            numericUpDown47.ValueChanged -= numericUpDown47_ValueChanged;
            trackBar47.Scroll -= trackBar47_Scroll;
            numericUpDown57.ValueChanged -= numericUpDown57_ValueChanged;
            trackBar57.Scroll -= trackBar57_Scroll;
            numericUpDown56.ValueChanged -= numericUpDown56_ValueChanged;
            trackBar56.Scroll -= trackBar56_Scroll;
            numericUpDown55.ValueChanged -= numericUpDown55_ValueChanged;
            trackBar55.Scroll -= trackBar55_Scroll;
            numericUpDown60.ValueChanged -= numericUpDown60_ValueChanged;
            trackBar60.Scroll -= trackBar60_Scroll;
            numericUpDown59.ValueChanged -= numericUpDown59_ValueChanged;
            trackBar59.Scroll -= trackBar59_Scroll;
            numericUpDown58.ValueChanged -= numericUpDown58_ValueChanged;
            trackBar58.Scroll -= trackBar58_Scroll;
            // Tonemap
            checkBox17.CheckedChanged -= checkBox17_CheckedChanged;
            numericUpDown69.ValueChanged -= numericUpDown69_ValueChanged;
            trackBar69.Scroll -= trackBar69_Scroll;
            numericUpDown67.ValueChanged -= numericUpDown67_ValueChanged;
            trackBar67.Scroll -= trackBar67_Scroll;
            numericUpDown61.ValueChanged -= numericUpDown61_ValueChanged;
            trackBar61.Scroll -= trackBar61_Scroll;
            numericUpDown63.ValueChanged -= numericUpDown63_ValueChanged;
            trackBar63.Scroll -= trackBar63_Scroll;
            numericUpDown62.ValueChanged -= numericUpDown62_ValueChanged;
            trackBar62.Scroll -= trackBar62_Scroll;
            numericUpDown66.ValueChanged -= numericUpDown66_ValueChanged;
            trackBar66.Scroll -= trackBar66_Scroll;
            numericUpDown64.ValueChanged -= numericUpDown64_ValueChanged;
            trackBar64.Scroll -= trackBar64_Scroll;
            numericUpDown65.ValueChanged -= numericUpDown65_ValueChanged;
            trackBar65.Scroll -= trackBar65_Scroll;
            // Vibrance
            checkBox18.CheckedChanged -= checkBox18_CheckedChanged;
            numericUpDown76.ValueChanged -= numericUpDown76_ValueChanged;
            trackBar76.Scroll -= trackBar76_Scroll;
            numericUpDown74.ValueChanged -= numericUpDown74_ValueChanged;
            trackBar74.Scroll -= trackBar74_Scroll;
            numericUpDown68.ValueChanged -= numericUpDown68_ValueChanged;
            trackBar68.Scroll -= trackBar68_Scroll;
            numericUpDown70.ValueChanged -= numericUpDown70_ValueChanged;
            trackBar70.Scroll -= trackBar70_Scroll;
            // Curves
            checkBox24.CheckedChanged -= checkBox24_CheckedChanged;
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            numericUpDown72.ValueChanged -= numericUpDown72_ValueChanged;
            trackBar72.Scroll -= trackBar72_Scroll;
            numericUpDown73.ValueChanged -= numericUpDown73_ValueChanged;
            trackBar73.Scroll -= trackBar73_Scroll;
            // Dither
            checkBox26.CheckedChanged -= checkBox26_CheckedChanged;
            comboBox3.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
            // Sepia
            checkBox22.CheckedChanged -= checkBox22_CheckedChanged;
            numericUpDown82.ValueChanged -= numericUpDown82_ValueChanged;
            trackBar82.Scroll -= trackBar82_Scroll;
            numericUpDown78.ValueChanged -= numericUpDown78_ValueChanged;
            trackBar78.Scroll -= trackBar78_Scroll;
            numericUpDown80.ValueChanged -= numericUpDown80_ValueChanged;
            trackBar80.Scroll -= trackBar80_Scroll;
            numericUpDown75.ValueChanged -= numericUpDown75_ValueChanged;
            trackBar75.Scroll -= trackBar75_Scroll;
            numericUpDown71.ValueChanged -= numericUpDown71_ValueChanged;
            trackBar71.Scroll -= trackBar71_Scroll;
            // Vignette
            checkBox25.CheckedChanged -= checkBox25_CheckedChanged;
            numericUpDown77.Scroll -= numericUpDown77_Scroll;
            trackBar77.ValueChanged -= trackBar77_ValueChanged;
            numericUpDown79.ValueChanged -= numericUpDown79_ValueChanged;
            trackBar79.Scroll -= trackBar79_Scroll;
            numericUpDown81.ValueChanged -= numericUpDown81_ValueChanged;
            trackBar81.Scroll -= trackBar81_Scroll;
            numericUpDown83.ValueChanged -= numericUpDown83_ValueChanged;
            trackBar83.Scroll -= trackBar83_Scroll;
            comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
            numericUpDown86.ValueChanged -= numericUpDown86_ValueChanged;
            trackBar86.Scroll -= trackBar86_Scroll;
            numericUpDown85.ValueChanged -= numericUpDown85_ValueChanged;
            trackBar85.Scroll -= trackBar85_Scroll;
            // Border
            checkBox28.CheckedChanged -= checkBox28_CheckedChanged;
            numericUpDown90.ValueChanged -= numericUpDown90_ValueChanged;
            trackBar90.Scroll -= trackBar90_Scroll;
            numericUpDown88.ValueChanged -= numericUpDown88_ValueChanged;
            trackBar88.Scroll -= trackBar88_Scroll;
            numericUpDown89.ValueChanged -= numericUpDown89_ValueChanged;
            trackBar89.Scroll -= trackBar89_Scroll;
            numericUpDown87.ValueChanged -= numericUpDown87_ValueChanged;
            trackBar87.Scroll -= trackBar87_Scroll;
            numericUpDown84.ValueChanged -= numericUpDown84_ValueChanged;
            trackBar84.Scroll -= trackBar84_Scroll;
            // Splitscreen
            checkBox27.CheckedChanged -= checkBox27_CheckedChanged;
            comboBox4.SelectedIndexChanged -= comboBox4_SelectedIndexChanged;
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

        #region DPX

        void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.Cineon_DPX.Enabled = checkBox9.Checked;
        }

        void numericUpDown39_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown39.ValueChanged -= new System.EventHandler(this.numericUpDown39_ValueChanged);
            trackBar39.Value = Convert.ToInt32(numericUpDown39.Value / (decimal)100);
            SweetFX.Technicolor.Blue_Negative_Amount = numericUpDown39.Value;
            this.numericUpDown39.ValueChanged += new System.EventHandler(this.numericUpDown39_ValueChanged);
        }

        void trackBar39_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown39.ValueChanged -= new System.EventHandler(this.numericUpDown39_ValueChanged);
            numericUpDown39.Value = (decimal)trackBar39.Value / (decimal)100;
            SweetFX.Technicolor.Blue_Negative_Amount = numericUpDown39.Value;
            this.numericUpDown39.ValueChanged += new System.EventHandler(this.numericUpDown39_ValueChanged);
        }

        void numericUpDown38_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar38_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown36_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar36_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown34_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar34_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown37_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar37_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown40_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar40_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown41_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar41_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown42_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar42_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown43_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar43_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Monochrome

        void checkBox11_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown54_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar54_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown44_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar44_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown45_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar45_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Lift Gamma Gain

        void checkBox13_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown53_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar53_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown46_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar46_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown47_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar47_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown57_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar57_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown56_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar56_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown55_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar55_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown60_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar60_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown59_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar59_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown58_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar58_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Tonemap

        void checkBox17_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown69_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar69_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown67_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar67_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown61_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar61_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown63_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar63_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown62_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar62_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown66_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar66_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown64_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar64_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown65_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar65_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Vibrance

        void checkBox18_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown76_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar76_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown74_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar74_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown68_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar68_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown70_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar70_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Curves

        void checkBox24_CheckedChanged(object sender, EventArgs e)
        {

        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown72_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar72_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown73_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar73_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Dither

        void checkBox26_CheckedChanged(object sender, EventArgs e)
        {

        }

        void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Sepia

        void checkBox22_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown82_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar82_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown78_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar78_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown80_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar80_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown75_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar75_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown71_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar71_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Vignette

        void checkBox25_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown77_Scroll(object sender, ScrollEventArgs e)
        {

        }

        void trackBar77_ValueChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown79_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar79_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown81_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar81_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown83_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar83_Scroll(object sender, EventArgs e)
        {

        }

        void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown86_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar86_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown85_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar85_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Border

        void checkBox28_CheckedChanged(object sender, EventArgs e)
        {

        }

        void numericUpDown90_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar90_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown88_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar88_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown89_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar89_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown87_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar87_Scroll(object sender, EventArgs e)
        {

        }

        void numericUpDown84_ValueChanged(object sender, EventArgs e)
        {

        }

        void trackBar84_Scroll(object sender, EventArgs e)
        {

        }

        #endregion

        #region Splitscreen

        void checkBox27_CheckedChanged(object sender, EventArgs e)
        {

        }

        void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
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
