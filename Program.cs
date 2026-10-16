using System;

class Program
{
    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("Auto-Kosten-Tracker");
            Console.WriteLine("1) Tankung erfassen");
            Console.WriteLine("2) Übersicht anzeigen");
            Console.WriteLine("0) Beenden");
            Console.Write("Auswahl: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Tankung wird erfasst.");
                    break;
                case "2":
                    Console.WriteLine("Übersicht.");
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Ungültige Eingabe, nochmal.");
                    break;
            }
        }

        Console.WriteLine("Tschüss.");
    }
}