using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class Nintendo_GameCube : Types.Image, IRom
    {
        public Nintendo_GameCube(IGame game) : base(game)
        {

        }
    }
}
