using System.Xml;
using System;
using System.IO;

namespace ArtixPost
{
    class Program
    {
        static void Error(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(err);
            Environment.Exit(1);
        }
        static void Main(string[] args)
        {
            Console.Title = "Artix Renderer";

            Console.WriteLine("Artix renderer");
            Console.WriteLine("V 1.2");

            if (args.Length == 1 && args[0] == "?" || args[0] == "--help")
            {
                Console.WriteLine("artix input_file.xml input_rules.xml <optional>output_file.html");
                Environment.Exit(0);
            }

            if (args.Length < 2)
            {
                Error("No input file and parse dictionary specification!");
            }

            if (args.Length > 3)
            {
                Error("Too much arguments!");
            }

            try
            {
                Console.WriteLine("Rendering ...");

                XmlDocument inputFile = new XmlDocument();
                inputFile.Load(args[0]);

                XmlDocument parseRules = new XmlDocument();
                parseRules.Load(args[1]);

                ParseMap map = ParseMap.LoadFromXML(parseRules);

                ArtixXmlElement article = new ArtixXmlElement(inputFile.DocumentElement);
                string html = article.Render(map);

                string outputPath = "output.html";

                if (args.Length == 3)
                {
                    outputPath = args[2];
                }

                File.WriteAllText(outputPath, html);

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"Done! Output file saved at: {outputPath}");
            }
            catch (Exception ex)
            {
                Error($"{ex.Message}\n{ex.StackTrace}");
            }
            
        }
    }
}
