using LibraryUserAndIntrebari;

namespace StocareData
{
    public class AdministrareQuizMemorie
    {
        // vectorul de obiecte în care se salvează datele
        public List<Intrebare> intrebari = new List<Intrebare>();

        // căutare după id (criteriu exact)
        public static Intrebare GetIntrebare(List<Intrebare> intrebari, int idIntrebare)
        {
            foreach (Intrebare intrebare in intrebari)
            {
                if (intrebare.IdIntrebare == idIntrebare)
                {
                    Console.WriteLine("Intrebarea se regaseste in lista!");
                    return intrebare;
                }
            }
            return null;
        }

        // căutare după domeniu (criteriu parțial - returnează listă)
        public static List<Intrebare> GetIntrebariiDupaDomeniu(List<Intrebare> intrebari, string domeniu)
        {
            List<Intrebare> rezultat = new List<Intrebare>();
            foreach (Intrebare intrebare in intrebari)
            {
                if (intrebare.Domeniu == domeniu)
                {
                    Console.WriteLine("Domeniu prezent in lista");
                    rezultat.Add(intrebare);
                }
            }
            return rezultat;
        }

        // citire intrebare de la tastatura
        public static Intrebare CitireIntrebareTastatura()
        {
            Console.WriteLine("Introduceti domeniul (C/C++, Java, Python, General)");
            string domeniu = Console.ReadLine();

            Console.WriteLine("Introduceti textul intrebarii");
            string text = Console.ReadLine();

            Console.WriteLine("Varianta A:");
            string varA = Console.ReadLine();
            Console.WriteLine("Varianta B:");
            string varB = Console.ReadLine();
            Console.WriteLine("Varianta C:");
            string varC = Console.ReadLine();
            Console.WriteLine("Varianta D:");
            string varD = Console.ReadLine();

            string[] variante = new string[] { varA, varB, varC, varD };

            Console.WriteLine("Introduceti indexul raspunsului corect (0=A, 1=B, 2=C, 3=D):");
            int.TryParse(Console.ReadLine(), out int raspunsCorect);

            Intrebare intrebare = new Intrebare(0, domeniu, text, variante, raspunsCorect);
            return intrebare;
        }

        // modificare variante pentru o intrebare după id
        public bool ModificaVarianteIntrebare(string[] varianteNoi, int idIntrebare)
        {
            foreach (Intrebare intrebare in intrebari)
            {
                if (intrebare.IdIntrebare == idIntrebare)
                {
                    Console.WriteLine("Intrebare gasita");
                    intrebare.SetVariante(varianteNoi);
                    return true;
                }
            }
            return false;
        }
    }
}
