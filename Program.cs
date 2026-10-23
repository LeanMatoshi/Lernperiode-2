using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

class Program
{
    static List<(double liter, double preis, int km)> tankungen = new();

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
                    ErfasseTankung();
                    break;
                case "2":
                    ZeigeÜbersicht();
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

    static double Zahl(string text)
    {
        Console.Write(text);
        return double.Parse(Console.ReadLine()!.Replace(',', '.'), CultureInfo.InvariantCulture);
    }

    static void ErfasseTankung()
    {
        tankungen.Add((Zahl("Liter: "), Zahl("Preis pro Liter: "), (int)Zahl("Kilometerstand: ")));
        Console.WriteLine("Gespeichert.");
    }

    static void ZeigeÜbersicht()
    {
        if (tankungen.Count < 2) { Console.WriteLine("Mind. 2 Tankungen nötig."); return; }
        int km = tankungen.Last().km - tankungen.First().km;
        double liter = tankungen.Skip(1).Sum(t => t.liter);
        double kosten = tankungen.Skip(1).Sum(t => t.liter * t.preis);
        Console.WriteLine($"Gefahren:  {km} km");
        Console.WriteLine($"Verbrauch: {liter / km * 100:F2} L/100 km");
        Console.WriteLine($"Kosten:    {kosten:F2} CHF ({kosten / km * 100:F2} CHF/100 km)");
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
