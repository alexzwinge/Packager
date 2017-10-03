using System;
using System.IO;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    class RomMame : Rom, IRom
    {
        public RomMame(IGame game) : base(game)
        {

        }

        public string File
        {
            get { return Path.GetFileNameWithoutExtension(Game.ApplicationPath); }

            set
            {
                return;
            }
        }

    }
}
