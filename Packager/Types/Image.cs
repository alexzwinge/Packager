using System;
using System.IO;
using System.Text.RegularExpressions;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Types
{
    class Image : BaseRom
    {
        public Image(IGame game) : base(game)
        {
            string filename = Path.GetFileNameWithoutExtension(Game.ApplicationPath);
            string discnum = GetDiscNumber(filename);
            string traknum = GetTrackNumber(filename);

            NameMeta = Name + CreateDiscMeta(discnum) + CreateTrackMeta(traknum);
        }

        public override void Package()
        {
            base.Package();

            foreach(IAdditionalApplication app in Game.GetAllAdditionalApplications())
            {
                if (app.UseEmulator && !AppHasCorrectFileName(app))
                {
                    RenameApp(app);
                }

                if (app.UseEmulator && !AppHasCorrectTitle(app))
                {
                    RenameAppTitle(app);
                }
            }
        }

        protected virtual void RenameApp(IAdditionalApplication app)
        {
            FileInfo appfile = new FileInfo(app.ApplicationPath);
            string filename = Path.GetFileNameWithoutExtension(app.ApplicationPath);
            string discnum = GetDiscNumber(filename);
            string traknum = GetTrackNumber(filename);
            string newname = Name + CreateDiscMeta(discnum) + CreateTrackMeta(traknum);

            FSOperations.Move(appfile, Container.FullName, newname);

            app.ApplicationPath = appfile.FullName;
        }

        protected virtual void RenameAppTitle(IAdditionalApplication app)
        {
            string filename = Path.GetFileNameWithoutExtension(app.ApplicationPath);
            string discnum = GetDiscNumber(filename);

            app.Name = "Play Disc " + discnum;
        }

        protected virtual bool AppHasCorrectFileName(IAdditionalApplication app)
        {
            if (!File.Exists(app.ApplicationPath))
            {
                throw new FileNotFoundException("Additional app (" + app.Name + ") for game " + Game.Title + " not found");
            }

            string filename = Path.GetFileNameWithoutExtension(app.ApplicationPath);
            string discnum = GetDiscNumber(filename);
            string traknum = GetTrackNumber(filename);
            string expectedName = Name + CreateDiscMeta(discnum) + CreateTrackMeta(traknum);

            if (Path.GetFileNameWithoutExtension(app.ApplicationPath) != expectedName)
            {
                return false;
            }

            return true;
        }

        protected virtual bool AppHasCorrectTitle(IAdditionalApplication app)
        {
            string filename = Path.GetFileNameWithoutExtension(app.ApplicationPath);
            string discnum = GetDiscNumber(filename);
            string expectedTitle = "Play Disc " + discnum;

            if (app.Name != expectedTitle)
            {
                return false;
            }

            return true;
        }

        protected virtual string GetDiscNumber(string filename)
        {
            string discnum = Regex.Match(filename, @"Dis[ck][\s_]*(\d+)", RegexOptions.IgnoreCase).Groups[1].Value;

            if (String.IsNullOrEmpty(discnum))
            {
                return null;
            }

            return int.Parse(discnum).ToString("D2");
        }

        protected virtual string CreateDiscMeta(string discnum)
        {
            return discnum == null ? null : " (Disc " + discnum + ")";
        }

        protected virtual string GetTrackNumber(string filename)
        {
            string traknum = Regex.Match(filename, @"Track[\s_]*(\d+)", RegexOptions.IgnoreCase).Groups[1].Value;

            if (String.IsNullOrEmpty(traknum))
            {
                return null;
            }

            return int.Parse(traknum).ToString("D2");
        }

        protected virtual string CreateTrackMeta(string tracknum)
        {
            return tracknum == null ? null : " (Track " + tracknum + ")";
        }
    }
}
