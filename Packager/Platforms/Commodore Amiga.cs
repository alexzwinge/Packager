using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class Commodore_Amiga : Types.Disk, IRom
    {
        public Commodore_Amiga(IGame game) : base(game)
        {

        }
    }
}
