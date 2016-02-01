using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public partial class MainForm : Form
    {
        private List<Message> MessagePump = new List<Message>();
        private Message _message = new Message("");
        private delegate void SweetFX_SaveSettingsFinishedD();
        GameManagerForm game_manager_form;
        SettingsForm settings_form;
        AboutForm about_form;
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
            List<Game> _gms = Settings.GetGames();
            foreach (Game _game in _gms)
            {
                if (_game.SweetFX_Install != null) { GameManagerForm_AddGame(_game); }
            }
            //
            SweetFX_Parser.SettingsSaved += SweetFX_SaveSettingsFinished;
            showActiveOnlyToolStripMenuItem.Checked = Settings.OnlyActive;
            SweetFX_Parser.GameLoaded += SweetFX_GameLoaded;
            GameManagerForm.AddGame += GameManagerForm_AddGame;
            GameManagerForm.RemoveGame += GameManagerForm_RemoveGame;
            GameManagerForm.GameEdited += GameManagerForm_GameEdited;
            if (Settings.LastGame != null && Settings.LastGame.SweetFX_Install != null)
            {
                this.Opacity = 100;
                this.ShowInTaskbar = true;
                SweetFX_Parser.Load(Settings.LastGame);
            }
            else
            {
                game_manager_form = new GameManagerForm(false);
                game_manager_form.FormClosed += game_manager_form_FormClosed;
                game_manager_form.Show();
            }
            tabControl1.SelectedIndex = Settings.LastTab;
        }

        private void GameManagerForm_GameEdited(Game old_game, Game new_game)
        {
            foreach (ToolStripItem item in gamesToolStripMenuItem1.DropDownItems)
            {
                if (item.Text == old_game.Name)
                {

                    break;
                }
            }
        }

        private void GameManagerForm_RemoveGame(Game _g)
        {
            foreach (ToolStripItem item in gamesToolStripMenuItem1.DropDownItems)
            {
                if (item.Text == _g.Name) { gamesToolStripMenuItem1.DropDownItems.Remove(item); break; }
            }
        }

        private void GameManagerForm_AddGame(Game _g)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = _g.Name;
            item.Click += item_Click;
            gamesToolStripMenuItem1.DropDownItems.Add(item);
        }

        private delegate void InstallManager_GameLoadedD();

        void SweetFX_GameLoaded()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InstallManager_GameLoadedD(SweetFX_GameLoaded));
                return;
            }
            this.Opacity = 100;
            this.ShowInTaskbar = true;
            LoadSFXConfig();
            SetMessage(new Message("Game loaded: " + Settings.LastGame.Name, 0));
            if (showActiveOnlyToolStripMenuItem.Checked) { HideUnactiveTabs(); }
        }

        void item_Click(object sender, EventArgs e)
        {
            SweetFX_Parser.Load(Settings.GetGame(((ToolStripMenuItem)sender).Text));
        }

        private void LoadSFXConfig()
        {
            StopFormCapture();
            // SMAA
            checkBox1.Checked = Settings.LastGame.SweetFX.SMAA.Enabled;
            sMAAToolStripMenuItem.Checked = Settings.LastGame.SweetFX.SMAA.Enabled;
            numericUpDown1.Value = Settings.LastGame.SweetFX.SMAA.Threshold;
            trackBar1.Value = Convert.ToInt32(Settings.LastGame.SweetFX.SMAA.Threshold * (decimal)100);
            numericUpDown2.Value = Settings.LastGame.SweetFX.SMAA.Max_Search_Steps;
            trackBar2.Value = Settings.LastGame.SweetFX.SMAA.Max_Search_Steps;
            numericUpDown3.Value = Settings.LastGame.SweetFX.SMAA.Max_Search_Steps_Diag;
            trackBar3.Value = Settings.LastGame.SweetFX.SMAA.Max_Search_Steps_Diag;
            numericUpDown4.Value = Settings.LastGame.SweetFX.SMAA.Corner_Rounding;
            trackBar4.Value = Settings.LastGame.SweetFX.SMAA.Corner_Rounding;
            comboBox6.SelectedIndex = (Settings.LastGame.SweetFX.SMAA.Color_Edge_Detection == 1) ? 0 : 1;
            checkBox3.Checked = Settings.LastGame.SweetFX.SMAA.DirectX9_Linear_Blend;
            // FXAA
            checkBox6.Checked = Settings.LastGame.SweetFX.FXAA.Enabled;
            fXAAToolStripMenuItem.Checked = Settings.LastGame.SweetFX.FXAA.Enabled;
            numericUpDown8.Value = Settings.LastGame.SweetFX.FXAA.Quality_Preset;
            trackBar8.Value = Settings.LastGame.SweetFX.FXAA.Quality_Preset;
            numericUpDown7.Value = Settings.LastGame.SweetFX.FXAA.Subpix;
            trackBar7.Value = Convert.ToInt32(Settings.LastGame.SweetFX.FXAA.Subpix * (decimal)1000);
            numericUpDown6.Value = Settings.LastGame.SweetFX.FXAA.Edge_Threshold;
            trackBar6.Value = Convert.ToInt32(Settings.LastGame.SweetFX.FXAA.Edge_Threshold * (decimal)1000);
            numericUpDown5.Value = Settings.LastGame.SweetFX.FXAA.Edge_Threshold_Min;
            trackBar5.Value = Convert.ToInt32(Settings.LastGame.SweetFX.FXAA.Edge_Threshold_Min * (decimal)1000);
            // Explosion
            checkBox4.Checked = Settings.LastGame.SweetFX.Explosion.Enabled;
            explosionCartoonToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Explosion.Enabled;
            numericUpDown12.Value = Settings.LastGame.SweetFX.Explosion.Radius;
            trackBar12.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Explosion.Radius * (decimal)10);
            // Cartoon
            checkBox5.Checked = Settings.LastGame.SweetFX.Cartoon.Enabled;
            cartoonToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Cartoon.Enabled;
            numericUpDown11.Value = Settings.LastGame.SweetFX.Cartoon.Power;
            trackBar9.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cartoon.Power * (decimal)10);
            numericUpDown9.Value = Settings.LastGame.SweetFX.Cartoon.Edge_Slope;
            trackBar11.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cartoon.Edge_Slope * (decimal)10);
            // CRT
            checkBox8.Checked = Settings.LastGame.SweetFX.CRT.Enabled;
            checkBox12.Checked = Settings.LastGame.SweetFX.CRT.Enabled;
            cRTToolStripMenuItem.Checked = Settings.LastGame.SweetFX.CRT.Enabled;
            numericUpDown19.Value = Settings.LastGame.SweetFX.CRT.Amount;
            trackBar19.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Amount * (decimal)100);
            numericUpDown18.Value = Settings.LastGame.SweetFX.CRT.Resolution;
            trackBar18.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Resolution * (decimal)10);
            numericUpDown17.Value = Settings.LastGame.SweetFX.CRT.Gamma;
            trackBar17.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Gamma * (decimal)10);
            numericUpDown16.Value = Settings.LastGame.SweetFX.CRT.Monitor_Gamma;
            trackBar16.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Monitor_Gamma * (decimal)10);
            numericUpDown10.Value = Settings.LastGame.SweetFX.CRT.Brightness;
            trackBar10.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Brightness * (decimal)10);
            numericUpDown13.Value = Settings.LastGame.SweetFX.CRT.Scanline_Intensity;
            trackBar13.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Scanline_Intensity * (decimal)10);
            checkBox10.Checked = Settings.LastGame.SweetFX.CRT.Scanline_Gaussian;
            numericUpDown23.Value = Settings.LastGame.SweetFX.CRT.Corner_Size;
            trackBar23.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Corner_Size * (decimal)10000);
            numericUpDown22.Value = Settings.LastGame.SweetFX.CRT.Distance;
            trackBar22.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Distance * (decimal)100);
            numericUpDown21.Value = Settings.LastGame.SweetFX.CRT.AngleX;
            trackBar21.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.AngleX * (decimal)100);
            numericUpDown20.Value = Settings.LastGame.SweetFX.CRT.AngleY;
            trackBar20.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.AngleY * (decimal)100);
            numericUpDown15.Value = Settings.LastGame.SweetFX.CRT.Curvature_Radius;
            trackBar15.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Curvature_Radius * (decimal)10);
            numericUpDown14.Value = Settings.LastGame.SweetFX.CRT.Overscan;
            trackBar14.Value = Convert.ToInt32(Settings.LastGame.SweetFX.CRT.Overscan * (decimal)100);
            checkBox7.Checked = Settings.LastGame.SweetFX.CRT.Curvature;
            trackBar15.Enabled = Settings.LastGame.SweetFX.CRT.Curvature;
            numericUpDown15.Enabled = Settings.LastGame.SweetFX.CRT.Curvature;
            checkBox14.Checked = Settings.LastGame.SweetFX.CRT.Oversample;
            // Bloom
            checkBox16.Checked = Settings.LastGame.SweetFX.Bloom.Enabled;
            bloomToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Bloom.Enabled;
            numericUpDown26.Value = Settings.LastGame.SweetFX.Bloom.Threshold;
            trackBar26.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Bloom.Threshold * (decimal)100);
            numericUpDown28.Value = Settings.LastGame.SweetFX.Bloom.Power;
            trackBar27.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Bloom.Power * (decimal)1000);
            numericUpDown27.Value = Settings.LastGame.SweetFX.Bloom.Width;
            trackBar28.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Bloom.Width * (decimal)10000);
            // HDR
            checkBox15.Checked = Settings.LastGame.SweetFX.HDR.Enabled;
            hDRToolStripMenuItem.Checked = Settings.LastGame.SweetFX.HDR.Enabled;
            numericUpDown25.Value = Settings.LastGame.SweetFX.HDR.Power;
            trackBar24.Value = Convert.ToInt32(Settings.LastGame.SweetFX.HDR.Power * (decimal)100);
            numericUpDown24.Value = Settings.LastGame.SweetFX.HDR.Radius;
            trackBar25.Value = Convert.ToInt32(Settings.LastGame.SweetFX.HDR.Radius * (decimal)100);
            // LumaSharpen
            checkBox20.Checked = Settings.LastGame.SweetFX.LumaSharpen.Enabled;
            lumaSharpenToolStripMenuItem.Checked = Settings.LastGame.SweetFX.LumaSharpen.Enabled;
            numericUpDown32.Value = Settings.LastGame.SweetFX.LumaSharpen.Strength;
            trackBar32.Value = Convert.ToInt32(Settings.LastGame.SweetFX.LumaSharpen.Strength * (decimal)100);
            numericUpDown31.Value = Settings.LastGame.SweetFX.LumaSharpen.Clamp;
            trackBar31.Value = Convert.ToInt32(Settings.LastGame.SweetFX.LumaSharpen.Clamp * (decimal)1000);
            comboBox5.SelectedIndex = Settings.LastGame.SweetFX.LumaSharpen.Pattern - 1;
            numericUpDown29.Value = Settings.LastGame.SweetFX.LumaSharpen.Offset_Bias;
            trackBar29.Value = Convert.ToInt32(Settings.LastGame.SweetFX.LumaSharpen.Offset_Bias * (decimal)10);
            checkBox19.Checked = Settings.LastGame.SweetFX.LumaSharpen.Show;
            // Levels
            checkBox21.Checked = Settings.LastGame.SweetFX.Levels.Enabled;
            levelsToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Levels.Enabled;
            numericUpDown35.Value = Settings.LastGame.SweetFX.Levels.Black_Point;
            trackBar48.Value = Settings.LastGame.SweetFX.Levels.Black_Point;
            numericUpDown33.Value = Settings.LastGame.SweetFX.Levels.White_Point;
            trackBar33.Value = Settings.LastGame.SweetFX.Levels.White_Point;
            // Technicolor
            checkBox23.Checked = Settings.LastGame.SweetFX.Technicolor.Enabled;
            technicolorToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Technicolor.Enabled;
            numericUpDown52.Value = Settings.LastGame.SweetFX.Technicolor.Amount;
            trackBar52.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Technicolor.Amount * (decimal)100);
            numericUpDown51.Value = Settings.LastGame.SweetFX.Technicolor.Power;
            trackBar51.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Technicolor.Power * (decimal)100);
            numericUpDown50.Value = Settings.LastGame.SweetFX.Technicolor.Red_Negative_Amount;
            trackBar50.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Technicolor.Red_Negative_Amount * (decimal)100);
            numericUpDown49.Value = Settings.LastGame.SweetFX.Technicolor.Green_Negative_Amount;
            trackBar49.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Technicolor.Green_Negative_Amount * (decimal)100);
            numericUpDown48.Value = Settings.LastGame.SweetFX.Technicolor.Blue_Negative_Amount;
            trackBar35.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Technicolor.Blue_Negative_Amount * (decimal)100);
            // DPX
            checkBox9.Checked = Settings.LastGame.SweetFX.Cineon_DPX.Enabled;
            dPXToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Cineon_DPX.Enabled;
            numericUpDown39.Value = Settings.LastGame.SweetFX.Cineon_DPX.Color_Gamma;
            trackBar39.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.Color_Gamma * (decimal)10);
            numericUpDown38.Value = Settings.LastGame.SweetFX.Cineon_DPX.Saturation;
            trackBar38.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.Saturation * (decimal)10);
            numericUpDown36.Value = Settings.LastGame.SweetFX.Cineon_DPX.Blend;
            trackBar36.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.Blend * (decimal)100);
            numericUpDown34.Value = Settings.LastGame.SweetFX.Cineon_DPX.Red;
            trackBar34.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.Red * (decimal)100);
            numericUpDown37.Value = Settings.LastGame.SweetFX.Cineon_DPX.Green;
            trackBar37.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.Green * (decimal)100);
            numericUpDown40.Value = Settings.LastGame.SweetFX.Cineon_DPX.Blue;
            trackBar40.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.Blue * (decimal)100);
            numericUpDown41.Value = Settings.LastGame.SweetFX.Cineon_DPX.RedC;
            trackBar41.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.RedC * (decimal)100);
            numericUpDown42.Value = Settings.LastGame.SweetFX.Cineon_DPX.GreenC;
            trackBar42.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.GreenC * (decimal)100);
            numericUpDown43.Value = Settings.LastGame.SweetFX.Cineon_DPX.BlueC;
            trackBar43.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Cineon_DPX.BlueC * (decimal)100);
            // Monochrome
            checkBox11.Checked = Settings.LastGame.SweetFX.Monochrome.Enabled;
            monochromeToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Monochrome.Enabled;
            numericUpDown54.Value = Settings.LastGame.SweetFX.Monochrome.Red;
            trackBar54.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Monochrome.Red * (decimal)100);
            numericUpDown44.Value = Settings.LastGame.SweetFX.Monochrome.Green;
            trackBar44.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Monochrome.Green * (decimal)100);
            numericUpDown45.Value = Settings.LastGame.SweetFX.Monochrome.Blue;
            trackBar45.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Monochrome.Blue * (decimal)100);
            // Lift Gamma Gain
            checkBox13.Checked = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Enabled;
            liftGammaGainToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Enabled;
            numericUpDown53.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Red;
            trackBar53.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Red * (decimal)1000);
            numericUpDown46.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Green;
            trackBar46.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Green * (decimal)1000);
            numericUpDown47.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Blue;
            trackBar47.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Blue * (decimal)1000);
            numericUpDown57.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Red;
            trackBar57.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Red * (decimal)1000);
            numericUpDown56.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Green;
            trackBar56.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Green * (decimal)1000);
            numericUpDown55.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Blue;
            trackBar55.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Blue * (decimal)1000);
            numericUpDown60.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Red;
            trackBar60.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Red * (decimal)1000);
            numericUpDown59.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Green;
            trackBar59.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Green * (decimal)1000);
            numericUpDown58.Value = Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Blue;
            trackBar58.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Blue * (decimal)1000);
            // Tonemap
            checkBox17.Checked = Settings.LastGame.SweetFX.Tonemap.Enabled;
            tonemapToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Tonemap.Enabled;
            numericUpDown69.Value = Settings.LastGame.SweetFX.Tonemap.Gamma;
            trackBar69.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Tonemap.Gamma * (decimal)1000);
            numericUpDown67.Value = Settings.LastGame.SweetFX.Tonemap.Exposure;
            trackBar67.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Tonemap.Exposure * (decimal)1000);
            numericUpDown61.Value = Settings.LastGame.SweetFX.Tonemap.Bleach;
            trackBar61.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Tonemap.Bleach * (decimal)1000);
            numericUpDown63.Value = Settings.LastGame.SweetFX.Tonemap.Defog;
            trackBar63.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Tonemap.Defog * (decimal)1000);
            numericUpDown62.Value = Settings.LastGame.SweetFX.Tonemap.Saturation;
            trackBar62.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Tonemap.Saturation * (decimal)1000);
            numericUpDown66.Value = Settings.LastGame.SweetFX.Tonemap.Fog_Red;
            trackBar66.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Tonemap.Fog_Red * (decimal)100);
            numericUpDown64.Value = Settings.LastGame.SweetFX.Tonemap.Fog_Green;
            trackBar64.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Tonemap.Fog_Green * (decimal)100);
            numericUpDown65.Value = Settings.LastGame.SweetFX.Tonemap.Fog_Blue;
            trackBar65.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Tonemap.Fog_Blue * (decimal)100);
            // Vibrance
            checkBox18.Checked = Settings.LastGame.SweetFX.Vibrance.Enabled;
            vibranceToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Vibrance.Enabled;
            numericUpDown76.Value = Settings.LastGame.SweetFX.Vibrance.Vibrance;
            trackBar76.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vibrance.Vibrance * (decimal)100);
            numericUpDown74.Value = Settings.LastGame.SweetFX.Vibrance.Red;
            trackBar74.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vibrance.Red * (decimal)100);
            numericUpDown68.Value = Settings.LastGame.SweetFX.Vibrance.Green;
            trackBar68.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vibrance.Green * (decimal)100);
            numericUpDown70.Value = Settings.LastGame.SweetFX.Vibrance.Blue;
            trackBar70.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vibrance.Blue * (decimal)100);
            // Curves
            checkBox24.Checked = Settings.LastGame.SweetFX.Curves.Enabled;
            curvesToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Curves.Enabled;
            comboBox1.SelectedIndex = Settings.LastGame.SweetFX.Curves.Mode;
            comboBox7.SelectedIndex = Settings.LastGame.SweetFX.Curves.Formula - 1;
            numericUpDown73.Value = Settings.LastGame.SweetFX.Curves.Contrast;
            trackBar73.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Curves.Contrast * (decimal)100);
            // Dither
            checkBox26.Checked = Settings.LastGame.SweetFX.Dither.Enabled;
            ditherToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Dither.Enabled;
            comboBox3.SelectedIndex = Settings.LastGame.SweetFX.Dither.Method - 1;
            // Sepia
            checkBox22.Checked = Settings.LastGame.SweetFX.Sepia.Enabled;
            sepiaToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Sepia.Enabled;
            numericUpDown82.Value = Settings.LastGame.SweetFX.Sepia.Grey_Power;
            trackBar82.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Sepia.Grey_Power * (decimal)100);
            numericUpDown78.Value = Settings.LastGame.SweetFX.Sepia.Power;
            trackBar78.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Sepia.Power * (decimal)100);
            numericUpDown80.Value = Settings.LastGame.SweetFX.Sepia.Red;
            trackBar80.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Sepia.Red * (decimal)100);
            numericUpDown75.Value = Settings.LastGame.SweetFX.Sepia.Green;
            trackBar75.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Sepia.Green * (decimal)100);
            numericUpDown71.Value = Settings.LastGame.SweetFX.Sepia.Blue;
            trackBar71.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Sepia.Blue * (decimal)100);
            // Vignette
            checkBox25.Checked = Settings.LastGame.SweetFX.Vignette.Enabled;
            vignetteToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Vignette.Enabled;
            numericUpDown77.Value = Settings.LastGame.SweetFX.Vignette.Ratio;
            trackBar77.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vignette.Ratio * (decimal)100);
            numericUpDown79.Value = Settings.LastGame.SweetFX.Vignette.Radius;
            trackBar79.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vignette.Radius * (decimal)100);
            numericUpDown81.Value = Settings.LastGame.SweetFX.Vignette.Amount;
            trackBar81.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vignette.Amount * (decimal)100);
            numericUpDown83.Value = Settings.LastGame.SweetFX.Vignette.Slope;
            trackBar83.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vignette.Slope);
            comboBox2.SelectedIndex = Settings.LastGame.SweetFX.Vignette.Type - 1;
            numericUpDown86.Value = Settings.LastGame.SweetFX.Vignette.Center_X;
            trackBar86.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vignette.Center_X * (decimal)1000);
            numericUpDown85.Value = Settings.LastGame.SweetFX.Vignette.Center_Y;
            trackBar85.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Vignette.Center_Y * (decimal)1000);
            if (Settings.LastGame.SweetFX.Vignette.Type == 1)
            {
                numericUpDown86.Enabled = true;
                trackBar86.Enabled = true;
                numericUpDown85.Enabled = true;
                trackBar85.Enabled = true;
            }
            else
            {
                numericUpDown86.Enabled = false;
                trackBar86.Enabled = false;
                numericUpDown85.Enabled = false;
                trackBar85.Enabled = false;
            }
            // Border
            checkBox28.Checked = Settings.LastGame.SweetFX.Border.Enabled;
            borderToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Border.Enabled;
            numericUpDown90.Value = Settings.LastGame.SweetFX.Border.Width_X;
            trackBar90.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Border.Width_X);
            numericUpDown88.Value = Settings.LastGame.SweetFX.Border.Width_Y;
            trackBar88.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Border.Width_Y);
            numericUpDown89.Value = Settings.LastGame.SweetFX.Border.Red;
            trackBar89.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Border.Red);
            numericUpDown87.Value = Settings.LastGame.SweetFX.Border.Green;
            trackBar87.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Border.Green);
            numericUpDown84.Value = Settings.LastGame.SweetFX.Border.Blue;
            trackBar84.Value = Convert.ToInt32(Settings.LastGame.SweetFX.Border.Blue);
            // Splitscreen
            checkBox27.Checked = Settings.LastGame.SweetFX.Splitscreen.Enabled;
            splitscreenToolStripMenuItem.Checked = Settings.LastGame.SweetFX.Splitscreen.Enabled;
            comboBox4.SelectedIndex = Settings.LastGame.SweetFX.Splitscreen.Mode - 1;
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
            SetMessage(new Message("Game loaded: " + Settings.LastGame.Name + ", last saved: " + DateTime.Now.ToShortTimeString(), 0));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.LastTab = tabControl1.SelectedIndex;
            Settings.Main_Window_Geometry = WindowGeometry.GeometryToString(this);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SweetFX_Parser.Dispose();
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
            this.comboBox6.SelectedIndexChanged += comboBox6_SelectedIndexChanged;
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
            comboBox5.SelectedIndexChanged += comboBox5_SelectedIndexChanged;
            numericUpDown29.ValueChanged += numericUpDown29_ValueChanged;
            trackBar29.Scroll += trackBar29_Scroll;
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
            trackBar49.Scroll += trackBar49_Scroll;
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
            comboBox7.SelectedIndexChanged += comboBox7_SelectedIndexChanged;
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
            numericUpDown77.ValueChanged += numericUpDown77_ValueChanged;
            trackBar77.Scroll += trackBar77_Scroll;
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
            this.comboBox6.SelectedIndexChanged -= new System.EventHandler(this.comboBox6_SelectedIndexChanged);
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
            comboBox5.SelectedIndexChanged -= comboBox5_SelectedIndexChanged;
            numericUpDown29.ValueChanged -= numericUpDown29_ValueChanged;
            trackBar29.Scroll -= trackBar29_Scroll;
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
            trackBar49.Scroll -= trackBar49_Scroll;
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
            comboBox7.SelectedIndexChanged -= comboBox7_SelectedIndexChanged;
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
            numericUpDown77.ValueChanged -= numericUpDown77_ValueChanged;
            trackBar77.Scroll -= trackBar77_Scroll;
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

        private void SetMessage(Message message_)
        {
            if (message_.Timeout == 0)
            {
                toolStripStatusLabel1.Text = message_.ToString();
                _message = message_;
                return;
            }
            if (!timer1.Enabled)
            {
                toolStripStatusLabel1.Text = message_.ToString();
                timer1.Interval = message_.Timeout;
                timer1.Enabled = true;
                timer1.Start();
            }
            else { MessagePump.Add(message_); }
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
                toolStripStatusLabel1.Text = _message.ToString();
                timer1.Enabled = false;
                timer1.Stop();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!GameManagerForm.isOpen)
            {
                game_manager_form = new GameManagerForm();
                game_manager_form.FormClosed += game_manager_form_FormClosed;
                game_manager_form.Show();
            }
            else { game_manager_form.BringToFront(); }
        }

        void game_manager_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            game_manager_form.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AboutForm.isOpen)
            {
                about_form = new AboutForm();
                about_form.FormClosed += about_form_FormClosed;
                about_form.Show();
            }
            else { about_form.BringToFront(); }
        }

        void about_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            about_form.Dispose();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SettingsForm.isOpen)
            {
                settings_form = new SettingsForm();
                settings_form.FormClosed += settings_form_FormClosed;
                settings_form.Show();
            }
            else { settings_form.BringToFront(); }
        }

        void settings_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            settings_form.Dispose();
        }

        private void onlyActiveToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Settings.OnlyActive = showActiveOnlyToolStripMenuItem.Checked;
            if (showActiveOnlyToolStripMenuItem.Checked) { HideUnactiveTabs(); }
            else { ShowAllTabs(); }
        }

        private void HideUnactiveTabs()
        {
            ShowAllTabs();
            if (!Settings.LastGame.SweetFX.SMAA.Enabled) { tabControl1.TabPages.Remove(tabPage1); }
            if (!Settings.LastGame.SweetFX.FXAA.Enabled) { tabControl1.TabPages.Remove(tabPage2); }
            if (!Settings.LastGame.SweetFX.Explosion.Enabled && !Settings.LastGame.SweetFX.Cartoon.Enabled) { tabControl1.TabPages.Remove(tabPage3); }
            if (!Settings.LastGame.SweetFX.CRT.Enabled) { tabControl1.TabPages.Remove(tabPage4); tabControl1.TabPages.Remove(tabPage5); }
            if (!Settings.LastGame.SweetFX.Bloom.Enabled && !Settings.LastGame.SweetFX.HDR.Enabled) { tabControl1.TabPages.Remove(tabPage6); }
            if (!Settings.LastGame.SweetFX.LumaSharpen.Enabled) { tabControl1.TabPages.Remove(tabPage8); }
            if (!Settings.LastGame.SweetFX.Levels.Enabled) { tabControl1.TabPages.Remove(tabPage9); }
            if (!Settings.LastGame.SweetFX.Technicolor.Enabled) { tabControl1.TabPages.Remove(tabPage10); }
            if (!Settings.LastGame.SweetFX.Cineon_DPX.Enabled) { tabControl1.TabPages.Remove(tabPage7); }
            if (!Settings.LastGame.SweetFX.Monochrome.Enabled) { tabControl1.TabPages.Remove(tabPage11); }
            if (!Settings.LastGame.SweetFX.Lift_Gamma_Gain.Enabled) { tabControl1.TabPages.Remove(tabPage12); }
            if (!Settings.LastGame.SweetFX.Tonemap.Enabled) { tabControl1.TabPages.Remove(tabPage13); }
            if (!Settings.LastGame.SweetFX.Vibrance.Enabled) { tabControl1.TabPages.Remove(tabPage14); }
            if (!Settings.LastGame.SweetFX.Curves.Enabled) { tabControl1.TabPages.Remove(tabPage15); }
            if (!Settings.LastGame.SweetFX.Sepia.Enabled && !Settings.LastGame.SweetFX.Dither.Enabled) { tabControl1.TabPages.Remove(tabPage16); }
            if (!Settings.LastGame.SweetFX.Vignette.Enabled) { tabControl1.TabPages.Remove(tabPage17); }
            if (!Settings.LastGame.SweetFX.Border.Enabled) { tabControl1.TabPages.Remove(tabPage18); }
            if (!Settings.LastGame.SweetFX.Splitscreen.Enabled) { tabControl1.TabPages.Remove(tabPage19); }
        }

        private void ShowAllTabs()
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tabPage1);
            tabControl1.TabPages.Add(tabPage2);
            tabControl1.TabPages.Add(tabPage3);
            tabControl1.TabPages.Add(tabPage4);
            tabControl1.TabPages.Add(tabPage5);
            tabControl1.TabPages.Add(tabPage6);
            tabControl1.TabPages.Add(tabPage7);
            tabControl1.TabPages.Add(tabPage8);
            tabControl1.TabPages.Add(tabPage9);
            tabControl1.TabPages.Add(tabPage10);
            tabControl1.TabPages.Add(tabPage11);
            tabControl1.TabPages.Add(tabPage12);
            tabControl1.TabPages.Add(tabPage13);
            tabControl1.TabPages.Add(tabPage14);
            tabControl1.TabPages.Add(tabPage15);
            tabControl1.TabPages.Add(tabPage16);
            tabControl1.TabPages.Add(tabPage17);
            tabControl1.TabPages.Add(tabPage18);
            tabControl1.TabPages.Add(tabPage19);
        }

        private void tabControl1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string file = files[0];
                if (System.IO.File.ReadAllText(file).ToLower().Contains("use_lumasharpen"))
                {
                    if (MessageBox.Show("Are you sure you want to overide all your " + '"' + Settings.LastGame.Name + '"' + " SweetFX settings?", "Sweeter SweetFX", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.IO.File.Copy(file, Settings.LastGame.Directory.FullName + @"\SweetFX_settings.txt", true);
                        SweetFX_Parser.Load(Settings.LastGame);
                    }
                }
            }
        }

        private void tabControl1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) { e.Effect = DragDropEffects.Copy; }
        }

        private void installManagerToolStripMenuItem_Click(object sender, EventArgs e)
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

        #region SMAA

        void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.SMAA.Color_Edge_Detection = (comboBox6.SelectedIndex == 1) ? 0 : 1;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.SMAA.DirectX9_Linear_Blend = checkBox3.Checked;
        }

        private void sMAAToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
            checkBox1.Checked = sMAAToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.SMAA.Enabled = checkBox1.Checked;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.SMAA.Enabled) { tabControl1.TabPages.Remove(tabPage1); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage1; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            sMAAToolStripMenuItem.CheckedChanged -= sMAAToolStripMenuItem_CheckedChanged;
            sMAAToolStripMenuItem.Checked = checkBox1.Checked;
            Settings.LastGame.SweetFX.SMAA.Enabled = checkBox1.Checked;
            sMAAToolStripMenuItem.CheckedChanged += sMAAToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.SMAA.Enabled) { tabControl1.TabPages.Remove(tabPage1); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage1; }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown1.ValueChanged -= new System.EventHandler(this.numericUpDown1_ValueChanged);
            numericUpDown1.Value = (decimal)trackBar1.Value / (decimal)100.00;
            Settings.LastGame.SweetFX.SMAA.Threshold = numericUpDown1.Value;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar1.Scroll -= new System.EventHandler(this.trackBar1_ValueChanged);
            Settings.LastGame.SweetFX.SMAA.Threshold = numericUpDown1.Value;
            trackBar1.Value = Convert.ToInt32(numericUpDown1.Value * (decimal)100.00);
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_ValueChanged);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown2.ValueChanged -= new System.EventHandler(this.numericUpDown2_ValueChanged);
            numericUpDown2.Value = trackBar2.Value;
            Settings.LastGame.SweetFX.SMAA.Max_Search_Steps = trackBar2.Value;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
            trackBar2.Value = Convert.ToInt32(numericUpDown2.Value);
            Settings.LastGame.SweetFX.SMAA.Max_Search_Steps = trackBar2.Value;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown3.ValueChanged -= new System.EventHandler(this.numericUpDown3_ValueChanged);
            numericUpDown3.Value = trackBar3.Value;
            Settings.LastGame.SweetFX.SMAA.Max_Search_Steps_Diag = trackBar3.Value;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar3.Scroll -= new System.EventHandler(this.trackBar3_ValueChanged);
            trackBar3.Value = Convert.ToInt32(numericUpDown3.Value);
            Settings.LastGame.SweetFX.SMAA.Max_Search_Steps_Diag = trackBar3.Value;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_ValueChanged);
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown4.ValueChanged -= new System.EventHandler(this.numericUpDown4_ValueChanged);
            numericUpDown4.Value = trackBar4.Value;
            Settings.LastGame.SweetFX.SMAA.Corner_Rounding = trackBar4.Value;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar4.Scroll -= new System.EventHandler(this.trackBar4_ValueChanged);
            trackBar4.Value = Convert.ToInt32(numericUpDown4.Value);
            Settings.LastGame.SweetFX.SMAA.Corner_Rounding = trackBar4.Value;
            this.trackBar4.Scroll += new System.EventHandler(this.trackBar4_ValueChanged);
        }

        #endregion

        #region FXAA

        private void fXAAToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox6.CheckedChanged -= checkBox6_CheckedChanged;
            checkBox6.Checked = fXAAToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.FXAA.Enabled = checkBox6.Checked;
            checkBox6.CheckedChanged += checkBox6_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.FXAA.Enabled) { tabControl1.TabPages.Remove(tabPage2); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage2; }
        }

        void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            fXAAToolStripMenuItem.CheckedChanged -= fXAAToolStripMenuItem_CheckedChanged;
            fXAAToolStripMenuItem.Checked = checkBox6.Checked;
            Settings.LastGame.SweetFX.FXAA.Enabled = checkBox6.Checked;
            fXAAToolStripMenuItem.CheckedChanged += fXAAToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.FXAA.Enabled) { tabControl1.TabPages.Remove(tabPage2); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage2; }
        }

        void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar8.Scroll -= new System.EventHandler(this.trackBar8_Scroll);
            Settings.LastGame.SweetFX.FXAA.Quality_Preset = Convert.ToInt32(numericUpDown8.Value);
            trackBar8.Value = Settings.LastGame.SweetFX.FXAA.Quality_Preset;
            this.trackBar8.Scroll += new System.EventHandler(this.trackBar8_Scroll);
        }

        void trackBar8_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown8.ValueChanged -= new System.EventHandler(this.numericUpDown8_ValueChanged);
            numericUpDown8.Value = trackBar8.Value;
            Settings.LastGame.SweetFX.FXAA.Quality_Preset = trackBar8.Value;
            this.numericUpDown8.ValueChanged += new System.EventHandler(this.numericUpDown8_ValueChanged);
        }

        void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar7.Scroll -= new System.EventHandler(this.trackBar7_Scroll);
            Settings.LastGame.SweetFX.FXAA.Subpix = numericUpDown7.Value;
            trackBar7.Value = Convert.ToInt32(numericUpDown7.Value * (decimal)1000);
            this.trackBar7.Scroll += new System.EventHandler(this.trackBar7_Scroll);
        }

        void trackBar7_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown7.ValueChanged -= new System.EventHandler(this.numericUpDown7_ValueChanged);
            numericUpDown7.Value = (decimal)trackBar7.Value / (decimal)1000;
            Settings.LastGame.SweetFX.FXAA.Subpix = numericUpDown7.Value;
            this.numericUpDown7.ValueChanged += new System.EventHandler(this.numericUpDown7_ValueChanged);
        }

        void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar6.Scroll -= new System.EventHandler(this.trackBar6_Scroll);
            Settings.LastGame.SweetFX.FXAA.Edge_Threshold = numericUpDown6.Value;
            trackBar6.Value = Convert.ToInt32(numericUpDown6.Value * (decimal)1000);
            this.trackBar6.Scroll += new System.EventHandler(this.trackBar6_Scroll);
        }

        void trackBar6_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown6.ValueChanged -= new System.EventHandler(this.numericUpDown6_ValueChanged);
            numericUpDown6.Value = (decimal)trackBar6.Value / (decimal)1000;
            Settings.LastGame.SweetFX.FXAA.Edge_Threshold = numericUpDown6.Value;
            this.numericUpDown6.ValueChanged += new System.EventHandler(this.numericUpDown6_ValueChanged);
        }

        void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar5.Scroll -= new System.EventHandler(this.trackBar5_Scroll);
            Settings.LastGame.SweetFX.FXAA.Edge_Threshold_Min = numericUpDown5.Value;
            trackBar5.Value = Convert.ToInt32(numericUpDown5.Value * (decimal)1000);
            this.trackBar5.Scroll += new System.EventHandler(this.trackBar5_Scroll);
        }

        void trackBar5_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown5.ValueChanged -= new System.EventHandler(this.numericUpDown5_ValueChanged);
            numericUpDown5.Value = (decimal)trackBar5.Value / (decimal)1000;
            Settings.LastGame.SweetFX.FXAA.Edge_Threshold_Min = numericUpDown5.Value;
            this.numericUpDown5.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
        }

        #endregion

        #region Explosion

        private void explosionCartoonToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.CheckedChanged -= checkBox4_CheckedChanged;
            checkBox4.Checked = explosionCartoonToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Explosion.Enabled = checkBox4.Checked;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Explosion.Enabled || Settings.LastGame.SweetFX.Cartoon.Enabled) { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage3; }
            else { tabControl1.TabPages.Remove(tabPage3); }
        }

        void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            explosionCartoonToolStripMenuItem.CheckedChanged -= explosionCartoonToolStripMenuItem_CheckedChanged;
            explosionCartoonToolStripMenuItem.Checked = checkBox4.Checked;
            Settings.LastGame.SweetFX.Explosion.Enabled = checkBox4.Checked;
            explosionCartoonToolStripMenuItem.CheckedChanged += explosionCartoonToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Explosion.Enabled || Settings.LastGame.SweetFX.Cartoon.Enabled) { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage3; }
            else { tabControl1.TabPages.Remove(tabPage3); }
        }

        void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar12.Scroll -= new System.EventHandler(this.trackBar12_Scroll);
            Settings.LastGame.SweetFX.Explosion.Radius = numericUpDown12.Value;
            trackBar12.Value = Convert.ToInt32(numericUpDown12.Value * (decimal)10);
            this.trackBar12.Scroll += new System.EventHandler(this.trackBar12_Scroll);
        }

        void trackBar12_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown12.ValueChanged -= new System.EventHandler(this.numericUpDown12_ValueChanged);
            numericUpDown12.Value = (decimal)trackBar12.Value / (decimal)10;
            Settings.LastGame.SweetFX.Explosion.Radius = numericUpDown12.Value;
            this.numericUpDown12.ValueChanged += new System.EventHandler(this.numericUpDown12_ValueChanged);
        }

        #endregion

        #region Cartoon

        private void cartoonToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox5.CheckedChanged -= checkBox5_CheckedChanged;
            checkBox5.Checked = cartoonToolStripMenuItem.Enabled;
            Settings.LastGame.SweetFX.Cartoon.Enabled = checkBox5.Checked;
            checkBox5.CheckedChanged += checkBox5_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Explosion.Enabled || Settings.LastGame.SweetFX.Cartoon.Enabled) { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage3; }
            else { tabControl1.TabPages.Remove(tabPage3); }
        }

        void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            cartoonToolStripMenuItem.CheckedChanged -= cartoonToolStripMenuItem_CheckedChanged;
            cartoonToolStripMenuItem.Enabled = checkBox5.Checked;
            Settings.LastGame.SweetFX.Cartoon.Enabled = checkBox5.Checked;
            cartoonToolStripMenuItem.CheckedChanged += cartoonToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Explosion.Enabled || Settings.LastGame.SweetFX.Cartoon.Enabled) { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage3; }
            else { tabControl1.TabPages.Remove(tabPage3); }
        }

        void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar9.Scroll -= new System.EventHandler(this.trackBar9_Scroll);
            trackBar9.Value = Convert.ToInt32(numericUpDown11.Value * (decimal)10);
            Settings.LastGame.SweetFX.Cartoon.Power = numericUpDown9.Value;
            this.trackBar9.Scroll += new System.EventHandler(this.trackBar9_Scroll);
        }

        void trackBar9_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown11.ValueChanged -= new System.EventHandler(this.numericUpDown11_ValueChanged);
            numericUpDown11.Value = (decimal)trackBar9.Value / (decimal)10;
            Settings.LastGame.SweetFX.Cartoon.Power = numericUpDown11.Value;
            this.numericUpDown11.ValueChanged += new System.EventHandler(this.numericUpDown11_ValueChanged);
        }

        void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar11.Scroll -= new System.EventHandler(this.trackBar11_Scroll);
            trackBar11.Value = Convert.ToInt32(numericUpDown9.Value * (decimal)10);
            Settings.LastGame.SweetFX.Cartoon.Edge_Slope = numericUpDown9.Value;
            this.trackBar11.Scroll += new System.EventHandler(this.trackBar11_Scroll);
        }

        void trackBar11_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown9.ValueChanged -= new System.EventHandler(this.numericUpDown9_ValueChanged);
            numericUpDown9.Value = (decimal)trackBar11.Value / (decimal)10;
            Settings.LastGame.SweetFX.Cartoon.Edge_Slope = numericUpDown9.Value;
            this.numericUpDown9.ValueChanged += new System.EventHandler(this.numericUpDown9_ValueChanged);
        }

        #endregion

        #region CRT

        private void cRTToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox12.CheckedChanged -= checkBox12_CheckedChanged;
            checkBox8.CheckedChanged -= checkBox8_CheckedChanged;
            Settings.LastGame.SweetFX.CRT.Enabled = cRTToolStripMenuItem.Checked;
            checkBox12.Checked = cRTToolStripMenuItem.Checked;
            checkBox8.Checked = cRTToolStripMenuItem.Checked;
            checkBox12.CheckedChanged += checkBox12_CheckedChanged;
            checkBox8.CheckedChanged += checkBox8_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.CRT.Enabled) { tabControl1.TabPages.Remove(tabPage4); tabControl1.TabPages.Remove(tabPage5); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage5; }
        }

        void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            checkBox12.CheckedChanged -= checkBox12_CheckedChanged;
            cRTToolStripMenuItem.CheckedChanged -= cRTToolStripMenuItem_CheckedChanged;
            Settings.LastGame.SweetFX.CRT.Enabled = checkBox8.Checked;
            checkBox12.Checked = checkBox8.Checked;
            cRTToolStripMenuItem.Checked = checkBox8.Checked;
            checkBox12.CheckedChanged += checkBox12_CheckedChanged;
            cRTToolStripMenuItem.CheckedChanged += cRTToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.CRT.Enabled) { tabControl1.TabPages.Remove(tabPage4); tabControl1.TabPages.Remove(tabPage5); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage5; }
        }

        void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            checkBox8.CheckedChanged -= checkBox8_CheckedChanged;
            cRTToolStripMenuItem.CheckedChanged -= cRTToolStripMenuItem_CheckedChanged;
            Settings.LastGame.SweetFX.CRT.Enabled = checkBox12.Checked;
            checkBox8.Checked = checkBox12.Checked;
            cRTToolStripMenuItem.Checked = checkBox12.Checked;
            checkBox8.CheckedChanged += checkBox8_CheckedChanged;
            cRTToolStripMenuItem.CheckedChanged += cRTToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.CRT.Enabled) { tabControl1.TabPages.Remove(tabPage4); tabControl1.TabPages.Remove(tabPage5); }
            else { HideUnactiveTabs(); }
        }

        void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar19.Scroll -= new System.EventHandler(this.trackBar19_Scroll);
            trackBar19.Value = Convert.ToInt32(numericUpDown19.Value * (decimal)100);
            Settings.LastGame.SweetFX.CRT.Amount = numericUpDown19.Value;
            this.trackBar19.Scroll += new System.EventHandler(this.trackBar19_Scroll);
        }

        void trackBar19_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown9.ValueChanged -= new System.EventHandler(this.numericUpDown19_ValueChanged);
            numericUpDown19.Value = (decimal)trackBar19.Value / (decimal)100;
            Settings.LastGame.SweetFX.CRT.Amount = numericUpDown19.Value;
            this.numericUpDown19.ValueChanged += new System.EventHandler(this.numericUpDown19_ValueChanged);
        }

        void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar18.Scroll -= new System.EventHandler(this.trackBar18_Scroll);
            trackBar18.Value = Convert.ToInt32(numericUpDown18.Value * (decimal)10);
            Settings.LastGame.SweetFX.CRT.Resolution = numericUpDown18.Value;
            this.trackBar18.Scroll += new System.EventHandler(this.trackBar18_Scroll);
        }

        void trackBar18_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown18.ValueChanged -= new System.EventHandler(this.numericUpDown18_ValueChanged);
            numericUpDown18.Value = (decimal)trackBar18.Value / (decimal)10;
            Settings.LastGame.SweetFX.CRT.Resolution = numericUpDown18.Value;
            this.numericUpDown18.ValueChanged += new System.EventHandler(this.numericUpDown18_ValueChanged);
        }

        void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar17.Scroll -= new System.EventHandler(this.trackBar17_Scroll);
            trackBar17.Value = Convert.ToInt32(numericUpDown17.Value * (decimal)10);
            Settings.LastGame.SweetFX.CRT.Gamma = numericUpDown17.Value;
            this.trackBar17.Scroll += new System.EventHandler(this.trackBar17_Scroll);
        }

        void trackBar17_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown17.ValueChanged -= new System.EventHandler(this.numericUpDown17_ValueChanged);
            numericUpDown17.Value = (decimal)trackBar17.Value / (decimal)10;
            Settings.LastGame.SweetFX.CRT.Gamma = numericUpDown17.Value;
            this.numericUpDown17.ValueChanged += new System.EventHandler(this.numericUpDown17_ValueChanged);
        }

        void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar16.Scroll -= new System.EventHandler(this.trackBar16_Scroll);
            trackBar16.Value = Convert.ToInt32(numericUpDown16.Value * (decimal)10);
            Settings.LastGame.SweetFX.CRT.Monitor_Gamma = numericUpDown16.Value;
            this.trackBar16.Scroll += new System.EventHandler(this.trackBar16_Scroll);
        }

        void trackBar16_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown16.ValueChanged -= new System.EventHandler(this.numericUpDown16_ValueChanged);
            numericUpDown16.Value = (decimal)trackBar16.Value / (decimal)10;
            Settings.LastGame.SweetFX.CRT.Monitor_Gamma = numericUpDown16.Value;
            this.numericUpDown16.ValueChanged += new System.EventHandler(this.numericUpDown16_ValueChanged);
        }

        void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar10.Scroll -= new System.EventHandler(this.trackBar10_Scroll);
            trackBar10.Value = Convert.ToInt32(numericUpDown10.Value * (decimal)10);
            Settings.LastGame.SweetFX.CRT.Brightness = numericUpDown10.Value;
            this.trackBar10.Scroll += new System.EventHandler(this.trackBar10_Scroll);
        }

        void trackBar10_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown10.ValueChanged -= new System.EventHandler(this.numericUpDown10_ValueChanged);
            numericUpDown10.Value = (decimal)trackBar10.Value / (decimal)10;
            Settings.LastGame.SweetFX.CRT.Brightness = numericUpDown10.Value;
            this.numericUpDown10.ValueChanged += new System.EventHandler(this.numericUpDown10_ValueChanged);
        }

        void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar13.Scroll -= new System.EventHandler(this.trackBar13_Scroll);
            trackBar13.Value = Convert.ToInt32(numericUpDown13.Value * (decimal)10);
            Settings.LastGame.SweetFX.CRT.Scanline_Intensity = numericUpDown13.Value;
            this.trackBar13.Scroll += new System.EventHandler(this.trackBar13_Scroll);
        }

        void trackBar13_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown13.ValueChanged -= new System.EventHandler(this.numericUpDown13_ValueChanged);
            numericUpDown13.Value = (decimal)trackBar13.Value / (decimal)10;
            Settings.LastGame.SweetFX.CRT.Scanline_Intensity = numericUpDown13.Value;
            this.numericUpDown13.ValueChanged += new System.EventHandler(this.numericUpDown13_ValueChanged);
        }

        void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.CRT.Scanline_Gaussian = checkBox10.Checked;
        }

        void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar23.Scroll -= new System.EventHandler(this.trackBar23_Scroll);
            trackBar23.Value = Convert.ToInt32(numericUpDown23.Value * (decimal)10000);
            Settings.LastGame.SweetFX.CRT.Corner_Size = numericUpDown23.Value;
            this.trackBar23.Scroll += new System.EventHandler(this.trackBar23_Scroll);
        }

        void trackBar23_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown23.ValueChanged -= new System.EventHandler(this.numericUpDown23_ValueChanged);
            numericUpDown23.Value = (decimal)trackBar23.Value / (decimal)10000;
            Settings.LastGame.SweetFX.CRT.Corner_Size = numericUpDown23.Value;
            this.numericUpDown23.ValueChanged += new System.EventHandler(this.numericUpDown23_ValueChanged);
        }

        void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar22.Scroll -= new System.EventHandler(this.trackBar22_Scroll);
            trackBar22.Value = Convert.ToInt32(numericUpDown22.Value * (decimal)100);
            Settings.LastGame.SweetFX.CRT.Distance = numericUpDown22.Value;
            this.trackBar22.Scroll += new System.EventHandler(this.trackBar22_Scroll);
        }

        void trackBar22_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown22.ValueChanged -= new System.EventHandler(this.numericUpDown22_ValueChanged);
            numericUpDown22.Value = (decimal)trackBar22.Value / (decimal)100;
            Settings.LastGame.SweetFX.CRT.Distance = numericUpDown22.Value;
            this.numericUpDown22.ValueChanged += new System.EventHandler(this.numericUpDown22_ValueChanged);
        }

        void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar21.Scroll -= new System.EventHandler(this.trackBar21_Scroll);
            trackBar21.Value = Convert.ToInt32(numericUpDown21.Value * (decimal)100);
            Settings.LastGame.SweetFX.CRT.AngleX = numericUpDown21.Value;
            this.trackBar21.Scroll -= new System.EventHandler(this.trackBar21_Scroll);
        }

        void trackBar21_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown21.ValueChanged -= new System.EventHandler(this.numericUpDown21_ValueChanged);
            numericUpDown21.Value = (decimal)trackBar21.Value / (decimal)100;
            Settings.LastGame.SweetFX.CRT.AngleX = numericUpDown21.Value;
            this.numericUpDown21.ValueChanged += new System.EventHandler(this.numericUpDown21_ValueChanged);
        }

        void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar20.Scroll -= new System.EventHandler(this.trackBar20_Scroll);
            trackBar20.Value = Convert.ToInt32(numericUpDown20.Value * (decimal)100);
            Settings.LastGame.SweetFX.CRT.AngleY = numericUpDown20.Value;
            this.trackBar20.Scroll += new System.EventHandler(this.trackBar20_Scroll);
        }

        void trackBar20_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown20.ValueChanged -= new System.EventHandler(this.numericUpDown20_ValueChanged);
            numericUpDown20.Value = (decimal)trackBar20.Value / (decimal)100;
            Settings.LastGame.SweetFX.CRT.AngleY = numericUpDown20.Value;
            this.numericUpDown20.ValueChanged += new System.EventHandler(this.numericUpDown20_ValueChanged);
        }

        void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar15.Scroll -= new System.EventHandler(this.trackBar15_Scroll);
            trackBar15.Value = Convert.ToInt32(numericUpDown15.Value * (decimal)10);
            Settings.LastGame.SweetFX.CRT.Curvature_Radius = numericUpDown15.Value;
            this.trackBar15.Scroll += new System.EventHandler(this.trackBar15_Scroll);
        }

        void trackBar15_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown15.ValueChanged -= new System.EventHandler(this.numericUpDown15_ValueChanged);
            numericUpDown15.Value = (decimal)trackBar15.Value / (decimal)10;
            Settings.LastGame.SweetFX.CRT.Curvature_Radius = numericUpDown15.Value;
            this.numericUpDown15.ValueChanged += new System.EventHandler(this.numericUpDown15_ValueChanged);
        }

        void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar14.Scroll -= new System.EventHandler(this.trackBar14_Scroll);
            trackBar14.Value = Convert.ToInt32(numericUpDown14.Value * (decimal)100);
            Settings.LastGame.SweetFX.CRT.Overscan = numericUpDown14.Value;
            this.trackBar14.Scroll += new System.EventHandler(this.trackBar14_Scroll);
        }

        void trackBar14_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown14.ValueChanged -= new System.EventHandler(this.numericUpDown14_ValueChanged);
            numericUpDown14.Value = (decimal)trackBar14.Value / (decimal)100;
            Settings.LastGame.SweetFX.CRT.Overscan = numericUpDown14.Value;
            this.numericUpDown14.ValueChanged += new System.EventHandler(this.numericUpDown14_ValueChanged);
        }

        void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.CRT.Curvature = checkBox7.Checked;
            trackBar15.Enabled = checkBox7.Checked;
            numericUpDown15.Enabled = checkBox7.Checked;
        }

        void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.CRT.Oversample = checkBox14.Checked;
        }

        #endregion

        #region Bloom

        private void bloomToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox16.CheckedChanged -= checkBox16_CheckedChanged;
            checkBox16.Checked = bloomToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Bloom.Enabled = checkBox16.Checked;
            checkBox16.CheckedChanged += checkBox16_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Bloom.Enabled || Settings.LastGame.SweetFX.HDR.Enabled) { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage6; }
            else { tabControl1.TabPages.Remove(tabPage6); }
        }

        void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            bloomToolStripMenuItem.CheckedChanged -= bloomToolStripMenuItem_CheckedChanged;
            bloomToolStripMenuItem.Checked = checkBox16.Checked;
            Settings.LastGame.SweetFX.Bloom.Enabled = checkBox16.Checked;
            bloomToolStripMenuItem.CheckedChanged += bloomToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Bloom.Enabled || Settings.LastGame.SweetFX.HDR.Enabled) { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage6; }
            else { tabControl1.TabPages.Remove(tabPage6); }
        }

        void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar26.Scroll -= new System.EventHandler(this.trackBar26_Scroll);
            trackBar26.Value = Convert.ToInt32(numericUpDown26.Value * (decimal)100);
            Settings.LastGame.SweetFX.Bloom.Threshold = numericUpDown26.Value;
            this.trackBar26.Scroll += new System.EventHandler(this.trackBar26_Scroll);
        }

        void trackBar26_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown26.ValueChanged -= new System.EventHandler(this.numericUpDown26_ValueChanged);
            numericUpDown26.Value = (decimal)trackBar26.Value / (decimal)100;
            Settings.LastGame.SweetFX.Bloom.Threshold = numericUpDown26.Value;
            this.numericUpDown26.ValueChanged += new System.EventHandler(this.numericUpDown26_ValueChanged);
        }

        void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar27.Scroll -= new System.EventHandler(this.trackBar27_Scroll);
            trackBar27.Value = Convert.ToInt32(numericUpDown28.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Bloom.Power = numericUpDown28.Value;
            this.trackBar27.Scroll += new System.EventHandler(this.trackBar27_Scroll);
        }

        void trackBar27_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown28.ValueChanged -= new System.EventHandler(this.numericUpDown28_ValueChanged);
            numericUpDown28.Value = (decimal)trackBar27.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Bloom.Power = numericUpDown28.Value;
            this.numericUpDown28.ValueChanged += new System.EventHandler(this.numericUpDown28_ValueChanged);
        }

        void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar28.Scroll -= new System.EventHandler(this.trackBar28_Scroll);
            trackBar28.Value = Convert.ToInt32(numericUpDown27.Value * (decimal)10000);
            Settings.LastGame.SweetFX.Bloom.Width = numericUpDown27.Value;
            this.trackBar28.Scroll += new System.EventHandler(this.trackBar28_Scroll);
        }

        void trackBar28_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown27.ValueChanged -= new System.EventHandler(this.numericUpDown27_ValueChanged);
            numericUpDown27.Value = (decimal)trackBar28.Value / (decimal)10000;
            Settings.LastGame.SweetFX.Bloom.Width = numericUpDown27.Value;
            this.numericUpDown27.ValueChanged += new System.EventHandler(this.numericUpDown27_ValueChanged);
        }

        #endregion

        #region HDR

        private void hDRToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox15.CheckedChanged -= checkBox15_CheckedChanged;
            checkBox15.Checked = hDRToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.HDR.Enabled = checkBox15.Checked;
            checkBox15.CheckedChanged += checkBox15_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Bloom.Enabled || Settings.LastGame.SweetFX.HDR.Enabled) { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage6; }
            else { tabControl1.TabPages.Remove(tabPage6); }
        }

        void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            hDRToolStripMenuItem.CheckedChanged -= hDRToolStripMenuItem_CheckedChanged;
            Settings.LastGame.SweetFX.HDR.Enabled = checkBox15.Checked;
            hDRToolStripMenuItem.Checked = checkBox15.Checked;
            hDRToolStripMenuItem.CheckedChanged += hDRToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Bloom.Enabled || Settings.LastGame.SweetFX.HDR.Enabled) { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage6; }
            else { tabControl1.TabPages.Remove(tabPage6); }
        }

        void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar24.Scroll -= new System.EventHandler(this.trackBar24_Scroll);
            trackBar24.Value = Convert.ToInt32(numericUpDown25.Value * (decimal)100);
            Settings.LastGame.SweetFX.HDR.Power = numericUpDown25.Value;
            this.trackBar24.Scroll += new System.EventHandler(this.trackBar24_Scroll);
        }

        void trackBar24_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown25.ValueChanged -= new System.EventHandler(this.numericUpDown25_ValueChanged);
            numericUpDown25.Value = (decimal)trackBar24.Value / (decimal)100;
            Settings.LastGame.SweetFX.HDR.Power = numericUpDown25.Value;
            this.numericUpDown25.ValueChanged += new System.EventHandler(this.numericUpDown25_ValueChanged);
        }

        void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar25.Scroll -= new System.EventHandler(this.trackBar25_Scroll);
            trackBar25.Value = Convert.ToInt32(numericUpDown24.Value * (decimal)100);
            Settings.LastGame.SweetFX.HDR.Radius = numericUpDown24.Value;
            this.trackBar25.Scroll += new System.EventHandler(this.trackBar25_Scroll);
        }

        void trackBar25_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown24.ValueChanged -= new System.EventHandler(this.numericUpDown24_ValueChanged);
            numericUpDown24.Value = (decimal)trackBar25.Value / (decimal)100;
            Settings.LastGame.SweetFX.HDR.Radius = numericUpDown24.Value;
            this.numericUpDown24.ValueChanged += new System.EventHandler(this.numericUpDown24_ValueChanged);
        }

        #endregion 

        #region LumaSharpen

        private void lumaSharpenToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox20.CheckedChanged -= checkBox20_CheckedChanged;
            checkBox20.Checked = lumaSharpenToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.LumaSharpen.Enabled = checkBox20.Checked;
            checkBox20.CheckedChanged += checkBox20_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.LumaSharpen.Enabled) { tabControl1.TabPages.Remove(tabPage8); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage8; }
        }

        void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            lumaSharpenToolStripMenuItem.CheckedChanged -= lumaSharpenToolStripMenuItem_CheckedChanged;
            lumaSharpenToolStripMenuItem.Checked = checkBox20.Checked;
            Settings.LastGame.SweetFX.LumaSharpen.Enabled = checkBox20.Checked;
            lumaSharpenToolStripMenuItem.CheckedChanged += lumaSharpenToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.LumaSharpen.Enabled) { tabControl1.TabPages.Remove(tabPage8); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage8; }
        }

        void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar32.Scroll -= new System.EventHandler(this.trackBar32_Scroll);
            trackBar32.Value = Convert.ToInt32(numericUpDown32.Value * (decimal)100);
            Settings.LastGame.SweetFX.LumaSharpen.Strength = numericUpDown32.Value;
            this.trackBar32.Scroll += new System.EventHandler(this.trackBar32_Scroll);
        }

        void trackBar32_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown32.ValueChanged -= new System.EventHandler(this.numericUpDown32_ValueChanged);
            numericUpDown32.Value = (decimal)trackBar32.Value / (decimal)100;
            Settings.LastGame.SweetFX.LumaSharpen.Strength = numericUpDown32.Value;
            this.numericUpDown32.ValueChanged += new System.EventHandler(this.numericUpDown32_ValueChanged);
        }

        void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar31.Scroll -= new System.EventHandler(this.trackBar31_Scroll);
            trackBar31.Value = Convert.ToInt32(numericUpDown31.Value * (decimal)1000);
            Settings.LastGame.SweetFX.LumaSharpen.Clamp = numericUpDown31.Value;
            this.trackBar31.Scroll += new System.EventHandler(this.trackBar31_Scroll);
        }

        void trackBar31_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown31.ValueChanged -= new System.EventHandler(this.numericUpDown31_ValueChanged);
            numericUpDown31.Value = (decimal)trackBar31.Value / (decimal)1000;
            Settings.LastGame.SweetFX.LumaSharpen.Clamp = numericUpDown31.Value;
            this.numericUpDown31.ValueChanged += new System.EventHandler(this.numericUpDown31_ValueChanged);
        }

        void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.LumaSharpen.Pattern = comboBox5.SelectedIndex + 1;
        }

        void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar29.Scroll -= new System.EventHandler(this.trackBar29_Scroll);
            trackBar29.Value = Convert.ToInt32(numericUpDown29.Value * (decimal)10);
            Settings.LastGame.SweetFX.LumaSharpen.Offset_Bias = numericUpDown29.Value;
            this.trackBar29.Scroll += new System.EventHandler(this.trackBar29_Scroll);
        }

        void trackBar29_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown29.ValueChanged -= new System.EventHandler(this.numericUpDown29_ValueChanged);
            numericUpDown29.Value = (decimal)trackBar29.Value / (decimal)10;
            Settings.LastGame.SweetFX.LumaSharpen.Offset_Bias = numericUpDown29.Value;
            this.numericUpDown29.ValueChanged += new System.EventHandler(this.numericUpDown29_ValueChanged);
        }

        void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.LumaSharpen.Show = checkBox19.Checked;
        }

        #endregion

        #region Levels

        private void levelsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox21.CheckedChanged -= checkBox21_CheckedChanged;
            checkBox21.Checked = levelsToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Levels.Enabled = checkBox21.Checked;
            checkBox21.CheckedChanged += checkBox21_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Levels.Enabled) { tabControl1.TabPages.Remove(tabPage9); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage9; }
        }

        void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            levelsToolStripMenuItem.CheckedChanged -= levelsToolStripMenuItem_CheckedChanged;
            levelsToolStripMenuItem.Checked = checkBox21.Checked;
            Settings.LastGame.SweetFX.Levels.Enabled = checkBox21.Checked;
            levelsToolStripMenuItem.CheckedChanged += levelsToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Levels.Enabled) { tabControl1.TabPages.Remove(tabPage9); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage9; }
        }

        void numericUpDown35_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar48.Scroll -= new System.EventHandler(this.trackBar48_Scroll);
            trackBar48.Value = Convert.ToInt32(numericUpDown35.Value);
            Settings.LastGame.SweetFX.Levels.Black_Point = trackBar48.Value;
            this.trackBar48.Scroll += new System.EventHandler(this.trackBar48_Scroll);
        }

        void trackBar48_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown35.ValueChanged -= new System.EventHandler(this.numericUpDown35_ValueChanged);
            numericUpDown35.Value = (decimal)trackBar48.Value;
            Settings.LastGame.SweetFX.Levels.Black_Point = trackBar48.Value;
            this.numericUpDown35.ValueChanged += new System.EventHandler(this.numericUpDown35_ValueChanged);
        }

        void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar33.Scroll -= new System.EventHandler(this.trackBar33_Scroll);
            trackBar33.Value = Convert.ToInt32(numericUpDown33.Value);
            Settings.LastGame.SweetFX.Levels.White_Point = trackBar33.Value;
            this.trackBar33.Scroll += new System.EventHandler(this.trackBar33_Scroll);
        }

        void trackBar33_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown33.ValueChanged -= new System.EventHandler(this.numericUpDown33_ValueChanged);
            numericUpDown33.Value = (decimal)trackBar33.Value;
            Settings.LastGame.SweetFX.Levels.White_Point = trackBar33.Value;
            this.numericUpDown33.ValueChanged += new System.EventHandler(this.numericUpDown33_ValueChanged);
        }

        #endregion

        #region Technicolor

        private void technicolorToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox23.CheckedChanged -= checkBox23_CheckedChanged;
            checkBox23.Checked = technicolorToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Technicolor.Enabled = checkBox23.Checked;
            checkBox23.CheckedChanged += checkBox23_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Technicolor.Enabled) { tabControl1.TabPages.Remove(tabPage10); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage10; }
        }

        void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            technicolorToolStripMenuItem.CheckedChanged -= technicolorToolStripMenuItem_CheckedChanged;
            technicolorToolStripMenuItem.Checked = checkBox23.Checked;
            Settings.LastGame.SweetFX.Technicolor.Enabled = checkBox23.Checked;
            technicolorToolStripMenuItem.CheckedChanged += technicolorToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Technicolor.Enabled) { tabControl1.TabPages.Remove(tabPage10); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage10; }
        }

        void numericUpDown52_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar52.Scroll -= new System.EventHandler(this.trackBar52_Scroll);
            trackBar52.Value = Convert.ToInt32(numericUpDown52.Value * (decimal)100);
            Settings.LastGame.SweetFX.Technicolor.Amount = numericUpDown52.Value;
            this.trackBar52.Scroll += new System.EventHandler(this.trackBar52_Scroll);
        }

        void trackBar52_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown52.ValueChanged -= new System.EventHandler(this.numericUpDown52_ValueChanged);
            numericUpDown52.Value = (decimal)trackBar52.Value / (decimal)100;
            Settings.LastGame.SweetFX.Technicolor.Amount = numericUpDown52.Value;
            this.numericUpDown52.ValueChanged += new System.EventHandler(this.numericUpDown52_ValueChanged);
        }

        void numericUpDown51_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar51.Scroll -= new System.EventHandler(this.trackBar51_Scroll);
            trackBar51.Value = Convert.ToInt32(numericUpDown51.Value * (decimal)100);
            Settings.LastGame.SweetFX.Technicolor.Power = numericUpDown51.Value;
            this.trackBar51.Scroll += new System.EventHandler(this.trackBar51_Scroll);
        }

        void trackBar51_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown51.ValueChanged -= new System.EventHandler(this.numericUpDown51_ValueChanged);
            numericUpDown51.Value = (decimal)trackBar51.Value / (decimal)100;
            Settings.LastGame.SweetFX.Technicolor.Power = numericUpDown51.Value;
            this.numericUpDown51.ValueChanged += new System.EventHandler(this.numericUpDown51_ValueChanged);
        }

        void numericUpDown50_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar50.Scroll -= new System.EventHandler(this.trackBar50_Scroll);
            trackBar50.Value = Convert.ToInt32(numericUpDown50.Value * (decimal)100);
            Settings.LastGame.SweetFX.Technicolor.Red_Negative_Amount = numericUpDown50.Value;
            this.trackBar50.Scroll += new System.EventHandler(this.trackBar50_Scroll);
        }

        void trackBar50_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown50.ValueChanged -= new System.EventHandler(this.numericUpDown50_ValueChanged);
            numericUpDown50.Value = (decimal)trackBar50.Value / (decimal)100;
            Settings.LastGame.SweetFX.Technicolor.Red_Negative_Amount = numericUpDown50.Value;
            this.numericUpDown50.ValueChanged += new System.EventHandler(this.numericUpDown50_ValueChanged);
        }

        void numericUpDown49_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar49.Scroll -= new System.EventHandler(this.trackBar49_Scroll);
            trackBar49.Value = Convert.ToInt32(numericUpDown49.Value * (decimal)100);
            Settings.LastGame.SweetFX.Technicolor.Green_Negative_Amount = numericUpDown49.Value;
            this.trackBar49.Scroll += new System.EventHandler(this.trackBar49_Scroll);
        }

        void trackBar49_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown49.ValueChanged -= new System.EventHandler(this.numericUpDown49_ValueChanged);
            numericUpDown49.Value = (decimal)trackBar49.Value / (decimal)100;
            Settings.LastGame.SweetFX.Technicolor.Green_Negative_Amount = numericUpDown49.Value;
            this.numericUpDown49.ValueChanged += new System.EventHandler(this.numericUpDown49_ValueChanged);
        }

        void numericUpDown48_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar35.Scroll -= new System.EventHandler(this.trackBar35_Scroll);
            trackBar35.Value = Convert.ToInt32(numericUpDown48.Value * (decimal)100);
            Settings.LastGame.SweetFX.Technicolor.Blue_Negative_Amount = numericUpDown48.Value;
            this.trackBar35.Scroll += new System.EventHandler(this.trackBar35_Scroll);
        }

        void trackBar35_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown48.ValueChanged -= new System.EventHandler(this.numericUpDown48_ValueChanged);
            numericUpDown48.Value = (decimal)trackBar35.Value / (decimal)100;
            Settings.LastGame.SweetFX.Technicolor.Blue_Negative_Amount = numericUpDown48.Value;
            this.numericUpDown48.ValueChanged += new System.EventHandler(this.numericUpDown48_ValueChanged);
        }

        #endregion

        #region DPX

        private void dPXToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox9.CheckedChanged -= checkBox9_CheckedChanged;
            checkBox9.Checked = dPXToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Cineon_DPX.Enabled = checkBox9.Checked;
            checkBox9.CheckedChanged += checkBox9_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Cineon_DPX.Enabled) { tabControl1.TabPages.Remove(tabPage7); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage7; }
        }

        void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            dPXToolStripMenuItem.CheckedChanged -= dPXToolStripMenuItem_CheckedChanged;
            dPXToolStripMenuItem.Checked = checkBox9.Checked;
            Settings.LastGame.SweetFX.Cineon_DPX.Enabled = checkBox9.Checked;
            dPXToolStripMenuItem.CheckedChanged += dPXToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Cineon_DPX.Enabled) { tabControl1.TabPages.Remove(tabPage7); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage7; }
        }

        void numericUpDown39_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar39.Scroll -= new System.EventHandler(this.trackBar39_Scroll);
            trackBar39.Value = Convert.ToInt32(numericUpDown39.Value * (decimal)10);
            Settings.LastGame.SweetFX.Cineon_DPX.Color_Gamma = numericUpDown39.Value;
            this.trackBar39.Scroll += new System.EventHandler(this.trackBar39_Scroll);
        }

        void trackBar39_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown39.ValueChanged -= new System.EventHandler(this.numericUpDown39_ValueChanged);
            numericUpDown39.Value = (decimal)trackBar39.Value / (decimal)10;
            Settings.LastGame.SweetFX.Cineon_DPX.Color_Gamma = numericUpDown39.Value;
            this.numericUpDown39.ValueChanged += new System.EventHandler(this.numericUpDown39_ValueChanged);
        }

        void numericUpDown38_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar38.Scroll -= new System.EventHandler(this.trackBar38_Scroll);
            trackBar38.Value = Convert.ToInt32(numericUpDown38.Value * (decimal)10);
            Settings.LastGame.SweetFX.Cineon_DPX.Saturation = numericUpDown38.Value;
            this.trackBar38.Scroll += new System.EventHandler(this.trackBar38_Scroll);
        }

        void trackBar38_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown38.ValueChanged -= new System.EventHandler(this.numericUpDown38_ValueChanged);
            numericUpDown38.Value = (decimal)trackBar38.Value / (decimal)10;
            Settings.LastGame.SweetFX.Cineon_DPX.Saturation = numericUpDown38.Value;
            this.numericUpDown38.ValueChanged += new System.EventHandler(this.numericUpDown38_ValueChanged);
        }

        void numericUpDown36_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar36.Scroll -= new System.EventHandler(this.trackBar36_Scroll);
            trackBar36.Value = Convert.ToInt32(numericUpDown36.Value * (decimal)100);
            Settings.LastGame.SweetFX.Cineon_DPX.Blend = numericUpDown36.Value;
            this.trackBar36.Scroll += new System.EventHandler(this.trackBar36_Scroll);
        }

        void trackBar36_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown36.ValueChanged -= new System.EventHandler(this.numericUpDown36_ValueChanged);
            numericUpDown36.Value = (decimal)trackBar36.Value / (decimal)100;
            Settings.LastGame.SweetFX.Cineon_DPX.Blend = numericUpDown36.Value;
            this.numericUpDown36.ValueChanged += new System.EventHandler(this.numericUpDown36_ValueChanged);
        }

        void numericUpDown34_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar34.Scroll -= new System.EventHandler(this.trackBar34_Scroll);
            trackBar34.Value = Convert.ToInt32(numericUpDown34.Value * (decimal)100);
            Settings.LastGame.SweetFX.Cineon_DPX.Red = numericUpDown34.Value;
            this.trackBar34.Scroll += new System.EventHandler(this.trackBar34_Scroll);
        }

        void trackBar34_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown34.ValueChanged -= new System.EventHandler(this.numericUpDown34_ValueChanged);
            numericUpDown34.Value = (decimal)trackBar34.Value / (decimal)100;
            Settings.LastGame.SweetFX.Cineon_DPX.Red = numericUpDown34.Value;
            this.numericUpDown34.ValueChanged += new System.EventHandler(this.numericUpDown34_ValueChanged);
        }

        void numericUpDown37_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar37.Scroll -= new System.EventHandler(this.trackBar37_Scroll);
            trackBar37.Value = Convert.ToInt32(numericUpDown37.Value * (decimal)100);
            Settings.LastGame.SweetFX.Cineon_DPX.Green = numericUpDown37.Value;
            this.trackBar37.Scroll += new System.EventHandler(this.trackBar37_Scroll);
        }

        void trackBar37_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown37.ValueChanged -= new System.EventHandler(this.numericUpDown37_ValueChanged);
            numericUpDown37.Value = (decimal)trackBar37.Value / (decimal)100;
            Settings.LastGame.SweetFX.Cineon_DPX.Green = numericUpDown37.Value;
            this.numericUpDown37.ValueChanged += new System.EventHandler(this.numericUpDown37_ValueChanged);
        }

        void numericUpDown40_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar40.Scroll -= new System.EventHandler(this.trackBar40_Scroll);
            trackBar40.Value = Convert.ToInt32(numericUpDown40.Value * (decimal)100);
            Settings.LastGame.SweetFX.Cineon_DPX.Blue = numericUpDown40.Value;
            this.trackBar40.Scroll += new System.EventHandler(this.trackBar40_Scroll);
        }

        void trackBar40_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown40.ValueChanged -= new System.EventHandler(this.numericUpDown40_ValueChanged);
            numericUpDown40.Value = (decimal)trackBar40.Value / (decimal)100;
            Settings.LastGame.SweetFX.Cineon_DPX.Blue = numericUpDown40.Value;
            this.numericUpDown40.ValueChanged += new System.EventHandler(this.numericUpDown40_ValueChanged);
        }

        void numericUpDown41_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar41.Scroll -= new System.EventHandler(this.trackBar41_Scroll);
            trackBar41.Value = Convert.ToInt32(numericUpDown41.Value * (decimal)100);
            Settings.LastGame.SweetFX.Cineon_DPX.RedC = numericUpDown41.Value;
            this.trackBar41.Scroll += new System.EventHandler(this.trackBar41_Scroll);
        }

        void trackBar41_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown41.ValueChanged -= new System.EventHandler(this.numericUpDown41_ValueChanged);
            numericUpDown41.Value = (decimal)trackBar41.Value / (decimal)100;
            Settings.LastGame.SweetFX.Cineon_DPX.RedC = numericUpDown41.Value;
            this.numericUpDown41.ValueChanged += new System.EventHandler(this.numericUpDown41_ValueChanged);
        }

        void numericUpDown42_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar42.Scroll -= new System.EventHandler(this.trackBar42_Scroll);
            trackBar42.Value = Convert.ToInt32(numericUpDown42.Value * (decimal)100);
            Settings.LastGame.SweetFX.Cineon_DPX.GreenC = numericUpDown42.Value;
            this.trackBar42.Scroll += new System.EventHandler(this.trackBar42_Scroll);
        }

        void trackBar42_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown42.ValueChanged -= new System.EventHandler(this.numericUpDown42_ValueChanged);
            numericUpDown42.Value = (decimal)trackBar42.Value / (decimal)100;
            Settings.LastGame.SweetFX.Cineon_DPX.GreenC = numericUpDown42.Value;
            this.numericUpDown42.ValueChanged += new System.EventHandler(this.numericUpDown42_ValueChanged);
        }

        void numericUpDown43_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar43.Scroll -= new System.EventHandler(this.trackBar43_Scroll);
            trackBar43.Value = Convert.ToInt32(numericUpDown43.Value * (decimal)100);
            Settings.LastGame.SweetFX.Cineon_DPX.BlueC = numericUpDown43.Value;
            this.trackBar43.Scroll -= new System.EventHandler(this.trackBar43_Scroll);
        }

        void trackBar43_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown43.ValueChanged -= new System.EventHandler(this.numericUpDown43_ValueChanged);
            numericUpDown43.Value = (decimal)trackBar43.Value / (decimal)100;
            Settings.LastGame.SweetFX.Cineon_DPX.BlueC = numericUpDown43.Value;
            this.numericUpDown43.ValueChanged += new System.EventHandler(this.numericUpDown43_ValueChanged);
        }

        #endregion

        #region Monochrome

        private void monochromeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox11.CheckedChanged -= checkBox11_CheckedChanged;
            checkBox11.Checked = monochromeToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Monochrome.Enabled = checkBox11.Checked;
            checkBox11.CheckedChanged += checkBox11_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Monochrome.Enabled) { tabControl1.TabPages.Remove(tabPage11); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage11; }
        }

        void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            monochromeToolStripMenuItem.CheckedChanged -= monochromeToolStripMenuItem_CheckedChanged;
            monochromeToolStripMenuItem.Checked = checkBox11.Checked;
            Settings.LastGame.SweetFX.Monochrome.Enabled = checkBox11.Checked;
            monochromeToolStripMenuItem.CheckedChanged += monochromeToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Monochrome.Enabled) { tabControl1.TabPages.Remove(tabPage11); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage11; }
        }

        void numericUpDown54_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar54.Scroll -= new System.EventHandler(this.trackBar54_Scroll);
            trackBar54.Value = Convert.ToInt32(numericUpDown54.Value * (decimal)100);
            Settings.LastGame.SweetFX.Monochrome.Red = numericUpDown54.Value;
            //
            decimal over = (Settings.LastGame.SweetFX.Monochrome.Red + Settings.LastGame.SweetFX.Monochrome.Green + Settings.LastGame.SweetFX.Monochrome.Blue) - 1;
            if (over != 0)
            {
                this.trackBar44.Scroll -= new System.EventHandler(this.trackBar44_Scroll);
                this.numericUpDown44.ValueChanged -= new System.EventHandler(this.numericUpDown44_ValueChanged);
                this.trackBar45.Scroll -= new System.EventHandler(this.trackBar45_Scroll);
                this.numericUpDown45.ValueChanged -= new System.EventHandler(this.numericUpDown45_ValueChanged);
                //
                decimal check = Settings.LastGame.SweetFX.Monochrome.Green - Settings.LastGame.SweetFX.Monochrome.Blue;
                if (over > 0)
                {
                    decimal green = 0;
                    decimal blue = 0;
                    if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Green -= green;
                    trackBar44.Value -= Convert.ToInt32(green * 100);
                    numericUpDown44.Value -= green;
                    Settings.LastGame.SweetFX.Monochrome.Blue -= blue;
                    trackBar45.Value -= Convert.ToInt32(blue * 100);
                    numericUpDown45.Value -= blue;
                }
                else
                {
                    over = over * -1;
                    decimal green = 0;
                    decimal blue = 0;
                    if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow > 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Green += green;
                    trackBar44.Value += Convert.ToInt32(green * 100);
                    numericUpDown44.Value += green;
                    Settings.LastGame.SweetFX.Monochrome.Blue += blue;
                    trackBar45.Value += Convert.ToInt32(blue * 100);
                    numericUpDown45.Value += blue;
                }
                //
                this.trackBar44.Scroll += new System.EventHandler(this.trackBar44_Scroll);
                this.numericUpDown44.ValueChanged += new System.EventHandler(this.numericUpDown44_ValueChanged);
                this.trackBar45.Scroll += new System.EventHandler(this.trackBar45_Scroll);
                this.numericUpDown45.ValueChanged += new System.EventHandler(this.numericUpDown45_ValueChanged);
            }
            //
            this.trackBar54.Scroll += new System.EventHandler(this.trackBar54_Scroll);
        }

        void trackBar54_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown54.ValueChanged -= new System.EventHandler(this.numericUpDown54_ValueChanged);
            numericUpDown54.Value = (decimal)trackBar54.Value / (decimal)100;
            Settings.LastGame.SweetFX.Monochrome.Red = numericUpDown54.Value;
            decimal over = (Settings.LastGame.SweetFX.Monochrome.Red + Settings.LastGame.SweetFX.Monochrome.Green + Settings.LastGame.SweetFX.Monochrome.Blue) - 1;
            if (over != 0)
            {
                this.trackBar44.Scroll -= new System.EventHandler(this.trackBar44_Scroll);
                this.numericUpDown44.ValueChanged -= new System.EventHandler(this.numericUpDown44_ValueChanged);
                this.trackBar45.Scroll -= new System.EventHandler(this.trackBar45_Scroll);
                this.numericUpDown45.ValueChanged -= new System.EventHandler(this.numericUpDown45_ValueChanged);
                //
                decimal check = Settings.LastGame.SweetFX.Monochrome.Green - Settings.LastGame.SweetFX.Monochrome.Blue;
                if (over > 0)
                {
                    decimal green = 0;
                    decimal blue = 0;
                    if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Green -= green;
                    trackBar44.Value -= Convert.ToInt32(green * 100);
                    numericUpDown44.Value -= green;
                    Settings.LastGame.SweetFX.Monochrome.Blue -= blue;
                    trackBar45.Value -= Convert.ToInt32(blue * 100);
                    numericUpDown45.Value -= blue;
                }
                else
                {
                    over = over * -1;
                    decimal green = 0;
                    decimal blue = 0;
                    if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow > 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Green += green;
                    trackBar44.Value += Convert.ToInt32(green * 100);
                    numericUpDown44.Value += green;
                    Settings.LastGame.SweetFX.Monochrome.Blue += blue;
                    trackBar45.Value += Convert.ToInt32(blue * 100);
                    numericUpDown45.Value += blue;
                }
                //
                this.trackBar44.Scroll += new System.EventHandler(this.trackBar44_Scroll);
                this.numericUpDown44.ValueChanged += new System.EventHandler(this.numericUpDown44_ValueChanged);
                this.trackBar45.Scroll += new System.EventHandler(this.trackBar45_Scroll);
                this.numericUpDown45.ValueChanged += new System.EventHandler(this.numericUpDown45_ValueChanged);
            }
            this.numericUpDown54.ValueChanged += new System.EventHandler(this.numericUpDown54_ValueChanged);
        }

        void numericUpDown44_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar44.Scroll -= new System.EventHandler(this.trackBar44_Scroll);
            trackBar44.Value = Convert.ToInt32(numericUpDown44.Value * (decimal)100);
            Settings.LastGame.SweetFX.Monochrome.Green = numericUpDown44.Value;
            decimal over = (Settings.LastGame.SweetFX.Monochrome.Red + Settings.LastGame.SweetFX.Monochrome.Green + Settings.LastGame.SweetFX.Monochrome.Blue) - 1;
            if (over != 0)
            {
                this.trackBar54.Scroll -= new System.EventHandler(this.trackBar54_Scroll);
                this.numericUpDown54.ValueChanged -= new System.EventHandler(this.numericUpDown54_ValueChanged);
                this.trackBar45.Scroll -= new System.EventHandler(this.trackBar45_Scroll);
                this.numericUpDown45.ValueChanged -= new System.EventHandler(this.numericUpDown45_ValueChanged);
                //
                decimal check = Settings.LastGame.SweetFX.Monochrome.Red - Settings.LastGame.SweetFX.Monochrome.Blue;
                if (over > 0)
                {
                    decimal green = 0;
                    decimal blue = 0;
                    if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Red -= green;
                    trackBar54.Value -= Convert.ToInt32(green * 100);
                    numericUpDown54.Value -= green;
                    Settings.LastGame.SweetFX.Monochrome.Blue -= blue;
                    trackBar45.Value -= Convert.ToInt32(blue * 100);
                    numericUpDown45.Value -= blue;
                }
                else
                {
                    over = over * -1;
                    decimal green = 0;
                    decimal blue = 0;
                    if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow > 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Red += green;
                    trackBar54.Value += Convert.ToInt32(green * 100);
                    numericUpDown54.Value += green;
                    Settings.LastGame.SweetFX.Monochrome.Blue += blue;
                    trackBar45.Value += Convert.ToInt32(blue * 100);
                    numericUpDown45.Value += blue;
                }
                //
                this.trackBar54.Scroll += new System.EventHandler(this.trackBar54_Scroll);
                this.numericUpDown54.ValueChanged += new System.EventHandler(this.numericUpDown54_ValueChanged);
                this.trackBar45.Scroll += new System.EventHandler(this.trackBar45_Scroll);
                this.numericUpDown45.ValueChanged += new System.EventHandler(this.numericUpDown45_ValueChanged);
            }
            this.trackBar44.Scroll += new System.EventHandler(this.trackBar44_Scroll);
        }

        void trackBar44_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown44.ValueChanged -= new System.EventHandler(this.numericUpDown44_ValueChanged);
            numericUpDown44.Value = (decimal)trackBar44.Value / (decimal)100;
            Settings.LastGame.SweetFX.Monochrome.Green = numericUpDown44.Value;
            decimal over = (Settings.LastGame.SweetFX.Monochrome.Red + Settings.LastGame.SweetFX.Monochrome.Green + Settings.LastGame.SweetFX.Monochrome.Blue) - 1;
            if (over != 0)
            {
                this.trackBar54.Scroll -= new System.EventHandler(this.trackBar54_Scroll);
                this.numericUpDown54.ValueChanged -= new System.EventHandler(this.numericUpDown54_ValueChanged);
                this.trackBar45.Scroll -= new System.EventHandler(this.trackBar45_Scroll);
                this.numericUpDown45.ValueChanged -= new System.EventHandler(this.numericUpDown45_ValueChanged);
                //
                decimal check = Settings.LastGame.SweetFX.Monochrome.Red - Settings.LastGame.SweetFX.Monochrome.Blue;
                if (over > 0)
                {
                    decimal green = 0;
                    decimal blue = 0;
                    if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Red -= green;
                    trackBar54.Value -= Convert.ToInt32(green * 100);
                    numericUpDown54.Value -= green;
                    Settings.LastGame.SweetFX.Monochrome.Blue -= blue;
                    trackBar45.Value -= Convert.ToInt32(blue * 100);
                    numericUpDown45.Value -= blue;
                }
                else
                {
                    over = over * -1;
                    decimal green = 0;
                    decimal blue = 0;
                    if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow > 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Red += green;
                    trackBar54.Value += Convert.ToInt32(green * 100);
                    numericUpDown54.Value += green;
                    Settings.LastGame.SweetFX.Monochrome.Blue += blue;
                    trackBar45.Value += Convert.ToInt32(blue * 100);
                    numericUpDown45.Value += blue;
                }
                //
                this.trackBar54.Scroll += new System.EventHandler(this.trackBar54_Scroll);
                this.numericUpDown54.ValueChanged += new System.EventHandler(this.numericUpDown54_ValueChanged);
                this.trackBar45.Scroll += new System.EventHandler(this.trackBar45_Scroll);
                this.numericUpDown45.ValueChanged += new System.EventHandler(this.numericUpDown45_ValueChanged);
            }
            this.numericUpDown44.ValueChanged += new System.EventHandler(this.numericUpDown44_ValueChanged);
        }

        void numericUpDown45_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar45.Scroll -= new System.EventHandler(this.trackBar45_Scroll);
            trackBar45.Value = Convert.ToInt32(numericUpDown45.Value * (decimal)100);
            Settings.LastGame.SweetFX.Monochrome.Blue = numericUpDown45.Value;
            decimal over = (Settings.LastGame.SweetFX.Monochrome.Red + Settings.LastGame.SweetFX.Monochrome.Green + Settings.LastGame.SweetFX.Monochrome.Blue) - 1;
            if (over != 0)
            {
                this.trackBar54.Scroll -= new System.EventHandler(this.trackBar54_Scroll);
                this.numericUpDown54.ValueChanged -= new System.EventHandler(this.numericUpDown54_ValueChanged);
                this.trackBar44.Scroll -= new System.EventHandler(this.trackBar44_Scroll);
                this.numericUpDown44.ValueChanged -= new System.EventHandler(this.numericUpDown44_ValueChanged);
                //
                decimal check = Settings.LastGame.SweetFX.Monochrome.Red - Settings.LastGame.SweetFX.Monochrome.Green;
                if (over > 0)
                {
                    decimal green = 0;
                    decimal blue = 0;
                    if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Red -= green;
                    trackBar54.Value -= Convert.ToInt32(green * 100);
                    numericUpDown54.Value -= green;
                    Settings.LastGame.SweetFX.Monochrome.Green -= blue;
                    trackBar44.Value -= Convert.ToInt32(blue * 100);
                    numericUpDown44.Value -= blue;
                }
                else
                {
                    over = over * -1;
                    decimal green = 0;
                    decimal blue = 0;
                    if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow > 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Red += green;
                    trackBar54.Value += Convert.ToInt32(green * 100);
                    numericUpDown54.Value += green;
                    Settings.LastGame.SweetFX.Monochrome.Green += blue;
                    trackBar44.Value += Convert.ToInt32(blue * 100);
                    numericUpDown44.Value += blue;
                }
                //
                this.trackBar54.Scroll += new System.EventHandler(this.trackBar54_Scroll);
                this.numericUpDown54.ValueChanged += new System.EventHandler(this.numericUpDown54_ValueChanged);
                this.trackBar44.Scroll += new System.EventHandler(this.trackBar44_Scroll);
                this.numericUpDown44.ValueChanged += new System.EventHandler(this.numericUpDown44_ValueChanged);
            }
            this.trackBar45.Scroll += new System.EventHandler(this.trackBar45_Scroll);
        }

        void trackBar45_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown45.ValueChanged -= new System.EventHandler(this.numericUpDown45_ValueChanged);
            numericUpDown45.Value = (decimal)trackBar45.Value / (decimal)100;
            Settings.LastGame.SweetFX.Monochrome.Blue = numericUpDown45.Value;
            decimal over = (Settings.LastGame.SweetFX.Monochrome.Red + Settings.LastGame.SweetFX.Monochrome.Green + Settings.LastGame.SweetFX.Monochrome.Blue) - 1;
            if (over != 0)
            {
                this.trackBar54.Scroll -= new System.EventHandler(this.trackBar54_Scroll);
                this.numericUpDown54.ValueChanged -= new System.EventHandler(this.numericUpDown54_ValueChanged);
                this.trackBar44.Scroll -= new System.EventHandler(this.trackBar44_Scroll);
                this.numericUpDown44.ValueChanged -= new System.EventHandler(this.numericUpDown44_ValueChanged);
                //
                decimal check = Settings.LastGame.SweetFX.Monochrome.Red - Settings.LastGame.SweetFX.Monochrome.Green;
                if (over > 0)
                {
                    decimal green = 0;
                    decimal blue = 0;
                    if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Red -= green;
                    trackBar54.Value -= Convert.ToInt32(green * 100);
                    numericUpDown54.Value -= green;
                    Settings.LastGame.SweetFX.Monochrome.Green -= blue;
                    trackBar44.Value -= Convert.ToInt32(blue * 100);
                    numericUpDown44.Value -= blue;
                }
                else
                {
                    over = over * -1;
                    decimal green = 0;
                    decimal blue = 0;
                    if (check < 0)
                    {
                        decimal overflow = (check * -1) - over;
                        if (overflow > 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            green = (over - overflow) + subtract;
                            blue = overflow - subtract;
                        }
                        else { green = over; }
                    }
                    else if (check > 0)
                    {
                        decimal overflow = check - over;
                        if (overflow < 0)
                        {
                            overflow = overflow * -1;
                            decimal subtract = Math.Round(overflow / 2, 2);
                            blue = (over - overflow) + subtract;
                            green = overflow - subtract;
                        }
                        else { blue = over; }
                    }
                    else
                    {
                        decimal subtract = Math.Round(over / 2, 2);
                        blue = subtract;
                        green = over - subtract;
                    }
                    Settings.LastGame.SweetFX.Monochrome.Red += green;
                    trackBar54.Value += Convert.ToInt32(green * 100);
                    numericUpDown54.Value += green;
                    Settings.LastGame.SweetFX.Monochrome.Green += blue;
                    trackBar44.Value += Convert.ToInt32(blue * 100);
                    numericUpDown44.Value += blue;
                }
                //
                this.trackBar54.Scroll += new System.EventHandler(this.trackBar54_Scroll);
                this.numericUpDown54.ValueChanged += new System.EventHandler(this.numericUpDown54_ValueChanged);
                this.trackBar44.Scroll += new System.EventHandler(this.trackBar44_Scroll);
                this.numericUpDown44.ValueChanged += new System.EventHandler(this.numericUpDown44_ValueChanged);
            }
            this.numericUpDown45.ValueChanged += new System.EventHandler(this.numericUpDown45_ValueChanged);
        }

        #endregion

        #region Lift Gamma Gain

        private void liftGammaGainToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox13.CheckedChanged -= checkBox13_CheckedChanged;
            checkBox13.Checked = liftGammaGainToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Enabled = checkBox13.Checked;
            checkBox13.CheckedChanged += checkBox13_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Lift_Gamma_Gain.Enabled) { tabControl1.TabPages.Remove(tabPage12); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage12; }
        }

        void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            liftGammaGainToolStripMenuItem.CheckedChanged -= liftGammaGainToolStripMenuItem_CheckedChanged;
            liftGammaGainToolStripMenuItem.Checked = checkBox13.Checked;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Enabled = checkBox13.Checked;
            liftGammaGainToolStripMenuItem.CheckedChanged += liftGammaGainToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Lift_Gamma_Gain.Enabled) { tabControl1.TabPages.Remove(tabPage12); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage12; }
        }

        void numericUpDown53_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar53.Scroll -= new System.EventHandler(this.trackBar53_Scroll);
            trackBar53.Value = Convert.ToInt32(numericUpDown53.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Red = numericUpDown53.Value;
            this.trackBar53.Scroll += new System.EventHandler(this.trackBar53_Scroll);
        }

        void trackBar53_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown53.ValueChanged -= new System.EventHandler(this.numericUpDown53_ValueChanged);
            numericUpDown53.Value = (decimal)trackBar53.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Red = numericUpDown53.Value;
            this.numericUpDown53.ValueChanged += new System.EventHandler(this.numericUpDown53_ValueChanged);
        }

        void numericUpDown46_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar46.Scroll -= new System.EventHandler(this.trackBar46_Scroll);
            trackBar46.Value = Convert.ToInt32(numericUpDown46.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Green = numericUpDown46.Value;
            this.trackBar46.Scroll += new System.EventHandler(this.trackBar46_Scroll);
        }

        void trackBar46_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown46.ValueChanged -= new System.EventHandler(this.numericUpDown46_ValueChanged);
            numericUpDown46.Value = (decimal)trackBar46.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Green = numericUpDown46.Value;
            this.numericUpDown46.ValueChanged += new System.EventHandler(this.numericUpDown46_ValueChanged);
        }

        void numericUpDown47_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar47.Scroll -= new System.EventHandler(this.trackBar47_Scroll);
            trackBar47.Value = Convert.ToInt32(numericUpDown47.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Blue = numericUpDown47.Value;
            this.trackBar47.Scroll += new System.EventHandler(this.trackBar47_Scroll);
        }

        void trackBar47_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown47.ValueChanged -= new System.EventHandler(this.numericUpDown47_ValueChanged);
            numericUpDown47.Value = (decimal)trackBar47.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Blue = numericUpDown47.Value;
            this.numericUpDown47.ValueChanged += new System.EventHandler(this.numericUpDown47_ValueChanged);
        }

        void numericUpDown57_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar57.Scroll -= new System.EventHandler(this.trackBar57_Scroll);
            trackBar57.Value = Convert.ToInt32(numericUpDown57.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Red = numericUpDown57.Value;
            this.trackBar57.Scroll += new System.EventHandler(this.trackBar57_Scroll);
        }

        void trackBar57_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown57.ValueChanged -= new System.EventHandler(this.numericUpDown57_ValueChanged);
            numericUpDown57.Value = (decimal)trackBar57.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Red = numericUpDown57.Value;
            this.numericUpDown57.ValueChanged += new System.EventHandler(this.numericUpDown57_ValueChanged);
        }

        void numericUpDown56_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar56.Scroll -= new System.EventHandler(this.trackBar56_Scroll);
            trackBar56.Value = Convert.ToInt32(numericUpDown56.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Green = numericUpDown56.Value;
            this.trackBar56.Scroll += new System.EventHandler(this.trackBar56_Scroll);
        }

        void trackBar56_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown56.ValueChanged -= new System.EventHandler(this.numericUpDown56_ValueChanged);
            numericUpDown56.Value = (decimal)trackBar56.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Green = numericUpDown56.Value;
            this.numericUpDown56.ValueChanged += new System.EventHandler(this.numericUpDown56_ValueChanged);
        }

        void numericUpDown55_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar55.Scroll -= new System.EventHandler(this.trackBar55_Scroll);
            trackBar55.Value = Convert.ToInt32(numericUpDown55.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Blue = numericUpDown55.Value;
            this.trackBar55.Scroll += new System.EventHandler(this.trackBar55_Scroll);
        }

        void trackBar55_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown55.ValueChanged -= new System.EventHandler(this.numericUpDown55_ValueChanged);
            numericUpDown55.Value = (decimal)trackBar55.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Blue = numericUpDown55.Value;
            this.numericUpDown55.ValueChanged += new System.EventHandler(this.numericUpDown55_ValueChanged);
        }

        void numericUpDown60_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar60.Scroll -= new System.EventHandler(this.trackBar60_Scroll);
            trackBar60.Value = Convert.ToInt32(numericUpDown60.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Red = numericUpDown60.Value;
            this.trackBar60.Scroll -= new System.EventHandler(this.trackBar60_Scroll);
        }

        void trackBar60_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown60.ValueChanged -= new System.EventHandler(this.numericUpDown60_ValueChanged);
            numericUpDown60.Value = (decimal)trackBar60.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Red = numericUpDown60.Value;
            this.numericUpDown60.ValueChanged += new System.EventHandler(this.numericUpDown60_ValueChanged);
        }

        void numericUpDown59_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar59.Scroll -= new System.EventHandler(this.trackBar59_Scroll);
            trackBar59.Value = Convert.ToInt32(numericUpDown59.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Green = numericUpDown59.Value;
            this.trackBar59.Scroll += new System.EventHandler(this.trackBar59_Scroll);
        }

        void trackBar59_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown59.ValueChanged -= new System.EventHandler(this.numericUpDown59_ValueChanged);
            numericUpDown59.Value = (decimal)trackBar59.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Green = numericUpDown59.Value;
            this.numericUpDown59.ValueChanged += new System.EventHandler(this.numericUpDown59_ValueChanged);
        }

        void numericUpDown58_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar58.Scroll -= new System.EventHandler(this.trackBar58_Scroll);
            trackBar58.Value = Convert.ToInt32(numericUpDown58.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Blue = numericUpDown58.Value;
            this.trackBar58.Scroll += new System.EventHandler(this.trackBar58_Scroll);
        }

        void trackBar58_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown58.ValueChanged -= new System.EventHandler(this.numericUpDown58_ValueChanged);
            numericUpDown58.Value = (decimal)trackBar58.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Blue = numericUpDown58.Value;
            this.numericUpDown58.ValueChanged += new System.EventHandler(this.numericUpDown58_ValueChanged);
        }

        #endregion

        #region Tonemap

        private void tonemapToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox17.CheckedChanged -= checkBox17_CheckedChanged;
            checkBox17.Checked = tonemapToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Tonemap.Enabled = checkBox17.Checked;
            checkBox17.CheckedChanged += checkBox17_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Tonemap.Enabled) { tabControl1.TabPages.Remove(tabPage13); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage13; }
        }

        void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            tonemapToolStripMenuItem.CheckedChanged -= tonemapToolStripMenuItem_CheckedChanged;
            Settings.LastGame.SweetFX.Tonemap.Enabled = checkBox17.Checked;
            tonemapToolStripMenuItem.Checked = checkBox17.Checked;
            tonemapToolStripMenuItem.CheckedChanged += tonemapToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Tonemap.Enabled) { tabControl1.TabPages.Remove(tabPage13); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage13; }
        }

        void numericUpDown69_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar69.Scroll -= new System.EventHandler(this.trackBar69_Scroll);
            trackBar69.Value = Convert.ToInt32(numericUpDown69.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Tonemap.Gamma = numericUpDown69.Value;
            this.trackBar69.Scroll += new System.EventHandler(this.trackBar69_Scroll);
        }

        void trackBar69_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown69.ValueChanged -= new System.EventHandler(this.numericUpDown69_ValueChanged);
            numericUpDown69.Value = (decimal)trackBar69.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Tonemap.Gamma = numericUpDown69.Value;
            this.numericUpDown69.ValueChanged += new System.EventHandler(this.numericUpDown69_ValueChanged);
        }

        void numericUpDown67_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar67.Scroll -= new System.EventHandler(this.trackBar67_Scroll);
            trackBar67.Value = Convert.ToInt32(numericUpDown67.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Tonemap.Exposure = numericUpDown67.Value;
            this.trackBar67.Scroll += new System.EventHandler(this.trackBar67_Scroll);
        }

        void trackBar67_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown67.ValueChanged -= new System.EventHandler(this.numericUpDown67_ValueChanged);
            numericUpDown67.Value = (decimal)trackBar67.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Tonemap.Exposure = numericUpDown67.Value;
            this.numericUpDown67.ValueChanged += new System.EventHandler(this.numericUpDown67_ValueChanged);
        }

        void numericUpDown61_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar61.Scroll -= new System.EventHandler(this.trackBar61_Scroll);
            trackBar61.Value = Convert.ToInt32(numericUpDown61.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Tonemap.Bleach = numericUpDown61.Value;
            this.trackBar61.Scroll += new System.EventHandler(this.trackBar61_Scroll);
        }

        void trackBar61_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown61.ValueChanged -= new System.EventHandler(this.numericUpDown61_ValueChanged);
            numericUpDown61.Value = (decimal)trackBar61.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Tonemap.Bleach = numericUpDown61.Value;
            this.numericUpDown61.ValueChanged += new System.EventHandler(this.numericUpDown61_ValueChanged);
        }

        void numericUpDown63_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar63.Scroll -= new System.EventHandler(this.trackBar63_Scroll);
            trackBar63.Value = Convert.ToInt32(numericUpDown63.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Tonemap.Defog = numericUpDown63.Value;
            this.trackBar63.Scroll += new System.EventHandler(this.trackBar63_Scroll);
        }

        void trackBar63_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown63.ValueChanged -= new System.EventHandler(this.numericUpDown63_ValueChanged);
            numericUpDown63.Value = (decimal)trackBar63.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Tonemap.Defog = numericUpDown63.Value;
            this.numericUpDown63.ValueChanged += new System.EventHandler(this.numericUpDown63_ValueChanged);
        }

        void numericUpDown62_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar62.Scroll -= new System.EventHandler(this.trackBar62_Scroll);
            trackBar62.Value = Convert.ToInt32(numericUpDown62.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Tonemap.Saturation = numericUpDown62.Value;
            this.trackBar62.Scroll += new System.EventHandler(this.trackBar62_Scroll);
        }

        void trackBar62_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown62.ValueChanged -= new System.EventHandler(this.numericUpDown62_ValueChanged);
            numericUpDown62.Value = (decimal)trackBar62.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Tonemap.Saturation = numericUpDown62.Value;
            this.numericUpDown62.ValueChanged += new System.EventHandler(this.numericUpDown62_ValueChanged);
        }

        void numericUpDown66_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar66.Scroll -= new System.EventHandler(this.trackBar66_Scroll);
            trackBar66.Value = Convert.ToInt32(numericUpDown66.Value * (decimal)100);
            Settings.LastGame.SweetFX.Tonemap.Fog_Red = numericUpDown66.Value;
            this.trackBar66.Scroll += new System.EventHandler(this.trackBar66_Scroll);
        }

        void trackBar66_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown66.ValueChanged -= new System.EventHandler(this.numericUpDown66_ValueChanged);
            numericUpDown66.Value = (decimal)trackBar66.Value / (decimal)100;
            Settings.LastGame.SweetFX.Tonemap.Fog_Red = numericUpDown66.Value;
            this.numericUpDown66.ValueChanged += new System.EventHandler(this.numericUpDown62_ValueChanged);
        }

        void numericUpDown64_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar64.Scroll -= new System.EventHandler(this.trackBar64_Scroll);
            trackBar64.Value = Convert.ToInt32(numericUpDown64.Value * (decimal)100);
            Settings.LastGame.SweetFX.Tonemap.Fog_Green = numericUpDown64.Value;
            this.trackBar64.Scroll += new System.EventHandler(this.trackBar64_Scroll);
        }

        void trackBar64_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown64.ValueChanged -= new System.EventHandler(this.numericUpDown64_ValueChanged);
            numericUpDown64.Value = (decimal)trackBar64.Value / (decimal)100;
            Settings.LastGame.SweetFX.Tonemap.Fog_Green = numericUpDown64.Value;
            this.numericUpDown64.ValueChanged += new System.EventHandler(this.numericUpDown64_ValueChanged);
        }

        void numericUpDown65_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar65.Scroll -= new System.EventHandler(this.trackBar65_Scroll);
            trackBar65.Value = Convert.ToInt32(numericUpDown65.Value * (decimal)100);
            Settings.LastGame.SweetFX.Tonemap.Fog_Blue = numericUpDown65.Value;
            this.trackBar65.Scroll += new System.EventHandler(this.trackBar65_Scroll);
        }

        void trackBar65_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown65.ValueChanged -= new System.EventHandler(this.numericUpDown65_ValueChanged);
            numericUpDown65.Value = (decimal)trackBar65.Value / (decimal)100;
            Settings.LastGame.SweetFX.Tonemap.Fog_Blue = numericUpDown65.Value;
            this.numericUpDown65.ValueChanged += new System.EventHandler(this.numericUpDown65_ValueChanged);
        }

        #endregion

        #region Vibrance

        private void vibranceToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox18.CheckedChanged -= checkBox18_CheckedChanged;
            checkBox18.Checked = vibranceToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Vibrance.Enabled = checkBox18.Checked;
            checkBox18.CheckedChanged += checkBox18_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Vibrance.Enabled) { tabControl1.TabPages.Remove(tabPage14); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage14; }
        }

        void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            vibranceToolStripMenuItem.CheckedChanged -= vibranceToolStripMenuItem_CheckedChanged;
            vibranceToolStripMenuItem.Checked = checkBox18.Checked;
            Settings.LastGame.SweetFX.Vibrance.Enabled = checkBox18.Checked;
            vibranceToolStripMenuItem.CheckedChanged += vibranceToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Vibrance.Enabled) { tabControl1.TabPages.Remove(tabPage14); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage14; }
        }

        void numericUpDown76_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar76.Scroll -= new System.EventHandler(this.trackBar76_Scroll);
            trackBar76.Value = Convert.ToInt32(numericUpDown76.Value * (decimal)100);
            Settings.LastGame.SweetFX.Vibrance.Vibrance = numericUpDown76.Value;
            this.trackBar76.Scroll += new System.EventHandler(this.trackBar76_Scroll);
        }

        void trackBar76_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown76.ValueChanged -= new System.EventHandler(this.numericUpDown76_ValueChanged);
            numericUpDown76.Value = (decimal)trackBar76.Value / (decimal)100;
            Settings.LastGame.SweetFX.Vibrance.Vibrance = numericUpDown76.Value;
            this.numericUpDown76.ValueChanged += new System.EventHandler(this.numericUpDown76_ValueChanged);
        }

        void numericUpDown74_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar74.Scroll -= new System.EventHandler(this.trackBar74_Scroll);
            trackBar74.Value = Convert.ToInt32(numericUpDown74.Value * (decimal)100);
            Settings.LastGame.SweetFX.Vibrance.Red = numericUpDown74.Value;
            this.trackBar74.Scroll += new System.EventHandler(this.trackBar74_Scroll);
        }

        void trackBar74_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown74.ValueChanged -= new System.EventHandler(this.numericUpDown74_ValueChanged);
            numericUpDown74.Value = (decimal)trackBar74.Value / (decimal)100;
            Settings.LastGame.SweetFX.Vibrance.Red = numericUpDown74.Value;
            this.numericUpDown74.ValueChanged += new System.EventHandler(this.numericUpDown74_ValueChanged);
        }

        void numericUpDown68_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar68.Scroll -= new System.EventHandler(this.trackBar68_Scroll);
            trackBar68.Value = Convert.ToInt32(numericUpDown68.Value * (decimal)100);
            Settings.LastGame.SweetFX.Vibrance.Green = numericUpDown68.Value;
            this.trackBar68.Scroll += new System.EventHandler(this.trackBar68_Scroll);
        }

        void trackBar68_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown68.ValueChanged -= new System.EventHandler(this.numericUpDown68_ValueChanged);
            numericUpDown68.Value = (decimal)trackBar68.Value / (decimal)100;
            Settings.LastGame.SweetFX.Vibrance.Green = numericUpDown68.Value;
            this.numericUpDown68.ValueChanged += new System.EventHandler(this.numericUpDown68_ValueChanged);
        }

        void numericUpDown70_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar70.Scroll -= new System.EventHandler(this.trackBar70_Scroll);
            trackBar70.Value = Convert.ToInt32(numericUpDown70.Value * (decimal)100);
            Settings.LastGame.SweetFX.Vibrance.Blue = numericUpDown70.Value;
            this.trackBar70.Scroll += new System.EventHandler(this.trackBar70_Scroll);
        }

        void trackBar70_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown70.ValueChanged -= new System.EventHandler(this.numericUpDown70_ValueChanged);
            numericUpDown70.Value = (decimal)trackBar70.Value / (decimal)100;
            Settings.LastGame.SweetFX.Vibrance.Blue = numericUpDown70.Value;
            this.numericUpDown70.ValueChanged += new System.EventHandler(this.numericUpDown70_ValueChanged);
        }

        #endregion

        #region Curves

        private void curvesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox24.CheckedChanged -= checkBox24_CheckedChanged;
            checkBox24.Checked = curvesToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Curves.Enabled = checkBox24.Checked;
            checkBox24.CheckedChanged += checkBox24_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Curves.Enabled) { tabControl1.TabPages.Remove(tabPage15); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage15; }
        }

        void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            curvesToolStripMenuItem.CheckedChanged -= curvesToolStripMenuItem_CheckedChanged;
            curvesToolStripMenuItem.Checked = checkBox24.Checked;
            Settings.LastGame.SweetFX.Curves.Enabled = checkBox24.Checked;
            curvesToolStripMenuItem.CheckedChanged += curvesToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Curves.Enabled) { tabControl1.TabPages.Remove(tabPage15); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage15; }
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.Curves.Mode = comboBox1.SelectedIndex;
        }

        void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.Curves.Formula = comboBox7.SelectedIndex + 1;
        }

        void numericUpDown73_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar73.Scroll -= new System.EventHandler(this.trackBar73_Scroll);
            trackBar73.Value = Convert.ToInt32(numericUpDown73.Value * (decimal)100);
            Settings.LastGame.SweetFX.Curves.Contrast = numericUpDown73.Value;
            this.trackBar73.Scroll += new System.EventHandler(this.trackBar73_Scroll);
        }

        void trackBar73_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown73.ValueChanged -= new System.EventHandler(this.numericUpDown73_ValueChanged);
            numericUpDown73.Value = (decimal)trackBar73.Value / (decimal)100;
            Settings.LastGame.SweetFX.Curves.Contrast = numericUpDown73.Value;
            this.numericUpDown73.ValueChanged += new System.EventHandler(this.numericUpDown73_ValueChanged);
        }

        #endregion

        #region Dither

        private void ditherToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox26.CheckedChanged -= checkBox26_CheckedChanged;
            checkBox26.Checked = ditherToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Dither.Enabled = checkBox26.Checked;
            checkBox26.CheckedChanged += checkBox26_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Sepia.Enabled || Settings.LastGame.SweetFX.Dither.Enabled) { tabControl1.TabPages.Add(tabPage16); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage16; }
        }

        void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            ditherToolStripMenuItem.CheckedChanged -= ditherToolStripMenuItem_CheckedChanged;
            ditherToolStripMenuItem.Checked = checkBox26.Checked;
            Settings.LastGame.SweetFX.Dither.Enabled = checkBox26.Checked;
            ditherToolStripMenuItem.CheckedChanged += ditherToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Sepia.Enabled || Settings.LastGame.SweetFX.Dither.Enabled) { tabControl1.TabPages.Add(tabPage16); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage16; }
        }

        void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.Dither.Method = comboBox3.SelectedIndex;
        }

        #endregion

        #region Sepia

        private void sepiaToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox22.CheckedChanged -= checkBox22_CheckedChanged;
            checkBox22.Checked = sepiaToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Sepia.Enabled = checkBox22.Checked;
            checkBox22.CheckedChanged += checkBox22_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Sepia.Enabled || Settings.LastGame.SweetFX.Dither.Enabled) { tabControl1.TabPages.Add(tabPage16); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage16; }
        }

        void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            sepiaToolStripMenuItem.CheckedChanged -= sepiaToolStripMenuItem_CheckedChanged;
            sepiaToolStripMenuItem.Checked = checkBox22.Checked;
            Settings.LastGame.SweetFX.Sepia.Enabled = checkBox22.Checked;
            sepiaToolStripMenuItem.CheckedChanged += sepiaToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (Settings.LastGame.SweetFX.Sepia.Enabled || Settings.LastGame.SweetFX.Dither.Enabled) { tabControl1.TabPages.Add(tabPage16); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage16; }
        }

        void numericUpDown82_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar82.Scroll -= new System.EventHandler(this.trackBar82_Scroll);
            trackBar82.Value = Convert.ToInt32(numericUpDown82.Value * (decimal)100);
            Settings.LastGame.SweetFX.Sepia.Grey_Power = numericUpDown82.Value;
            this.trackBar82.Scroll += new System.EventHandler(this.trackBar82_Scroll);
        }

        void trackBar82_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown82.ValueChanged -= new System.EventHandler(this.numericUpDown82_ValueChanged);
            numericUpDown82.Value = (decimal)trackBar82.Value / (decimal)100;
            Settings.LastGame.SweetFX.Sepia.Grey_Power = numericUpDown82.Value;
            this.numericUpDown82.ValueChanged += new System.EventHandler(this.numericUpDown82_ValueChanged);
        }

        void numericUpDown78_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar78.Scroll -= new System.EventHandler(this.trackBar78_Scroll);
            trackBar78.Value = Convert.ToInt32(numericUpDown78.Value * (decimal)100);
            Settings.LastGame.SweetFX.Sepia.Power = numericUpDown78.Value;
            this.trackBar78.Scroll += new System.EventHandler(this.trackBar78_Scroll);
        }

        void trackBar78_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown78.ValueChanged -= new System.EventHandler(this.numericUpDown78_ValueChanged);
            numericUpDown78.Value = (decimal)trackBar78.Value / (decimal)100;
            Settings.LastGame.SweetFX.Sepia.Power = numericUpDown78.Value;
            this.numericUpDown78.ValueChanged += new System.EventHandler(this.numericUpDown78_ValueChanged);
        }

        void numericUpDown80_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar80.Scroll -= new System.EventHandler(this.trackBar80_Scroll);
            trackBar80.Value = Convert.ToInt32(numericUpDown80.Value * (decimal)100);
            Settings.LastGame.SweetFX.Sepia.Red = numericUpDown80.Value;
            this.trackBar80.Scroll += new System.EventHandler(this.trackBar80_Scroll);
        }

        void trackBar80_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown80.ValueChanged -= new System.EventHandler(this.numericUpDown80_ValueChanged);
            numericUpDown80.Value = (decimal)trackBar80.Value / (decimal)100;
            Settings.LastGame.SweetFX.Sepia.Red = numericUpDown80.Value;
            this.numericUpDown80.ValueChanged += new System.EventHandler(this.numericUpDown80_ValueChanged);
        }

        void numericUpDown75_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar75.Scroll -= new System.EventHandler(this.trackBar75_Scroll);
            trackBar75.Value = Convert.ToInt32(numericUpDown75.Value * (decimal)100);
            Settings.LastGame.SweetFX.Sepia.Green = numericUpDown75.Value;
            this.trackBar75.Scroll += new System.EventHandler(this.trackBar75_Scroll);
        }

        void trackBar75_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown75.ValueChanged -= new System.EventHandler(this.numericUpDown75_ValueChanged);
            numericUpDown75.Value = (decimal)trackBar75.Value / (decimal)100;
            Settings.LastGame.SweetFX.Sepia.Green = numericUpDown75.Value;
            this.numericUpDown75.ValueChanged += new System.EventHandler(this.numericUpDown75_ValueChanged);
        }

        void numericUpDown71_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar71.Scroll -= new System.EventHandler(this.trackBar71_Scroll);
            trackBar71.Value = Convert.ToInt32(numericUpDown71.Value * (decimal)100);
            Settings.LastGame.SweetFX.Sepia.Blue = numericUpDown71.Value;
            this.trackBar71.Scroll += new System.EventHandler(this.trackBar71_Scroll);
        }

        void trackBar71_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown71.ValueChanged -= new System.EventHandler(this.numericUpDown71_ValueChanged);
            numericUpDown71.Value = (decimal)trackBar71.Value / (decimal)100;
            Settings.LastGame.SweetFX.Sepia.Blue = numericUpDown71.Value;
            this.numericUpDown71.ValueChanged += new System.EventHandler(this.numericUpDown71_ValueChanged);
        }

        #endregion

        #region Vignette

        private void vignetteToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox25.CheckedChanged -= checkBox25_CheckedChanged;
            checkBox25.Checked = vignetteToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Vignette.Enabled = checkBox25.Checked;
            checkBox25.CheckedChanged += checkBox25_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Vignette.Enabled) { tabControl1.TabPages.Remove(tabPage17); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage17; }
        }

        void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            vignetteToolStripMenuItem.CheckedChanged -= vignetteToolStripMenuItem_CheckedChanged;
            vignetteToolStripMenuItem.Checked = checkBox25.Checked;
            Settings.LastGame.SweetFX.Vignette.Enabled = checkBox25.Checked;
            vignetteToolStripMenuItem.CheckedChanged += vignetteToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Vignette.Enabled) { tabControl1.TabPages.Remove(tabPage17); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage17; }
        }

        void numericUpDown77_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar77.Scroll -= new System.EventHandler(this.trackBar77_Scroll);
            trackBar77.Value = Convert.ToInt32(numericUpDown77.Value * (decimal)100);
            Settings.LastGame.SweetFX.Vignette.Ratio = numericUpDown77.Value;
            this.trackBar77.Scroll += new System.EventHandler(this.trackBar77_Scroll);
        }

        void trackBar77_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown77.ValueChanged -= new System.EventHandler(this.numericUpDown77_ValueChanged);
            numericUpDown77.Value = (decimal)trackBar77.Value / (decimal)100;
            Settings.LastGame.SweetFX.Vignette.Ratio = numericUpDown77.Value;
            this.numericUpDown77.ValueChanged += new System.EventHandler(this.numericUpDown77_ValueChanged);
        }

        void numericUpDown79_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar79.Scroll -= new System.EventHandler(this.trackBar79_Scroll);
            trackBar79.Value = Convert.ToInt32(numericUpDown79.Value * (decimal)100);
            Settings.LastGame.SweetFX.Vignette.Radius = numericUpDown79.Value;
            this.trackBar79.Scroll += new System.EventHandler(this.trackBar79_Scroll);
        }

        void trackBar79_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown79.ValueChanged -= new System.EventHandler(this.numericUpDown79_ValueChanged);
            numericUpDown79.Value = (decimal)trackBar79.Value / (decimal)100;
            Settings.LastGame.SweetFX.Vignette.Radius = numericUpDown79.Value;
            this.numericUpDown79.ValueChanged += new System.EventHandler(this.numericUpDown79_ValueChanged);
        }

        void numericUpDown81_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar81.Scroll -= new System.EventHandler(this.trackBar81_Scroll);
            trackBar81.Value = Convert.ToInt32(numericUpDown81.Value * (decimal)100);
            Settings.LastGame.SweetFX.Vignette.Amount = numericUpDown81.Value;
            this.trackBar81.Scroll += new System.EventHandler(this.trackBar81_Scroll);
        }

        void trackBar81_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown81.ValueChanged -= new System.EventHandler(this.numericUpDown81_ValueChanged);
            numericUpDown81.Value = (decimal)trackBar81.Value / (decimal)100;
            Settings.LastGame.SweetFX.Vignette.Amount = numericUpDown81.Value;
            this.numericUpDown81.ValueChanged += new System.EventHandler(this.numericUpDown81_ValueChanged);
        }

        void numericUpDown83_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar83.Scroll -= new System.EventHandler(this.trackBar83_Scroll);
            trackBar83.Value = Convert.ToInt32(numericUpDown83.Value);
            Settings.LastGame.SweetFX.Vignette.Slope = trackBar83.Value;
            this.trackBar83.Scroll += new System.EventHandler(this.trackBar83_Scroll);
        }

        void trackBar83_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown83.ValueChanged -= new System.EventHandler(this.numericUpDown83_ValueChanged);
            numericUpDown83.Value = (decimal)trackBar83.Value;
            Settings.LastGame.SweetFX.Vignette.Slope = trackBar83.Value;
            this.numericUpDown83.ValueChanged += new System.EventHandler(this.numericUpDown83_ValueChanged);
        }

        void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.Vignette.Type = comboBox2.SelectedIndex + 1;
            if (Settings.LastGame.SweetFX.Vignette.Type == 1)
            {
                numericUpDown86.Enabled = true;
                trackBar86.Enabled = true;
                numericUpDown85.Enabled = true;
                trackBar85.Enabled = true;
            }
            else
            {
                numericUpDown86.Enabled = false;
                trackBar86.Enabled = false;
                numericUpDown85.Enabled = false;
                trackBar85.Enabled = false;
            }
        }

        void numericUpDown86_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar86.Scroll -= new System.EventHandler(this.trackBar86_Scroll);
            trackBar86.Value = Convert.ToInt32(numericUpDown85.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Vignette.Center_X = trackBar86.Value;
            this.trackBar86.Scroll += new System.EventHandler(this.trackBar86_Scroll);
        }

        void trackBar86_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown86.ValueChanged -= new System.EventHandler(this.numericUpDown86_ValueChanged);
            numericUpDown86.Value = (decimal)trackBar86.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Vignette.Center_X = numericUpDown86.Value;
            this.numericUpDown86.ValueChanged += new System.EventHandler(this.numericUpDown86_ValueChanged);
        }

        void numericUpDown85_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar85.Scroll -= new System.EventHandler(this.trackBar85_Scroll);
            trackBar85.Value = Convert.ToInt32(numericUpDown85.Value * (decimal)1000);
            Settings.LastGame.SweetFX.Vignette.Center_Y = trackBar85.Value;
            this.trackBar85.Scroll += new System.EventHandler(this.trackBar85_Scroll);
        }

        void trackBar85_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown85.ValueChanged -= new System.EventHandler(this.numericUpDown85_ValueChanged);
            numericUpDown85.Value = (decimal)trackBar85.Value / (decimal)1000;
            Settings.LastGame.SweetFX.Vignette.Center_Y = numericUpDown85.Value;
            this.numericUpDown85.ValueChanged += new System.EventHandler(this.numericUpDown85_ValueChanged);
        }

        #endregion

        #region Border

        private void borderToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox28.CheckedChanged -= checkBox28_CheckedChanged;
            checkBox28.Checked = borderToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Border.Enabled = checkBox28.Checked;
            checkBox28.CheckedChanged += checkBox28_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Border.Enabled) { tabControl1.TabPages.Remove(tabPage18); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage18; }
        }

        void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            borderToolStripMenuItem.CheckedChanged -= borderToolStripMenuItem_CheckedChanged;
            borderToolStripMenuItem.Checked = checkBox28.Checked;
            Settings.LastGame.SweetFX.Border.Enabled = checkBox28.Checked;
            borderToolStripMenuItem.CheckedChanged += borderToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Border.Enabled) { tabControl1.TabPages.Remove(tabPage18); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage18; }
        }

        void numericUpDown90_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar90.Scroll -= new System.EventHandler(this.trackBar90_Scroll);
            trackBar90.Value = Convert.ToInt32(numericUpDown90.Value);
            Settings.LastGame.SweetFX.Border.Width_X = trackBar90.Value;
            this.trackBar90.Scroll += new System.EventHandler(this.trackBar90_Scroll);
        }

        void trackBar90_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown90.ValueChanged -= new System.EventHandler(this.numericUpDown90_ValueChanged);
            numericUpDown90.Value = (decimal)trackBar90.Value;
            Settings.LastGame.SweetFX.Border.Width_X= trackBar90.Value;
            this.numericUpDown90.ValueChanged += new System.EventHandler(this.numericUpDown90_ValueChanged);
        }

        void numericUpDown88_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar88.Scroll -= new System.EventHandler(this.trackBar88_Scroll);
            trackBar88.Value = Convert.ToInt32(numericUpDown88.Value);
            Settings.LastGame.SweetFX.Border.Width_Y = trackBar88.Value;
            this.trackBar88.Scroll += new System.EventHandler(this.trackBar88_Scroll);
        }

        void trackBar88_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown88.ValueChanged -= new System.EventHandler(this.numericUpDown88_ValueChanged);
            numericUpDown88.Value = (decimal)trackBar88.Value;
            Settings.LastGame.SweetFX.Border.Width_Y = trackBar88.Value;
            this.numericUpDown88.ValueChanged += new System.EventHandler(this.numericUpDown88_ValueChanged);
        }

        void numericUpDown89_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar89.Scroll -= new System.EventHandler(this.trackBar89_Scroll);
            trackBar89.Value = Convert.ToInt32(numericUpDown89.Value);
            Settings.LastGame.SweetFX.Border.Red = trackBar89.Value;
            this.trackBar89.Scroll += new System.EventHandler(this.trackBar89_Scroll);
        }

        void trackBar89_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown89.ValueChanged -= new System.EventHandler(this.numericUpDown89_ValueChanged);
            numericUpDown89.Value = (decimal)trackBar89.Value;
            Settings.LastGame.SweetFX.Border.Red = trackBar89.Value;
            this.numericUpDown89.ValueChanged += new System.EventHandler(this.numericUpDown89_ValueChanged);
        }

        void numericUpDown87_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar87.Scroll -= new System.EventHandler(this.trackBar87_Scroll);
            trackBar87.Value = Convert.ToInt32(numericUpDown87.Value);
            Settings.LastGame.SweetFX.Border.Green = trackBar87.Value;
            this.trackBar87.Scroll += new System.EventHandler(this.trackBar87_Scroll);
        }

        void trackBar87_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown87.ValueChanged -= new System.EventHandler(this.numericUpDown87_ValueChanged);
            numericUpDown87.Value = (decimal)trackBar87.Value;
            Settings.LastGame.SweetFX.Border.Green = trackBar87.Value;
            this.numericUpDown87.ValueChanged += new System.EventHandler(this.numericUpDown87_ValueChanged);
        }

        void numericUpDown84_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar84.Scroll -= new System.EventHandler(this.trackBar84_Scroll);
            trackBar84.Value = Convert.ToInt32(numericUpDown84.Value);
            Settings.LastGame.SweetFX.Border.Blue = trackBar84.Value;
            this.trackBar84.Scroll += new System.EventHandler(this.trackBar84_Scroll);
        }

        void trackBar84_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown84.ValueChanged -= new System.EventHandler(this.numericUpDown84_ValueChanged);
            numericUpDown84.Value = (decimal)trackBar84.Value;
            Settings.LastGame.SweetFX.Border.Blue = trackBar84.Value;
            this.numericUpDown84.ValueChanged += new System.EventHandler(this.numericUpDown84_ValueChanged);
        }

        #endregion

        #region Splitscreen

        private void splitscreenToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            checkBox27.CheckedChanged -= checkBox27_CheckedChanged;
            checkBox27.Checked = splitscreenToolStripMenuItem.Checked;
            Settings.LastGame.SweetFX.Splitscreen.Enabled = checkBox27.Checked;
            checkBox27.CheckedChanged += checkBox27_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Splitscreen.Enabled) { tabControl1.TabPages.Remove(tabPage19); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage19; }
        }

        void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            splitscreenToolStripMenuItem.CheckedChanged -= splitscreenToolStripMenuItem_CheckedChanged;
            splitscreenToolStripMenuItem.Checked = checkBox27.Checked;
            Settings.LastGame.SweetFX.Splitscreen.Enabled = checkBox27.Checked;
            splitscreenToolStripMenuItem.CheckedChanged += splitscreenToolStripMenuItem_CheckedChanged;
            if (!showActiveOnlyToolStripMenuItem.Checked) return;
            if (!Settings.LastGame.SweetFX.Splitscreen.Enabled) { tabControl1.TabPages.Remove(tabPage19); }
            else { HideUnactiveTabs(); tabControl1.SelectedTab = tabPage19; }
        }

        void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LastGame.SweetFX.Splitscreen.Mode = comboBox4.SelectedIndex + 1;
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
