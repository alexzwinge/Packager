using System;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Types
{
    class Dump : BaseRom
    {
        public Dump(IGame game) : base(game)
        {

        }

        public override void Package()
        {
            if (!HasCorrectFolderName())
            {
                RenameFolder();
            }

            if (!HasCorrectVideoName())
            {
                RenameVideo();
            }

            Game.Version = "";
        }
    }
}
