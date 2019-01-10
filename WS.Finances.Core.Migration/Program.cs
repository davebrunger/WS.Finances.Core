using System;
using System.IO;
using System.Text.RegularExpressions;

namespace WS.Finances.Core.Migration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Path not defined");
                    return;
                }
                var path = args[0];
                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"Path {path} does not exist");
                    return;
                }
                foreach (var filename in Directory.EnumerateFiles(path))
                {
                    var fileData = ParseFile(filename);
                    if (fileData == null)
                    {
                        Console.WriteLine($"Non data file: {filename}");
                    }
                    else
                    {
                        var newFilename = fileData.ToString();
                        Console.WriteLine($"Moving file {filename} to {newFilename}");
                        var newPath = Path.GetDirectoryName(newFilename);
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }
                        File.Move(filename, newFilename);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static FileData ParseFile(string filename)
        {
            var regex = new Regex(@"(?<folder>.*)/(?<year>\d{4})-(?<month>\d{1,2})-(?<filename>.*\.json)");
            var match = regex.Match(filename);
            if (!match.Success)
            {
                return null;
            }
            else
            {
                return new FileData(match.Groups["folder"].Value, int.Parse(match.Groups["year"].Value),
                    int.Parse(match.Groups["month"].Value), match.Groups["filename"].Value);
            }
        }
    }

    public class FileData
    {
        public string Folder { get; }
        public int Year { get; }
        public int Month { get; }
        public string Filename { get; }

        public FileData(string folder, int year, int month, string filename)
        {
            Folder = folder;
            Year = year;
            Month = month;
            Filename = filename;
        }

        public override string ToString()
        {
            return Path.Combine(Folder, $"{Year:0000}", $"{Month:00}", Filename);
        }
    }
}
