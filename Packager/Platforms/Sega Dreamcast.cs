using System;
using Unbroken.LaunchBox.Plugins.Data;

namespace Packager.Platforms
{
    class Sega_Dreamcast : Types.ImageTOC, IRom
    {
        public Sega_Dreamcast(IGame game) : base(game)
        {
            throw new NotImplementedException();
        }
    }
}
