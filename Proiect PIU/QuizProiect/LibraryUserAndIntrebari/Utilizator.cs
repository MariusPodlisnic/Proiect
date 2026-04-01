using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUserAndIntrebari
{
    public class Utilizator
    {
        private const char SEPARATOR = ',';

        public int IdUtilizator { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public int Varsta { get; set; }

        // constructor implicit
        public Utilizator()
        {
            Nume = string.Empty;
            Prenume = string.Empty;
            Varsta = 0;
        }

        // constructor cu parametri
        public Utilizator(int idUtilizator, string nume, string prenume, int varsta)
        {
            IdUtilizator = idUtilizator;
            Nume = nume;
            Prenume = prenume;
            Varsta = varsta;
        }

        // constructor fisier
        public Utilizator(string linieFisier)
        {
            linieFisier = linieFisier.Trim();
            if (string.IsNullOrEmpty(linieFisier))
                throw new ArgumentException("Linie invalida: gol");

            string[] campuri = linieFisier.Split(SEPARATOR);

            if (campuri.Length != 4)
                throw new ArgumentException("Linie invalida: " + linieFisier);

            IdUtilizator = int.Parse(campuri[0]);
            Nume = campuri[1];
            Prenume = campuri[2];
            Varsta = int.Parse(campuri[3]);
        }




        // conversie la sir pentru scriere in fisier text
        public string ConversieLaSirPentruFisier()
        {
            return $"{IdUtilizator}{SEPARATOR}{Nume}{SEPARATOR}{Prenume}{SEPARATOR}{Varsta}";
        }

        public string Info()
        {
            string info = $"Id:{IdUtilizator} Nume:{Nume ?? "NECUNOSCUT"} Prenume:{Prenume ?? "NECUNOSCUT"} Varsta:{Varsta}";
            return info;
        }
    }
}
