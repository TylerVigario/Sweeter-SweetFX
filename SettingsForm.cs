using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public partial class SettingsForm : Form
    {
        private static bool _open;

        public SettingsForm()
        {
            _open = true;
            InitializeComponent();
        }

        public static bool isOpen { get { return _open; } }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Settings_Window_Geometry = WindowGeometry.GeometryToString(this);
            _open = false;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.Settings_Window_Geometry, this);
        }
    }
}
