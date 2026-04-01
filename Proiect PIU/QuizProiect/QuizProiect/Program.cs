using LibraryUserAndIntrebari;
using StocareData;

namespace ProiectPiu
{
    class Program
    {
        public static void Main()
        {
            AdministrareQuizMemorie admin = new AdministrareQuizMemorie();
            string basePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            string caleUtilizatori = Path.Combine(basePath, "Data", "utilizatori.txt");
            string caleIntrebari = Path.Combine(basePath, "Data", "intrebari.txt");

            AdministrareUtilizatoriFisierText adminUtilizatori =
                new AdministrareUtilizatoriFisierText(caleUtilizatori);

            AdministrareIntrebariFisierText adminIntrebari =
                new AdministrareIntrebariFisierText(caleIntrebari); 

            Intrebare? intrebareNoua = null;
            Utilizator? utilizatorNou = null;
            string optiune;

            do
            {
                Console.WriteLine("--- INTREBARI (memorie) ---");
                Console.WriteLine("C. Citire intrebare de la tastatura");
                Console.WriteLine("I. Afisare ultima intrebare introdusa");
                Console.WriteLine("A. Afisare intrebari din lista memorie");
                Console.WriteLine("S. Salvare intrebare in lista memorie");
                Console.WriteLine("F. Cautare intrebare dupa id (memorie)");
                Console.WriteLine("D. Cautare intrebari dupa domeniu (memorie)");
                Console.WriteLine("M. Modificare variante intrebare (memorie)");
                Console.WriteLine("--- INTREBARI (fisier) ---");
                Console.WriteLine("W. Salvare intrebare in fisier");
                Console.WriteLine("E. Afisare intrebari din fisier");
                Console.WriteLine("R. Cautare intrebare dupa id (fisier)");
                Console.WriteLine("T. Cautare intrebari dupa domeniu (fisier)");
                Console.WriteLine("U. Modificare intrebare in fisier");
                Console.WriteLine("--- UTILIZATORI (fisier) ---");
                Console.WriteLine("P. Citire utilizator de la tastatura si salvare in fisier");
                Console.WriteLine("L. Afisare utilizatori din fisier");
                Console.WriteLine("K. Cautare utilizator dupa nume si prenume (fisier)");
                Console.WriteLine("X. Inchidere program");
                Console.WriteLine("Alegeti o optiune");
                optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

                switch (optiune)
                {
                   
                    case "C":
                        intrebareNoua = AdministrareQuizMemorie.CitireIntrebareTastatura();
                        break;

                    case "I":
                        AfisareIntrebare(intrebareNoua);
                        break;

                    case "A":
                        AfisareIntrebari(admin.intrebari);
                        break;

                    case "S":
                        intrebareNoua.IdIntrebare = admin.intrebari.Count + 1;
                        admin.intrebari.Add(intrebareNoua);
                        Console.WriteLine("Intrebare salvata in memorie.");
                        break;

                    case "F":
                        Console.WriteLine("Introdu id-ul intrebarii:");
                        int.TryParse(Console.ReadLine(), out int idCautat);
                        AdministrareQuizMemorie.GetIntrebare(admin.intrebari, idCautat);
                        break;

                    case "D":
                        Console.WriteLine("Introduceti domeniul pentru cautare:");
                        string domeniu = Console.ReadLine();
                        List<Intrebare> rezultatDomeniu = AdministrareQuizMemorie.GetIntrebariiDupaDomeniu(admin.intrebari, domeniu);
                        AfisareIntrebari(rezultatDomeniu);
                        break;

                    case "M":
                        Console.WriteLine("Introdu id-ul intrebarii:");
                        int.TryParse(Console.ReadLine(), out int idIntrebare);
                        Console.WriteLine("Varianta A:"); string vA = Console.ReadLine();
                        Console.WriteLine("Varianta B:"); string vB = Console.ReadLine();
                        Console.WriteLine("Varianta C:"); string vC = Console.ReadLine();
                        Console.WriteLine("Varianta D:"); string vD = Console.ReadLine();
                        admin.ModificaVarianteIntrebare(new string[] { vA, vB, vC, vD }, idIntrebare);
                        break;

                    
                    case "W":
                        if (intrebareNoua == null)
                        {
                            Console.WriteLine("Mai intii introduceti datele! (optiunea C)");
                            break;
                        }
                        adminIntrebari.AddIntrebare(intrebareNoua);
                        Console.WriteLine("Intrebare salvata in fisier.");
                        break;

                    case "E":
                        AfisareIntrebari(adminIntrebari.GetIntrebari());
                        break;

                    case "R":
                        Console.WriteLine("Introdu id-ul intrebarii:");
                        int.TryParse(Console.ReadLine(), out int idFisier);
                        Intrebare gasitaFisier = adminIntrebari.GetIntrebare(idFisier);
                        AfisareIntrebare(gasitaFisier);
                        break;

                    case "T":
                        Console.WriteLine("Introduceti domeniul pentru cautare:");
                        string domeniuFisier = Console.ReadLine();
                        AfisareIntrebari(adminIntrebari.GetIntrebariDupaDomeniu(domeniuFisier));
                        break;

                    case "U":
                        Console.WriteLine("Introdu id-ul intrebarii de modificat:");
                        int.TryParse(Console.ReadLine(), out int idModificat);
                        Intrebare intrebareDeModificat = adminIntrebari.GetIntrebare(idModificat);
                        if (intrebareDeModificat == null)
                        {
                            Console.WriteLine("Intrebarea nu a fost gasita.");
                            break;
                        }
                        Console.WriteLine("Varianta A noua:"); string nvA = Console.ReadLine();
                        Console.WriteLine("Varianta B noua:"); string nvB = Console.ReadLine();
                        Console.WriteLine("Varianta C noua:"); string nvC = Console.ReadLine();
                        Console.WriteLine("Varianta D noua:"); string nvD = Console.ReadLine();
                        intrebareDeModificat.SetVariante(new string[] { nvA, nvB, nvC, nvD });
                        adminIntrebari.UpdateIntrebare(intrebareDeModificat);
                        Console.WriteLine("Intrebare modificata in fisier.");
                        break;

                    
                    case "P":
                        utilizatorNou = CitireUtilizatorTastatura();
                        adminUtilizatori.AddUtilizator(utilizatorNou);
                        Console.WriteLine("Utilizator salvat in fisier.");
                        break;

                    case "L":
                        AfisareUtilizatori(adminUtilizatori.GetUtilizatori());
                        break;

                    case "K":
                        Console.WriteLine("Introduceti numele:");
                        string numeC = Console.ReadLine();
                        Console.WriteLine("Introduceti prenumele:");
                        string prenumeC = Console.ReadLine();
                        Utilizator utilizatorGasit = adminUtilizatori.GetUtilizator(numeC, prenumeC);
                        AfisareUtilizator(utilizatorGasit);
                        break;

                    case "X":
                        Console.WriteLine("Aplicatia va fi inchisa");
                        return;

                    default:
                        Console.WriteLine("Optiune inexistenta");
                        break;
                }

            } while (optiune.ToUpper() != "X");

            Console.ReadKey();
        }

