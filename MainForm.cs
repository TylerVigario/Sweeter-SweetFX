using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SweetFX_Configurator
{
    public partial class MainForm : Form
    {
        private List<Message> MessagePump = new List<Message>();
        private delegate void SweetFX_SaveSettingsFinishedD();
        InstallManagerForm install_manager_form;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Settings.Load();
            WindowGeometry.GeometryFromString(Settings.Main_Window_Geometry, this);
            //
            SweetFX.SaveSettingsFinished += SweetFX_SaveSettingsFinished;
            InstallManager.GameLoaded += InstallManager_GameLoaded;
            if (Settings.LastGame.isSweetFXInstalled)
            {
                InstallManager.LoadGame(Settings.LastGame);
            }
            //
            List<Game> _gms = Settings.GetGames();
            foreach (Game _game in _gms)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = _game.Name;
                item.Click += item_Click;
                gamesToolStripMenuItem1.DropDownItems.Add(item);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            InstallManager.LoadGame(((ToolStripMenuItem)sender).Text);
        }

        private delegate void InstallManager_GameLoadedD();

        void InstallManager_GameLoaded()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InstallManager_GameLoadedD(InstallManager_GameLoaded));
                return;
            }
            LoadSFXConfig();
        }

        private void LoadSFXConfig()
        {
            StopFormCapture();
            // SMAA
            checkBox1.Checked = SweetFX.SMAA.Enabled;
            numericUpDown1.Value = SweetFX.SMAA.Threshold;
            trackBar1.Value = Convert.ToInt32(SweetFX.SMAA.Threshold * (decimal)100.00);
            numericUpDown2.Value = SweetFX.SMAA.Max_Search_Steps;
            trackBar2.Value = SweetFX.SMAA.Max_Search_Steps;
            numericUpDown3.Value = SweetFX.SMAA.Max_Search_Steps_Diag;
            trackBar3.Value = SweetFX.SMAA.Max_Search_Steps_Diag;
            numericUpDown4.Value = SweetFX.SMAA.Corner_Rounding;
            trackBar4.Value = SweetFX.SMAA.Corner_Rounding;
            checkBox2.Checked = SweetFX.SMAA.Color_Edge_Detection;
            checkBox3.Checked = SweetFX.SMAA.DirectX9_Linear_Blend;
            //
            StartFormCapture();
        }

        void SweetFX_SaveSettingsFinished()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SweetFX_SaveSettingsFinishedD(SweetFX_SaveSettingsFinished));
                return;
            }
            SetMessage("SweetFX configuration saved");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Main_Window_Geometry = WindowGeometry.GeometryToString(this);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SweetFX.Dispose();
            Environment.Exit(0);
        }

        private void StartFormCapture()
        {
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_ValueChanged);
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_ValueChanged);
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            this.trackBar4.Scroll += new System.EventHandler(this.trackBar4_ValueChanged);
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
        }

        private void StopFormCapture()
        {
            this.checkBox1.CheckedChanged -= new System.EventHandler(this.checkBox1_CheckedChanged);
            this.numericUpDown1.ValueChanged -= new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.trackBar1.Scroll -= new System.EventHandler(this.trackBar1_ValueChanged);
            this.numericUpDown2.ValueChanged -= new System.EventHandler(this.numericUpDown2_ValueChanged);
            this.trackBar2.Scroll -= new System.EventHandler(this.trackBar2_ValueChanged);
            this.numericUpDown3.ValueChanged -= new System.EventHandler(this.numericUpDown3_ValueChanged);
            this.trackBar3.Scroll -= new System.EventHandler(this.trackBar3_ValueChanged);
            this.numericUpDown4.ValueChanged -= new System.EventHandler(this.numericUpDown4_ValueChanged);
            this.trackBar4.Scroll -= new System.EventHandler(this.trackBar4_ValueChanged);
            this.checkBox2.CheckedChanged -= new System.EventHandler(this.checkBox2_CheckedChanged);
            this.checkBox3.CheckedChanged -= new System.EventHandler(this.checkBox3_CheckedChanged);
        }

        private void SetMessage(string _message)
        {
            SetMessage(new Message(_message));
        }

        private void SetMessage(Message _message)
        {
            if (!timer1.Enabled)
            {
                toolStripStatusLabel1.Text = _message.ToString();
                timer1.Interval = _message.Timeout;
                timer1.Enabled = true;
                timer1.Start();
            }
            else { MessagePump.Add(_message); }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MessagePump.Count > 0)
            {
                toolStripStatusLabel1.Text = MessagePump[0].ToString();
                timer1.Interval = MessagePump[0].Timeout;
                MessagePump.Remove(MessagePump[0]);
            }
            else
            {
                toolStripStatusLabel1.Text = "";
                timer1.Enabled = false;
                timer1.Stop();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!InstallManagerForm.isOpen)
            {
                install_manager_form = new InstallManagerForm();
                install_manager_form.FormClosed += install_manager_form_FormClosed;
                install_manager_form.Show();
            }
            else { install_manager_form.BringToFront(); }
        }

        void install_manager_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            install_manager_form.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //about
        }

        #region SMAA

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.SMAA.Color_Edge_Detection = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.SMAA.DirectX9_Linear_Blend = checkBox3.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SweetFX.SMAA.Enabled = checkBox1.Checked;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown1.ValueChanged -= new System.EventHandler(this.numericUpDown1_ValueChanged);
            numericUpDown1.Value = (decimal)trackBar1.Value / (decimal)100.00;
            SweetFX.SMAA.Threshold = numericUpDown1.Value;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar1.Scroll -= new System.EventHandler(this.trackBar1_ValueChanged);
            SweetFX.SMAA.Threshold = numericUpDown1.Value * (decimal)100.00;
            trackBar1.Value = Convert.ToInt32(SweetFX.SMAA.Threshold);
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_ValueChanged);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown2.ValueChanged -= new System.EventHandler(this.numericUpDown2_ValueChanged);
            numericUpDown2.Value = trackBar2.Value;
            SweetFX.SMAA.Max_Search_Steps = trackBar2.Value;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
            trackBar2.Value = Convert.ToInt32(numericUpDown2.Value);
            SweetFX.SMAA.Max_Search_Steps = trackBar2.Value;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown3.ValueChanged -= new System.EventHandler(this.numericUpDown3_ValueChanged);
            numericUpDown3.Value = trackBar3.Value;
            SweetFX.SMAA.Max_Search_Steps_Diag = trackBar3.Value;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar3.Scroll -= new System.EventHandler(this.trackBar3_ValueChanged);
            trackBar3.Value = Convert.ToInt32(numericUpDown3.Value);
            SweetFX.SMAA.Max_Search_Steps_Diag = trackBar3.Value;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_ValueChanged);
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown4.ValueChanged -= new System.EventHandler(this.numericUpDown4_ValueChanged);
            numericUpDown4.Value = trackBar4.Value;
            SweetFX.SMAA.Corner_Rounding = trackBar4.Value;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            this.trackBar4.Scroll -= new System.EventHandler(this.trackBar4_ValueChanged);
            trackBar4.Value = Convert.ToInt32(numericUpDown4.Value);
            SweetFX.SMAA.Corner_Rounding = trackBar4.Value;
            this.trackBar4.Scroll += new System.EventHandler(this.trackBar4_ValueChanged);
        }

        #endregion
    }

    public class Message
    {
        private string _action;
        private string _text;
        private int _timeout;

        public Message(string tex)
        {
            _text = tex;
            _action = "";
            _timeout = 5;
        }

        public Message(string tex, string a)
        {
            _text = tex;
            _action = a;
            _timeout = 5;
        }

        public Message(string tex, int t)
        {
            _text = tex;
            _action = "";
            _timeout = t;
        }

        public Message(string tex, string a, int t)
        {
            _text = tex;
            _action = a;
            _timeout = t;
        }

        public string Action { get { return _action; } }

        public string Text { get { return _text; } }

        public int Timeout { get { return _timeout * 1000; } }

        public override string ToString()
        {
            string _final = "";
            if (!String.IsNullOrEmpty(_action)) { _final = _action + ": "; }
            return _final + _text;
        }
    }
}
