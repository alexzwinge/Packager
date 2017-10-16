using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class Sega_Saturn : Types.ImageTOC, IRom
    {
        public Sega_Saturn(IGame game) : base(game)
        {

        }
    }
}
