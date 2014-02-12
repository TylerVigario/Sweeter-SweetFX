using System;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public partial class AddGameForm : Form
    {
        private static bool open = false;

        public AddGameForm()
        {
            open = true;
            InitializeComponent();
        }

        public static bool isOpen { get { return open; } }

        private void AddGameForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.AddGame_Window_Geometry, this);
        }

        private void AddGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.AddGame_Window_Geometry = WindowGeometry.GeometryToString(this);
        }

        private void AddGameForm_FormClosed(object sender, FormClosedEventArgs e)
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
                MessageBox.Show("Please add the name of the game");
                return;
            }
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Click Browse to select the game directory");
                return;
            }
            Settings.AddGame(new Game(textBox2.Text.Trim(), textBox1.Text.Trim()));
            // Open Installer
            this.Close();
        }
    }
}
