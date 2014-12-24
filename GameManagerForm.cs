using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

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
        Keys key_toggle;
        Keys key_screenshot;
        Keys key_reload;

        public GameManagerForm(bool noclose = true)
        {
            _open = true;
            InitializeComponent();
            no_close = noclose;
        }

        public static bool isOpen { get { return _open; } }

        // Save/Load ObjectLiveView state
        private void GameManagerForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.GameManager_Window_Geometry, this);
            this.olvColumn2.AspectToStringConverter = delegate(object x)
            {
                SweetFX_Install sfx = (SweetFX_Install)x;
                if (sfx == null) { return "None"; }
                return sfx.Name;
            };
            // Load games
            Games = Settings.GetGames();
            fastObjectListView1.SetObjects(Games);
            Settings.GameAdded += Settings_GameAdded;
            // Load running processes
            Process[] procs = Process.GetProcesses();
            Array.Sort<Process>(procs, delegate (Process proc1, Process proc2) { return proc1.ProcessName.CompareTo(proc2.ProcessName); });
            foreach (Process proc in procs) { if (!comboBox2.Items.Contains(proc.ProcessName)) { comboBox2.Items.Add(proc.ProcessName); } }
            // Load available SweetFX installs
            foreach (SweetFX_Install sfx in InstallManagerForm.GetSweetFXInstalls()) { comboBox1.Items.Add(sfx.Name); };
            comboBox1.SelectedIndex = 0;
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
            if (_g.SweetFX_Install != null)
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
                SweetFX_Install sfx = null;
                foreach (SweetFX_Install _sfx in InstallManagerForm.GetSweetFXInstalls())
                {
                    if (_sfx.Name == comboBox1.SelectedItem.ToString()) { sfx = _sfx; break; }
                }
                if (sfx == null) { return; }
                InstallSweetFX(game, sfx);
                game.SweetFX_Install = sfx;
                button4.Text = "Uninstall";
                SweetFXInstalled(game);
            }
            else
            {
                UninstallSweetFX(game.DirectoryInfo);
                game.SweetFX_Install = null;
                button4.Text = "Install"; 
                SweetFXUninstalled(game);
            }
            fastObjectListView1.RefreshObject(game);
        }

        private void InstallManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _open = false;
        }

        private void fastObjectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game selected = ((Game)fastObjectListView1.SelectedObject);
            if (selected != null)
            {
                button1.Enabled = true;

                button4.Enabled = true;
                comboBox1.Enabled = true;
                if (selected.SweetFX_Install != null)
                {
                    button4.Text = "Uninstall";
                    button3.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                    // Load hotkeys
                    if (File.Exists(selected.SweetFX_Install.DirectoryInfo.FullName + @"\injector.ini"))
                    {
                        IniFile ini = new IniFile(selected.SweetFX_Install.DirectoryInfo.FullName + @"\injector.ini");
                        string key = ini.GetString("injector", "key_toggle", "145");
                        key = key.Substring(0, key.IndexOf(';')).Trim();
                        key_toggle = (Keys)new KeysConverter().ConvertFromString(key);
                        comboBox3.SelectedIndex = comboBox3.Items.IndexOf(key_toggle.ToString());
                        //
                        key = ini.GetString("injector", "key_screenshot", "44");
                        key = key.Substring(0, key.IndexOf(';')).Trim();
                        key_screenshot = (Keys)new KeysConverter().ConvertFromString(key);
                        comboBox4.SelectedIndex = comboBox4.Items.IndexOf(key_screenshot.ToString());
                        //
                        key = ini.GetString("injector", "key_reload", "19");
                        key = key.Substring(0, key.IndexOf(';')).Trim();
                        key_reload = (Keys)new KeysConverter().ConvertFromString(key);
                        comboBox5.SelectedIndex = comboBox5.Items.IndexOf(key_reload.ToString());
                    }
                }
                else
                {
                    button4.Text = "Install";
                    button3.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                }
            }
            else
            {
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                comboBox1.Enabled = false;
            }
        }

        public static bool isSweetFXInstalled(DirectoryInfo _directory)
        {
            return File.Exists(_directory.FullName + @"\SweetFX_settings.txt");
        }

        private void InstallSweetFX(Game _game, SweetFX_Install sfx)
        {
            string[] files = Directory.GetFiles(sfx.DirectoryInfo.FullName, "*.*", SearchOption.AllDirectories);
            string[] _files = new string[files.Length];
            _files[0] = sfx.DirectoryInfo.FullName;
            for (int i = 1; i < _files.Length; i++)
            {
                _files[i] = files[i].Replace(sfx.DirectoryInfo.FullName, _game.DirectoryInfo.FullName);
                string _dir = Path.GetDirectoryName(_files[i]);
                if (!Directory.Exists(_dir)) { Directory.CreateDirectory(_dir); };
                File.Copy(files[i], _files[i], true);
            }
            File.WriteAllLines(_game.DirectoryInfo.FullName + @"\SweetFX_Configurator.txt", _files);
           string[] dirs = Directory.GetDirectories(sfx.DirectoryInfo.FullName);
            string[] _dirs = new string[dirs.Length];
            for (int i = 0; i < _dirs.Length; i++)
            {
                _dirs[i] = dirs[i].Replace(sfx.DirectoryInfo.FullName, _game.DirectoryInfo.FullName);
            }
            File.AppendAllLines(_game.DirectoryInfo.FullName + @"\SweetFX_Configurator.txt", _dirs);
        }

        // Add common files sweep
        private void UninstallSweetFX(DirectoryInfo dir)
        {
            if (File.Exists(dir.FullName + @"\SweetFX_Configurator.txt"))
            {
                string[] files = File.ReadAllLines(dir.FullName + @"\SweetFX_Configurator.txt");
                for (int i = 1; i < files.Length; i++)
                {
                    if (File.Exists(files[i]))
                    {
                        FileAttributes attr = File.GetAttributes(files[i]);
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            Directory.Delete(files[i], true);
                        else
                            File.Delete(files[i]);
                    }
                }
                File.Delete(dir.FullName + @"\SweetFX_Configurator.txt");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (Game g in fastObjectListView1.Objects) { if (g.Name == comboBox2.SelectedItem.ToString()) return; }
            Process[] procs = Process.GetProcessesByName(comboBox2.SelectedItem.ToString());
            if (procs.Length > 0)
            {
                Game g = new Game(procs[0].ProcessName, new DirectoryInfo(Path.GetDirectoryName(procs[0].Modules[0].FileName)));
                Settings.AddGame(g);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button5.Enabled = !String.IsNullOrEmpty(comboBox2.Text);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Keys key = (Keys)new KeysConverter().ConvertFromString(comboBox3.SelectedItem.ToString());
            if (!key.Equals(key_toggle))
            {
                IniFile ini = new IniFile(((Game)fastObjectListView1.SelectedObject).SweetFX_Install.DirectoryInfo.FullName + @"\injector.ini");
                ini.WriteValue("injector", "key_toggle", ((int)key).ToString() + " ; " + ((int)key).ToString() + " = the " + key.ToString() + " key");
            }
        }
    }

    public class Game
    {
        public Game(string _name, DirectoryInfo _directory_info)
        {
            Name = _name;
            this.DirectoryInfo = _directory_info;
            if (GameManagerForm.isSweetFXInstalled(this.DirectoryInfo))
            {
                if (File.Exists(this.DirectoryInfo.FullName + @"\SweetFX_Configurator.txt"))
                    this.SweetFX_Install = new SweetFX_Install(new DirectoryInfo(File.ReadAllLines(this.DirectoryInfo.FullName + @"\SweetFX_Configurator.txt")[0]));
                else { this.SweetFX_Install = new SweetFX_Install(new DirectoryInfo(Application.StartupPath + @"\SweetFX\Vanilla")); }
            }
        }

        public string Name { get; set; }

        public DirectoryInfo DirectoryInfo { get; set; }

        public SweetFX_Install SweetFX_Install { get; set; }
    }
}
