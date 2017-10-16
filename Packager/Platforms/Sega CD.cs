using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class Sega_CD : Types.ImageTOC, IRom
    {
        public Sega_CD(IGame game) : base(game)
        {

        }
    }
}
