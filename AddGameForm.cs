using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public partial class AddGameForm : Form
    {
        public event GameD GameAdded;
        public event GameEditedD GameEdited;
        private static bool open = false;
        Game last_game = null;

        public AddGameForm()
        {
            open = true;
            InitializeComponent();
        }

        public AddGameForm(Game _game)
        {
            open = true;
            InitializeComponent();
            last_game = _game;
            this.Text = "Edit Game";
            //
            textBox3.Text = last_game.Name;
            textBox1.Text = last_game.Directory.FullName;
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
            if (vistaOpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(vistaOpenFileDialog1.FileName);
                if (String.IsNullOrEmpty(textBox3.Text))
                    textBox3.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(file.Name));
                textBox1.Text = file.Directory.FullName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Game new_game = new Game(textBox3.Text.Trim(), new DirectoryInfo(textBox1.Text.Trim()));
            if (last_game == null)
            {
                Settings.AddGame(new_game);
                GameAdded(new_game);
            }
            else
            {
                GameEdited(last_game, new_game);
            }
            this.Close();
        }

        private void AddGameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                button2_Click(this, null);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox3.Text) && !String.IsNullOrEmpty(textBox1.Text))
            {
                button2.Enabled = true;
            }
            else { button2.Enabled = false; }
        }
    }
}
