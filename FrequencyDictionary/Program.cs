using FrequencyCalculationService;
using System;
using System.IO;

namespace FrequencyDictionary
{
    class Program
    {
        #region private
        private const string Help = @"Frequency dictionary.
Usage:
FrequencyDictionary.exe [input file] [output file]

Input file should have Windows-1251 codepage.
";

        private const int MinBlockSize = 1024;
        private const int MaxWordLength = 20;

        private static readonly char[] delimiters = new char[] { '\r', '\n', ' ' };

        private static void WriteHelp()
        {
            Console.WriteLine(Help);
        }
        #endregion

        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                WriteHelp();
                return;
            }

            string inputFilePath = args[0];
            string outputFilePath = args[1];

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file '{inputFilePath}' does not exists or not accessible.");
                return;
            }
            
            NewFrequencyCalculationService calculationService =
                new NewFrequencyCalculationService(
                    new FileDataReader(inputFilePath, MinBlockSize, MaxWordLength, delimiters),
                    new FileDataWriter(outputFilePath),
                    new FrequencyCalculatorWithUpdater(MaxWordLength));

            Console.WriteLine("Calculating frequencies...");

            calculationService.RunCalculationAsync().Wait();

            Console.WriteLine("Done.");
            Console.WriteLine($"Results written to output file: {outputFilePath}");
        }
    }
}
