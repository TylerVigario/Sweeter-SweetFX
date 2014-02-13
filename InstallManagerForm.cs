using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
        }

        void add_game_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            add_game_form.Dispose();
            _open = false;
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
            // install SweetFX
        }

        private void InstallManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _open = false;
        }
    }
}
