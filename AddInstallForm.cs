using System;
using System.Windows.Forms;
using System.IO;

namespace SweetFX_Configurator
{
    public delegate void InstallReturnD(Install i);

    public partial class AddInstallForm : Form
    {
        private static bool open = false;
        public event InstallReturnD InstallReturn;

        public AddInstallForm()
        {
            open = true;
            InitializeComponent();
        }

        public static bool isOpen { get { return open; } }

        private void AddInstallForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.AddInstall_Window_Geometry, this);
        }

        private void AddInstallForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.AddInstall_Window_Geometry = WindowGeometry.GeometryToString(this);
        }

        private void AddInstallForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (vistaFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = vistaFolderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please name the SweetFX package");
                return;
            }
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Click Browse to select the SweetFX directory");
                return;
            }
            string[] paths = textBox1.Text.Split('\\');
            string path = paths[paths.Length - 1];
            Directory.Move(textBox1.Text, Application.StartupPath + @"\SweetFX\" + path);
            InstallReturn(new Install(path, Application.StartupPath + @"\SweetFX\" + path));
            this.Close();
        }

        private void AddInstallForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                button2_Click(this, null);
            }
        }
    }
}
