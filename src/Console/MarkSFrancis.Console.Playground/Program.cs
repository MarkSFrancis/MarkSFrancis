﻿namespace MarkSFrancis.Console.Playground
{
    class Program
    {
        public static ConsoleHelper Console = new ConsoleHelper();

        static void Main(string[] args)
        {
            Console.WriteInfo("Welcome to the playground application for the MarkSFrancis.Console library");

            if (Console.YesNo("Clear the intro message?"))
            {
                Console.UndoWriteLine(2);
            }
            else
            {
                Console.UndoWriteLine();
            }

            var result = Console.GetInt("Please enter an integer:");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(false);
        }
    }
}
