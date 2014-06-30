using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public partial class InstallManagerForm : Form
    {
        private static bool _open = false;
        private List<Install> Installs;
        private AddInstallForm add_install_form;

        public InstallManagerForm()
        {
            _open = true;
            InitializeComponent();
        }

        public static bool isOpen { get { return _open; } }

        private void InstallManagerForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.InstallManager_Window_Geometry, this);
            Installs = new List<Install>();
            string[] folders = Directory.GetDirectories(Application.StartupPath + @"\SweetFX");
            foreach (string folder in folders)
            {
                string[] paths = folder.Split('\\');
                Installs.Add(new Install(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(paths[paths.Length - 1].Replace("_", " ")), folder));
            }
            fastObjectListView1.SetObjects(Installs);
        }

        private void InstallManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.InstallManager_Window_Geometry = WindowGeometry.GeometryToString(this);
        }

        private void InstallManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _open = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!AddInstallForm.isOpen)
            {
                add_install_form = new AddInstallForm();
                add_install_form.FormClosed += add_install_form_FormClosed;
                add_install_form.InstallReturn += add_install_form_InstallReturn;
                add_install_form.Show();
            }
            else { add_install_form.BringToFront(); }
        }

        void add_install_form_InstallReturn(Install i)
        {
            Installs.Add(i);
            fastObjectListView1.AddObject(i);
        }

        void add_install_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            add_install_form.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Install _install = (Install)fastObjectListView1.SelectedObject;
            if (MessageBox.Show("Are you sure you want to remove " + '"' + _install.Name + '"' + "?", "Remove SweetFX Installation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Directory.Delete(_install.Directory);
            }
        }

        private void fastObjectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fastObjectListView1.SelectedObject != null) { button1.Enabled = true; }
            else { button1.Enabled = false; }
        }
    }

    public class Install
    {
        private string _name;
        private string _directory;

        public Install(string n, string d)
        {
            _name = n;
            _directory = d;
        }

        public string Name { get { return _name; } set { _name = value; } }

        public string Directory { get { return _directory; } set { _directory = value; } }
    }
}
