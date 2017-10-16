using System;
using System.Text.RegularExpressions;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    public static class Dispatch
    {
        public static void Gentrify(IGame game)
        {
            string platform = game.Platform;

            platform = Regex.Replace(platform, @"[\s-]", "_");
            platform = Regex.Replace(platform, @"^[0-9]", "_");

            Type type = Type.GetType("Packager.Platforms." + platform);
            
            if (type == null)
            {
                type = Type.GetType("Packager.Platforms.Generic");
            }

            IRom rom = (IRom)Activator.CreateInstance(type, game);

            rom.Package();
        }
    }
}
