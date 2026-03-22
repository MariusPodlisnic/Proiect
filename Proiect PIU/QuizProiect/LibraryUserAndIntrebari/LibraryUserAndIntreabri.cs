namespace LibraryUserAndIntrebari
{
    public class Intrebare
    {
        // data membră privată
        private string[] variante;

        // proprietăți auto-implemented
        public int IdIntrebare { get; set; }
        public string Domeniu { get; set; }
        public string TextIntrebare { get; set; }
        public int RaspunsCorect { get; set; } // indexul variantei corecte (0=A, 1=B, ...)

        public void SetVariante(string[] _variante)
        {
            variante = new string[_variante.Length];
            _variante.CopyTo(variante, 0);
        }

        public string[] GetVariante()
        {
            // returnează o copie pentru a proteja datele interne
            return (string[])variante.Clone();
        }

        // constructor implicit
        public Intrebare()
        {
            Domeniu = string.Empty;
            TextIntrebare = string.Empty;
            variante = new string[0];
            RaspunsCorect = 0;
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
                          $"\n   Raspuns corect:{(char)('A' + RaspunsCorect)}";
            return info;
        }
    }
}
