using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Packager.Types
{
    class TOC
    {
        private string Target;
        private string Content;
        private bool Updated;

        public TOC(string tocpath)
        {
            Target = tocpath;
            Content = File.ReadAllText(tocpath);
            Updated = false;
        }

        public virtual void Update(string oldname, string newname)
        {
            Content = Content.Replace(oldname, "\"" + newname + "\"");
            Content = Content.Replace("\"\"", "\"");
            Updated = true;
        }

        public virtual void Write()
        {
            if (Updated)
            {
                File.WriteAllText(Target, Content);
            }
        }

        public virtual List<string> GetImageList()
        {
            List <string> images = new List<string>();

            foreach (Match match in new Regex("\"(.*)\"").Matches(Content))
            {
                string image = match.Groups[1].Value;

                if (!String.IsNullOrEmpty(image))
                {
                    images.Add(image);
                }
            }

            return images;
        }
    }
}
