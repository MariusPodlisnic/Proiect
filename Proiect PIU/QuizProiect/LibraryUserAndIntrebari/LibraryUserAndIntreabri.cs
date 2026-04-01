namespace LibraryUserAndIntrebari
{
    public enum Dificultate
    {
        Usor,
        Mediu,
        Greu
    }

    [Flags]
    public enum TipCunostinte
    {
        Niciuna = 0,
        Teorie = 1,
        Practica = 2,
        Sintaxa = 4,
        Algoritmi = 8
    }

    public class Intrebare
    {
        private const char SEPARATOR = ',';
        private const char SEPARATOR_VARIANTE = '|';

        private string[] variante;

        public int IdIntrebare { get; set; }
        public string Domeniu { get; set; }
        public string TextIntrebare { get; set; }
        public int RaspunsCorect { get; set; }
        public Dificultate Dificultate { get; set; }
        public TipCunostinte TipCunostinte { get; set; }

        public void SetVariante(string[] _variante)
        {
            variante = new string[_variante.Length];
            _variante.CopyTo(variante, 0);
        }

        public string[] GetVariante()
        {
            return (string[])variante.Clone();
        }

        // constructor implicit
        public Intrebare()
        {
            Domeniu = string.Empty;
            TextIntrebare = string.Empty;
            variante = new string[0];
            RaspunsCorect = 0;
            Dificultate = Dificultate.Usor;
            TipCunostinte = TipCunostinte.Niciuna;
        }

        // constructor cu parametri 
        public Intrebare(int idIntrebare, string domeniu, string textIntrebare,
                         string[] _variante, int raspunsCorect)
        {
            IdIntrebare = idIntrebare;
            Domeniu = domeniu;
            TextIntrebare = textIntrebare;
            RaspunsCorect = raspunsCorect;
            variante = new string[_variante.Length];
            _variante.CopyTo(variante, 0);
            Dificultate = Dificultate.Usor;
            TipCunostinte = TipCunostinte.Niciuna;
        }

        // constructor cu parametri(pentru enum)
        public Intrebare(int idIntrebare, string domeniu, string textIntrebare,
                         string[] _variante, int raspunsCorect,
                         Dificultate dificultate, TipCunostinte tipCunostinte)
        {
            IdIntrebare = idIntrebare;
            Domeniu = domeniu;
            TextIntrebare = textIntrebare;
            RaspunsCorect = raspunsCorect;
            variante = new string[_variante.Length];
            _variante.CopyTo(variante, 0);
            Dificultate = dificultate;
            TipCunostinte = tipCunostinte;
        }

        // constructor (pentru citire din fisier text)
        public Intrebare(string linieFisier)
        {
            string[] campuri = linieFisier.Split(SEPARATOR);

            if (campuri.Length < 7)
                throw new Exception("Linie intrebare invalida: " + linieFisier);

            IdIntrebare = int.Parse(campuri[0].Trim());
            Domeniu = campuri[1].Trim();
            TextIntrebare = campuri[2].Trim();

            variante = campuri[3].Split(SEPARATOR_VARIANTE);

            RaspunsCorect = int.Parse(campuri[4].Trim());

            Enum.TryParse(campuri[5].Trim(), out Dificultate dif);
            Dificultate = dif;

            Enum.TryParse(campuri[6].Trim(), out TipCunostinte tip);
            TipCunostinte = tip;
        }


        // conversie la sir pentru scriere in fisier text
        public string ConversieLaSirPentruFisier()
        {
            string sVariante = string.Join(SEPARATOR_VARIANTE.ToString(), variante);
            return $"{IdIntrebare}{SEPARATOR}{Domeniu}{SEPARATOR}{TextIntrebare}{SEPARATOR}{sVariante}{SEPARATOR}{RaspunsCorect}{SEPARATOR}{Dificultate}{SEPARATOR}{TipCunostinte}";
        }

        public string Info()
        {
            string sVariante = string.Empty;
            if (variante != null)
            {
                char litera = 'A';
                foreach (string v in variante)
                {
                    sVariante += $"\n   {litera}) {v}";
                    litera++;
                }
            }

            string info = $"Id:{IdIntrebare} Domeniu:{Domeniu ?? "NECUNOSCUT"}" +
                          $"\n   Intrebare:{TextIntrebare ?? "NECUNOSCUT"}" +
                          $"{sVariante}" +
                          $"\n   Raspuns corect:{(char)('A' + RaspunsCorect)}" +
                          $"\n   Dificultate:{Dificultate}" +
                          $"\n   Tip cunostinte:{TipCunostinte}";
            return info;
        }
    }
}