using System;
using System.IO;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public static class SweetFX
    {
        public static string[] lines;
        private static bool _loading = false;

        public static _SMAA SMAA = new _SMAA();

        public static void Load(string file)
        {
            _loading = true;
            lines = File.ReadAllLines(file);
            foreach (string line in lines)
            {
                if (line.StartsWith("#") && line.Contains("#define"))
                {
                    string setting = line;
                    int index = setting.IndexOf("//");
                    if (index >= 0) { setting = setting.Substring(0, --index).Trim(); }
                    while (setting.Contains("  ")) { setting = setting.Replace("  ", " "); }
                    string[] settings = setting.Split(' ');
                    //
                    try
                    {
                        switch (settings[1])
                        {
                            case "USE_SMAA_ANTIALIASING":
                                SMAA.Enabled = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                                break;
                            case "SMAA_THRESHOLD":
                                SMAA.Threshold = Convert.ToDecimal(settings[2]);
                                break;
                            case "SMAA_MAX_SEARCH_STEPS":
                                SMAA.Max_Search_Steps = Convert.ToInt32(settings[2]);
                                break;
                            case "SMAA_MAX_SEARCH_STEPS_DIAG":
                                SMAA.Max_Search_Steps_Diag = Convert.ToInt32(settings[2]);
                                break;
                            case "SMAA_CORNER_ROUNDING":
                                SMAA.Corner_Rounding = Convert.ToInt32(settings[2]);
                                break;
                            case "COLOR_EDGE_DETECTION":
                                SMAA.Color_Edge_Detection = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                                break;
                            case "SMAA_DIRECTX9_LINEAR_BLEND":
                                SMAA.DirectX9_Linear_Blend = Convert.ToBoolean(Convert.ToInt32(settings[2]));
                                break;
                        }
                    }
                    catch (Exception e) { MessageBox.Show(e.ToString()); }
                }
            }
            _loading = false;
        }

        public static void SaveSetting(string key, string value)
        {
            if (_loading) { return; }
            foreach (string line in lines)
            {
                if (line.Contains(key))
                {

                }
            }
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
                SweetFX.SaveSetting("USE_SMAA_ANTIALIASING", (Convert.ToInt32(value)).ToString());
            }
        }

        public decimal Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                SweetFX.SaveSetting("SMAA_THRESHOLD", value.ToString());
            }
        }

        public int Max_Search_Steps
        {
            get { return _max_search_steps; }
            set
            {
                _max_search_steps = value;
                SweetFX.SaveSetting("SMAA_MAX_SEARCH_STEPS", value.ToString());
            }
        }

        public int Max_Search_Steps_Diag
        {
            get { return _max_search_steps_diag; }
            set
            {
                _max_search_steps_diag = value;
                SweetFX.SaveSetting("SMAA_MAX_SEARCH_STEPS_DIAG", value.ToString());
            }
        }

        public int Corner_Rounding
        {
            get { return _corner_rounding; }
            set
            {
                _corner_rounding = value;
                SweetFX.SaveSetting("SMAA_CORNER_ROUNDING", value.ToString());
            }
        }

        public bool Color_Edge_Detection
        {
            get { return _color_edge_detection; }
            set
            {
                _color_edge_detection = value;
                SweetFX.SaveSetting("COLOR_EDGE_DETECTION", (Convert.ToInt32(value)).ToString());
            }
        }

        public bool DirectX9_Linear_Blend
        {
            get { return _directx9_linear_blend; }
            set
            {
                _directx9_linear_blend = value;
                SweetFX.SaveSetting("SMAA_DIRECTX9_LINEAR_BLEND", (Convert.ToInt32(value)).ToString());
            }
        }
    }

    public class _LumaSharpen
    {
        private bool _enabled;
    }
}
