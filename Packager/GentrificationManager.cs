using System;
using System.IO;
using System.Security.Cryptography;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    public class PackagedRom
    {
        private IGame Game;
        private IRom Rom;

        public PackagedRom(IGame game)
        {
            Game = game;

            string ext = Path.GetExtension(Game.ApplicationPath).ToLower();

            if (ext == ".cue" || ext == ".gdi")
            {
                Rom = new RomToc(Game);
            }
            else if (ext == ".zip" || ext == ".chd")
            {
                Rom = new RomMame(Game);
            }
            else
            {
                Rom = new RomCart(Game);
            }
    }

        public void Gentrify()
        {
            if (Rom.Folder != Rom.Name)
            {
                Rom.Folder = Rom.Name;
            }

            if (Rom.File != Rom.Name)
            {
                Rom.File = Rom.Name;
            }

            if (!String.IsNullOrEmpty(Rom.Video) && Rom.Video != Rom.Name)
            {
                Rom.Video = Rom.Name;
            }
        }

        private string Md5HashFile(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "");
                }
            }
        }
    }
}
