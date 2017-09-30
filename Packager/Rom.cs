using System;
using System.IO;
using System.Text.RegularExpressions;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    class Rom
    {
        protected readonly IGame Game;
        protected readonly DirectoryInfo RomFolder;

        public string Name;

        public Rom(IGame game)
        {
            Game = game;
            Name = FsSafeName(Game.Title);

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
                string video;

                try
                {
                    video = Path.GetFileNameWithoutExtension(Game.GetVideoPath());
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Error opening video for rom " + Game.Title);
                    throw e;
                }

                if (String.IsNullOrEmpty(video)) {
                    return "";
                }

                if (video.IndexOf(" (Video Snap)") == -1) {
                    return "WRONGFORMAT";
                }

                return video.Replace(" (Video Snap)", "");
            }

            set
            {
                 

                FileInfo video;

                try
                {
                    video = new FileInfo(Game.GetVideoPath());
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Error opening video for rom " + Game.Title);
                    throw e;
                }

                if (video == null)
                {
                    Game.VideoPath = "";
                    return;
                }


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
            return Regex.Replace(filename, @"\(.*\)", "").Trim();
        }

        protected string ExtractDiscNumber(string filename)
        {
            int discnum = int.Parse(Regex.Match(filename, @"\(Disc (\d+)\)").Groups[1].Value);
            return discnum.ToString("D2");
        }

        protected string ExtractMeta(string filename)
        {
            string name = ExtractFileName(filename);
            return Regex.Replace(filename, "^" + name, "").Trim();
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
