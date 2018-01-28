using System;

namespace Treehouse
{
    class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.Write(": ");
                string input = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                string output;

                if (input[0] == '.' || input[0] == '-')
                {
                    output = MorseCodeTranslator.ToText(input);
                }
                else
                {
                    output = MorseCodeTranslator.ToMorse(input);
                }

                Console.WriteLine(output);
            }
        }
    }
}