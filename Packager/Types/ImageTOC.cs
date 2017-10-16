using System;
using System.IO;
using System.Text.RegularExpressions;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Types
{
    class ImageTOC : Image
    {
        public ImageTOC(IGame game) : base(game)
        {
            string filename = Path.GetFileNameWithoutExtension(Game.ApplicationPath);
            string discnum = GetDiscNumber(filename);

            NameMeta = Name + CreateDiscMeta(discnum);
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

            TOC toc = new TOC(Game.ApplicationPath);

            foreach (string image in toc.GetImageList())
            {
                if (!ImageHasCorrectFileName(image))
                {
                    string name = RenameImage(image); 
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

                TOC apptoc = new TOC(app.ApplicationPath);

                foreach (string image in apptoc.GetImageList())
                {
                    if (!ImageHasCorrectFileName(image))
                    {
                        string name = RenameImage(image);
                        apptoc.Update(image, name);
                    }
                }

                apptoc.Write();
            }
        }

        protected virtual string RenameImage(string imagename)
        {
            string imagepath = Container.FullName + "\\" + imagename;
            string filename = Path.GetFileNameWithoutExtension(imagepath);
            string discnum = GetDiscNumber(filename);
            string traknum = GetTrackNumber(filename);
            string name = Name + CreateDiscMeta(discnum) + CreateTrackMeta(traknum);
            FileInfo file = new FileInfo(imagepath);

            FSOperations.Move(file, name);

            return file.Name;
        }

        protected virtual bool ImageHasCorrectFileName(string imagename)
        {
            string imagepath = Container.FullName + "\\" + imagename;

            if (!File.Exists(imagepath))
            {
                throw new FileNotFoundException("Could not find " + imagepath + " from TOC for " + Game.Title);
            }

            string filename = Path.GetFileNameWithoutExtension(imagepath);
            string discnum = GetDiscNumber(filename);
            string traknum = GetTrackNumber(filename);
            string expectedName = Name + CreateDiscMeta(discnum) + CreateTrackMeta(traknum);

            if (filename != expectedName)
            {
                return false;
            }

            return true;
        }
    }
}
