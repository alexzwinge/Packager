using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class Arcade : Types.Dump, IRom
    {
        public Arcade(IGame game) : base(game)
        {

        }
    }
}
