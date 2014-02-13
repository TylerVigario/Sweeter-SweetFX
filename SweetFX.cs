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
        private static bool _loading = false;
        private static Game _game = null;
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
            string[] lines = File.ReadAllLines(g.Directory + @"\SweetFX_settings.txt");
            _game = g;
            foreach (string line in lines)
            {
                if (line.StartsWith("#define"))
                {
                    string setting = line;
                    int index = setting.IndexOf("//");
                    if (index >= 0) { setting = setting.Substring(0, --index).Trim(); }
                    while (setting.Contains("  ")) { setting = setting.Replace("  ", " "); }
                    string[] settings = setting.Split(' ');
                    //
                    switch (settings[1].ToLower())
                    {
                        // SMAA
                        case "use_smaa_antialiasing":
                            SMAA.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "smaa_threshold":
                            SMAA.Threshold = Convert.ToDecimal(settings[2]);
                            break;
                        case "smaa_max_search_steps":
                            SMAA.Max_Search_Steps = Convert.ToInt32(settings[2]);
                            break;
                        case "smaa_max_search_steps_diag":
                            SMAA.Max_Search_Steps_Diag = Convert.ToInt32(settings[2]);
                            break;
                        case "smaa_corner_rounding":
                            SMAA.Corner_Rounding = Convert.ToInt32(settings[2]);
                            break;
                        case "color_edge_detection":
                            SMAA.Color_Edge_Detection = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "smaa_directx9_linear_blend":
                            SMAA.DirectX9_Linear_Blend = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        // FXAA
                        case "use_fxaa_antialiasing":
                            FXAA.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "fxaa_quality__preset":
                            FXAA.Quality_Preset = Convert.ToInt32(settings[2]);
                            break;
                        case "fxaa_subpix":
                            FXAA.Subpix = Convert.ToDecimal(settings[2]);
                            break;
                        case "fxaa_edgethreshold":
                            FXAA.Edge_Threshold = Convert.ToDecimal(settings[2]);
                            break;
                        case "fxaa_edgethresholdmin":
                            FXAA.Edge_Threshold_Min = Convert.ToDecimal(settings[2]);
                            break;
                        // Explosion
                        case "use_explosion":
                            Explosion.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "explosion_radius":
                            Explosion.Radius = Convert.ToDecimal(settings[2]);
                            break;
                        // Cartoon
                        case "use_cartoon":
                            Explosion.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "cartoonpower":
                            Cartoon.Power = Convert.ToDecimal(settings[2]);
                            break;
                        case "cartoonedgeslope":
                            Cartoon.Edge_Slope = Convert.ToDecimal(settings[2]);
                            break;
                        // CRT
                        case "use_advanced_crt":
                            CRT.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "crtamount":
                            CRT.Amount = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtresolution":
                            CRT.Resolution = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtgamma":
                            CRT.Gamma = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtmonitorgamma":
                            CRT.Monitor_Gamma = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtbrightness":
                            CRT.Brightness = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtscanlineintensity":
                            CRT.Scanline_Intensity = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtscanlinegaussian":
                            CRT.Scanline_Gaussian = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "crtcurvature":
                            CRT.Curvature = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "crtcurvatureradius":
                            CRT.Curvature_Radius = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtcornersize":
                            CRT.Corner_Size = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtdistance":
                            CRT.Distance = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtanglex":
                            CRT.AngleX = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtangley":
                            CRT.AngleY = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtoverscan":
                            CRT.Overscan = Convert.ToDecimal(settings[2]);
                            break;
                        case "crtoversample":
                            CRT.Oversample = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        // Bloom
                        case "use_bloom":
                            Bloom.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "bloomthreshold":
                            Bloom.Threshold = Convert.ToDecimal(settings[2]);
                            break;
                        case "bloompower":
                            Bloom.Power = Convert.ToDecimal(settings[2]);
                            break;
                        case "bloomwidth":
                            Bloom.Width = Convert.ToDecimal(settings[2]);
                            break;
                        // HDR
                        case "use_hdr":
                            HDR.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "hdrpower":
                            HDR.Power = Convert.ToDecimal(settings[2]);
                            break;
                        case "radius2":
                            HDR.Radius = Convert.ToDecimal(settings[2]);
                            break;
                        // LumaSharpen
                        case "use_lumasharpen":
                            LumaSharpen.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "sharp_strength":
                            LumaSharpen.Strength = Convert.ToDecimal(settings[2]);
                            break;
                        case "sharp_clamp":
                            LumaSharpen.Clamp = Convert.ToDecimal(settings[2]);
                            break;
                        case "pattern":
                            LumaSharpen.Pattern = Convert.ToInt32(settings[2]);
                            break;
                        case "offset_bias":
                            LumaSharpen.Offset_Bias = Convert.ToDecimal(settings[2]);
                            break;
                        case "show_sharpen":
                            LumaSharpen.Show = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        // Levels
                        case "use_levels":
                            Levels.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "levels_black_point":
                            Levels.Black_Point = Convert.ToInt32(settings[2]);
                            break;
                        case "levels_white_point":
                            Levels.White_Point = Convert.ToInt32(settings[2]);
                            break;
                        // Technicolor
                        case "use_technicolor":
                            Technicolor.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                            break;
                        case "techniamount":
                            Technicolor.Amount = Convert.ToDecimal(settings[2]);
                            break;
                        case "technipower":
                            Technicolor.Power = Convert.ToDecimal(settings[2]);
                            break;
                        case "rednegativeamount":
                            Technicolor.Red_Negative_Amount = Convert.ToDecimal(settings[2]);
                            break;
                        case "greennegativeamount":
                            Technicolor.Green_Negative_Amount = Convert.ToDecimal(settings[2]);
                            break;
                        case "bluenegativeamount":
                            Technicolor.Blue_Negative_Amount = Convert.ToDecimal(settings[2]);
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
            string[] lines = File.ReadAllLines(_game.Directory + @"\SweetFX_settings.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string low_line = lines[i].ToLower();
                foreach (Setting set in SaveSettingQueue)
                {
                    if (low_line.Contains(set.Key))
                    {
                        string setting = lines[i];
                        int index = setting.IndexOf("//");
                        if (index >= 0) { setting = setting.Substring(0, --index).Trim(); }
                        while (setting.Contains("  ")) { setting = setting.Replace("  ", " "); }
                        string[] settings = setting.Split(' ');
                        lines[i] = lines[i].Replace(settings[2], set.Value);
                        SaveSettingQueue.Remove(set);
                        break;
                    }
                }
            }
            File.WriteAllLines(_game.Directory + @"\SweetFX_settings.txt", lines);
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

        public Setting(string key, string value)
        {
            _key = key;
            _value = value;
        }

        public string Key { get { return _key; } }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
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

    }
}
