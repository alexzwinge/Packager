using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    class Rom
    {
        protected readonly DirectoryInfo RomFolder;

        private IGame _Game;
        public IGame Game => _Game;

        private string _Name;
        public string Name => _Name;

        public Rom(IGame game)
        {
            _Game = game;
            _Name = FsSafeName(Game.Title);

            try
            {
                RomFolder = new FileInfo(Game.ApplicationPath).Directory;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error opening for folder for rom " + Game.Title);
                throw e;
            }
        }

        public string Folder
        {
            get { return RomFolder.Name; }

            set
            {
                string oldName = RomFolder.FullName + "\\";

                Move(RomFolder, value);

                Game.ApplicationPath = Game.ApplicationPath.Replace(oldName, RomFolder.FullName);
                Game.ManualPath = Game.ManualPath.Replace(oldName, RomFolder.FullName);
                Game.MusicPath = Game.MusicPath.Replace(oldName, RomFolder.FullName);
                Game.VideoPath = Game.VideoPath.Replace(oldName, RomFolder.FullName);

                foreach (IAdditionalApplication app in Game.GetAllAdditionalApplications())
                {
                    app.ApplicationPath = app.ApplicationPath.Replace(oldName, RomFolder.FullName);
                }
            }
        }

        public string Video
        {
            get
            {
                string video = Path.GetFileNameWithoutExtension(Game.GetVideoPath());

                if (String.IsNullOrEmpty(video)) {
                    return "";
                }

                if (!File.Exists(Game.GetVideoPath()))
                {
                    Game.VideoPath = "";
                    return this.Video;
                }


                if (video.IndexOf(" (Video Snap)") == -1) {
                    return "WRONGFORMAT";
                }

                return video.Replace(" (Video Snap)", "");
            }

            set
            {
                FileInfo video = new FileInfo(Game.GetVideoPath());

                Move(video, value + " (Video Snap)");
                Game.VideoPath = video.FullName;
            }
        }

        private string FsSafeName(string name)
        {
            return name.
                Replace(": ", " - ")
                .Replace(":", "-")
                .Replace("/", "-")
                .Replace("\\", "-")
                .Replace("?", "");
        }

        private void Move(DirectoryInfo fsObj, string target)
        {
            try
            {
                fsObj.MoveTo(RomFolder.Parent.FullName + "\\tempmove");
                fsObj.MoveTo(RomFolder.Parent.FullName + "\\" + target);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error renaming folder from\n\n "
                    + fsObj.FullName
                    + "\n\nto\n\n"
                    + RomFolder.Parent.FullName + "\\" + target
                );
                throw e;
            }

        }

        private void Move(FileInfo fsObj, string target)
        {
            try
            {
                fsObj.MoveTo(RomFolder.FullName + "\\tempmove" + fsObj.Extension);
                fsObj.MoveTo(RomFolder.FullName + "\\" + target + fsObj.Extension.ToLower());
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error renaming file from\n\n "
                    + fsObj.FullName
                    + "\n\nto\n\n"
                    + RomFolder.FullName + "\\" + target + fsObj.Extension.ToLower()
                );
                throw e;
            }
        }

        protected string ExtractFileName(string filename)
        {
            return Regex.Replace(filename, @"\((Disc|Track) \d+\)", "").Trim();
        }

        protected string ExtractDiscNumber(string filename)
        {
            string discnum = Regex.Match(filename, @"\(Disc (\d+)\)").Groups[1].Value;

            if (String.IsNullOrEmpty(discnum))
            {
                return null;
            }

            return int.Parse(discnum).ToString("D2");
        }

        protected string ExtracTrackNumber(string filename)
        {
            string tracknum = Regex.Match(filename, @"\(Track (\d+)\)").Groups[1].Value;

            if (String.IsNullOrEmpty(tracknum))
            {
                return null;
            }

            return int.Parse(tracknum).ToString("D2");
        }

        protected string ExtractMeta(string filename)
        {
            List<string> meta = new List<string>();
            string discnum = ExtractDiscNumber(filename);
            string traknum = ExtracTrackNumber(filename);

            if (!String.IsNullOrEmpty(discnum))
            {
                meta.Add("(Disc " + discnum + ")");
            }

            if (!String.IsNullOrEmpty(traknum))
            {
                meta.Add("(Track " + traknum + ")");
            }

            return String.Join(" ", meta);
        }

        protected FileInfo RenameRom(string romfile, string targetname)
        {
            FileInfo rom = new FileInfo(romfile);

            string filename = Path.GetFileNameWithoutExtension(romfile);
            string name = ExtractFileName(filename);
            string meta = ExtractMeta(filename);

            if (name != targetname)
            {
                Move(rom, (targetname + " " + meta).Trim());
            }

            return rom;
        }

        protected void SetGamePath(IGame game, FileInfo rom)
        {
            game.ApplicationPath = rom.FullName;
        }

        protected void SetGamePath(IAdditionalApplication game, FileInfo disc)
        {
            game.ApplicationPath = disc.FullName;
            game.Name = "Play Disc " + ExtractDiscNumber(disc.Name);
        }

        protected string SetGamePath(string toc, string oldval, string newval)
        {
            return toc.Replace(oldval, newval);
        }
    }
}
