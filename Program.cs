using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

class Tankung
{
    public DateTime Datum { get; set; }
    public double Liter { get; set; }
    public double Preis { get; set; }
    public int Km { get; set; }
}

class Program
{
    const string Datei = "tankungen.json";
    static List<Tankung> liste = new();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        if (File.Exists(Datei))
            liste = JsonSerializer.Deserialize<List<Tankung>>(File.ReadAllText(Datei)) ?? new();
        Hilfe();

        while (true)
        {
            Console.WriteLine("\n  AUTO-KOSTEN-TRACKER");
            Console.WriteLine("  1 Erfassen  2 Übersicht  3 Bearbeiten  4 Speichern  5 Export  6 Hilfe  0 Ende");
            Console.Write("  Auswahl: ");
            switch (Console.ReadLine())
            {
                case "1": Erfassen(); break;
                case "2": Übersicht(); break;
                case "3": Bearbeiten(); break;
                case "4": Speichern(); Farbe("Gespeichert.", ConsoleColor.Green); break;
                case "5": Export(); break;
                case "6": Hilfe(); break;
                case "0": return;
                default: Farbe("Ungültige Eingabe.", ConsoleColor.Red); break;
            }
        }
    }

    static void Erfassen()
    {
        double liter = Zahl("  Liter: ");
        double preis = Zahl("  Preis pro Liter (CHF): ");
        int km = (int)Zahl("  Kilometerstand: ");
        if (liste.Count > 0 && km <= liste.Last().Km)
        {
            Farbe($"Kilometerstand muss über {liste.Last().Km} liegen.", ConsoleColor.Red);
            return;
        }
        liste.Add(new Tankung { Datum = DateTime.Now, Liter = liter, Preis = preis, Km = km });
        Speichern();
        Farbe($"Gespeichert: {liter:F2} L für {liter * preis:F2} CHF.", ConsoleColor.Green);
    }

    static void Übersicht()
    {
        if (liste.Count < 2) { Farbe("Mind. 2 Tankungen nötig.", ConsoleColor.Red); return; }

        // Die erste Tankung ist nur der Startpunkt und zählt nicht zum Verbrauch.
        int km = liste.Last().Km - liste.First().Km;
        double liter = liste.Skip(1).Sum(t => t.Liter);
        double kosten = liste.Skip(1).Sum(t => t.Liter * t.Preis);
        double verbrauch = liter / km * 100;
        double tank = liste.Max(t => t.Liter);   // grösste Tankung = Schätzung der Tankgrösse

        Console.WriteLine($"\n  Gefahren:        {km} km");
        Console.WriteLine($"  Ø Verbrauch:     {verbrauch:F2} L/100 km");
        Console.WriteLine($"  Kosten:          {kosten:F2} CHF ({kosten / km * 100:F2} CHF/100 km)");
        Console.WriteLine($"  Reichweite ca.:  {tank / verbrauch * 100:F0} km pro Tank");
    }

    static void Bearbeiten()
    {
        for (int i = 0; i < liste.Count; i++)
            Console.WriteLine($"  {i + 1,2}  {liste[i].Datum:dd.MM.yy}  {liste[i].Liter,6:F2} L  {liste[i].Preis:F3} CHF/L  {liste[i].Km} km");

        Console.Write("  Nummer (Enter = zurück): ");
        if (!int.TryParse(Console.ReadLine(), out int nr) || nr < 1 || nr > liste.Count) return;
        var t = liste[nr - 1];

        Console.Write("  b = bearbeiten, l = löschen: ");
        string a = Console.ReadLine() ?? "";
        if (a == "l") liste.RemoveAt(nr - 1);
        else if (a == "b")
        {
            Console.WriteLine("  Enter = Wert behalten");
            double liter = Zahl($"  Liter [{t.Liter}]: ", t.Liter);
            double preis = Zahl($"  Preis [{t.Preis}]: ", t.Preis);
            int km = (int)Zahl($"  Kilometerstand [{t.Km}]: ", t.Km);

            int davor = nr > 1 ? liste[nr - 2].Km : -1;
            int danach = nr < liste.Count ? liste[nr].Km : int.MaxValue;
            if (km <= davor || km >= danach)
            {
                Farbe("Kilometerstand passt nicht zur Reihenfolge. Nichts geändert.", ConsoleColor.Red);
                return;
            }
            t.Liter = liter; t.Preis = preis; t.Km = km;
        }
        else return;

        Speichern();
        Farbe("Gespeichert.", ConsoleColor.Green);
    }

    static void Export()
    {
        var zeilen = liste.Select(t => string.Create(CultureInfo.InvariantCulture,
            $"{t.Datum:dd.MM.yyyy};{t.Liter:F2};{t.Preis:F3};{t.Km};{t.Liter * t.Preis:F2}"));
        File.WriteAllLines("tankungen.csv", zeilen.Prepend("Datum;Liter;Preis;Kilometerstand;Kosten"));
        Farbe($"Exportiert: {Path.GetFullPath("tankungen.csv")}", ConsoleColor.Green);
    }

    static void Speichern() =>
        File.WriteAllText(Datei, JsonSerializer.Serialize(liste, new JsonSerializerOptions { WriteIndented = true }));

    static void Hilfe()
    {
        Console.WriteLine("\n  So geht's:");
        Console.WriteLine("  1  Tankung eintragen (Liter, Preis, Kilometerstand). Bitte immer volltanken.");
        Console.WriteLine("  2  Verbrauch, Kosten und Reichweite ansehen (ab 2 Tankungen).");
        Console.WriteLine("  3  Einträge korrigieren oder löschen.");
        Console.WriteLine("  4  Speichern (passiert nach jeder Änderung auch automatisch).");
        Console.WriteLine("  5  Als CSV für Excel exportieren.");
    }

    static double Zahl(string text, double? alt = null)
    {
        while (true)
        {
            Console.Write(text);
            string s = (Console.ReadLine() ?? "").Replace(',', '.');
            if (s == "" && alt != null) return alt.Value;
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double w) && w > 0) return w;
            Farbe("Bitte eine Zahl grösser als 0.", ConsoleColor.Red);
        }
    }

    static void Farbe(string text, ConsoleColor farbe)
    {
        Console.ForegroundColor = farbe;
        Console.WriteLine($"  {text}");
        Console.ResetColor();
    }
}
