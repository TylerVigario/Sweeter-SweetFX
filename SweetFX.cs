using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public delegate void SaveSettingsFinishedD();

    public static class SweetFX
    {
        private static bool _loading = false;
        private static string _file;
        private static List<Setting> SaveSettingQueue = new List<Setting>();
        private static System.Timers.Timer _timer = new System.Timers.Timer(5000);
        public static event SaveSettingsFinishedD SaveSettingsFinished;

        public static _SMAA SMAA;
        public static _LumaSharpen LumaSharpen;

        /// <summary>
        /// Load last read SweetFX config file
        /// </summary>
        /// <returns>False = No previous file loaded</returns>
        public static bool Load()
        {
            if (!String.IsNullOrEmpty(_file)) { Load(_file); return true; }
            return false;
        }

        /// <summary>
        /// Parse and load a SweetFX config file
        /// </summary>
        /// <param name="file">Full file path</param>
        public static void Load(string file)
        {
            if (_loading) { return; }
            _loading = true;
            SMAA = new _SMAA();
            LumaSharpen = new _LumaSharpen();
            string[] lines = File.ReadAllLines(file);
            _file = file;
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
                    }

                }
            }
            _timer.Elapsed += _timer_Elapsed;
            _loading = false;
        }

        /// <summary>
        /// Unload all SweetFX config memory objects
        /// </summary>
        public static void Unload()
        {
            SMAA = null;
            LumaSharpen = null;
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
            string[] lines = File.ReadAllLines(_file);
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
            File.WriteAllLines(_file, lines);
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
}
