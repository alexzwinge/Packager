using Unbroken.LaunchBox.Plugins.Data;

namespace Packager
{
    interface IRom
    {
        IGame Game { get; }

        string Name { get; }

        string Folder { get; set; }

        string File { get; set; }

        string Video { get; set; }
    }
}
