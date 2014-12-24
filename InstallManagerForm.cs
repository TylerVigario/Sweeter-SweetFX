using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public partial class InstallManagerForm : Form
    {
        private static bool _open = false;

        public InstallManagerForm()
        {
            _open = true;
            InitializeComponent();
        }

        public static bool isOpen { get { return _open; } }

        // Save/Load ObjectLiveView state
        private void InstallManagerForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.InstallManager_Window_Geometry, this);
            this.olvColumn2.AspectToStringConverter = delegate (object x)
            {
                return ((DirectoryInfo)x).FullName;
            };
            //
            fastObjectListView1.SetObjects(GetSweetFXInstalls());
        }

        private void InstallManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.InstallManager_Window_Geometry = WindowGeometry.GeometryToString(this);
        }

        private void InstallManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _open = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SweetFX_Install _install = (SweetFX_Install)fastObjectListView1.SelectedObject;
            if (MessageBox.Show("Are you sure you want to remove " + '"' + _install.Name + '"' + "?", "Remove SweetFX Installation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Directory.Delete(_install.DirectoryInfo.FullName);
            }
        }

        private void fastObjectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fastObjectListView1.SelectedObject != null) { button1.Enabled = true; }
            else { button1.Enabled = false; }
        }

        public static SweetFX_Install[] GetSweetFXInstalls()
        {
            string[] folders = Directory.GetDirectories(Application.StartupPath + @"\SweetFX");
            List<SweetFX_Install> sfx_installs = new List<SweetFX_Install>();
            foreach (string folder in folders) { sfx_installs.Add(new SweetFX_Install(new DirectoryInfo(folder))); }
            return sfx_installs.ToArray();
        }
    }

    public class SweetFX_Install
    {
        public SweetFX_Install(DirectoryInfo _folder)
        {
            this.DirectoryInfo = _folder;
        }

        public string Name { get { return this.DirectoryInfo.Name; } }

        public DirectoryInfo DirectoryInfo { get; set; }
    }
}
