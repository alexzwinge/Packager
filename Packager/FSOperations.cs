using System.IO;

namespace Packager
{
    public static class FSOperations
    {

        public static string Name(string title)
        {
            return title.
                Replace(": ", " - ")
                .Replace(":", "-")
                .Replace("/", "-")
                .Replace("\\", "-")
                .Replace("?", "");
        }

        public static void Move(DirectoryInfo fsObj, string target)
        {
            fsObj.MoveTo(fsObj.Parent.FullName + "\\tempmove");
            fsObj.MoveTo(fsObj.Parent.FullName + "\\" + target);
        }

        public static void Move(FileInfo fsObj, string target)
        {
            fsObj.MoveTo(fsObj.Directory.FullName + "\\tempmove" + fsObj.Extension);
            fsObj.MoveTo(fsObj.Directory.FullName + "\\" + target + fsObj.Extension.ToLower());
        }

    }
}
