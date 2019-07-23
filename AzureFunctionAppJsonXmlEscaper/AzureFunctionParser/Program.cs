using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AzureFunctionParser
{
    class Program
    {
        public static double VersionNo { get; set; } = 1.0;

        static void Main(string[] args)
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));
           

            Console.WriteLine($"Azure Function Parser v{VersionNo.ToString()}");

            //var output = ConvertAzureFunctionToHumanReadable();
            var output = ConvertHumanReadableToAzureFunction();

            //Console.WriteLine($"\n\n\nParsed Output: \n\n{output}");

            string destination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "AzureParserOutput.txt";
            
            Console.WriteLine("The output has been saved to " + destination + ", edit with notepad++ or similar text editor (not notepad.exe, newlines are not displayed properly");
            Console.WriteLine("\n\nExecution finshed, press enter to exit");
            Console.ReadLine();

        }
        private static string ConvertHumanReadableToAzureFunction()
        {
            Console.WriteLine("\nPaste the path of the file containing the text you want to parse to azure format: ");

            var path = Console.ReadLine();

            path = path.Replace("\"", "");
            var stringToParse = File.ReadAllText(path);

            //remove newlines and tabs
            string replacement = Regex.Replace(stringToParse, @"\t|\n|\r", "");

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            replacement = regex.Replace(replacement, " ");

            string destination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (File.Exists(destination + "\\AzureParserOutput.txt"))
            {
                File.Delete(destination + "\\AzureParserOutput.txt");
            }
            
            File.WriteAllText(destination + "\\AzureParserOutput.txt", replacement);

            return replacement;
        }

        private static string ConvertAzureFunctionToHumanReadable()
        {
            string closingBracesStr = string.Empty;

            Console.WriteLine("\nPaste the string you want to parse to human readable format: ");

            var stringToParse = Console.ReadLine();

            List<string> lines = new List<string>();

            var parts = stringToParse.Split('(');

            int counter = 0;

            foreach (var part in parts)
            {

                if (part.Contains(')'))
                {
                    var temp = part.Split(')')[0];
                    for (int i = 0; i < counter - 1; i++)
                    {
                        temp = "\t" + temp;
                    }
                    lines.Add(temp);
                    int indexOfFirstBrace = part.IndexOf(')');

                    closingBracesStr = part.Substring(indexOfFirstBrace, part.Length - indexOfFirstBrace);
                }
                else
                {

                    string temp = $"{part}(";

                    for (int i = 0; i < counter; i++)
                    {
                        temp = "\t" + temp;
                    }

                    counter++;
                    lines.Add(temp);
                }

            }

            var closingBracesParts = closingBracesStr.Split(')');
            counter = closingBracesParts.Length - 2;

            foreach (var part in closingBracesParts)
            {

                string temp = $"{part})";
                for (int i = 0; i < counter; i++)
                {
                    temp = "\t" + temp;
                }

                lines.Add(temp);
                counter--;
            }

            string output = string.Empty;

            foreach (var line in lines)
            {
                output += $"\n{line}";
            }

            output = output.Remove(output.LastIndexOf(')'), 1);
            string destination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (File.Exists(destination + "\\AzureParserOutput.txt"))
            {
                File.Delete(destination + "\\AzureParserOutput.txt");
            }

            File.WriteAllText(destination + "\\AzureParserOutput.txt", output);
            return output;

        }
    }
}
