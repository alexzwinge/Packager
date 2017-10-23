using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Packager.Types
{
    class GDI : TOC
    {
        public GDI(string gdipath) : base(gdipath)
        {

        }

        public override List<string> GetImageList()
        {
            List<string> images = new List<string>();

            foreach (Match match in new Regex(@"(?=2352\s""*(.*)""*\s0)").Matches(Content))
            {
                string image = match.Groups[1].Value.Replace("\"", "");

                if (!String.IsNullOrEmpty(image))
                {
                    images.Add(image);
                }
            }

            return images;
        }
    }
}
