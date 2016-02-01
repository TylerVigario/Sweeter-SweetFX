using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace SweetFX_Configurator
{
    public delegate void GameD(Game _g);
    public delegate void GameEditedD(Game old_game, Game new_game);

    public partial class GameManagerForm : Form
    {
        public static event GameD AddGame;
        public static event GameD RemoveGame;
        public static event GameEditedD GameEdited;
        //
        AddGameForm add_game_form;
        private static bool _open = false;
        private bool no_close;
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
            this.olvColumn2.AspectToStringConverter = delegate (object x)
            {
                SweetFX_Install sfx = (SweetFX_Install)x;
                if (sfx == null) { return "None"; }
                return sfx.Name;
            };
            // Load games
            fastObjectListView1.SetObjects(Settings.GetGames());
            // Load running processes
            Process[] procs = Process.GetProcesses();
            Array.Sort<Process>(procs, delegate (Process proc1, Process proc2) { return proc1.ProcessName.CompareTo(proc2.ProcessName); });
            foreach (Process proc in procs) { if (!comboBox2.Items.Contains(proc.ProcessName)) { comboBox2.Items.Add(proc.ProcessName); } }
            // Load available SweetFX installs
            foreach (SweetFX_Install sfx in InstallManagerForm.GetSweetFXInstalls()) { comboBox1.Items.Add(sfx.Name); };
            comboBox1.SelectedIndex = 0;
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
                add_game_form.GameAdded += Add_game_form_GameAdded;
                add_game_form.Show();
            }
            else { add_game_form.BringToFront(); }
        }

        private void Add_game_form_GameAdded(Game _g)
        {
            fastObjectListView1.AddObject(_g);
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
                SweetFX_Parser.Load(_g);
                no_close = true;
            }
        }

        // Add common files sweep
        private void button4_Click(object sender, EventArgs e)
        {
            Game _game = (Game)fastObjectListView1.SelectedObject;
            if (button4.Text == "Install")
            {
                SweetFX_Install sfx = null;
                foreach (SweetFX_Install _sfx in InstallManagerForm.GetSweetFXInstalls())
                {
                    if (_sfx.Name == comboBox1.SelectedItem.ToString()) { sfx = _sfx; break; }
                }
                if (sfx == null) { return; }
                //
                string[] files = Directory.GetFiles(sfx.Directory.FullName, "*.*", SearchOption.AllDirectories);
                string[] _files = new string[files.Length];
                _files[0] = sfx.Directory.FullName;
                for (int i = 1; i < _files.Length; i++)
                {
                    _files[i] = files[i].Replace(sfx.Directory.FullName, _game.Directory.FullName);
                    string _dir = Path.GetDirectoryName(_files[i]);
                    if (!Directory.Exists(_dir)) { Directory.CreateDirectory(_dir); };
                    File.Copy(files[i], _files[i], true);
                }
                File.WriteAllLines(_game.Directory.FullName + @"\SweetFX_Configurator.txt", _files);
                string[] dirs = Directory.GetDirectories(sfx.Directory.FullName);
                string[] _dirs = new string[dirs.Length];
                for (int i = 0; i < _dirs.Length; i++)
                {
                    _dirs[i] = dirs[i].Replace(sfx.Directory.FullName, _game.Directory.FullName);
                }
                File.AppendAllLines(_game.Directory.FullName + @"\SweetFX_Configurator.txt", _dirs);
                //
                _game.SweetFX_Install = sfx;
                fastObjectListView1.RefreshObject(_game);
                button4.Text = "Uninstall";
                AddGame(_game);
            }
            else
            {
                // Add common files sweep
                if (File.Exists(_game.Directory.FullName + @"\SweetFX_Configurator.txt"))
                {
                    string[] files = File.ReadAllLines(_game.Directory.FullName + @"\SweetFX_Configurator.txt");
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
                    File.Delete(_game.Directory.FullName + @"\SweetFX_Configurator.txt");
                    _game.SweetFX_Install = null;
                    fastObjectListView1.RefreshObject(_game);
                    button4.Text = "Install";
                    RemoveGame(_game);
                }
                else
                {
                    MessageBox.Show("Currently you can only uninstall SweetFX versions that where installed with this configurator. This will changed in the future!");
                }
            }
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
                if (selected.SweetFX_Install != null)
                {
                    button4.Text = "Uninstall";
                    button3.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                    comboBox1.Enabled = false;
                    // Load hotkeys
                    if (File.Exists(selected.SweetFX_Install.Directory.FullName + @"\injector.ini"))
                    {
                        IniFile ini = new IniFile(selected.SweetFX_Install.Directory.FullName + @"\injector.ini");
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
                    comboBox1.Enabled = true;
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

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (Game g in fastObjectListView1.Objects)
            {
                if (g.Name == comboBox2.SelectedItem.ToString())
                {
                    MessageBox.Show("Game already added.");
                    return;
                }
            }
            Process[] procs = Process.GetProcessesByName(comboBox2.SelectedItem.ToString());
            if (procs.Length > 0)
            {
                try
                {
                    Game _g = new Game(procs[0].ProcessName, new DirectoryInfo(Path.GetDirectoryName(procs[0].Modules[0].FileName)));
                    fastObjectListView1.AddObject(_g);
                    Settings.AddGame(_g);
                    if (isSweetFXInstalled(_g.Directory)) AddGame(_g);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading process: " + procs[0].ProcessName + ". Error: " + ex.Message + ".");
                }
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
                IniFile ini = new IniFile(((Game)fastObjectListView1.SelectedObject).SweetFX_Install.Directory.FullName + @"\injector.ini");
                ini.WriteValue("injector", "key_toggle", ((int)key).ToString() + " ; " + ((int)key).ToString() + " = the " + key.ToString() + " key");
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AddGameForm.isOpen)
            {
                add_game_form = new AddGameForm((Game)fastObjectListView1.SelectedObject);
                add_game_form.FormClosed += add_game_form_FormClosed;
                add_game_form.GameEdited += Add_game_form_GameEdited;
                add_game_form.Show();
            }
            else { add_game_form.BringToFront(); }
        }

        private void Add_game_form_GameEdited(Game old_game, Game new_game)
        {
            fastObjectListView1.RemoveObject(old_game);
            fastObjectListView1.AddObject(new_game);
            Settings.EditGame(old_game, new_game);
            GameEdited(old_game, new_game);
        }
    }

    public class Game
    {
        public Game(string _name, DirectoryInfo sweetfx_dir)
        {
            Name = _name;
            this.Directory = sweetfx_dir;
            if (GameManagerForm.isSweetFXInstalled(this.Directory))
            {
                if (File.Exists(this.Directory.FullName + @"\SweetFX_Configurator.txt"))
                    this.SweetFX_Install = new SweetFX_Install(new DirectoryInfo(File.ReadAllLines(this.Directory.FullName + @"\SweetFX_Configurator.txt")[0]));
                else this.SweetFX_Install = new SweetFX_Install("Unknown", new DirectoryInfo(Application.StartupPath + @"\SweetFX\Vanilla"));
            }
        }

        public string Name { get; set; }

        public DirectoryInfo Directory { get; set; }

        public SweetFX_Install SweetFX_Install { get; set; }

        public _SweetFX SweetFX { get; set; }
    }
}
