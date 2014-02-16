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
    public partial class AboutForm : Form
    {
        private static bool _open;

        public AboutForm()
        {
            _open = true;
            InitializeComponent();
        }

        public static bool isOpen { get { return _open; } }

        private void AboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.About_Window_Geometry = WindowGeometry.GeometryToString(this);
            _open = false;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.About_Window_Geometry, this);
        }
    }
}
