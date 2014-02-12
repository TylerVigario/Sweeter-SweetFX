using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SweetFX_Configurator
{
    public delegate void GameLoadedD();

    public static class InstallManager
    {
        public static event GameLoadedD GameLoaded;

        public static void LoadGame(Game _game)
        {
            SweetFX.Load(_game);
            GameLoaded();
        }
    }

    public class Game
    {
        string _name;
        string _directory;

        public Game(string n, string d)
        {
            _name = n;
            _directory = d;
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
    }
}
