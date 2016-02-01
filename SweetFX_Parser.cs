using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Ookii.Dialogs;

namespace SweetFX_Configurator
{
    public delegate void SettingsSavedD();
    public delegate void GameLoadedD();

    public static class SweetFX_Parser
    {
        private static bool _loading = false;
        private static List<Setting> SaveSettingQueue = new List<Setting>();
        private static Timer _timer = new Timer(5000);
        public static event SettingsSavedD SettingsSaved;
        public static event GameLoadedD GameLoaded;

        public static void Load(Game g)
        {
            if (_loading) { return; }
            if (!File.Exists(g.Directory.FullName + @"\SweetFX_settings.txt")) { return; }
            _loading = true;
            Settings.LastGame = g;
            if (SaveSettingQueue.Count > 0) { _timer_Elapsed(null, null); }
            g.SweetFX = new _SweetFX();
            string[] lines = File.ReadAllLines(g.Directory.FullName + @"\SweetFX_settings.txt");
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
                            Settings.LastGame.SweetFX.SMAA.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_FXAA_ANTIALIASING", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "smaa_threshold":
                            Settings.LastGame.SweetFX.SMAA.Threshold = OutOfRangeCheck("SMAA_THRESHOLD", Convert.ToDecimal(value), new decimal[] { (decimal)0.05, (decimal)0.20 });
                            break;
                        case "smaa_max_search_steps":
                            Settings.LastGame.SweetFX.SMAA.Max_Search_Steps = OutOfRangeCheck("SMAA_MAX_SEARCH_STEPS", Convert.ToInt32(value), new int[] { 0, 98 });
                            break;
                        case "smaa_max_search_steps_diag":
                            Settings.LastGame.SweetFX.SMAA.Max_Search_Steps_Diag = OutOfRangeCheck("SMAA_MAX_SEARCH_STEPS_DIAG", Convert.ToInt32(value), new int[] { 0, 16 });
                            break;
                        case "smaa_corner_rounding":
                            Settings.LastGame.SweetFX.SMAA.Corner_Rounding = OutOfRangeCheck("SMAA_CORNER_ROUNDING", Convert.ToInt32(value), new int[] { 0, 100 });
                            break;
                        case "color_edge_detection":
                            Settings.LastGame.SweetFX.SMAA.Color_Edge_Detection = OutOfRangeCheck("COLOR_EDGE_DETECTION", Convert.ToInt32(value), new int[] { 0, 1 });
                            break;
                        case "smaa_directx9_linear_blend":
                            Settings.LastGame.SweetFX.SMAA.DirectX9_Linear_Blend = Convert.ToBoolean(OutOfRangeCheck("SMAA_DIRECTX9_LINEAR_BLEND", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        // FXAA
                        case "use_fxaa_antialiasing":
                            Settings.LastGame.SweetFX.FXAA.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_FXAA_ANTIALIASING", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "fxaa_quality__preset":
                            Settings.LastGame.SweetFX.FXAA.Quality_Preset = OutOfRangeCheck("FXAA_QUALITY__PRESET", Convert.ToInt32(value), new int[] { 1, 9 });
                            break;
                        case "fxaa_subpix":
                            Settings.LastGame.SweetFX.FXAA.Subpix = OutOfRangeCheck("fxaa_Subpix", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "fxaa_edgethreshold":
                            Settings.LastGame.SweetFX.FXAA.Edge_Threshold = OutOfRangeCheck("fxaa_EdgeThreshold", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "fxaa_edgethresholdmin":
                            Settings.LastGame.SweetFX.FXAA.Edge_Threshold_Min = OutOfRangeCheck("fxaa_EdgeThresholdMin", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        // Explosion
                        case "use_explosion":
                            Settings.LastGame.SweetFX.Explosion.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_EXPLOSION", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "explosion_radius":
                            Settings.LastGame.SweetFX.Explosion.Radius = OutOfRangeCheck("Explosion_Radius", Convert.ToDecimal(value), new decimal[] { (decimal)0.2, (decimal)100 });
                            break;
                        // Cartoon
                        case "use_cartoon":
                            Settings.LastGame.SweetFX.Cartoon.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_CARTOON", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "cartoonpower":
                            Settings.LastGame.SweetFX.Cartoon.Power = OutOfRangeCheck("CartoonPower", Convert.ToDecimal(value), new decimal[] { (decimal)0.1, (decimal)10 });
                            break;
                        case "cartoonedgeslope":
                            Settings.LastGame.SweetFX.Cartoon.Edge_Slope = OutOfRangeCheck("CartoonEdgeSlope", Convert.ToDecimal(value), new decimal[] { (decimal)0.1, (decimal)8 });
                            break;
                        // CRT
                        case "use_advanced_crt":
                            Settings.LastGame.SweetFX.CRT.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_ADVANCED_CRT", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "crtamount":
                            Settings.LastGame.SweetFX.CRT.Amount = OutOfRangeCheck("CRTAmount", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "crtresolution":
                            Settings.LastGame.SweetFX.CRT.Resolution = OutOfRangeCheck("CRTResolution", Convert.ToDecimal(value), new decimal[] { (decimal)1, (decimal)8 });
                            break;
                        case "crtgamma":
                            Settings.LastGame.SweetFX.CRT.Gamma = OutOfRangeCheck("CRTgamma", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)4 });
                            break;
                        case "crtmonitorgamma":
                            Settings.LastGame.SweetFX.CRT.Monitor_Gamma = OutOfRangeCheck("CRTmonitorgamma", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)4 });
                            break;
                        case "crtbrightness":
                            Settings.LastGame.SweetFX.CRT.Brightness = OutOfRangeCheck("CRTBrightness", Convert.ToDecimal(value), new decimal[] { (decimal)1, (decimal)3 });
                            break;
                        case "crtscanlineintensity":
                            Settings.LastGame.SweetFX.CRT.Scanline_Intensity = OutOfRangeCheck("CRTScanlineIntensity", Convert.ToDecimal(value), new decimal[] { (decimal)2, (decimal)4 });
                            break;
                        case "crtscanlinegaussian":
                            Settings.LastGame.SweetFX.CRT.Scanline_Gaussian = Convert.ToBoolean(OutOfRangeCheck("CRTScanlineGaussian", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "crtcurvature":
                            Settings.LastGame.SweetFX.CRT.Curvature = Convert.ToBoolean(OutOfRangeCheck("CRTCurvature", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "crtcurvatureradius":
                            Settings.LastGame.SweetFX.CRT.Curvature_Radius = OutOfRangeCheck("CRTCurvatureRadius", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)2 });
                            break;
                        case "crtcornersize":
                            Settings.LastGame.SweetFX.CRT.Corner_Size = OutOfRangeCheck("CRTCornerSize", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "crtdistance":
                            Settings.LastGame.SweetFX.CRT.Distance = OutOfRangeCheck("CRTDistance", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)4 });
                            break;
                        case "crtanglex":
                            Settings.LastGame.SweetFX.CRT.AngleX = OutOfRangeCheck("CRTAngleX", Convert.ToDecimal(value), new decimal[] { (decimal)-0.2, (decimal)0.2 });
                            break;
                        case "crtangley":
                            Settings.LastGame.SweetFX.CRT.AngleY = OutOfRangeCheck("CRTAngleY", Convert.ToDecimal(value), new decimal[] { (decimal)-0.2, (decimal)0.2 });
                            break;
                        case "crtoverscan":
                            Settings.LastGame.SweetFX.CRT.Overscan = OutOfRangeCheck("CRTOverScan", Convert.ToDecimal(value), new decimal[] { (decimal)1, (decimal)1.1 });
                            break;
                        case "crtoversample":
                            Settings.LastGame.SweetFX.CRT.Oversample = Convert.ToBoolean(OutOfRangeCheck("CRTOversample", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        // Bloom
                        case "use_bloom":
                            Settings.LastGame.SweetFX.Bloom.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_BLOOM", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "bloomthreshold":
                            Settings.LastGame.SweetFX.Bloom.Threshold = OutOfRangeCheck("BloomThreshold", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)50 });
                            break;
                        case "bloompower":
                            Settings.LastGame.SweetFX.Bloom.Power = OutOfRangeCheck("BloomPower", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)8 });
                            break;
                        case "bloomwidth":
                            Settings.LastGame.SweetFX.Bloom.Width = OutOfRangeCheck("BloomWidth", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        // HDR
                        case "use_hdr":
                            Settings.LastGame.SweetFX.HDR.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_HDR", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "hdrpower":
                            Settings.LastGame.SweetFX.HDR.Power = OutOfRangeCheck("HDRPower", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)8 });
                            break;
                        case "radius2":
                            Settings.LastGame.SweetFX.HDR.Radius = OutOfRangeCheck("radius2", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)8 });
                            break;
                        // LumaSharpen
                        case "use_lumasharpen":
                            Settings.LastGame.SweetFX.LumaSharpen.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_LUMASHARPEN", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "sharp_strength":
                            Settings.LastGame.SweetFX.LumaSharpen.Strength = OutOfRangeCheck("sharp_strength", Convert.ToDecimal(value), new decimal[] { (decimal)0.10, (decimal)3 });
                            break;
                        case "sharp_clamp":
                            Settings.LastGame.SweetFX.LumaSharpen.Clamp = OutOfRangeCheck("sharp_clamp", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "pattern":
                            Settings.LastGame.SweetFX.LumaSharpen.Pattern = OutOfRangeCheck("pattern", Convert.ToInt32(value), new int[] { 1, 4 });
                            break;
                        case "offset_bias":
                            Settings.LastGame.SweetFX.LumaSharpen.Offset_Bias = OutOfRangeCheck("offset_bias", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)6 });
                            break;
                        case "show_sharpen":
                            Settings.LastGame.SweetFX.LumaSharpen.Show = Convert.ToBoolean(OutOfRangeCheck("show_sharpen", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        // Levels
                        case "use_levels":
                            Settings.LastGame.SweetFX.Levels.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_LEVELS", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "levels_black_point":
                            Settings.LastGame.SweetFX.Levels.Black_Point = OutOfRangeCheck("Levels_black_point", Convert.ToInt32(value), new int[] { 0, 255 });
                            break;
                        case "levels_white_point":
                            Settings.LastGame.SweetFX.Levels.White_Point = OutOfRangeCheck("Levels_white_point", Convert.ToInt32(value), new int[] { 0, 255 });
                            break;
                        // Technicolor
                        case "use_technicolor":
                            Settings.LastGame.SweetFX.Technicolor.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_TECHNICOLOR", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "techniamount":
                            Settings.LastGame.SweetFX.Technicolor.Amount = OutOfRangeCheck("TechniAmount", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "technipower":
                            Settings.LastGame.SweetFX.Technicolor.Power = OutOfRangeCheck("TechniPower", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)8 });
                            break;
                        case "rednegativeamount":
                            Settings.LastGame.SweetFX.Technicolor.Red_Negative_Amount = OutOfRangeCheck("redNegativeAmount", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "greennegativeamount":
                            Settings.LastGame.SweetFX.Technicolor.Green_Negative_Amount = OutOfRangeCheck("greenNegativeAmount", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "bluenegativeamount":
                            Settings.LastGame.SweetFX.Technicolor.Blue_Negative_Amount = OutOfRangeCheck("blueNegativeAmount", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        // Cineon DPX
                        case "use_dpx":
                            Settings.LastGame.SweetFX.Cineon_DPX.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_DPX", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "red":
                            Settings.LastGame.SweetFX.Cineon_DPX.Red = OutOfRangeCheck("Red", Convert.ToDecimal(value), new decimal[] { (decimal)1, (decimal)15 });
                            break;
                        case "green":
                            Settings.LastGame.SweetFX.Cineon_DPX.Green = OutOfRangeCheck("Green", Convert.ToDecimal(value), new decimal[] { (decimal)1, (decimal)15 });
                            break;
                        case "blue":
                            Settings.LastGame.SweetFX.Cineon_DPX.Blue = OutOfRangeCheck("Blue", Convert.ToDecimal(value), new decimal[] { (decimal)1, (decimal)15 });
                            break;
                        case "colorgamma":
                            Settings.LastGame.SweetFX.Cineon_DPX.Color_Gamma = OutOfRangeCheck("ColorGamma", Convert.ToDecimal(value), new decimal[] { (decimal)0.1, (decimal)2.5 });
                            break;
                        case "dpxsaturation":
                            Settings.LastGame.SweetFX.Cineon_DPX.Saturation = OutOfRangeCheck("DPXSaturation", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)8 });
                            break;
                        case "redc":
                            Settings.LastGame.SweetFX.Cineon_DPX.RedC = OutOfRangeCheck("RedC", Convert.ToDecimal(value), new decimal[] { (decimal)0.2, (decimal)0.6 });
                            break;
                        case "greenc":
                            Settings.LastGame.SweetFX.Cineon_DPX.GreenC = OutOfRangeCheck("GreenC", Convert.ToDecimal(value), new decimal[] { (decimal)0.2, (decimal)0.6 });
                            break;
                        case "bluec":
                            Settings.LastGame.SweetFX.Cineon_DPX.BlueC = OutOfRangeCheck("BlueC", Convert.ToDecimal(value), new decimal[] { (decimal)0.2, (decimal)0.6 });
                            break;
                        case "blend":
                            Settings.LastGame.SweetFX.Cineon_DPX.Blend = OutOfRangeCheck("Blend", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        // Monochrome
                        case "use_monochrome":
                            Settings.LastGame.SweetFX.Monochrome.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_MONOCHROME", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "monochrome_conversion_values":
                            Settings.LastGame.SweetFX.Monochrome.Red = OutOfRangeCheck("monochrome_conversion_values", Convert.ToDecimal(_rgb[0].Trim()), new decimal[] { (decimal)0, (decimal)1 });
                            Settings.LastGame.SweetFX.Monochrome.Green = OutOfRangeCheck("monochrome_conversion_values", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)1 });
                            Settings.LastGame.SweetFX.Monochrome.Blue = OutOfRangeCheck("monochrome_conversion_values", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        // Lift Gamma Gain
                        case "use_liftgammagain":
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_LIFTGAMMAGAIN", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "rgb_lift":
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Red = OutOfRangeCheck("RGB_Lift", Convert.ToDecimal(_rgb[0].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Green = OutOfRangeCheck("RGB_Lift", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Lift_Blue = OutOfRangeCheck("RGB_Lift", Convert.ToDecimal(_rgb[2].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            break;
                        case "rgb_gamma":
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Red = OutOfRangeCheck("RGB_Gamma", Convert.ToDecimal(_rgb[0].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Green = OutOfRangeCheck("RGB_Gamma", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gamma_Blue = OutOfRangeCheck("RGB_Gamma", Convert.ToDecimal(_rgb[2].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            break;
                        case "rgb_gain":
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Red = OutOfRangeCheck("RGB_Gain", Convert.ToDecimal(_rgb[0].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Green = OutOfRangeCheck("RGB_Gain", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            Settings.LastGame.SweetFX.Lift_Gamma_Gain.Gain_Blue = OutOfRangeCheck("RGB_Gain", Convert.ToDecimal(_rgb[2].Trim()), new decimal[] { (decimal)0, (decimal)2 });
                            break;
                        // Tonemap
                        case "use_tonemap":
                            Settings.LastGame.SweetFX.Tonemap.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_TONEMAP", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "gamma":
                            Settings.LastGame.SweetFX.Tonemap.Gamma = OutOfRangeCheck("Gamma", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)2 });
                            break;
                        case "exposure":
                            Settings.LastGame.SweetFX.Tonemap.Exposure = OutOfRangeCheck("Exposure", Convert.ToDecimal(value), new decimal[] { (decimal)-1, (decimal)1 });
                            break;
                        case "saturation":
                            Settings.LastGame.SweetFX.Tonemap.Saturation = OutOfRangeCheck("Saturation", Convert.ToDecimal(value), new decimal[] { (decimal)-1, (decimal)1 });
                            break;
                        case "bleach":
                            Settings.LastGame.SweetFX.Tonemap.Bleach = OutOfRangeCheck("Bleach", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "defog":
                            Settings.LastGame.SweetFX.Tonemap.Defog = OutOfRangeCheck("Defog", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "fogcolor":
                            Settings.LastGame.SweetFX.Tonemap.Fog_Red = OutOfRangeCheck("FogColor", Convert.ToDecimal(_rgb[0].Trim()), new decimal[] { (decimal)0, (decimal)2.55 });
                            Settings.LastGame.SweetFX.Tonemap.Fog_Green = OutOfRangeCheck("FogColor", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)2.55 });
                            Settings.LastGame.SweetFX.Tonemap.Fog_Blue = OutOfRangeCheck("FogColor", Convert.ToDecimal(_rgb[2].Trim()), new decimal[] { (decimal)0, (decimal)2.55 });
                            break;
                        // Vibrance
                        case "use_vibrance":
                            Settings.LastGame.SweetFX.Vibrance.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_VIBRANCE", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "vibrance":
                            Settings.LastGame.SweetFX.Vibrance.Vibrance = OutOfRangeCheck("Vibrance", Convert.ToDecimal(value), new decimal[] { (decimal)-1, (decimal)1 });
                            break;
                        case "vibrance_rgb_balance":
                            Settings.LastGame.SweetFX.Vibrance.Red = OutOfRangeCheck("Vibrance_RGB_balance", Convert.ToDecimal(_rgb[0].Trim()), new decimal[] { (decimal)-10, (decimal)10 });
                            Settings.LastGame.SweetFX.Vibrance.Green = OutOfRangeCheck("Vibrance_RGB_balance", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)-10, (decimal)10 });
                            Settings.LastGame.SweetFX.Vibrance.Blue = OutOfRangeCheck("Vibrance_RGB_balance", Convert.ToDecimal(_rgb[2].Trim()), new decimal[] { (decimal)-10, (decimal)10 });
                            break;
                        // Curves
                        case "use_curves":
                            Settings.LastGame.SweetFX.Curves.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_CURVES", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "curves_mode":
                            Settings.LastGame.SweetFX.Curves.Mode = OutOfRangeCheck("Curves_mode", Convert.ToInt32(value), new int[] { 0, 2 });
                            break;
                        case "curves_contrast":
                            Settings.LastGame.SweetFX.Curves.Contrast = OutOfRangeCheck("Curves_contrast", Convert.ToDecimal(value), new decimal[] { (decimal)-1, (decimal)1 });
                            break;
                        case "curves_formula":
                            Settings.LastGame.SweetFX.Curves.Formula = OutOfRangeCheck("Curves_formula", Convert.ToInt32(value), new int[] { 1, 10 });
                            break;
                        // Sepia
                        case "use_sepia":
                            Settings.LastGame.SweetFX.Sepia.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_SEPIA", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "colortone":
                            Settings.LastGame.SweetFX.Sepia.Red = OutOfRangeCheck("ColorTone", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)2.55 });
                            Settings.LastGame.SweetFX.Sepia.Green = OutOfRangeCheck("ColorTone", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)2.55 });
                            Settings.LastGame.SweetFX.Sepia.Blue = OutOfRangeCheck("ColorTone", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)2.55 });
                            break;
                        case "greypower":
                            Settings.LastGame.SweetFX.Sepia.Grey_Power = OutOfRangeCheck("GreyPower", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        case "sepiapower":
                            Settings.LastGame.SweetFX.Sepia.Power = OutOfRangeCheck("SepiaPower", Convert.ToDecimal(value), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        // Vignette
                        case "use_vignette":
                            Settings.LastGame.SweetFX.Vignette.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_VIGNETTE", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "vignettetype":
                            Settings.LastGame.SweetFX.Vignette.Type = OutOfRangeCheck("VignetteType", Convert.ToInt32(value), new int[] { 1, 3 });
                            break;
                        case "vignetteratio":
                            Settings.LastGame.SweetFX.Vignette.Ratio = OutOfRangeCheck("VignetteRatio", Convert.ToDecimal(value), new decimal[] { (decimal)0.15, (decimal)6 });
                            break;
                        case "vignetteradius":
                            Settings.LastGame.SweetFX.Vignette.Radius = OutOfRangeCheck("VignetteRadius", Convert.ToDecimal(value), new decimal[] { (decimal)-1, (decimal)3 });
                            break;
                        case "vignetteamount":
                            Settings.LastGame.SweetFX.Vignette.Amount = OutOfRangeCheck("VignetteAmount", Convert.ToDecimal(value), new decimal[] { (decimal)-2, (decimal)1 });
                            break;
                        case "vignetteslope":
                            Settings.LastGame.SweetFX.Vignette.Slope = OutOfRangeCheck("VignetteSlope", Convert.ToInt32(value), new int[] { 2, 16 });
                            break;
                        case "vignettecenter":
                            Settings.LastGame.SweetFX.Vignette.Center_X = OutOfRangeCheck("VignetteCenter", Convert.ToDecimal(_rgb[0].Trim()), new decimal[] { (decimal)0, (decimal)1 });
                            Settings.LastGame.SweetFX.Vignette.Center_Y = OutOfRangeCheck("VignetteCenter", Convert.ToDecimal(_rgb[1].Trim()), new decimal[] { (decimal)0, (decimal)1 });
                            break;
                        // Vignette
                        case "use_dither":
                            Settings.LastGame.SweetFX.Dither.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_VIGNETTE", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "dither_method":
                            Settings.LastGame.SweetFX.Dither.Method = OutOfRangeCheck("dither_method", Convert.ToInt32(value), new int[] { 1, 2 });
                            break;
                        // Border
                        case "use_border":
                            Settings.LastGame.SweetFX.Border.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_BORDER", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "border_width":
                            Settings.LastGame.SweetFX.Border.Width_X = OutOfRangeCheck("border_width", Convert.ToInt32(_rgb[0].Trim()), new int[] { 0, 2048 });
                            Settings.LastGame.SweetFX.Border.Width_Y = OutOfRangeCheck("border_width", Convert.ToInt32(_rgb[1].Trim()), new int[] { 0, 2048 });
                            break;
                        case "border_color":
                            Settings.LastGame.SweetFX.Border.Red = OutOfRangeCheck("border_color", Convert.ToInt32(_rgb[0].Trim()), new int[] { 0, 255 });
                            Settings.LastGame.SweetFX.Border.Green = OutOfRangeCheck("border_color", Convert.ToInt32(_rgb[1].Trim()), new int[] { 0, 255 });
                            Settings.LastGame.SweetFX.Border.Blue = OutOfRangeCheck("border_color", Convert.ToInt32(_rgb[2].Trim()), new int[] { 0, 255 });
                            break;
                        // Splitscreen
                        case "use_splitscreen":
                            Settings.LastGame.SweetFX.Splitscreen.Enabled = Convert.ToBoolean(OutOfRangeCheck("USE_SPLITSCREEN", Convert.ToInt32(value), new int[] { 0, 1 }));
                            break;
                        case "splitscreen_mode":
                            Settings.LastGame.SweetFX.Splitscreen.Mode = OutOfRangeCheck("splitscreen_mode", Convert.ToInt32(value), new int[] { 1, 6 });
                            break;
                    }
                }
            }
            _timer.Elapsed += _timer_Elapsed;
            GameLoaded();
            _loading = false;
        }

        public static void Unload()
        {
            Settings.LastGame.SweetFX.SMAA = null;
            Settings.LastGame.SweetFX.FXAA = null;
            Settings.LastGame.SweetFX.Explosion = null;
            Settings.LastGame.SweetFX.Cartoon = null;
            Settings.LastGame.SweetFX.CRT = null;
            Settings.LastGame.SweetFX.Bloom = null;
            Settings.LastGame.SweetFX.HDR = null;
            Settings.LastGame.SweetFX.LumaSharpen = null;
            Settings.LastGame.SweetFX.Levels = null;
            Settings.LastGame.SweetFX.Technicolor = null;
            Settings.LastGame.SweetFX.Cineon_DPX = null;
        }

        public static void SaveSetting(Setting set, bool bypass = false)
        {
            if (!bypass && _loading) { return; }
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

        public static void Dispose()
        {
            if (SaveSettingQueue.Count > 0) { _timer_Elapsed(null, null); }
            Unload();
            _timer.Dispose();
        }

        private static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string[] lines = File.ReadAllLines(Settings.LastGame.Directory.FullName + @"\SweetFX_settings.txt");
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
                        if (key.ToLower() == set.Key)
                        {
                            setting = setting.Substring(index, (setting.Length - 1) - index).Trim();
                            string value;
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
            }
            File.WriteAllLines(Settings.LastGame.Directory.FullName + @"\SweetFX_settings.txt", lines);
            _timer.Enabled = false;
            _timer.Stop();
            SettingsSaved();
        }

        public static int OutOfRangeCheck(string setting, int value, int[] range)
        {
            int new_value = 0;
            if (value < range[0]) { new_value = range[0]; }
            else if (value > range[1]) { new_value = range[1]; }
            else { return value; }
            TaskDialog task = new TaskDialog();
            task.WindowTitle = "SweetFX setting " + '"' + setting + '"' + " out of range";
            task.Content = "The setting " + '"' + setting + '"' + " value of " + value.ToString() + " is out of range. The recommended and only range Sweeter SweetFX works with right now is: " + range[0].ToString() + " - " + range[1].ToString() + ". Would you like the value to automatically changed to: " + new_value.ToString() + "?";
            TaskDialogButton yes_button = new TaskDialogButton(ButtonType.Yes);
            task.Buttons.Add(yes_button);
            task.Buttons.Add(new TaskDialogButton(ButtonType.No));
            if (task.Show() == yes_button) { SaveSetting(new Setting(setting.ToLower(), new_value.ToString()), true); return new_value; }
            else { Environment.Exit(0); return 0; }
        }

        public static decimal OutOfRangeCheck(string setting, decimal value, decimal[] range)
        {
            decimal new_value = 0;
            if (value < range[0]) { new_value = range[0]; }
            else if (value > range[1]) { new_value = range[1]; }
            else { return value; }
            TaskDialog task = new TaskDialog();
            task.WindowTitle = "SweetFX setting " + '"' + setting + '"' + " out of range";
            task.Content = "The setting " + '"' + setting + '"' + " value of " + value.ToString() + " is out of range. The recommended and only range Sweeter SweetFX works with right now is: " + range[0].ToString() + " - " + range[1].ToString() + ". Would you like the value to automatically changed to: " + new_value.ToString() + "?";
            TaskDialogButton yes_button = new TaskDialogButton(ButtonType.Yes);
            task.Buttons.Add(yes_button);
            task.Buttons.Add(new TaskDialogButton(ButtonType.No));
            if (task.Show() == yes_button) { SaveSetting(new Setting(setting.ToLower(), new_value.ToString()), true); return new_value; }
            else { Environment.Exit(0); return 0; }
        }
    }

    public class _SweetFX
    {
        public _SweetFX()
        {
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
        }

        public _SMAA SMAA { get; set; }
        public _FXAA FXAA { get; set; }
        public _Explosion Explosion { get; set; }
        public _Cartoon Cartoon { get; set; }
        public _CRT CRT { get; set; }
        public _Bloom Bloom { get; set; }
        public _HDR HDR { get; set; }
        public _LumaSharpen LumaSharpen { get; set; }
        public _Levels Levels { get; set; }
        public _Technicolor Technicolor { get; set; }
        public _Cineon_DPX Cineon_DPX { get; set; }
        public _Monochrome Monochrome { get; set; }
        public _Lift_Gamma_Gain Lift_Gamma_Gain { get; set; }
        public _Tonemap Tonemap { get; set; }
        public _Vibrance Vibrance { get; set; }
        public _Curves Curves { get; set; }
        public _Sepia Sepia { get; set; }
        public _Vignette Vignette { get; set; }
        public _Dither Dither { get; set; }
        public _Border Border { get; set; }
        public _Splitscreen Splitscreen { get; set; }
    }

    public class Setting
    {
        private string _key;
        private string _value;

        public Setting(string k, string v)
        {
            _key = k;
            _value = v;
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
        private bool _enabled = true;
        public decimal _threshold = (decimal)0.1;
        public int _max_search_steps = 16;
        public int _max_search_steps_diag = 6;
        public int _corner_rounding = 0;
        public int _color_edge_detection = 1;
        public bool _directx9_linear_blend = false;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_smaa_antialiasing", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                SweetFX_Parser.SaveSetting(new Setting("smaa_threshold", value.ToString()));
            }
        }

        public int Max_Search_Steps
        {
            get { return _max_search_steps; }
            set
            {
                _max_search_steps = value;
                SweetFX_Parser.SaveSetting(new Setting("smaa_max_search_steps", value.ToString()));
            }
        }

        public int Max_Search_Steps_Diag
        {
            get { return _max_search_steps_diag; }
            set
            {
                _max_search_steps_diag = value;
                SweetFX_Parser.SaveSetting(new Setting("smaa_max_search_steps_diag", value.ToString()));
            }
        }

        public int Corner_Rounding
        {
            get { return _corner_rounding; }
            set
            {
                _corner_rounding = value;
                SweetFX_Parser.SaveSetting(new Setting("smaa_corner_rounding", value.ToString()));
            }
        }

        public int Color_Edge_Detection
        {
            get { return _color_edge_detection; }
            set
            {
                _color_edge_detection = value;
                SweetFX_Parser.SaveSetting(new Setting("color_edge_detection", value.ToString()));
            }
        }

        public bool DirectX9_Linear_Blend
        {
            get { return _directx9_linear_blend; }
            set
            {
                _directx9_linear_blend = value;
                SweetFX_Parser.SaveSetting(new Setting("smaa_directx9_linear_blend", (Convert.ToInt32(value)).ToString()));
            }
        }
    }

    public class _FXAA
    {
        private bool _enabled = false;
        private int _quality_preset = 9;
        private decimal _subpix = (decimal)0.4;
        private decimal _edge_threshold = (decimal)0.25;
        private decimal _edge_threshold_min = (decimal)0.06;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_fxaa_antialiasing", (Convert.ToInt32(value)).ToString()));
            }
        }

        public int Quality_Preset
        {
            get { return _quality_preset; }
            set
            {
                if (value < 1) { return; }
                if (value > 9) { return; }
                _quality_preset = value;
                SweetFX_Parser.SaveSetting(new Setting("fxaa_quality__preset", value.ToString()));
            }
        }

        public decimal Subpix
        {
            get { return _subpix; }
            set
            {
                _subpix = value;
                SweetFX_Parser.SaveSetting(new Setting("fxaa_subpix", value.ToString()));
            }
        }

        public decimal Edge_Threshold
        {
            get { return _edge_threshold; }
            set
            {
                _edge_threshold = value;
                SweetFX_Parser.SaveSetting(new Setting("fxaa_edgethreshold", value.ToString()));
            }
        }

        public decimal Edge_Threshold_Min
        {
            get { return _edge_threshold_min; }
            set
            {
                _edge_threshold_min = value;
                SweetFX_Parser.SaveSetting(new Setting("fxaa_edgethresholdmin", value.ToString()));
            }
        }
    }

    public class _Explosion
    {
        private bool _enabled = false;
        private decimal _radius = (decimal)2.5;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_explosion", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                SweetFX_Parser.SaveSetting(new Setting("explosion_radius", value.ToString()));
            }
        }
    }

    public class _Cartoon
    {
        private bool _enabled = false;
        private decimal _power = (decimal)1.5;
        private decimal _edge_slope = (decimal)1.5;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_cartoon", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX_Parser.SaveSetting(new Setting("cartoonpower", value.ToString()));
            }
        }

        public decimal Edge_Slope
        {
            get { return _edge_slope; }
            set
            {
                _edge_slope = value;
                SweetFX_Parser.SaveSetting(new Setting("cartoonedgeslope", value.ToString()));
            }
        }
    }

    public class _CRT
    {
        private bool _enabled = false;
        private decimal _amount = 1;
        private decimal _resolution = 2;
        private decimal _gamma = (decimal)2.2;
        private decimal _monitor_gamma = (decimal)2.4;
        private decimal _brightness = (decimal)1.2;
        private decimal _scanline_intensity = 2;
        private bool _scanline_gaussian = true;
        private bool _curvature = true;
        private decimal _curvature_radius = 2;
        private decimal _corner_size = (decimal)0.01;
        private decimal _distance = 2;
        private decimal _angle_x = 0;
        private decimal _angle_y = (decimal)-0.15;
        private decimal _overscan = 1;
        private bool _oversample = false;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_advanced_crt", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                SweetFX_Parser.SaveSetting(new Setting("crtamount", value.ToString()));
            }
        }

        public decimal Resolution
        {
            get { return _resolution; }
            set
            {
                _resolution = value;
                SweetFX_Parser.SaveSetting(new Setting("crtresolution", value.ToString()));
            }
        }

        public decimal Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                SweetFX_Parser.SaveSetting(new Setting("crtgamma", value.ToString()));
            }
        }

        public decimal Monitor_Gamma
        {
            get { return _monitor_gamma; }
            set
            {
                _monitor_gamma = value;
                SweetFX_Parser.SaveSetting(new Setting("crtmonitorgamma", value.ToString()));
            }
        }

        public decimal Brightness
        {
            get { return _brightness; }
            set
            {
                _brightness = value;
                SweetFX_Parser.SaveSetting(new Setting("crtbrightness", value.ToString()));
            }
        }

        public decimal Scanline_Intensity
        {
            get { return _scanline_intensity; }
            set
            {
                _scanline_intensity = value;
                SweetFX_Parser.SaveSetting(new Setting("crtscanlineintensity", value.ToString()));
            }
        }

        public bool Scanline_Gaussian
        {
            get { return _scanline_gaussian; }
            set
            {
                _scanline_gaussian = value;
                SweetFX_Parser.SaveSetting(new Setting("crtscanlinegaussian", (Convert.ToInt32(value)).ToString()));
            }
        }

        public bool Curvature
        {
            get { return _curvature; }
            set
            {
                _curvature = value;
                SweetFX_Parser.SaveSetting(new Setting("crtcurvature", (Convert.ToInt32(value)).ToString()));
            }
        }

        public decimal Curvature_Radius
        {
            get { return _curvature_radius; }
            set
            {
                _curvature_radius = value;
                SweetFX_Parser.SaveSetting(new Setting("crtcurvatureradius", value.ToString()));
            }
        }

        public decimal Corner_Size
        {
            get { return _corner_size; }
            set
            {
                _corner_size = value;
                SweetFX_Parser.SaveSetting(new Setting("crtcornersize", value.ToString()));
            }
        }

        public decimal Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                SweetFX_Parser.SaveSetting(new Setting("crtdistance", value.ToString()));
            }
        }

        public decimal AngleX
        {
            get { return _angle_x; }
            set
            {
                _angle_x = value;
                SweetFX_Parser.SaveSetting(new Setting("crtanglex", value.ToString()));
            }
        }

        public decimal AngleY
        {
            get { return _angle_y; }
            set
            {
                _angle_y = value;
                SweetFX_Parser.SaveSetting(new Setting("crtangley", value.ToString()));
            }
        }

        public decimal Overscan
        {
            get { return _overscan; }
            set
            {
                _overscan = value;
                SweetFX_Parser.SaveSetting(new Setting("crtoverscan", value.ToString()));
            }
        }

        public bool Oversample
        {
            get { return _oversample; }
            set
            {
                _oversample = value;
                SweetFX_Parser.SaveSetting(new Setting("crtoversample", (Convert.ToInt32(value)).ToString()));
            }
        }
    }

    public class _Bloom
    {
        private bool _enabled = false;
        private decimal _threshold = (decimal)20.25;
        private decimal _power = (decimal)1.446;
        private decimal _width = (decimal)0.0142;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_bloom", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                SweetFX_Parser.SaveSetting(new Setting("bloomthreshold", value.ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX_Parser.SaveSetting(new Setting("bloompower", value.ToString()));
            }
        }

        public decimal Width
        {
            get { return _width; }
            set
            {
                _width = value;
                SweetFX_Parser.SaveSetting(new Setting("bloomwidth", value.ToString()));
            }
        }
    }

    public class _HDR
    {
        private bool _enabled = false;
        private decimal _power = (decimal)1.3;
        private decimal _radius = (decimal)0.87;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_hdr", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX_Parser.SaveSetting(new Setting("hdrpower", value.ToString()));
            }
        }

        public decimal Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                SweetFX_Parser.SaveSetting(new Setting("radius2", value.ToString()));
            }
        }
    }

    public class _LumaSharpen
    {
        private bool _enabled = true;
        private decimal _strength = (decimal)0.65;
        private decimal _clamp = (decimal)0.035;
        private int _pattern = 2;
        private decimal _offset_bias = 1;
        private bool _show = false;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_lumasharpen", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Strength
        {
            get { return _strength; }
            set
            {
                _strength = value;
                SweetFX_Parser.SaveSetting(new Setting("sharp_strength", value.ToString()));
            }
        }

        public decimal Clamp
        {
            get { return _clamp; }
            set
            {
                _clamp = value;
                SweetFX_Parser.SaveSetting(new Setting("sharp_clamp", value.ToString()));
            }
        }

        public int Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
                SweetFX_Parser.SaveSetting(new Setting("pattern", value.ToString()));
            }
        }

        public decimal Offset_Bias
        {
            get { return _offset_bias; }
            set
            {
                _offset_bias = value;
                SweetFX_Parser.SaveSetting(new Setting("offset_bias", value.ToString()));
            }
        }

        public bool Show
        {
            get { return _show; }
            set
            {
                _show = value;
                SweetFX_Parser.SaveSetting(new Setting("show_sharpen", Convert.ToInt32(value).ToString()));
            }
        }
    }

    public class _Levels
    {
        private bool _enabled = false;
        private int _black_point = 16;
        private int _white_point = 235;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_levels", Convert.ToInt32(value).ToString()));
            }
        }

        public int Black_Point
        {
            get { return _black_point; }
            set
            {
                _black_point = value;
                SweetFX_Parser.SaveSetting(new Setting("levels_black_point", value.ToString()));
            }
        }

        public int White_Point
        {
            get { return _white_point; }
            set
            {
                _white_point = value;
                SweetFX_Parser.SaveSetting(new Setting("levels_white_point", value.ToString()));
            }
        }
    }

    public class _Technicolor
    {
        private bool _enabled = false;
        private decimal _amount = (decimal)0.4;
        private decimal _power = 4;
        private decimal _red_negative_amount = (decimal)0.88;
        private decimal _green_negative_amount = (decimal)0.88;
        private decimal _blue_negative_amount = (decimal)0.88;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_technicolor", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                SweetFX_Parser.SaveSetting(new Setting("techniamount", value.ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX_Parser.SaveSetting(new Setting("technipower", value.ToString()));
            }
        }

        public decimal Red_Negative_Amount
        {
            get { return _red_negative_amount; }
            set
            {
                _red_negative_amount = value;
                SweetFX_Parser.SaveSetting(new Setting("rednegativeamount", value.ToString()));
            }
        }

        public decimal Green_Negative_Amount
        {
            get { return _green_negative_amount; }
            set
            {
                _green_negative_amount = value;
                SweetFX_Parser.SaveSetting(new Setting("greennegativeamount", value.ToString()));
            }
        }

        public decimal Blue_Negative_Amount
        {
            get { return _blue_negative_amount; }
            set
            {
                _blue_negative_amount = value;
                SweetFX_Parser.SaveSetting(new Setting("bluenegativeamount", value.ToString()));
            }
        }
    }

    public class _Cineon_DPX
    {
        private bool _enabled = false;
        private decimal _red = 8;
        private decimal _green = 8;
        private decimal _blue = 8;
        private decimal _color_gamma = (decimal)2.5;
        private decimal _saturation = 3;
        private decimal _redc = (decimal)0.36;
        private decimal _greenc = (decimal)0.36;
        private decimal _bluec = (decimal)0.34;
        private decimal _blend = (decimal)0.2;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_dpx", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Red
        {
            get { return _red; }
            set
            {
                _red = value;
                SweetFX_Parser.SaveSetting(new Setting("red", value.ToString()));
            }
        }

        public decimal Green
        {
            get { return _green; }
            set
            {
                _green = value;
                SweetFX_Parser.SaveSetting(new Setting("green", value.ToString()));
            }
        }

        public decimal Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                SweetFX_Parser.SaveSetting(new Setting("blue", value.ToString()));
            }
        }

        public decimal Color_Gamma
        {
            get { return _color_gamma; }
            set
            {
                _color_gamma = value;
                SweetFX_Parser.SaveSetting(new Setting("colorgamma", value.ToString()));
            }
        }

        public decimal Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = value;
                SweetFX_Parser.SaveSetting(new Setting("dpxsaturation", value.ToString()));
            }
        }

        public decimal RedC
        {
            get { return _redc; }
            set
            {
                _redc = value;
                SweetFX_Parser.SaveSetting(new Setting("redc", value.ToString()));
            }
        }

        public decimal GreenC
        {
            get { return _greenc; }
            set
            {
                _greenc = value;
                SweetFX_Parser.SaveSetting(new Setting("greenc", value.ToString()));
            }
        }

        public decimal BlueC
        {
            get { return _bluec; }
            set
            {
                _bluec = value;
                SweetFX_Parser.SaveSetting(new Setting("bluec", value.ToString()));
            }
        }

        public decimal Blend
        {
            get { return _blend; }
            set
            {
                _blend = value;
                SweetFX_Parser.SaveSetting(new Setting("blend", value.ToString()));
            }
        }
    }

    public class _Monochrome
    {
        private bool _enabled = false;
        private decimal _red = (decimal)0.18;
        private decimal _green = (decimal)0.41;
        private decimal _blue = (decimal)0.41;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_monochrome", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Red
        {
            get { return _red; }
            set
            {
                _red = value;
                SweetFX_Parser.SaveSetting(new Setting("monochrome_conversion_values", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public decimal Green
        {
            get { return _green; }
            set
            {
                _green = value;
                SweetFX_Parser.SaveSetting(new Setting("monochrome_conversion_values", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public decimal Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                SweetFX_Parser.SaveSetting(new Setting("monochrome_conversion_values", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }
    }

    public class _Lift_Gamma_Gain
    {
        private bool _enabled = false;
        private decimal _lift_red = 1;
        private decimal _lift_green = 1;
        private decimal _lift_blue = 1;
        private decimal _gamma_red = 1;
        private decimal _gamma_green = 1;
        private decimal _gamma_blue = 1;
        private decimal _gain_red = 1;
        private decimal _gain_green = 1;
        private decimal _gain_blue = 1;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_liftgammagain", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Lift_Red
        {
            get { return _lift_red; }
            set
            {
                _lift_red = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_lift", _lift_red + ", " + _lift_green + ", " + _lift_blue));
            }
        }

        public decimal Lift_Green
        {
            get { return _lift_green; }
            set
            {
                _lift_green = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_lift", _lift_red + ", " + _lift_green + ", " + _lift_blue));
            }
        }

        public decimal Lift_Blue
        {
            get { return _lift_green; }
            set
            {
                _lift_blue = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_lift", _lift_red + ", " + _lift_green + ", " + _lift_blue));
            }
        }

        public decimal Gamma_Red
        {
            get { return _gamma_red; }
            set
            {
                _gamma_red = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_gamma", _gamma_red + ", " + _gamma_green + ", " + _gamma_blue));
            }
        }

        public decimal Gamma_Green
        {
            get { return _gamma_green; }
            set
            {
                _gamma_green = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_gamma", _gamma_red + ", " + _gamma_green + ", " + _gamma_blue));
            }
        }

        public decimal Gamma_Blue
        {
            get { return _gamma_blue; }
            set
            {
                _gamma_blue = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_gamma", _gamma_red + ", " + _gamma_green + ", " + _gamma_blue));
            }
        }

        public decimal Gain_Red
        {
            get { return _gain_red; }
            set
            {
                _gain_red = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_gain", _gain_red + ", " + _gain_green + ", " + _gain_blue));
            }
        }

        public decimal Gain_Green
        {
            get { return _gain_green; }
            set
            {
                _gain_green = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_gain", _gain_red + ", " + _gain_green + ", " + _gain_blue));
            }
        }

        public decimal Gain_Blue
        {
            get { return _gain_blue; }
            set
            {
                _gain_blue = value;
                SweetFX_Parser.SaveSetting(new Setting("rgb_gain", _gain_red + ", " + _gain_green + ", " + _gain_blue));
            }
        }
    }

    public class _Tonemap
    {
        private bool _enabled = false;
        private decimal _gamma = 1;
        private decimal _exposure = 0;
        private decimal _saturation = 0;
        private decimal _bleach = 0;
        private decimal _defog = 0;
        private decimal _fog_red = 0;
        private decimal _fog_green = 0;
        private decimal _fog_blue = (decimal)2.55;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_tonemap", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                SweetFX_Parser.SaveSetting(new Setting("gamma", value.ToString()));
            }
        }

        public decimal Exposure
        {
            get { return _exposure; }
            set
            {
                _exposure = value;
                SweetFX_Parser.SaveSetting(new Setting("exposure", value.ToString()));
            }
        }

        public decimal Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = value;
                SweetFX_Parser.SaveSetting(new Setting("saturation", value.ToString()));
            }
        }

        public decimal Bleach
        {
            get { return _bleach; }
            set
            {
                _bleach = value;
                SweetFX_Parser.SaveSetting(new Setting("bleach", value.ToString()));
            }
        }

        public decimal Defog
        {
            get { return _defog; }
            set
            {
                _defog = value;
                SweetFX_Parser.SaveSetting(new Setting("defog", value.ToString()));
            }
        }

        public decimal Fog_Red
        {
            get { return _fog_red; }
            set
            {
                _fog_red = value;
                SweetFX_Parser.SaveSetting(new Setting("fogcolor", _fog_red.ToString() + ", " + _fog_green + ", " + _fog_blue));
            }
        }

        public decimal Fog_Green
        {
            get { return _fog_green; }
            set
            {
                _fog_green = value;
                SweetFX_Parser.SaveSetting(new Setting("fogcolor", _fog_red.ToString() + ", " + _fog_green + ", " + _fog_blue));
            }
        }

        public decimal Fog_Blue
        {
            get { return _fog_blue; }
            set
            {
                _fog_blue = value;
                SweetFX_Parser.SaveSetting(new Setting("fogcolor", _fog_red.ToString() + ", " + _fog_green + ", " + _fog_blue));
            }
        }
    }

    public class _Vibrance
    {
        private bool _enabled = true;
        private decimal vibrance_ = (decimal)0.15;
        private decimal _red = 1;
        private decimal _green = 1;
        private decimal _blue = 1;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_vibrance", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Vibrance
        {
            get { return vibrance_; }
            set
            {
                vibrance_ = value;
                SweetFX_Parser.SaveSetting(new Setting("vibrance", value.ToString()));
            }
        }

        public decimal Red
        {
            get { return _red; }
            set
            {
                _red = value;
                SweetFX_Parser.SaveSetting(new Setting("vibrance_rgb_balance", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public decimal Green
        {
            get { return _green; }
            set
            {
                _green = value;
                SweetFX_Parser.SaveSetting(new Setting("vibrance_rgb_balance", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public decimal Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                SweetFX_Parser.SaveSetting(new Setting("vibrance_rgb_balance", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }
    }

    public class _Curves
    {
        private bool _enabled = false;
        private int _mode = 0;
        private decimal _contrast = (decimal)0.15;
        private int _formula = 2;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_curves", Convert.ToInt32(value).ToString()));
            }
        }

        public int Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                SweetFX_Parser.SaveSetting(new Setting("curves_mode", value.ToString()));
            }
        }

        public decimal Contrast
        {
            get { return _contrast; }
            set
            {
                _contrast = value;
                SweetFX_Parser.SaveSetting(new Setting("curves_contrast", value.ToString()));
            }
        }

        public int Formula
        {
            get { return _formula; }
            set
            {
                _formula = value;
                SweetFX_Parser.SaveSetting(new Setting("curves_formula", value.ToString()));
            }
        }
    }

    public class _Sepia
    {
        private bool _enabled = false;
        private decimal _red = (decimal)1.40;
        private decimal _green = (decimal)1.10;
        private decimal _blue = (decimal)0.90;
        private decimal _grey_power = (decimal)0.11;
        private decimal _power = (decimal)0.58;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_sepia", Convert.ToInt32(value).ToString()));
            }
        }

        public decimal Red
        {
            get { return _red; }
            set
            {
                _red = value;
                SweetFX_Parser.SaveSetting(new Setting("colortone", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public decimal Green
        {
            get { return _green; }
            set
            {
                _green = value;
                SweetFX_Parser.SaveSetting(new Setting("colortone", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public decimal Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                SweetFX_Parser.SaveSetting(new Setting("colortone", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public decimal Grey_Power
        {
            get { return _grey_power; }
            set
            {
                _grey_power = value;
                SweetFX_Parser.SaveSetting(new Setting("greypower", value.ToString()));
            }
        }

        public decimal Power
        {
            get { return _power; }
            set
            {
                _power = value;
                SweetFX_Parser.SaveSetting(new Setting("sepiapower", value.ToString()));
            }
        }
    }

    public class _Vignette
    {
        private bool _enabled = false;
        private int _type = 1;
        private decimal _ratio = 1;
        private decimal _radius = 1;
        private decimal _amount = -1;
        private int _slope = 8;
        private decimal _center_x = (decimal)0.5;
        private decimal _center_y = (decimal)0.5;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_vignette", Convert.ToInt32(value).ToString()));
            }
        }

        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                SweetFX_Parser.SaveSetting(new Setting("vignettetype", value.ToString()));
            }
        }

        public decimal Ratio
        {
            get { return _ratio; }
            set
            {
                _ratio = value;
                SweetFX_Parser.SaveSetting(new Setting("vignetteratio", value.ToString()));
            }
        }

        public decimal Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                SweetFX_Parser.SaveSetting(new Setting("vignetteradius", value.ToString()));
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                SweetFX_Parser.SaveSetting(new Setting("vignetteamount", value.ToString()));
            }
        }

        public int Slope
        {
            get { return _slope; }
            set
            {
                _slope = value;
                SweetFX_Parser.SaveSetting(new Setting("vignetteslope", value.ToString()));
            }
        }

        public decimal Center_X
        {
            get { return _center_x; }
            set
            {
                _center_x = value;
                SweetFX_Parser.SaveSetting(new Setting("vignettecenter", _center_x + ", " + _center_y));
            }
        }

        public decimal Center_Y
        {
            get { return _center_y; }
            set
            {
                _center_y = value;
                SweetFX_Parser.SaveSetting(new Setting("vignettecenter", _center_x + ", " + _center_y));
            }
        }
    }

    public class _Dither
    {
        private bool _enabled = false;
        private int _method = 1;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_dither", Convert.ToInt32(value).ToString()));
            }
        }

        public int Method
        {
            get { return _method; }
            set
            {
                _method = value;
                SweetFX_Parser.SaveSetting(new Setting("dithermethod", value.ToString()));
            }
        }
    }

    public class _Border
    {
        private bool _enabled = false;
        private int _width_x = 1;
        private int _width_y = 20;
        private int _red = 0;
        private int _green = 0;
        private int _blue = 0;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_border", Convert.ToInt32(value).ToString()));
            }
        }

        public int Width_X
        {
            get { return _width_x; }
            set
            {
                _width_x = value;
                SweetFX_Parser.SaveSetting(new Setting("border_width", _width_x + ", " + _width_y));
            }
        }

        public int Width_Y
        {
            get { return _width_y; }
            set
            {
                _width_y = value;
                SweetFX_Parser.SaveSetting(new Setting("border_width", _width_x + ", " + _width_y));
            }
        }

        public int Red
        {
            get { return _red; }
            set
            {
                _red = value;
                SweetFX_Parser.SaveSetting(new Setting("border_color", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public int Green
        {
            get { return _green; }
            set
            {
                _green = value;
                SweetFX_Parser.SaveSetting(new Setting("border_color", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }

        public int Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                SweetFX_Parser.SaveSetting(new Setting("border_color", _red.ToString() + ", " + _green.ToString() + ", " + _blue.ToString()));
            }
        }
    }

    public class _Splitscreen
    {
        private bool _enabled = false;
        private int _mode = 1;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SweetFX_Parser.SaveSetting(new Setting("use_splitscreen", Convert.ToInt32(value).ToString()));
            }
        }

        public int Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                SweetFX_Parser.SaveSetting(new Setting("splitscreen_mode", value.ToString()));
            }
        }
    }
}
