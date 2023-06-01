using System.Collections;
using System.Data;
using System.Diagnostics;

//napravljeno: 03.03.2023.
//poslednja izmena: 24.05.2023.

//inicijalizacija

const string verzija = "1.5.1";
Console.Title = "TobiScript v" + verzija;
Console.ForegroundColor = ConsoleColor.Green;
string unos;
string putanja = "C:/";
//DateTime danasnjiDan = DateTime.Now;
Console.WriteLine("TobiScript v" + verzija + "\nAutor: Luka Stefanovic\n" + DateTime.Now.ToString("dd.MM.yyyy.") + "\nUnesite 'help' za listu svih komandi\n" + "https://www.lukastefanovic.com\n" + "https://github.com/LukaStef/tobiscript\n\n");

//program

while (true) //glavni
{
    try
    {
        Console.Write(putanja + ">");
        unos = Console.ReadLine();
        string[] podela = new string[2];
        if (unos.Contains(": "))
        {
            podela = unos.Split(": ");
            string komanda = podela[0];
            string argumenti = podela[1];
            IzvrsiKomandu(komanda, argumenti);
        }
        else
        {
            IzvrsiKomanduBezArg(unos);
        }
    }
    catch(Exception e)
    {
        //Console.WriteLine("greska u komandi \"" + unos + "\"");
        Console.WriteLine("greska: " + e.Message);
        Console.Write("\a");
    }
}
void IzvrsiKomandu(string komanda,string argumenti) //parsiranje argumenata i prepoznavanje komande
{
    string povratnaVrednost = "", arg;
    string[] podela = argumenti.Split(';');
    ArrayList listaArg = new();
    foreach (string a in podela)
    {
        arg = Parsiranje(a);
        povratnaVrednost += arg;
        listaArg.Add(arg);
    }
    string tekst = "";
    string trenutnaPutanja = putanja + povratnaVrednost;
    switch (komanda)
    {
        case "echo": //ispisuje unos
            Console.WriteLine(povratnaVrednost);
            break;
        case "title": //menja naslov konzole na unos
            Console.Title = povratnaVrednost;
            break;
        case "size": //menja velicinu konzole, height je broj redova, width je broj kolona
            BrojArgumenata(listaArg.Count,2);
            Console.WindowHeight = int.Parse(listaArg[0].ToString());
            Console.WindowWidth = int.Parse(listaArg[1].ToString());
            break;
        //--fajlovi--
        case "crtxt": //pravi .txt
            //BrojArgumenata(listaArg.Count, 1);
            FileStream fs = File.Create(trenutnaPutanja + ".txt");
            Console.WriteLine("Datoteka uspesno napravljena");
            break;
        case "optxt": //otvara .txt
            BrojArgumenata(listaArg.Count, 1);
            Process.Start(@"notepad.exe", trenutnaPutanja);
            break;
        case "wrtxt": //skroz menja sadrzaj za .txt
            if (listaArg.Count < 2)
            {
                Console.WriteLine("komanda \"" + unos + "\" mora da ima 2 ili vise argumenta");
                Console.Write("\a");
                return;
            }
            for (int i = 1; i < listaArg.Count; i++)
            {
                tekst = Parsiranje(tekst);
                tekst += listaArg[i].ToString();
            }
            File.WriteAllText(putanja + listaArg[0].ToString(), tekst);
            Console.WriteLine("Datoteka uspesno izmenjena");
            tekst = "";
            break;
        case "aptxt": //dodaje na .txt
            string mesto = putanja + listaArg[0].ToString();
            if (listaArg.Count < 2)
            {
                Console.WriteLine("komanda \"" + unos + "\" mora da ima 2 ili vise argumenta");
                Console.Write("\a");
                return;
            }
            for (int i = 1; i < listaArg.Count; i++)
            {
                tekst = Parsiranje(tekst);
                tekst += listaArg[i].ToString();
            }
            File.AppendAllText(mesto, listaArg[1].ToString());
            Console.WriteLine("Datoteka uspesno izmenjena");
            tekst = "";
            break;
        case "dltxt": //brise .txt
            if (File.Exists(povratnaVrednost))
            {
                File.Delete(putanja + listaArg[0].ToString());
                Console.WriteLine("Datoteka uspesno obrisana");
            }
            else
            {
                Console.WriteLine("Putanja ne postoji");
                Console.Write("\a");
            }
            break;
        case "dir": //ulazi u direktorijum
            if (Directory.Exists(povratnaVrednost))
            {
                putanja = povratnaVrednost;
                if (putanja[^1] != '/')
                {
                    putanja += "/";
                }
            }
            else
            {
                Console.WriteLine("Putanja ne postoji");
                Console.Write("\a");
            }
            
            break;
        case "mkdir": //pravi folder
            if (!Directory.Exists(trenutnaPutanja))
            {
                Directory.CreateDirectory(trenutnaPutanja);
                Console.WriteLine("Putanja uspesno napravljena");
            }
            else
            {
                Console.WriteLine("Putanja vec postoji");
                Console.Write("\a");
            }
            break;
        case "dldir": //brise folder
            //BrojArgumenata(listaArg.Count, 1);
            if (Directory.Exists(trenutnaPutanja))
            {
                Directory.Delete(trenutnaPutanja);
                Console.WriteLine("Putanja uspesno obrisana");
            }
            else
            {
                Console.WriteLine("Putanja ne postoji");
                Console.Write("\a");
            }
            break;
        case "openexe":
            try 
            { 
                Process.Start(trenutnaPutanja);
                Console.WriteLine("Aplikacija uspesno otvorena");
            }
            catch
            {
                Console.WriteLine("Aplikacija nije pronadjena");
                Console.Write("\a");
            }
            break;
        case "fcrd":
            try
            {
                Console.WriteLine(File.GetCreationTime(trenutnaPutanja));
            }
            catch
            {
                Console.WriteLine("Putanja ne postoji");
                Console.Write("\a");
            }
            break;
        case "mvfl":
            try
            {
                BrojArgumenata(listaArg.Count,2);
                File.Move(listaArg[0].ToString(), listaArg[1].ToString());
                Console.WriteLine("Putanja uspesno promenjena");
            }
            catch
            {
                Console.WriteLine("Putanja ne postoji");
                Console.Write("\a");
            }
            break;
        //--ostalo--
        case "site":
            var url = povratnaVrednost;
            var psi = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = url
            };
            Process.Start(psi);
            break;
        case "bgclr":
            BgColor(povratnaVrednost);
            break;
        case "fgclr":
            FgColor(povratnaVrednost);
            break;
        default:
            Console.WriteLine("Komanda " + komanda + " ne postoji");
            Console.Write("\a");
            break;
    }
}
string Parsiranje(string argument)
{
    string povratnaVrednost;
    try
    {
        DataTable t = new();
        double r;
        if (argument.Contains('/'))
        {
            r = (double)t.Compute(argument, "");
        }
        else
        {
            r = (int)t.Compute(argument, "");
        }
        povratnaVrednost = r.ToString();
    }
    catch
    {
        povratnaVrednost = argument;
    }
    return povratnaVrednost;
}
void BrojArgumenata(int listaBr, int brojArg) //proverava da li ima dovoljno argumenata u komandi
{
    if (listaBr != brojArg)
    {
        Console.WriteLine("Komanda \"" + unos + "\" mora da ima " + brojArg + " argumenata");
        Console.Write("\a");
        return;
    }
}
void IzvrsiKomanduBezArg(string komanda)
{
    const string help =
    "echo: arg1;arg2;arg3... - ispisuje uneti tekst i resenja izraza\n" +
    "title: arg1;arg2;arg3... - uneti tekst i resenja izraza postavlja kao naslov aplikacije\n" +
    "clear - cisti konzolu\n" +
    "site: url - otvara uneti sajt\n" +
    "size: izraz1;izraz2 - podesava velicinu aplikacije. izraz1 je broj redova, a izraz2 je broj kolona\n" +
    "bgclr: broj - podesava boju pozadine konzole. prihvata brojeve 0-15\n" +
    "fgclr: broj - podesava boju teksta konzole. prihvata brojeve 0-15\n" +
    "clearclr - resetuje boju teksta i pozadine konzole\n" +
    "crtxt: putanja;sadrzaj - pravi .txt fajl sa unetom putanjom i sadrzajem\n" +
    "optxt: putanja - otvara .txt fajl sa unetom putanjom. Ako nema fajla na toj putanji, program ce napraviti fajl\n" +
    "wrtxt: putanja;arg1;arg2... - menja trenutni sadrzaj fajla na unetoj putanji na nov unet sadrzaj\n" +
    "aptxt: putanja;arg1;arg2... - dodaje sadrzaj na .txt fajl\n" +
    "mkdir: putanja - pravi folder sa unetom putanjom\n" +
    "dldir: putanja - brise folder sa unetom putanjom\n" +
    "openexe: putanja - otvara .exe aplikaciju sa unetom putanjom\n" +
    "fcrd: putanja - ispisuje datum i vreme kad je napravljen fajl sa unetom putanjom\n" +
    "mvfl: stara-putanja;nova-putanja - menja putanju jednog fajla\n";
    switch (komanda)
    {
        case "help":
            Console.WriteLine(help);
            break;
        case "stop":
            Environment.Exit(0);
            break;
        case "clear":
            Console.Clear();
            Console.WriteLine("TobiScript v" + verzija + "\nAutor: Luka Stefanovic\n" + DateTime.Now.ToString("dd.MM.yyyy.") + "\nUnesite 'help' za listu svih komandi\n" + "lukastefanovic.com\n\n");
            break;
        case "clearclr":
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            break;
        default:
            Console.WriteLine("Komanda " + komanda + " ne postoji");
            Console.Write("\a");
            break;
    }
}
void BgColor(string index)
{
    ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
    for (int i = 1; i <= 15; i++)
    {
        if (i == int.Parse(index))
        {
            Console.BackgroundColor = colors[i];
            Console.Clear();
            Console.WriteLine("TobiScript v" + verzija + "\nAutor: Luka Stefanovic\n" + DateTime.Now.ToString("dd.MM.yyyy.") + "\nUnesite 'help' za listu svih komandi\n" + "https://www.lukastefanovic.com\n\n");
        }
    }
}
void FgColor(string index)
{
    ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
    for (int i = 0; i <= 15; i++)
    {
        if (i == int.Parse(index))
        {
            Console.ForegroundColor = colors[i];
            Console.Clear();
            Console.WriteLine("TobiScript v" + verzija + "\nAutor: Luka Stefanovic\n" + DateTime.Now.ToString("dd.MM.yyyy.") + "\nUnesite 'help' za listu svih komandi\n" + "https://www.lukastefanovic.com\n\n");
        }
    }
}