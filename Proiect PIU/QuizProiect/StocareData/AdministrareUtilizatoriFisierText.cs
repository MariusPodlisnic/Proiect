using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibraryUserAndIntrebari;

namespace StocareData
{
    public class AdministrareUtilizatoriFisierText
    {
        private const int ID_PRIMUL_UTILIZATOR = 1;
        private const int INCREMENT = 1;
        private string numeFisier;

        public AdministrareUtilizatoriFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            Stream streamFisierText = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisierText.Close();
        }

        public void AddUtilizator(Utilizator utilizator)
        {
            utilizator.IdUtilizator = GetNextIdUtilizator();

            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(utilizator.ConversieLaSirPentruFisier());
            }
        }

        public List<Utilizator> GetUtilizatori()
        {
            List<Utilizator> utilizatori = new List<Utilizator>();

            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linieFisier;
                while ((linieFisier = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linieFisier))
                        continue;

                    utilizatori.Add(new Utilizator(linieFisier));
                }
            }

            return utilizatori;
        }

        // cautare dupa nume si prenume
        public Utilizator GetUtilizator(string nume, string prenume)
        {
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linieFisier;
                while ((linieFisier = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linieFisier))
                        continue;

                    Utilizator utilizator = new Utilizator(linieFisier);
                    if (string.Equals(utilizator.Nume?.Trim(), nume.Trim(), StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(utilizator.Prenume?.Trim(), prenume.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        return utilizator;
                    }
                }
            }

            return null;
        }


        // cautare dupa id
        public Utilizator GetUtilizator(int idUtilizator)
        {
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linieFisier;
                while ((linieFisier = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linieFisier))
                        continue;

                    Utilizator utilizator = new Utilizator(linieFisier);
                    if (utilizator.IdUtilizator == idUtilizator)
                        return utilizator;
                }
            }

            return null;
        }

        //modificare utilizator
        public bool UpdateUtilizator(Utilizator utilizatorActualizat)
        {
            List<Utilizator> utilizatori = GetUtilizatori();
            bool succes = false;

            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (Utilizator utilizator in utilizatori)
                {
                    Utilizator pentruScris = utilizator;
                    if (utilizator.IdUtilizator == utilizatorActualizat.IdUtilizator)
                        pentruScris = utilizatorActualizat;

                    sw.WriteLine(pentruScris.ConversieLaSirPentruFisier());
                }
                succes = true;
            }

            return succes;
        }

        private int GetNextIdUtilizator()
        {
            List<Utilizator> utilizatori = GetUtilizatori();
            if (utilizatori.Count == 0)
                return ID_PRIMUL_UTILIZATOR;

            return utilizatori.Last().IdUtilizator + INCREMENT;
        }
    }
}
