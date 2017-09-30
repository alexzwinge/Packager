using System;
using System.Drawing;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    class ContextMenuItem : IGameMenuItemPlugin
    {
        public bool SupportsMultipleGames => true;

        public string Caption => "Package Rom";

        public Image IconImage => null;

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => false;

        public bool GetIsValidForGame(IGame selectedGame)
        {
            return true;
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            return true;
        }

        public void OnSelected(IGame selectedGame)
        {

            var do_cotinue = MessageBox.Show(String.Format("Package {0}?", selectedGame.Title),
                "", MessageBoxButtons.YesNo);

            if (do_cotinue == DialogResult.No) { return; }

            var package = new PackagedRom(selectedGame);
            package.Gentrify();

            MessageBox.Show("All Done!");
        }

        public void OnSelected(IGame[] selectedGames)
        {

            var do_cotinue = MessageBox.Show(String.Format("Package {0} roms?", selectedGames.Length),
                "", MessageBoxButtons.YesNo);

            if (do_cotinue == DialogResult.No) { return; }

            foreach (var game in selectedGames)
            {
                var package = new PackagedRom(game);
                package.Gentrify();
            }

            MessageBox.Show(String.Format("All Done! {0} roms have been packaged.", selectedGames.Length));
        }
    }
}
