using System;

class Program
{
    static void Main()
    {
        ZeigeBegrüssung();

        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("Auto-Kosten-Tracker");
            Console.WriteLine("1) Tankung erfassen");
            Console.WriteLine("2) Übersicht anzeigen");
            Console.WriteLine("3) Hilfe / Erklärung");
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
                case "3":
                    ZeigeErklärung();
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

    static void ZeigeBegrüssung()
    {
       Console.WriteLine("  Willkommen beim Auto-Kosten-Tracker!");
        ZeigeErklärung();
    }

    static void ZeigeErklärung()
    {
        Console.WriteLine();
        Console.WriteLine("So funktioniert's:");
        Console.WriteLine(" - Mit '1' erfasst du eine Tankung");
        Console.WriteLine("   (z.B. Liter, Preis, Kilometerstand).");
        Console.WriteLine(" - Mit '2' siehst du eine Übersicht deiner");
        Console.WriteLine("   Kosten und deines Verbrauchs.");
        Console.WriteLine(" - Mit '3' kommt diese Erklärung nochmal.");
        Console.WriteLine(" - Mit '0' beendest du das Programm.");
    }
}
