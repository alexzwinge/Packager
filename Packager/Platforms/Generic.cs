using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    static class GenericPlatforms
    {
        static public List<string> Rejected = new List<string>();
        static public List<string> Known = new List<string>(new string[] {
            "Nintendo",
            "Nintendo 64",
            "Nintendo DS",
            "Nintendo 3DS",
            "Nintendo Entertainment System",
            "Nintendo Game Boy Advance",
            "Nintendo Game Boy Color",
            "Nintendo Game Boy",
            "Nintendo Wii U",
            "Nintendo Wii",
            "Sega Genesis",
            "Sony Playstation 3",
            "Sony Playstation Vita",
            "Super Nintendo Entertainment System",
            "TurboGrafx-16",
            "WoW Action Max",
        });
    }

    class Generic : Types.Image, IRom
    {
        private bool Continue = true;

        public Generic(IGame game) : base(game)
        {
            if (!GenericPlatforms.Known.Contains(Game.Platform))
            {
                DialogResult do_continue = MessageBox.Show(
                    String.Format("Packager can't be certain {0} is a cart based / single file rom. It's probably a bad idea to package this game\n\nAre you sure you want to continue?", Game.Platform),
                    "", MessageBoxButtons.YesNo);

                if (do_continue == DialogResult.No)
                {
                    Continue = false;
                    GenericPlatforms.Rejected.Add(Game.Platform);
                }
                else
                {
                    GenericPlatforms.Known.Add(Game.Platform);
                }
            }
        }

        public override void Package()
        {
            if (Continue)
            {
                base.Package();
            }
        }
    }
}
