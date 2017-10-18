using System;
using System.IO;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Types
{
    class BaseRom
    {
        protected readonly IGame Game;
        protected readonly string Name;
        protected string NameMeta;
        protected readonly DirectoryInfo Container;

        public BaseRom(IGame game)
        {
            Game = game;
            Name = FSOperations.Name(Game.Title).Trim();
            NameMeta = Name;
            Container = new FileInfo(Game.ApplicationPath).Directory;
        }

        public virtual void Package()
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
        }

        protected virtual void RenameFolder()
        {
            string initialPath = Container.FullName + "\\";

            FSOperations.Move(Container, Name);

            Game.MusicPath = Game.MusicPath.Replace(initialPath, Container.FullName);
            Game.VideoPath = Game.VideoPath.Replace(initialPath, Container.FullName);
            Game.ManualPath = Game.ManualPath.Replace(initialPath, Container.FullName);
            Game.ApplicationPath = Game.ApplicationPath.Replace(initialPath, Container.FullName);

            foreach (IAdditionalApplication app in Game.GetAllAdditionalApplications())
            {
                app.ApplicationPath = app.ApplicationPath.Replace(initialPath, Container.FullName);
            }
        }

        protected virtual void RenameFile()
        {
            FileInfo rom = new FileInfo(Game.ApplicationPath);
            string initialPath = rom.FullName;

            FSOperations.Move(rom, Container.FullName, NameMeta);

            Game.ApplicationPath = rom.FullName;

            foreach (IAdditionalApplication app in Game.GetAllAdditionalApplications())
            {
                if (app.ApplicationPath == initialPath)
                {
                    app.ApplicationPath = rom.FullName;
                }
            }
        }

        protected virtual void RenameVideo()
        {
            FileInfo video = new FileInfo(Game.GetVideoPath());
            FSOperations.Move(video, Container.FullName, Name + " (Video Snap)");

            Game.VideoPath = video.FullName;
        }

        protected virtual bool HasCorrectFileName()
        {
            if (!File.Exists(Game.ApplicationPath))
            {
                throw new FileNotFoundException("Rom file for " + Game.Title + " not found.");
            }

            return Path.GetFileNameWithoutExtension(Game.ApplicationPath) == NameMeta ? true : false;
        }

        protected virtual bool HasCorrectFolderName()
        {
            if (!Container.Exists)
            {
                throw new DirectoryNotFoundException("Parent folder for " + Game.Title + " not found.");
            }

            return Container.Name == Name ? true : false;
        }

        protected virtual bool HasCorrectVideoName()
        {
            if (String.IsNullOrEmpty(Game.GetVideoPath()))
            {
                return true;
            }

            if (!File.Exists(Game.GetVideoPath()))
            {
                throw new FileNotFoundException("Video file for " + Game.Title + " not found.");
            }

            if (new FileInfo(Game.GetVideoPath()).Directory.FullName.ToLower() != Container.FullName.ToLower())
            {
                return false;
            }

            if (Path.GetFileNameWithoutExtension(Game.GetVideoPath()) != Name + " (Video Snap)")
            {
                return false;
            }

            return true;
        }
    }
}
