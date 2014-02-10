using System;
using System.IO;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    class Settings
    {
        private static IniFile ini;

        public static void load()
        {
            string path = Application.StartupPath + "\\settings.ini";
            if (!File.Exists(path)) { File.Create(path); }
            //
            ini = new IniFile(path);
        }

        #region Main_Window_Geometry

        public static string Main_Window_Geometry
        {
            get
            {
                return ini.GetString("Settings", "Main_Window_Geometry", "");
            }
            set
            {
                ini.WriteValue("Settings", "Main_Window_Geometry", value);
            }
        }

        #endregion
    }
}
