using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryUserAndIntrebari;

namespace StocareData
{
    public class AdministrareIntrebariFisierText
    {
        private const int ID_PRIMA_INTREBARE = 1;
        private const int INCREMENT = 1;
        private string numeFisier;

        public AdministrareIntrebariFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            Stream streamFisierText = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisierText.Close();
        }

        public void AddIntrebare(Intrebare intrebare)
        {
            intrebare.IdIntrebare = GetNextIdIntrebare();

            using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, true))
            {
                streamWriterFisierText.WriteLine(intrebare.ConversieLaSirPentruFisier());
            }
        }

        public List<Intrebare> GetIntrebari()
        {
            List<Intrebare> intrebari = new List<Intrebare>();

            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;

                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linieFisier))
                        continue;

                    intrebari.Add(new Intrebare(linieFisier));
                }
            }

            return intrebari;
        }


        // cautare dupa id
        public Intrebare GetIntrebare(int idIntrebare)
        {
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;

                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linieFisier))
                        continue;

                    Intrebare intrebare = new Intrebare(linieFisier);
                    if (intrebare.IdIntrebare == idIntrebare)
                        return intrebare;
                }

            }

            return null;
        }

        // cautare dupa domeniu
        public List<Intrebare> GetIntrebariDupaDomeniu(string domeniu)
        {
            List<Intrebare> rezultat = new List<Intrebare>();

            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;

                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linieFisier))
                        continue;

                    Intrebare intrebare = new Intrebare(linieFisier);
                    if (intrebare.Domeniu == domeniu)
                        rezultat.Add(intrebare);
                }

            }

            return rezultat;
        }

        // modificare intrebare
        public bool UpdateIntrebare(Intrebare intrebareActualizata)
        {
            List<Intrebare> intrebari = GetIntrebari();
            bool actualizareCuSucces = false;

            using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, false))
            {
                foreach (Intrebare intrebare in intrebari)
                {
                    Intrebare intrebarePentruScris = intrebare;
                    if (intrebare.IdIntrebare == intrebareActualizata.IdIntrebare)
                    {
                        intrebarePentruScris = intrebareActualizata;
                    }
                    streamWriterFisierText.WriteLine(intrebarePentruScris.ConversieLaSirPentruFisier());
                }
                actualizareCuSucces = true;
            }

            return actualizareCuSucces;
        }

        private int GetNextIdIntrebare()
        {
            List<Intrebare> intrebari = GetIntrebari();

            if (intrebari.Count == 0)
                return ID_PRIMA_INTREBARE;

            return intrebari.Last().IdIntrebare + INCREMENT;
        }
    }
}