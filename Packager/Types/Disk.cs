using System.IO;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Types
{
    class Disk : Image
    {
        public Disk(IGame game) : base(game)
        {

        }

        protected override void RenameAppTitle(IAdditionalApplication app)
        {
            string filename = Path.GetFileNameWithoutExtension(app.ApplicationPath);
            string discnum = GetDiscNumber(filename);

            app.Name = "Play Disk " + discnum;
        }

        protected override bool AppHasCorrectTitle(IAdditionalApplication app)
        {
            string filename = Path.GetFileNameWithoutExtension(app.ApplicationPath);
            string discnum = GetDiscNumber(filename);
            string expectedTitle = "Play Disk " + discnum;

            if (app.Name != expectedTitle)
            {
                return false;
            }

            return true;
        }

        protected override string CreateDiscMeta(string discnum)
        {
            return discnum == null ? null : " (Disk " + discnum + ")";
        }
    }
}
