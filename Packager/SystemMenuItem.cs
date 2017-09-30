using System;
using System.Drawing;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;

namespace Packager
{
    class SystemMenuItem : ISystemMenuItemPlugin
    {
        public string Caption => "Package Roms";

        public Image IconImage => null;

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => false;

        public bool AllowInBigBoxWhenLocked => false;

        public void OnSelected()
        {

            var do_cotinue = MessageBox.Show("This will pacakge all rom images and vidoes into the owning rom's directory. This can be be intesive depending on the number of roms." +
                "\n\nAre you sure you want to continue?",
                "", MessageBoxButtons.YesNo);

            if (do_cotinue == DialogResult.No) { return; }

            var games = PluginHelper.DataManager.GetAllGames();

            foreach (var game in games)
            {
                var package = new PackagedRom(game);
                package.Gentrify();
            }

            MessageBox.Show(String.Format("All Done! {0} roms have been packaged.", games.Length));
        }
    }
}
