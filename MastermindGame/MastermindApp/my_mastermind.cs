using System;
using System.Collections.Generic;
using System.Linq;

namespace MastermindGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string codeArg = null;
            int maxAttempts = 10;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-c" && i + 1 < args.Length)
                    codeArg = args[i + 1];
                else if (args[i] == "-t" && i + 1 < args.Length && int.TryParse(args[i + 1], out int t))
                    maxAttempts = t;
            }

            MastermindGame game = new MastermindGame(codeArg, maxAttempts);
            game.Start();
        }
    }

    class MastermindGame
    {
        private readonly string secretCode;
        private readonly int maxAttempts;

        public MastermindGame(string code, int attempts)
        {
            secretCode = string.IsNullOrEmpty(code) ? CodeGenerator.GenerateCode() : code;
            if (!Validator.IsValidCode(secretCode))
            {
                Console.WriteLine("Invalid secret code. It must be 4 distinct digits between 0-8.");
                Environment.Exit(1);
            }
            maxAttempts = attempts;
        }

        public void Start()
        {
            Console.WriteLine("Can you break the code? Enter a valid guess.");

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Console.Write($"---Round {attempt}>");
                string input = Console.ReadLine();

                if (input == "^D")
                {
                    Console.WriteLine("EOF received. Exiting.");
                    return;
                }

                if (!Validator.IsValidCode(input))
                {
                    Console.WriteLine("Wrong input!");
                    attempt--;
                    continue;
                }

                if (input == secretCode)
                {
                    Console.WriteLine("Congratz! You did it!");
                    return;
                }

                int wellPlaced = Feedback.GetWellPlaced(secretCode, input);
                int misplaced = Feedback.GetMisplaced(secretCode, input);

                Console.WriteLine($"Well placed pieces: {wellPlaced}");
                Console.WriteLine($"Misplaced pieces: {misplaced}");
            }

            Console.WriteLine("You've used all attempts! Game over.");
            Console.WriteLine($"The correct code was: {secretCode}");
        }
    }

    static class CodeGenerator
    {
        public static string GenerateCode()
        {
            Random rnd = new Random();
            HashSet<int> digits = new HashSet<int>();

            while (digits.Count < 4)
                digits.Add(rnd.Next(0, 9));

            return string.Concat(digits);
        }
    }

    static class Validator
    {
        public static bool IsValidCode(string code)
        {
            return code != null &&
                   code.Length == 4 &&
                   code.All(c => "012345678".Contains(c)) &&
                   code.Distinct().Count() == 4;
        }
    }

    static class Feedback
    {
        public static int GetWellPlaced(string secret, string guess)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
                if (secret[i] == guess[i]) count++;
            return count;
        }

        public static int GetMisplaced(string secret, string guess)
        {
            var secretList = secret.ToList();
            var guessList = guess.ToList();

            int misplaced = 0;

            for (int i = 0; i < 4; i++)
            {
                if (secret[i] == guess[i])
                {
                    secretList[i] = guessList[i] = '_';
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (guessList[i] != '_' && secretList.Contains(guessList[i]))
                {
                    misplaced++;
                    secretList[secretList.IndexOf(guessList[i])] = '_';
                }
            }

            return misplaced;
        }
    }
}
