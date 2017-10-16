using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class Generic : Types.Image, IRom
    {
        private bool Continue = true;
        private List<string> WarnExts = new List<string>();
        private List<string> Exceptions = new List<string>();

        public Generic(IGame game) : base(game)
        {
            string ext = Path.GetExtension(Game.ApplicationPath);

            if (WarnExts.Contains(ext) && !Exceptions.Contains(Game.Platform))
            {
                string msg = "Packager can't be certain " + Game.Title + " is a cart based / single file rom. It's probably a bad idea to package this game";

                DialogResult do_continue = MessageBox.Show(msg + "\n\nAre you sure you want to continue?", "", MessageBoxButtons.YesNo);

                if (do_continue == DialogResult.No)
                {
                    Continue = false;
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
