using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace SweetFX_Configurator
{
    public delegate void SaveSettingsFinishedD();
    public delegate void GameLoadedD();

    public static class SweetFX
    {
        #region Hide Me

        private static bool _loading = false;
        private static List<Setting> SaveSettingQueue = new List<Setting>();
        private static Timer _timer = new Timer(5000);
        public static event SaveSettingsFinishedD SaveSettingsFinished;
        public static event GameLoadedD GameLoaded;

        public static _SMAA SMAA;
        public static _FXAA FXAA;
        public static _Explosion Explosion;
        public static _Cartoon Cartoon;
        public static _CRT CRT;
        public static _Bloom Bloom;
        public static _HDR HDR;
        public static _LumaSharpen LumaSharpen;
        public static _Levels Levels;
        public static _Technicolor Technicolor;
        public static _Cineon_DPX Cineon_DPX;
        public static _Monochrome Monochrome;
        public static _Lift_Gamma_Gain Lift_Gamma_Gain;
        public static _Tonemap Tonemap;
        public static _Vibrance Vibrance;
        public static _Curves Curves;
        public static _Sepia Sepia;
        public static _Vignette Vignette;
        public static _Dither Dither;
        public static _Border Border;
        public static _Splitscreen Splitscreen;

        #endregion

        /// <summary>
        /// Parse and load a SweetFX config file
        /// </summary>
        /// <param name="file">Full file path</param>
        public static void Load(Game g)
        {
            if (_loading) { return; }
            if (!File.Exists(g.Directory + @"\SweetFX_settings.txt")) { return; }
            _loading = true;
            Settings.LastGame = g;
            if (SaveSettingQueue.Count > 0) { SaveSettingWorker(); }
            SMAA = new _SMAA();
            FXAA = new _FXAA();
            Explosion = new _Explosion();
            Cartoon = new _Cartoon();
            CRT = new _CRT();
            Bloom = new _Bloom();
            HDR = new _HDR();
            LumaSharpen = new _LumaSharpen();
            Levels = new _Levels();
            Technicolor = new _Technicolor();
            Cineon_DPX = new _Cineon_DPX();
            Monochrome = new _Monochrome();
            Lift_Gamma_Gain = new _Lift_Gamma_Gain();
            Tonemap = new _Tonemap();
            Vibrance = new _Vibrance();
            Curves = new _Curves();
            Sepia = new _Sepia();
            Vignette = new _Vignette();
            Dither = new _Dither();
            Border = new _Border();
            Splitscreen = new _Splitscreen();
            string[] lines = File.ReadAllLines(g.Directory + @"\SweetFX_settings.txt");
            foreach (string line in lines)
            {
                if (line.StartsWith("#define"))
                {
                    string setting = line;
                    int index = setting.IndexOf(" ") + 1;
                    setting = setting.Substring(index, (setting.Length - 1) - index);
                    index = setting.IndexOf(" ") + 1;
                    string key = setting.Substring(0, index - 1);
                    setting = setting.Substring(index, (setting.Length - 1) - index).Trim();
                    string value;
                    string[] _rgb = new string[0];
                    if (setting.StartsWith("float"))
                    {
                        index = setting.IndexOf(")");
                        setting = setting.Substring(0, index).Trim();
                        index = setting.IndexOf("(");
                        value = setting.Substring(index + 1, (setting.Length - 1) - index).Trim();
                        _rgb = value.Split(',');
                    }
                    else
                    {
                        index = setting.IndexOf(" ");
                        value = setting.Substring(0, index);
                    }
                    switch (key.ToLower())
                    {
                        // SMAA
                        case "use_smaa_antialiasing":
                            SMAA.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "smaa_threshold":
                            SMAA.Threshold = Convert.ToDecimal(value);
                            break;
                        case "smaa_max_search_steps":
                            SMAA.Max_Search_Steps = Convert.ToInt32(value);
                            break;
                        case "smaa_max_search_steps_diag":
                            SMAA.Max_Search_Steps_Diag = Convert.ToInt32(value);
                            break;
                        case "smaa_corner_rounding":
                            SMAA.Corner_Rounding = Convert.ToInt32(value);
                            break;
                        case "color_edge_detection":
                            SMAA.Color_Edge_Detection = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "smaa_directx9_linear_blend":
                            SMAA.DirectX9_Linear_Blend = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        // FXAA
                        case "use_fxaa_antialiasing":
                            FXAA.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "fxaa_quality__preset":
                            FXAA.Quality_Preset = Convert.ToInt32(value);
                            break;
                        case "fxaa_subpix":
                            FXAA.Subpix = Convert.ToDecimal(value);
                            break;
                        case "fxaa_edgethreshold":
                            FXAA.Edge_Threshold = Convert.ToDecimal(value);
                            break;
                        case "fxaa_edgethresholdmin":
                            FXAA.Edge_Threshold_Min = Convert.ToDecimal(value);
                            break;
                        // Explosion
                        case "use_explosion":
                            Explosion.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "explosion_radius":
                            Explosion.Radius = Convert.ToDecimal(value);
                            break;
                        // Cartoon
                        case "use_cartoon":
                            Explosion.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "cartoonpower":
                            Cartoon.Power = Convert.ToDecimal(value);
                            break;
                        case "cartoonedgeslope":
                            Cartoon.Edge_Slope = Convert.ToDecimal(value);
                            break;
                        // CRT
                        case "use_advanced_crt":
                            CRT.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "crtamount":
                            CRT.Amount = Convert.ToDecimal(value);
                            break;
                        case "crtresolution":
                            CRT.Resolution = Convert.ToDecimal(value);
                            break;
                        case "crtgamma":
                            CRT.Gamma = Convert.ToDecimal(value);
                            break;
                        case "crtmonitorgamma":
                            CRT.Monitor_Gamma = Convert.ToDecimal(value);
                            break;
                        case "crtbrightness":
                            CRT.Brightness = Convert.ToDecimal(value);
                            break;
                        case "crtscanlineintensity":
                            CRT.Scanline_Intensity = Convert.ToDecimal(value);
                            break;
                        case "crtscanlinegaussian":
                            CRT.Scanline_Gaussian = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "crtcurvature":
                            CRT.Curvature = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "crtcurvatureradius":
                            CRT.Curvature_Radius = Convert.ToDecimal(value);
                            break;
                        case "crtcornersize":
                            CRT.Corner_Size = Convert.ToDecimal(value);
                            break;
                        case "crtdistance":
                            CRT.Distance = Convert.ToDecimal(value);
                            break;
                        case "crtanglex":
                            CRT.AngleX = Convert.ToDecimal(value);
                            break;
                        case "crtangley":
                            CRT.AngleY = Convert.ToDecimal(value);
                            break;
                        case "crtoverscan":
                            CRT.Overscan = Convert.ToDecimal(value);
                            break;
                        case "crtoversample":
                            CRT.Oversample = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        // Bloom
                        case "use_bloom":
                            Bloom.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "bloomthreshold":
                            Bloom.Threshold = Convert.ToDecimal(value);
                            break;
                        case "bloompower":
                            Bloom.Power = Convert.ToDecimal(value);
                            break;
                        case "bloomwidth":
                            Bloom.Width = Convert.ToDecimal(value);
                            break;
                        // HDR
                        case "use_hdr":
                            HDR.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "hdrpower":
                            HDR.Power = Convert.ToDecimal(value);
                            break;
                        case "radius2":
                            HDR.Radius = Convert.ToDecimal(value);
                            break;
                        // LumaSharpen
                        case "use_lumasharpen":
                            LumaSharpen.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "sharp_strength":
                            LumaSharpen.Strength = Convert.ToDecimal(value);
                            break;
                        case "sharp_clamp":
                            LumaSharpen.Clamp = Convert.ToDecimal(value);
                            break;
                        case "pattern":
                            LumaSharpen.Pattern = Convert.ToInt32(value);
                            break;
                        case "offset_bias":
                            LumaSharpen.Offset_Bias = Convert.ToDecimal(value);
                            break;
                        case "show_sharpen":
                            LumaSharpen.Show = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        // Levels
                        case "use_levels":
                            Levels.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "levels_black_point":
                            Levels.Black_Point = Convert.ToInt32(value);
                            break;
                        case "levels_white_point":
                            Levels.White_Point = Convert.ToInt32(value);
                            break;
                        // Technicolor
                        case "use_technicolor":
                            Technicolor.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "techniamount":
                            Technicolor.Amount = Convert.ToDecimal(value);
                            break;
                        case "technipower":
                            Technicolor.Power = Convert.ToDecimal(value);
                            break;
                        case "rednegativeamount":
                            Technicolor.Red_Negative_Amount = Convert.ToDecimal(value);
                            break;
                        case "greennegativeamount":
                            Technicolor.Green_Negative_Amount = Convert.ToDecimal(value);
                            break;
                        case "bluenegativeamount":
                            Technicolor.Blue_Negative_Amount = Convert.ToDecimal(value);
                            break;
                        // Cineon DPX
                        case "use_dpx":
                            Cineon_DPX.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "red":
                            Cineon_DPX.Red = Convert.ToDecimal(value);
                            break;
                        case "green":
                            Cineon_DPX.Green = Convert.ToDecimal(value);
                            break;
                        case "blue":
                            Cineon_DPX.Blue = Convert.ToDecimal(value);
                            break;
                        case "colorgamma":
                            Cineon_DPX.Color_Gamma = Convert.ToDecimal(value);
                            break;
                        case "dpxsaturation":
                            Cineon_DPX.Saturation = Convert.ToDecimal(value);
                            break;
                        case "redc":
                            Cineon_DPX.RedC = Convert.ToDecimal(value);
                            break;
                        case "greenc":
                            Cineon_DPX.GreenC = Convert.ToDecimal(value);
                            break;
                        case "bluec":
                            Cineon_DPX.BlueC = Convert.ToDecimal(value);
                            break;
                        case "blend":
                            Cineon_DPX.Blend = Convert.ToDecimal(value);
                            break;
                        // Monochrome
                        case "use_monochrome":
                            Monochrome.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "monochrome_conversion_values":
                            Monochrome.Conversion_Values = new RGB(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()), Convert.ToDecimal(_rgb[2].Trim()));
                            break;
                        // Lift Gamma Gain
                        case "use_liftgammagain":
                            Lift_Gamma_Gain.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "rgb_lift":
                            Lift_Gamma_Gain.Lift = new RGB(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()), Convert.ToDecimal(_rgb[2].Trim()));
                            break;
                        case "rgb_gamma":
                            Lift_Gamma_Gain.Gamma = new RGB(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()), Convert.ToDecimal(_rgb[2].Trim()));
                            break;
                        case "rgb_gain":
                            Lift_Gamma_Gain.Gain = new RGB(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()), Convert.ToDecimal(_rgb[2].Trim()));
                            break;
                        // Tonemap
                        case "use_tonemap":
                            Tonemap.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "gamma":
                            Tonemap.Gamma = Convert.ToDecimal(value);
                            break;
                        case "saturation":
                            Tonemap.Saturation = Convert.ToDecimal(value);
                            break;
                        case "bleach":
                            Tonemap.Bleach = Convert.ToDecimal(value);
                            break;
                        case "defog":
                            Tonemap.Defog = Convert.ToDecimal(value);
                            break;
                        case "fogcolor":
                            Tonemap.Fog_Color = new RGB(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()), Convert.ToDecimal(_rgb[2].Trim()));
                            break;
                        // Vibrance
                        case "use_vibrance":
                            Vibrance.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "vibrance":
                            Vibrance.Vibrance = Convert.ToDecimal(value);
                            break;
                        case "vibrance_rgb_balance":
                            Vibrance.RGB_Balance = new RGB(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()), Convert.ToDecimal(_rgb[2].Trim()));
                            break;
                        // Curves
                        case "use_curves":
                            Curves.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "curves_mode":
                            Curves.Mode= Convert.ToInt32(value);
                            break;
                        case "curves_contrast":
                            Curves.Contrast = Convert.ToDecimal(value);
                            break;
                        case "curves_formula":
                            Curves.Formula = Convert.ToInt32(value);
                            break;
                        // Sepia
                        case "use_sepia":
                            Sepia.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "colortone":
                            Sepia.Color_Tone = new RGB(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()), Convert.ToDecimal(_rgb[2].Trim()));
                            break;
                        case "greypower":
                            Sepia.Grey_Power = Convert.ToDecimal(value);
                            break;
                        case "sepiapower":
                            Sepia.Power = Convert.ToDecimal(value);
                            break;
                        // Vignette
                        case "use_vignette":
                            Vignette.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "vignettetype":
                            Vignette.Type = Convert.ToInt32(value);
                            break;
                        case "vignetteratio":
                            Vignette.Ratio = Convert.ToDecimal(value);
                            break;
                        case "vignetteradius":
                            Vignette.Radius = Convert.ToDecimal(value);
                            break;
                        case "vignetteamount":
                            Vignette.Amount = Convert.ToDecimal(value);
                            break;
                        case "vignetteslope":
                            Vignette.Slope = Convert.ToInt32(value);
                            break;
                        case "vignettecenter":
                            Vignette.Center = new RG(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()));
                            break;
                        // Vignette
                        case "use_dither":
                            Dither.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "dither_method":
                            Dither.Method = Convert.ToInt32(value);
                            break;
                        // Border
                        case "use_border":
                            Border.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "border_width":
                            Border.Width = new RG(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()));
                            break;
                        case "border_color":
                            Border.Color = new RGB(Convert.ToDecimal(_rgb[0].Trim()), Convert.ToDecimal(_rgb[1].Trim()), Convert.ToDecimal(_rgb[2].Trim()));
                            break;
                        // Splitscreen
                        case "use_splitscreen":
                            Splitscreen.Enabled = Convert.ToBoolean(Convert.ToInt32(value));
                            break;
                        case "splitscreen_mode":
                            Splitscreen.Mode = Convert.ToInt32(value);
                            break;
                    }
                }
            }
            _timer.Elapsed += _timer_Elapsed;
            GameLoaded();
            _loading = false;
        }

        /// <summary>
        /// Unload all SweetFX config memory objects
        /// </summary>
        public static void Unload()
        {
            SMAA = null;
            FXAA = null;
            Explosion = null;
            Cartoon = null;
            CRT = null;
            Bloom = null;
            HDR = null;
            LumaSharpen = null;
            Levels = null;
            Technicolor = null;
            Cineon_DPX = null;
        }

        /// <summary>
        /// Save SweetFX setting to config file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveSetting(Setting set)
        {
            if (_loading) { return; }
            //
            bool existed = false;
            if (SaveSettingQueue.Count > 0)
            {
                foreach (Setting _set in SaveSettingQueue)
                {
                    if (_set.Key == set.Key) { _set.Value = set.Value; existed = true; break; }
                }
            }
            if (!existed) { SaveSettingQueue.Add(set); }
            _timer.Stop();
            _timer.Enabled = true;
            _timer.Start();
        }

        private static void SaveSettingWorker()
        {
            string[] lines = File.ReadAllLines(Settings.LastGame.Directory + @"\SweetFX_settings.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string low_line = lines[i].ToLower();
                foreach (Setting set in SaveSettingQueue)
                {
                    if (low_line.Contains(set.Key))
                    {
                        string setting = lines[i];
                        int index = setting.IndexOf(" ") + 1;
                        setting = setting.Substring(index, (setting.Length - 1) - index);
                        index = setting.IndexOf(" ") + 1;
                        string key = setting.Substring(0, index - 1);
                        setting = setting.Substring(index, (setting.Length - 1) - index).Trim();
                        string value;
                        string[] rgb;
                        if (setting.StartsWith("float"))
                        {
                            index = setting.IndexOf(")");
                            setting = setting.Substring(0, index).Trim();
                            index = setting.IndexOf("(");
                            value = setting.Substring(index + 1, (setting.Length - 1) - index).Trim();
                        }
                        else
                        {
                            index = setting.IndexOf(" ");
                            value = setting.Substring(0, index);
                        }
                        lines[i] = lines[i].Replace(value, set.Value);
                        SaveSettingQueue.Remove(set);
                        break;
                    }
                }
            }
            File.WriteAllLines(Settings.LastGame.Directory + @"\SweetFX_settings.txt", lines);
            _timer.Enabled = false;
            _timer.Stop();
            SaveSettingsFinished();
        }

        public static void Dispose()
        {
            if (SaveSettingQueue.Count > 0) { SaveSettingWorker(); }
            Unload();
            _timer.Dispose();
        }

        private static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SaveSettingWorker();
        }
    }

    public class Setting
    {
        private string _key;
        private string _value;
        private bool _float;

        public Setting(string k, string v, bool f = false)
        {
            _key = k;
            _value = v;
            _float = f;
        }

        public string Key { get { return _key; } }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool Float { get { return _float; } }
    }

    public class _SMAA
    {
        private bool _enabled;
        public decimal _threshold;
        public int _max_search_steps;
        public int _max_search_steps_diag;
        public int _corner_rounding;
        public bool _color_edge_detection;
        public bool _directx9_linear_blend;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_smaa_antialiasing", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                SweetFX.SaveSetting(new Setting("smaa_threshold", value.ToString()));
            }
        }

        public int Max_Search_Steps
        {
            get { return _max_search_steps; }
            set
            {
                _max_search_steps = value;
                SweetFX.SaveSetting(new Setting("smaa_max_search_steps", value.ToString()));
            }
        }

        public int Max_Search_Steps_Diag
        {
            get { return _max_search_steps_diag; }
            set
            {
                _max_search_steps_diag = value;
                SweetFX.SaveSetting(new Setting("smaa_max_search_steps_diag", value.ToString()));
            }
        }

        public int Corner_Rounding
        {
            get { return _corner_rounding; }
            set
            {
                _corner_rounding = value;
                SweetFX.SaveSetting(new Setting("smaa_corner_rounding", value.ToString()));
            }
        }

        public bool Color_Edge_Detection
        {
            get { return _color_edge_detection; }
            set
            {
                _color_edge_detection = value;
                SweetFX.SaveSetting(new Setting("color_edge_detection", (Convert.ToInt32(value)).ToString()));
            }
        }

        public bool DirectX9_Linear_Blend
        {
            get { return _directx9_linear_blend; }
            set
            {
                _directx9_linear_blend = value;
                SweetFX.SaveSetting(new Setting("smaa_directx9_linear_blend", (Convert.ToInt32(value)).ToString()));
            }
        }
    }

    public class _FXAA
    {
        private bool _enabled;
        private int _quality_preset;
        private decimal _subpix;
        private decimal _edge_threshold;
        private decimal _edge_threshold_min;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_fxaa_antialiasing", (Convert.ToInt32(value)).ToString()));
            }
        }

        public int Quality_Preset
        {
            get { return _quality_preset; }
            set
            {
                _quality_preset = value;
                SweetFX.SaveSetting(new Setting("fxaa_quality__preset", value.ToString()));
            }
        }

        public decimal Subpix
        {
            get { return _subpix; }
            set
            {
                _subpix = value;
                SweetFX.SaveSetting(new Setting("fxaa_subpix", value.ToString()));
            }
        }

        public decimal Edge_Threshold
        {
            get { return _edge_threshold; }
            set
            {
                _edge_threshold = value;
                SweetFX.SaveSetting(new Setting("fxaa_edgethreshold", value.ToString()));
            }
        }

        public decimal Edge_Threshold_Min
        {
            get { return _edge_threshold_min; }
            set
            {
                _edge_threshold_min = value;
                SweetFX.SaveSetting(new Setting("fxaa_edgethresholdmin", value.ToString()));
            }
        }
    }

    public class _Explosion
    {
        private bool _enabled;
        private decimal _radius;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_explosion", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                SweetFX.SaveSetting(new Setting("explosion_radius", value.ToString()));
            }
        }
    }

    public class _Cartoon
    {
        private bool _enabled;
        private decimal _power;
        private decimal _edge_slope;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_cartoon", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX.SaveSetting(new Setting("CartoonPower", value.ToString()));
            }
        }

        public decimal Edge_Slope
        {
            get { return _edge_slope; }
            set
            {
                _edge_slope = value;
                SweetFX.SaveSetting(new Setting("CartoonEdgeSlope", value.ToString()));
            }
        }
    }

    public class _CRT
    {
        private bool _enabled;
        private decimal _amount;
        private decimal _resolution;
        private decimal _gamma;
        private decimal _monitor_gamma;
        private decimal _brightness;
        private decimal _scanline_intensity;
        private bool _scanline_gaussian;
        private bool _curvature;
        private decimal _curvature_radius;
        private decimal _corner_size;
        private decimal _distance;
        private decimal _angle_x;
        private decimal _angle_y;
        private decimal _overscan;
        private bool _oversample;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_advanced_crt", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                SweetFX.SaveSetting(new Setting("crtamount", value.ToString()));
            }
        }

        public decimal Resolution
        {
            get { return _resolution; }
            set
            {
                _resolution = value;
                SweetFX.SaveSetting(new Setting("crtresolution", value.ToString()));
            }
        }

        public decimal Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                SweetFX.SaveSetting(new Setting("crtgamma", value.ToString()));
            }
        }

        public decimal Monitor_Gamma
        {
            get { return _monitor_gamma; }
            set
            {
                _monitor_gamma = value;
                SweetFX.SaveSetting(new Setting("crtmonitorgamma", value.ToString()));
            }
        }

        public decimal Brightness
        {
            get { return _brightness; }
            set
            {
                _brightness = value;
                SweetFX.SaveSetting(new Setting("crtbrightness", value.ToString()));
            }
        }

        public decimal Scanline_Intensity
        {
            get { return _scanline_intensity; }
            set
            {
                _scanline_intensity = value;
                SweetFX.SaveSetting(new Setting("crtscanlineintensity", value.ToString()));
            }
        }

        public bool Scanline_Gaussian
        {
            get { return _scanline_gaussian; }
            set
            {
                _scanline_gaussian = value;
                SweetFX.SaveSetting(new Setting("crtscanlinegaussian", (Convert.ToInt32(value)).ToString()));
            }
        }

        public bool Curvature
        {
            get { return _curvature; }
            set
            {
                _curvature = value;
                SweetFX.SaveSetting(new Setting("crtcurvature", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Curvature_Radius
        {
            get { return _curvature_radius; }
            set
            {
                _curvature_radius = value;
                SweetFX.SaveSetting(new Setting("crtcurvatureradius", value.ToString()));
            }
        }

        public decimal Corner_Size
        {
            get { return _corner_size; }
            set
            {
                _corner_size = value;
                SweetFX.SaveSetting(new Setting("crtcornersize", value.ToString()));
            }
        }

        public decimal Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                SweetFX.SaveSetting(new Setting("crtdistance", value.ToString()));
            }
        }

        public decimal AngleX
        {
            get { return _angle_x; }
            set
            {
                _angle_x = value;
                SweetFX.SaveSetting(new Setting("crtanglex", value.ToString()));
            }
        }

        public decimal AngleY
        {
            get { return _angle_y; }
            set
            {
                _angle_y = value;
                SweetFX.SaveSetting(new Setting("crtangley", value.ToString()));
            }
        }

        public decimal Overscan
        {
            get { return _overscan; }
            set
            {
                _overscan = value;
                SweetFX.SaveSetting(new Setting("crtoverscan", value.ToString()));
            }
        }

        public bool Oversample
        {
            get { return _oversample; }
            set
            {
                _oversample = value;
                SweetFX.SaveSetting(new Setting("crtoversample", (Convert.ToInt32(value)).ToString()));
            }
        }
    }

    public class _Bloom
    {
        private bool _enabled;
        private decimal _threshold;
        private decimal _power;
        private decimal _width;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_bloom", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                SweetFX.SaveSetting(new Setting("bloomthreshold", value.ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX.SaveSetting(new Setting("bloompower", value.ToString()));
            }
        }

        public decimal Width
        {
            get { return _width; }
            set
            {
                _width = value;
                SweetFX.SaveSetting(new Setting("bloomwidth", value.ToString()));
            }
        }
    }

    public class _HDR
    {
        private bool _enabled;
        private decimal _power;
        private decimal _radius;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_hdr", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX.SaveSetting(new Setting("hdrpower", value.ToString()));
            }
        }

        public decimal Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                SweetFX.SaveSetting(new Setting("radius2", value.ToString()));
            }
        }
    }

    public class _LumaSharpen
    {
        private bool _enabled;
        private decimal _strength;
        private decimal _clamp;
        private int _pattern;
        private decimal _offset_bias;
        private bool _show;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_lumasharpen", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Strength
        {
            get { return _strength; }
            set
            {
                _strength = value;
                SweetFX.SaveSetting(new Setting("sharp_strength", value.ToString()));
            }
        }

        public decimal Clamp
        {
            get { return _clamp; }
            set
            {
                _clamp = value;
                SweetFX.SaveSetting(new Setting("sharp_clamp", value.ToString()));
            }
        }

        public int Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
                SweetFX.SaveSetting(new Setting("pattern", value.ToString()));
            }
        }

        public decimal Offset_Bias
        {
            get { return _offset_bias; }
            set
            {
                _offset_bias = value;
                SweetFX.SaveSetting(new Setting("offset_bias", value.ToString()));
            }
        }

        public bool Show
        {
            get { return _show; }
            set
            {
                _show = value;
                SweetFX.SaveSetting(new Setting("show_sharpen", Convert.ToInt32(value).ToString()));
            }
        }
    }

    public class _Levels
    {
        private bool _enabled;
        private int _black_point;
        private int _white_point;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_levels", Convert.ToInt32(value).ToString()));
            }
        }

        public int Black_Point
        {
            get { return _black_point; }
            set
            {
                _black_point = value;
                SweetFX.SaveSetting(new Setting("levels_black_point", value.ToString()));
            }
        }

        public int White_Point
        {
            get { return _white_point; }
            set
            {
                _white_point = value;
                SweetFX.SaveSetting(new Setting("levels_white_point", value.ToString()));
            }
        }
    }

    public class _Technicolor
    {
        private bool _enabled;
        private decimal _amount;
        private decimal _power;
        private decimal _red_negative_amount;
        private decimal _green_negative_amount;
        private decimal _blue_negative_amount;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_technicolor", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                SweetFX.SaveSetting(new Setting("techniamount", value.ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX.SaveSetting(new Setting("techniamount", value.ToString()));
            }
        }

        public decimal Red_Negative_Amount
        {
            get { return _red_negative_amount; }
            set
            {
                _red_negative_amount = value;
                SweetFX.SaveSetting(new Setting("rednegativeamount", value.ToString()));
            }
        }

        public decimal Green_Negative_Amount
        {
            get { return _green_negative_amount; }
            set
            {
                _green_negative_amount = value;
                SweetFX.SaveSetting(new Setting("greennegativeamount", value.ToString()));
            }
        }

        public decimal Blue_Negative_Amount
        {
            get { return _blue_negative_amount; }
            set
            {
                _blue_negative_amount = value;
                SweetFX.SaveSetting(new Setting("bluenegativeamount", value.ToString()));
            }
        }
    }

    public class _Cineon_DPX
    {
        private bool _enabled;
        private decimal _red;
        private decimal _green;
        private decimal _blue;
        private decimal _color_gamma;
        private decimal _saturation;
        private decimal _redc;
        private decimal _greenc;
        private decimal _bluec;
        private decimal _blend;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_dpx", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Red
        {
            get { return _red; }
            set
            {
                _red = value;
                SweetFX.SaveSetting(new Setting("red", value.ToString()));
            }
        }

        public decimal Green
        {
            get { return _green; }
            set
            {
                _green = value;
                SweetFX.SaveSetting(new Setting("green", value.ToString()));
            }
        }

        public decimal Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                SweetFX.SaveSetting(new Setting("blue", value.ToString()));
            }
        }

        public decimal Color_Gamma
        {
            get { return _color_gamma; }
            set
            {
                _color_gamma = value;
                SweetFX.SaveSetting(new Setting("colorgamma", value.ToString()));
            }
        }

        public decimal Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = value;
                SweetFX.SaveSetting(new Setting("dpxsaturation", value.ToString()));
            }
        }

        public decimal RedC
        {
            get { return _redc; }
            set
            {
                _redc = value;
                SweetFX.SaveSetting(new Setting("redc", value.ToString()));
            }
        }

        public decimal GreenC
        {
            get { return _greenc; }
            set
            {
                _greenc = value;
                SweetFX.SaveSetting(new Setting("greenc", value.ToString()));
            }
        }

        public decimal BlueC
        {
            get { return _bluec; }
            set
            {
                _bluec = value;
                SweetFX.SaveSetting(new Setting("bluec", value.ToString()));
            }
        }

        public decimal Blend
        {
            get { return _blend; }
            set
            {
                _blend = value;
                SweetFX.SaveSetting(new Setting("blend", value.ToString()));
            }
        }
    }

    public class _Monochrome
    {
        private bool _enabled;
        private RGB _conversion_values;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_monochrome", Convert.ToInt32(value).ToString()));
            }
        }

        public RGB Conversion_Values
        {
            get { return _conversion_values; }
            set
            {
                _conversion_values = value;
                SweetFX.SaveSetting(new Setting("monochrome_conversion_values", value.ToString(), true));
            }
        }
    }

    public class _Lift_Gamma_Gain
    {
        private bool _enabled;
        private RGB _lift;
        private RGB _gamma;
        private RGB _gain;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_liftgammagain", Convert.ToInt32(value).ToString()));
            }
        }

        public RGB Lift
        {
            get { return _lift; }
            set
            {
                _lift = value;
                SweetFX.SaveSetting(new Setting("rgb_lift", value.ToString(), true));
            }
        }

        public RGB Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                SweetFX.SaveSetting(new Setting("rgb_gamma", value.ToString(), true));
            }
        }

        public RGB Gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
                SweetFX.SaveSetting(new Setting("rgb_gain", value.ToString(), true));
            }
        }
    }

    public class _Tonemap
    {
        private bool _enabled;
        private decimal _gamma;
        private decimal _exposure;
        private decimal _saturation;
        private decimal _bleach;
        private decimal _defog;
        private RGB _fog_color;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_tonemap", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                SweetFX.SaveSetting(new Setting("gamma", value.ToString()));
            }
        }

        public decimal Exposure
        {
            get { return _exposure; }
            set
            {
                _exposure = value;
                SweetFX.SaveSetting(new Setting("exposure", value.ToString()));
            }
        }

        public decimal Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = value;
                SweetFX.SaveSetting(new Setting("saturation", value.ToString()));
            }
        }

        public decimal Bleach
        {
            get { return _bleach; }
            set
            {
                _bleach = value;
                SweetFX.SaveSetting(new Setting("bleach", value.ToString()));
            }
        }

        public decimal Defog
        {
            get { return _defog; }
            set
            {
                _defog = value;
                SweetFX.SaveSetting(new Setting("defog", value.ToString()));
            }
        }

        public RGB Fog_Color
        {
            get { return _fog_color; }
            set
            {
                _fog_color = value;
                SweetFX.SaveSetting(new Setting("fogcolor", value.ToString(), true));
            }
        }
    }

    public class _Vibrance
    {
        private bool _enabled;
        private decimal vibrance_;
        private RGB _rgb_balance;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_vibrance", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Vibrance
        {
            get { return vibrance_; }
            set
            {
                vibrance_ = value;
                SweetFX.SaveSetting(new Setting("vibrance", value.ToString()));
            }
        }

        public RGB RGB_Balance
        {
            get { return _rgb_balance; }
            set
            {
                _rgb_balance = value;
                SweetFX.SaveSetting(new Setting("vibrance_rgb_balance", value.ToString(), true));
            }
        }
    }

    public class _Curves
    {
        private bool _enabled;
        private int _mode;
        private decimal _contrast;
        private int _formula;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_curves", Convert.ToInt32(value).ToString()));
            }
        }

        public int Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                SweetFX.SaveSetting(new Setting("curves_mode", value.ToString()));
            }
        }

        public decimal Contrast
        {
            get { return _contrast; }
            set
            {
                _contrast = value;
                SweetFX.SaveSetting(new Setting("curves_contrast", value.ToString()));
            }
        }

        public int Formula
        {
            get { return _formula; }
            set
            {
                _formula = value;
                SweetFX.SaveSetting(new Setting("curves_formula", value.ToString()));
            }
        }
    }

    public class _Sepia
    {
        private bool _enabled;
        private RGB _color_tone;
        private decimal _grey_power;
        private decimal _power;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_sepia", Convert.ToInt32(value).ToString()));
            }
        }

        public RGB Color_Tone
        {
            get { return _color_tone; }
            set
            {
                _color_tone = value;
                SweetFX.SaveSetting(new Setting("colortone", value.ToString(), true));
            }
        }

        public decimal Grey_Power
        {
            get { return _grey_power; }
            set
            {
                _grey_power = value;
                SweetFX.SaveSetting(new Setting("greypower", value.ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX.SaveSetting(new Setting("sepiapower", value.ToString()));
            }
        }
    }

    public class _Vignette
    {
        private bool _enabled;
        private int _type;
        private decimal _ratio;
        private decimal _radius;
        private decimal _amount;
        private int _slope;
        private RG _center;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_vignette", Convert.ToInt32(value).ToString()));
            }
        }

        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                SweetFX.SaveSetting(new Setting("vignettetype", value.ToString()));
            }
        }

        public decimal Ratio
        {
            get { return _ratio; }
            set
            {
                _ratio = value;
                SweetFX.SaveSetting(new Setting("vignetteratio", value.ToString()));
            }
        }

        public decimal Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                SweetFX.SaveSetting(new Setting("vignetteradius", value.ToString()));
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                SweetFX.SaveSetting(new Setting("vignetteamount", value.ToString()));
            }
        }

        public int Slope
        {
            get { return _slope; }
            set
            {
                _slope = value;
                SweetFX.SaveSetting(new Setting("vignetteslope", value.ToString()));
            }
        }

        public RG Center
        {
            get { return _center; }
            set
            {
                _center = value;
                SweetFX.SaveSetting(new Setting("vignettecenter", value.ToString()));
            }
        }
    }

    public class _Dither
    {
        private bool _enabled;
        private int _method;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_dither", Convert.ToInt32(value).ToString()));
            }
        }

        public int Method
        {
            get { return _method; }
            set
            {
                _method = value;
                SweetFX.SaveSetting(new Setting("dithermethod", value.ToString()));
            }
        }
    }

    public class _Border
    {
        private bool _enabled;
        private RG _width;
        private RGB _color;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_border", Convert.ToInt32(value).ToString()));
            }
        }

        public RG Width
        {
            get { return _width; }
            set
            {
                _width = value;
                SweetFX.SaveSetting(new Setting("border_width", value.ToString()));
            }
        }

        public RGB Color
        {
            get { return _color; }
            set
            {
                _color = value;
                SweetFX.SaveSetting(new Setting("border_color", value.ToString()));
            }
        }
    }

    public class _Splitscreen
    {
        private bool _enabled;
        private int _mode;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX.SaveSetting(new Setting("use_splitscreen", Convert.ToInt32(value).ToString()));
            }
        }

        public int Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                SweetFX.SaveSetting(new Setting("splitscreen_mode", value.ToString()));
            }
        }
    }

    public class RG
    {
        public decimal Red;
        public decimal Green;

        public RG(decimal r, decimal g)
        {
            Red = r;
            Green = g;
        }

        public override string ToString()
        {
            return "float2(" + Red.ToString() + ", " + Green.ToString() + ")";
        }
    }

    public class RGB
    {
        public decimal Red;
        public decimal Green;
        public decimal Blue;

        public RGB(decimal r, decimal g, decimal b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        public override string ToString()
        {
            return "float3(" + Red.ToString() + ", " + Green.ToString() + ", " + Blue.ToString() + ")";
        }
    }
}
