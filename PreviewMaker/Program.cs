using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PreviewMaker
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Build directory:");
            string path = Console.ReadLine();

            string cssContent = GetFileContent(
                FindFile(path, "main*.css"));
            string jsContent = GetFileContent(
                FindFile(path, "main*.js"));

            //Remove sourcemap link
            cssContent = Regex.Replace(cssContent, @"\/\*#(.*?)\*\/", "").Trim();
            jsContent = Regex.Replace(jsContent, @"\/\/(.*?)$", "").Trim();

            string htmlPage = $"<!DOCTYPE html><html><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width,initial-scale=1,shrink-to-fit=no\"><link href=\"https://fonts.googleapis.com/css?family=Roboto\" rel=\"stylesheet\"><title>Preview</title><style></style><style>{cssContent}</style></head><body><noscript>You need to enable JavaScript to run this app.</noscript><div id=\"root\"></div><script>{jsContent}</script></body></html>";

            using (StreamWriter streamWriter = new StreamWriter("preview-var.js"))
            {
                streamWriter.Write($"export const previewPage = \"{Escape(htmlPage)}\";");
            }

            Console.WriteLine("Complete!");
            Console.ReadKey();
        }

        private static string FindFile(string path, string mask)
        {
            string[] files = Directory.GetFiles(path, mask, SearchOption.AllDirectories);
            return files.Length > 0 ? files[0] : "";
        }

        private static string GetFileContent(string fileName)
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static string Escape(string input)
        {
            return input
                .Replace(@"\", @"\\")
                //.Replace(@"/", @"\/")
                //.Replace(@"'", @"\'")
                .Replace(@"""", @"\""");
        }
    }
}