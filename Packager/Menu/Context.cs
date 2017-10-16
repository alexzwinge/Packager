using System;
using System.Drawing;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Menu
{
    class Context : IGameMenuItemPlugin
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

            DialogResult do_continue = MessageBox.Show("Package " + selectedGame.Title + "? ", "", MessageBoxButtons.YesNo);

            if (do_continue == DialogResult.No) { return; }

            Dispatch.Gentrify(selectedGame);

            MessageBox.Show("All Done!");
        }

        public void OnSelected(IGame[] selectedGames)
        {

            DialogResult do_continue = MessageBox.Show("Package " + selectedGames.Length + " roms ?", "", MessageBoxButtons.YesNo);

            if (do_continue == DialogResult.No) { return; }

            foreach (IGame game in selectedGames)
            {
                Dispatch.Gentrify(game);
            }

            MessageBox.Show("All Done! " + selectedGames.Length + " roms have been packaged.");
        }
    }
}
