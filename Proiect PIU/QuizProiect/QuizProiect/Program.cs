using LibraryUserAndIntrebari;
using StocareData;

namespace ProiectPiu
{
    class Program
    {
        public static void Main()
        {
            AdministrareQuizMemorie admin = new AdministrareQuizMemorie();
            Intrebare? intrebareNoua = null;
            string optiune;

            do
            {
                Console.WriteLine("C. Citire informatii intrebare de la tastatura");
                Console.WriteLine("I. Afisarea informatiilor despre ultima intrebare introdusa");
                Console.WriteLine("A. Afisare intrebari din lista");
                Console.WriteLine("S. Salvare intrebare in lista");
                Console.WriteLine("X. Inchidere program");
                Console.WriteLine("F. Cautare intrebare dupa id");
                Console.WriteLine("D. Cautare lista intrebari dupa domeniu");
                Console.WriteLine("M. Modificare variante intrebare");
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
                        Console.WriteLine("Intrebare salvata.");
                        break;

                    case "X":
                        Console.WriteLine("Aplicatia va fi inchisa");
                        return;

                    case "F":
                        Console.WriteLine("Introdu id-ul intrebarii:");
                        int.TryParse(Console.ReadLine(), out int idCautat);
                        AdministrareQuizMemorie.GetIntrebare(admin.intrebari, idCautat);
                        break;

                    case "D":
                        Console.WriteLine("Introduceti domeniul pentru cautare:");
                        string domeniu = Console.ReadLine();
                        AdministrareQuizMemorie.GetIntrebariiDupaDomeniu(admin.intrebari, domeniu);
                        break;

                    case "M":
                        Console.WriteLine("Introdu id-ul intrebarii:");
                        int.TryParse(Console.ReadLine(), out int idIntrebare);
                        Console.WriteLine("Introdu numarul de variante:");
                        int.TryParse(Console.ReadLine(), out int nrVariante);
                        string[] varianteNoi = new string[nrVariante];
                        for (int i = 0; i < nrVariante; i++)
                        {
                            Console.WriteLine($"Varianta {(char)('A' + i)}:");
                            varianteNoi[i] = Console.ReadLine() ?? string.Empty;
                        }
                        admin.ModificaVarianteIntrebare(varianteNoi, idIntrebare);
                        break;

                    default:
                        Console.WriteLine("Optiune inexistenta");
                        break;
                }

            } while (optiune.ToUpper() != "X");

            Console.ReadKey();
        }
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
    }
}






















