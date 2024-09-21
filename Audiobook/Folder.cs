using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Audiobook
{
    internal class Folder
    {
        public string Title { get; set; }
        public string Name { get; set; }

        internal Folder(string currentFolder)
        {
            var iposn = currentFolder.LastIndexOf('\\');
            Title = currentFolder.Substring(iposn + 1);

            iposn = Title.IndexOf(" - ");
            if (iposn > 0)
            {
                Name = Title.Substring(0, iposn);
            }
            else
            {
                Name = "Unknown Author";
            }
        }

        internal static List<string> BuildContent(string title, string name)
        {
            List<string> content = new();

            content.Add("$directory = \"${PWD}/audiobooks\"");
            content.Add("$files = Get-ChildItem -Path $directory -Filter *.mp3 | Sort-Object Name");
            content.Add("$dockerCommand = \"docker run -it --rm -v \"\"${PWD}/audiobooks:/mnt\"\" sandreas/m4b-tool:latest merge\"");
            content.Add("\nforeach ($file in $files) {");
            content.Add("\t$dockerCommand += \" \"\"/mnt/$($file.Name)\"\"\"");
            content.Add("\n# Write-Host \" \"\"/mnt/$($file.Name)\"\"\"");
            content.Add("}");
            content.Add($"\n$dockerCommand += \" --output-file \"\"/mnt/{title}.m4b\"\" --series \"\"\"\" --name=\"\"{title}\"\" --series-part=1 --artist \"\"{name}\"\" --albumartist=\"\"{name}\"\" --use-filenames-as-chapters --cover \"\"/mnt/cover.jpg\"\" --jobs=8 --audio-channels=2 --audio-samplerate=44100\"");
            content.Add("\n#Write-Host $dockerCommand");
            content.Add("\n# Execute the command");
            content.Add("Invoke-Expression $dockerCommand");

            return content;
        }

        internal static void WritePsFile(string currentFolder, List<string> content)
        {
            var newFile = "makebook.ps1";

            File.WriteAllLines(currentFolder + "\\" + newFile, content, Encoding.UTF8);
        }

        internal static void MoveMp3s(string sourceFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine($"Error: Source folder '{sourceFolder}' does not exist.");
                return;
            }

            string destinationFolder = Path.Combine(sourceFolder, "audiobooks");

            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                string[] mp3Files = Directory.GetFiles(sourceFolder, "*.mp3");

                if (mp3Files.Length == 0)
                {
                    Console.WriteLine("No .mp3 files found in the source folder.");
                    return;
                }

                foreach (var file in mp3Files)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        string destPath = Path.Combine(destinationFolder, fileName);

                        if (File.Exists(destPath))
                        {
                            Console.WriteLine($"File '{fileName}' already exists in the destination folder. Skipping.");
                            continue;
                        }

                        File.Move(file, destPath);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Error moving file '{file}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        internal static void MoveArt(string sourceFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine($"Error: Source folder '{sourceFolder}' does not exist.");
                return;
            }

            string destinationFolder = Path.Combine(sourceFolder, "audiobooks");

            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                string[] art = Directory.GetFiles(sourceFolder, "cover.jpg");

                if (art.Length == 0)
                {
                    Console.WriteLine("No 'cover.jpg' file found in the source folder.");
                    return;
                }

                string cover = Path.GetFileName(art[0]);
                string destinationPath = Path.Combine(destinationFolder, cover);

                if (File.Exists(destinationPath))
                {
                    Console.WriteLine($"The file 'cover.jpg' already exists in the destination folder. Skipping.");
                    return;
                }

                File.Move(art[0], destinationPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while moving the cover file: {ex.Message}");
            }
        }
    }
}
