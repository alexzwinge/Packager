using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    class Rom
    {




        protected string ExtractFileName(string filename)
        {
            return Regex.Replace(filename, @"\(.*\)", "").Trim();
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
