using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace Audiobook
{
    internal class Program
    {
        public static List<string> Content = new();

        public static void Main(string[] args)
        {
            var currentFolder = Environment.CurrentDirectory;

            var folder = new Folder(currentFolder);

            var content = Folder.BuildContent(folder.Title, folder.Name);

            Folder.WritePsFile(currentFolder, content);

            Folder.MoveMp3s(currentFolder);

            Folder.MoveArt(currentFolder);
        }
    }
}
