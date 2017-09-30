﻿using System;
using System.IO;
using System.Security.Cryptography;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    public class PackagedRom
    {
        private IGame Game;
        private RomToc rom;

        public PackagedRom(IGame game)
        {
            Game = game;
            rom = new RomToc(Game);
        }

        public void Gentrify()
        {

            if (!IsValidPlatform(Game.Platform))
            {
                return;
            }

            if (rom.Folder != rom.Name)
            {
                rom.Folder = rom.Name;
            }

            if (rom.File != rom.Name)
            {
                rom.File = rom.Name;
            }

            if (!String.IsNullOrEmpty(rom.Video) && rom.Video != rom.Name)
            {
                rom.Video = rom.Name;
            }
        }

        private bool IsValidPlatform(string platform)
        {
            // if (platform == "Arcade") { return true; }
            // if (platform == "SNK Neo Geo") { return true; }

            if (platform == "Nintendo Entertainment System") { return true; }
            if (platform == "Super Nintendo Entertainment System") { return true; }
            if (platform == "Nintendo 64") { return true; }
            if (platform == "Nintendo GameCube") { return true; }
            if (platform == "Nintendo Wii") { return true; }
            if (platform == "Nintendo Wii U") { return true; }
            if (platform == "Nintendo Game Boy") { return true; }
            if (platform == "Nintendo Game Boy Color") { return true; }
            if (platform == "Nintendo Game Boy Advance") { return true; }
            if (platform == "Nintendo DS") { return true; }
            if (platform == "Nintendo 3DS") { return true; }

            if (platform == "Sega Genesis") { return true; }
            if (platform == "Sega CD") { return true; }
            if (platform == "Sega 32X") { return true; }
            if (platform == "Sega Saturn") { return true; }
            // if (platform == "Sega Dreamcast") { return true; }

            if (platform == "Sony Playstation") { return true; }
            if (platform == "Sony Playstation 2") { return true; }
            if (platform == "Sony Playstation 3") { return true; }
            if (platform == "Sony PSP") { return true; }
            if (platform == "Sony Vita") { return true; }

            if (platform == "TurboGrafx-16") { return true; }
            if (platform == "TurboGrafx-CD") { return true; }

            if (platform == "3DO Interactive Multiplayer") { return true; }
            // if (platform == "Philips CD-i") { return true; }
            if (platform == "WoW Action Max") { return true; }


            return false;
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
