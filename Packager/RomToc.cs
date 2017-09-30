using System.IO;
using System.Text.RegularExpressions;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    class RomToc : Rom, IRom
    {
        public RomToc(IGame game) : base(game)
        {

        }

        public string File
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Game.ApplicationPath);
            }

            set
            {
                string initialPath = Game.ApplicationPath;

                FileInfo cue = RenameRom(Game.ApplicationPath, value);
                SetGamePath(Game, cue);
                RenameImages(cue, value);                

                foreach (IAdditionalApplication app in Game.GetAllAdditionalApplications())
                {
                    if (app.ApplicationPath == initialPath)
                    {
                        app.ApplicationPath = cue.FullName;
                    }

                    FileInfo appcue = RenameRom(app.ApplicationPath, value);
                    SetGamePath(app, appcue);
                    RenameImages(appcue, value);                    
                }
            }
        }

        private void RenameImages(FileInfo toc, string targetName)
        {
            string tocOld = System.IO.File.ReadAllText(toc.FullName);
            string tocNew = tocOld;

            foreach (Match match in new Regex("\"(.*)\"").Matches(tocOld))
            {
                FileInfo bin = RenameRom(RomFolder.FullName + "\\" + match.Groups[1].Value, targetName);
                tocNew = SetGamePath(tocNew, match.Groups[1].Value, bin.Name);
            }

            if (tocNew != tocOld)
            {
                System.IO.File.WriteAllText(toc.FullName, tocNew);
            }
        }
    }
}
