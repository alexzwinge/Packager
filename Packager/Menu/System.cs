using System.Drawing;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Menu
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
            string msg = "This will rename all roms, vidoes, and folders to match the games title. This operation cannot be undone.";
            DialogResult do_continue = MessageBox.Show(msg + "\n\nAre you sure you want to continue?", "", MessageBoxButtons.YesNo);

            if (do_continue == DialogResult.No) { return; }

            IGame[] games = PluginHelper.DataManager.GetAllGames();

            foreach (IGame game in games)
            {
                Dispatch.Gentrify(game);
            }

            MessageBox.Show("All Done! " + games.Length + " roms have been packaged.");
        }
    }
}
