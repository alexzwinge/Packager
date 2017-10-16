using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class TurboGrafx_CD : Types.ImageTOC, IRom
    {
        public TurboGrafx_CD(IGame game) : base(game)
        {

        }
    }
}