        //citire
        public static Utilizator CitireUtilizatorTastatura()
        {
            Console.WriteLine("Introduceti numele");
            string nume = Console.ReadLine();
            Console.WriteLine("Introduceti prenumele");
            string prenume = Console.ReadLine();
            Console.WriteLine("Introduceti varsta");
            int.TryParse(Console.ReadLine(), out int varsta);
            return new Utilizator(0, nume, prenume, varsta);
        }

        //afisare intrebare
        public static void AfisareIntrebare(Intrebare intrebare)
        {
            if (intrebare == null)
            {
                Console.WriteLine("Mai intii introduceti datele!");
                return;
            }
            Console.WriteLine(intrebare.Info());
        }

        public static void AfisareIntrebari(List<Intrebare> intrebari)
        {
            Console.WriteLine("Intrebarile sunt:");
            foreach (Intrebare intrebare in intrebari)
            {
                AfisareIntrebare(intrebare);
            }
        }

        //afisare utilizator
        public static void AfisareUtilizator(Utilizator utilizator)
        {
            if (utilizator == null)
            {
                Console.WriteLine("Utilizatorul nu a fost gasit!");
                return;
            }
            Console.WriteLine(utilizator.Info());
        }

        public static void AfisareUtilizatori(List<Utilizator> utilizatori)
        {
            Console.WriteLine("Utilizatorii sunt:");
            foreach (Utilizator utilizator in utilizatori)
            {
                AfisareUtilizator(utilizator);
            }
        }
    }
}