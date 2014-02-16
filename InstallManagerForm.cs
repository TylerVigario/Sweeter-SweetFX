using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace SweetFX_Configurator
{
    public partial class InstallManagerForm : Form
    {
        AddGameForm add_game_form;
        private static bool _open = false;
        private List<Game> Games;

        public InstallManagerForm()
        {
            _open = true;
            InitializeComponent();
        }

        public static bool isOpen { get { return _open; } }

        private void InstallManagerForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.InstallManager_Window_Geometry, this);
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
            Settings.InstallManager_Window_Geometry = WindowGeometry.GeometryToString(this);
            _open = false;
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
            SweetFX.Load((Game)fastObjectListView1.SelectedObject);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Game game = (Game)fastObjectListView1.SelectedObject;
            if (button4.Text == "Install")
            {
                InstallSweetFX(game.Directory);
                game.isSweetFXInstalled = true;
                button4.Text = "Uninstall";
            }
            else
            {
                UninstallSweetFX(game.Directory);
                game.isSweetFXInstalled = false;
                button4.Text = "Install";
            }
            fastObjectListView1.RefreshObject(game);
        }

        private void InstallSweetFX(string dir)
        {
            string[] files = Directory.GetFiles(Application.StartupPath + @"\SweetFX", "*.*", SearchOption.AllDirectories);
            string[] _files = new string[files.Length];
            for (int i = 0; i < _files.Length; i++)
            {
                _files[i] = files[i].Replace(Application.StartupPath + @"\SweetFX", dir);
                string _dir = Path.GetDirectoryName(_files[i]);
                if (!Directory.Exists(_dir)) { Directory.CreateDirectory(_dir); };
                File.Copy(files[i], _files[i], true);
            }
            File.WriteAllLines(dir + @"\SweetFX_Uninstall.txt", _files);
            string[] dirs = Directory.GetDirectories(Application.StartupPath + @"\SweetFX");
            string[] _dirs = new string[dirs.Length];
            for (int i = 0; i < _dirs.Length; i++)
            {
                _dirs[i] = dirs[i].Replace(Application.StartupPath + @"\SweetFX", dir);
            }
            File.AppendAllLines(dir + @"\SweetFX_Uninstall.txt", _dirs);
        }

        private void UninstallSweetFX(string dir)
        {
            string[] files = File.ReadAllLines(dir + @"\SweetFX_Uninstall.txt");
            foreach (string file in files)
            {
                FileAttributes attr = File.GetAttributes(file);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    Directory.Delete(file, true);
                else
                    File.Delete(file);
            }
            File.Delete(dir + @"\SweetFX_Uninstall.txt");
        }

        private void InstallManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _open = false;
        }

        public static bool isSweetFXInstalled(string _directory)
        {
            if (System.IO.File.Exists(_directory + @"\SweetFX_settings.txt")) { return true; }
            else { return false; }
        }

        private void fastObjectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game selected = ((Game)fastObjectListView1.SelectedObject);
            if (selected != null) { button1.Enabled = true; button3.Enabled = true; }
            else { button1.Enabled = false; button3.Enabled = false; button4.Text = "Install"; button4.Enabled = false; return; }
            if (!selected.isSweetFXInstalled) { button4.Text = "Install"; button4.Enabled = true; }
            else { button4.Text = "Uninstall"; button4.Enabled = true; }
        }
    }

    public class Game
    {
        string _name;
        string _directory;
        bool _sweetfx_installed;

        public Game(string n, string d)
        {
            _name = n;
            _directory = d;
            _sweetfx_installed = InstallManagerForm.isSweetFXInstalled(d);
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

        public bool isSweetFXInstalled
        {
            get { return _sweetfx_installed; }
            set { _sweetfx_installed = value; }
        }
    }
}
