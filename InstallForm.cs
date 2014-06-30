using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SweetFX_Configurator
{
    public partial class InstallForm : Form
    {
        public InstallForm()
        {
            InitializeComponent();
        }

        private void InstallSweetFX(string sweetfx, string dir)
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

        public static void UninstallSweetFX(string dir)
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
    }
}
