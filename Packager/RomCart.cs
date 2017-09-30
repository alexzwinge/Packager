using System;
using System.IO;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    class RomCart : Rom, IRom
    {
        public RomCart(IGame game) : base(game)
        {

        }

        public string File
        {
            get { return Path.GetFileNameWithoutExtension(Game.ApplicationPath); }

            set
            {
                string initialPath = Game.ApplicationPath;

                FileInfo rom = RenameRom(Game.ApplicationPath, value);
                SetGamePath(Game, rom);

                foreach (IAdditionalApplication app in Game.GetAllAdditionalApplications())
                {
                    if (app.ApplicationPath == initialPath)
                    {
                        app.ApplicationPath = rom.FullName;
                    }

                    FileInfo approm = RenameRom(app.ApplicationPath, value);
                    SetGamePath(app, approm);
                }
            }
        }
    }
}
