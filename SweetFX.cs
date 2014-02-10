using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetFX_Configurator
{
    public static class SweetFX
    {
        private static IniFile ini;

        public static void Load(string file)
        {
            ini = new IniFile(file);
        }
    }
}
