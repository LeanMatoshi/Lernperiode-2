using TextAdventure;

// --- Name abfragen und begrüssen ---
Console.Write("Wie heisst du? ");
string? name = Console.ReadLine()?.Trim();
if (string.IsNullOrEmpty(name))
{
    name = "Unbekannter";
}
Console.WriteLine($"Hallo, {name}");

// --- Räume anlegen und verbinden ---
var keller = new Raum("Keller", "Es ist kalt und feucht. Irgendwo tropft es.");
var halle = new Raum("Eingangshalle", "Eine grosse Halle mit knarrendem Holzboden.");
var turm = new Raum("Turmzimmer", "Ein enger Raum mit Aussicht auf nichts Besonderes.");

keller.Nord = halle;
halle.Sued = keller;
halle.Nord = turm;
turm.Sued = halle;

// --- Items verteilen ---
keller.Items.Add("Schlüssel");
halle.Items.Add("Fackel");

// --- Verschlossene Tür: Turm geht nur mit Schlüssel ---
turm.BenoetigtesItem = "Schlüssel";

var inventar = new List<string>();
Raum aktuellerRaum = halle;

Console.WriteLine("Tipp: Mit 'hilfe' siehst du alle Befehle.");
aktuellerRaum.Ausgeben();

// --- Hauptschleife: endet erst bei "beenden" ---
bool laeuft = true;
while (laeuft)
{
    Console.Write("> ");
    string eingabe = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    if (eingabe.StartsWith("nimm "))
    {
        Nimm(aktuellerRaum, inventar, eingabe.Substring(5).Trim());
        continue;
    }

    switch (eingabe)
    {
        case "beenden":
            laeuft = false;
            break;

        case "gehe nord":
            aktuellerRaum = Gehe(aktuellerRaum, aktuellerRaum.Nord, inventar);
            break;

        case "gehe süd":
        case "gehe sued":
            aktuellerRaum = Gehe(aktuellerRaum, aktuellerRaum.Sued, inventar);
            break;

        case "umschauen":
            aktuellerRaum.Ausgeben();
            break;

        case "inventar":
            if (inventar.Count == 0)
                Console.WriteLine("Du trägst nichts bei dir.");
            else
                Console.WriteLine($"Du trägst: {string.Join(", ", inventar)}");
            break;

        case "hilfe":
            ZeigeHilfe();
            break;

        case "":
            break;

        default:
            Console.WriteLine("Unbekannter Befehl. Tipp: 'hilfe'");
            break;
    }
}

Console.WriteLine($"Tschüss, {name}!");

// Wechselt in den Zielraum, falls es einen gibt und die Tür nicht verschlossen ist.
static Raum Gehe(Raum aktuell, Raum? ziel, List<string> inventar)
{
    if (ziel == null)
    {
        Console.WriteLine("Da geht es nicht weiter.");
        return aktuell;
    }

    if (ziel.BenoetigtesItem != null)
    {
        bool hatItem = inventar.Any(i => i.Equals(ziel.BenoetigtesItem, StringComparison.OrdinalIgnoreCase));
        if (!hatItem)
        {
            Console.WriteLine($"Die Tür ist verschlossen. Dir fehlt: {ziel.BenoetigtesItem}");
            return aktuell;
        }

        Console.WriteLine($"Du schliesst die Tür mit dem {ziel.BenoetigtesItem} auf.");
        ziel.BenoetigtesItem = null;
    }

    ziel.Ausgeben();
    return ziel;
}

// Nimmt ein Item aus dem Raum ins Inventar.
static void Nimm(Raum raum, List<string> inventar, string itemName)
{
    string? gefunden = raum.Items.FirstOrDefault(i => i.Equals(itemName, StringComparison.OrdinalIgnoreCase));

    if (gefunden == null)
    {
        Console.WriteLine($"Hier gibt es kein '{itemName}'.");
        return;
    }

    raum.Items.Remove(gefunden);
    inventar.Add(gefunden);
    Console.WriteLine($"Du nimmst: {gefunden}");
}

static void ZeigeHilfe()
{
    Console.WriteLine("Befehle:");
    Console.WriteLine("  gehe nord / gehe süd  - Raum wechseln");
    Console.WriteLine("  umschauen             - Raum nochmal anzeigen");
    Console.WriteLine("  nimm [item]           - Gegenstand aufheben");
    Console.WriteLine("  inventar              - zeigt, was du trägst");
    Console.WriteLine("  hilfe                 - diese Liste");
    Console.WriteLine("  beenden               - Spiel beenden");
}
