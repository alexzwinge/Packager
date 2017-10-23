using System.IO;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Types
{
    class ImageGDI : ImageTOC
    {
        public ImageGDI(IGame game) : base(game)
        {

        }

        public override void Package()
        {
            if (!HasCorrectFolderName())
            {
                RenameFolder();
            }

            if (!HasCorrectFileName())
            {
                RenameFile();
            }

            if (!HasCorrectVideoName())
            {
                RenameVideo();
            }

            Game.Version = "";

            string filename = Path.GetFileNameWithoutExtension(Game.ApplicationPath);
            string discnum = GetDiscNumber(filename);
            GDI toc = new GDI(Game.ApplicationPath);

            foreach (string image in toc.GetImageList())
            {
                if (!ImageHasCorrectFileName(image, discnum))
                {
                    string name = RenameImage(image, discnum);
                    toc.Update(image, name);
                }
            }

            toc.Write();

            foreach (IAdditionalApplication app in Game.GetAllAdditionalApplications())
            {
                if (app.UseEmulator && !AppHasCorrectFileName(app))
                {
                    RenameApp(app);
                }

                if (app.UseEmulator && !AppHasCorrectTitle(app))
                {
                    RenameAppTitle(app);
                }

                string appfilename = Path.GetFileNameWithoutExtension(app.ApplicationPath);
                string appdiscnum = GetDiscNumber(appfilename);
                GDI apptoc = new GDI(app.ApplicationPath);

                foreach (string image in apptoc.GetImageList())
                {
                    if (!ImageHasCorrectFileName(image, appdiscnum))
                    {
                        string name = RenameImage(image, appdiscnum);
                        apptoc.Update(image, name);
                    }
                }

                apptoc.Write();
            }
        }
    }
}
