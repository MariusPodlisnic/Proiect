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

        
        public Intrebare()
        {
            Domeniu = string.Empty;
            TextIntrebare = string.Empty;
            variante = new string[0];
            RaspunsCorect = 0;
            Dificultate = Dificultate.Usor;
            TipCunostinte = TipCunostinte.Niciuna;
        }

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
