using System.IO;

namespace SweetFX_Configurator
{
    public delegate void GameLoadedD();

    public static class InstallManager
    {
        public static event GameLoadedD GameLoaded;

        public static void LoadGame(string _game)
        {
            SweetFX.Load(Settings.GetGame(_game));
            GameLoaded();
        }

        public static void LoadGame(Game _game)
        {
            SweetFX.Load(_game);
            GameLoaded();
        }

        public static bool isSweetFXInstalled(string _directory)
        {
            if (File.Exists(_directory + @"\SweetFX_settings.txt")) { return true; }
            else { return false; }
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
            _sweetfx_installed = InstallManager.isSweetFXInstalled(d);
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
        }

        public bool RescanForSweetFX()
        {
            _sweetfx_installed = InstallManager.isSweetFXInstalled(_directory);
            return _sweetfx_installed;
        }
    }
}
