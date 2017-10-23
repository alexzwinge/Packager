using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class Sega_Dreamcast : Types.ImageGDI, IRom
    {
        public Sega_Dreamcast(IGame game) : base(game)
        {

        }
    }
}
