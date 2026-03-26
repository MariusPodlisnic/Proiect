using LibraryUserAndIntrebari;

namespace StocareData
{
    public class AdministrareQuizMemorie
    {
        
        public List<Intrebare> intrebari = new List<Intrebare>();

        //cautare dupa id folosing Linq
        public static Intrebare GetIntrebare(List<Intrebare> intrebari, int idIntrebare)
        {
            Intrebare intrebare = intrebari.FirstOrDefault(i => i.IdIntrebare == idIntrebare);
            if (intrebare != null)
                Console.WriteLine("Intrebarea se regaseste in lista!");
            return intrebare;
        }

        //cautare dupa domeniu
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

            Console.WriteLine("Dificultate (Usor, Mediu, Greu):");
            Enum.TryParse(Console.ReadLine(), out Dificultate dificultate);

            Console.WriteLine("Tip cunostinte (Teorie, Practica, Sintaxa, Algoritmi):");
            Enum.TryParse(Console.ReadLine(), out TipCunostinte tipCunostinte);

            Intrebare intrebare = new Intrebare(0, domeniu, text, variante, raspunsCorect, dificultate, tipCunostinte);
            return intrebare;
        }

        // modificarea variantelor din intrebare
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
