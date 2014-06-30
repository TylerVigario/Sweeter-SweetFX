using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace SweetFX_Configurator
{
    public partial class GameManagerForm : Form
    {
        AddGameForm add_game_form;
        private static bool _open = false;
        private List<Game> Games;
        private bool no_close;
        public event GameAddedD SweetFXInstalled;
        public event GameAddedD SweetFXUninstalled;

        public GameManagerForm(bool noclose = true)
        {
            _open = true;
            InitializeComponent();
            no_close = noclose;
        }

        public static bool isOpen { get { return _open; } }

        private void GameManagerForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.GameManager_Window_Geometry, this);
            Games = Settings.GetGames();
            fastObjectListView1.SetObjects(Games);
            Settings.GameAdded += Settings_GameAdded;
        }

        void Settings_GameAdded(Game _g)
        {
            Games.Add(_g);
            fastObjectListView1.AddObject(_g);
        }

        private void InstallManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.GameManager_Window_Geometry = WindowGeometry.GeometryToString(this);
            _open = false;
            if (!no_close) { Environment.Exit(0); }
        }

        void add_game_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            add_game_form.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!AddGameForm.isOpen)
            {
                add_game_form = new AddGameForm();
                add_game_form.FormClosed += add_game_form_FormClosed;
                add_game_form.Show();
            }
            else { add_game_form.BringToFront(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings.DeleteGame((Game)fastObjectListView1.SelectedObject);
            fastObjectListView1.RemoveObject(fastObjectListView1.SelectedObject);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Game _g = (Game)fastObjectListView1.SelectedObject;
            if (_g.SweetFX > 0)
            {
                SweetFX.Load(_g);
                no_close = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Game game = (Game)fastObjectListView1.SelectedObject;
            if (button4.Text == "Install")
            {
                InstallSweetFX(game.Directory);
                game.SweetFX = 1;
                button4.Text = "Uninstall";
                SweetFXInstalled(game);
            }
            else
            {
                InstallForm.UninstallSweetFX(game.Directory);
                game.SweetFX = 0;
                button4.Text = "Install"; 
                SweetFXUninstalled(game);
            }
            fastObjectListView1.RefreshObject(game);
        }


        private void InstallManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _open = false;
        }

        public static int isSweetFXInstalled(string _directory)
        {
            if (System.IO.File.Exists(_directory + @"\SweetFX_settings.txt")) { return 1; }
            else { return 0; }
        }

        private void fastObjectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game selected = ((Game)fastObjectListView1.SelectedObject);
            if (selected != null)
            {
                button1.Enabled = true;
                if (selected.SweetFX > 0)
                {
                    button4.Text = "Install";
                    button4.Enabled = true;
                    button3.Enabled = false;
                }
                else
                {
                    button3.Enabled = true;
                    button4.Text = "Uninstall";
                    button4.Enabled = (File.Exists(selected.Directory + @"\SweetFX_Uninstall.txt"));
                }
            }
            else
            {
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Text = "Install";
                button4.Enabled = false;
                return;
            }
        }
    }

    public class Game
    {
        string _name;
        string _directory;
        int _sweetfx;

        public Game(string n, string d)
        {
            _name = n;
            _directory = d;
            _sweetfx = GameManagerForm.isSweetFXInstalled(d);
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        public int SweetFX
        {
            get { return _sweetfx; }
            set { _sweetfx = value; }
        }
    }
}
