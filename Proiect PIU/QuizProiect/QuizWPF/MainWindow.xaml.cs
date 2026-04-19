using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using LibraryUserAndIntrebari;
using StocareData;

namespace QuizWPF
{
    public partial class MainWindow : Window
    {
        private AdministrareIntrebariFisierText _adminIntrebari;

        public MainWindow()
        {
            InitializeComponent();

            // Calculează calea spre fișierul de date (același mecanism ca în consolă)
            string basePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                            .Parent.Parent.Parent.Parent.FullName;
            string caleIntrebari = Path.Combine(basePath, "QuizProiect", "Data", "intrebari.txt");

            _adminIntrebari = new AdministrareIntrebariFisierText(caleIntrebari);
        }

        private void OnCautaIntrebare(object sender, RoutedEventArgs e)
        {
            CardIntrebare.Visibility = Visibility.Collapsed;
            CardMesaj.Visibility = Visibility.Collapsed;

            // Căutare după ID dacă e completat
            if (!string.IsNullOrWhiteSpace(TxtIdIntrebare.Text))
            {
                if (int.TryParse(TxtIdIntrebare.Text.Trim(), out int id))
                {
                    Intrebare intrebare = _adminIntrebari.GetIntrebare(id);
                    if (intrebare != null)
                    {
                        AfiseazaIntrebare(intrebare);
                        return;
                    }
                }
                AfiseazaMesaj($"Nicio întrebare găsită cu ID = {TxtIdIntrebare.Text.Trim()}");
                return;
            }

            // Căutare după domeniu
            if (!string.IsNullOrWhiteSpace(TxtDomeniu.Text))
            {
                var lista = _adminIntrebari.GetIntrebariDupaDomeniu(TxtDomeniu.Text.Trim());
                if (lista.Count > 0)
                {
                    AfiseazaIntrebare(lista[0]); // Afișează prima din listă
                    return;
                }
                AfiseazaMesaj($"Nicio intrebare gasita pentru domeniul '{TxtDomeniu.Text.Trim()}'");
                return;
            }

            AfiseazaMesaj("Introduceți un ID sau un domeniu pentru căutare.");
        }

        private void AfiseazaIntrebare(Intrebare i)
        {
            TbIdIntrebare.Text = i.IdIntrebare.ToString();
            TbDomeniu.Text = i.Domeniu;
            TbTextIntrebare.Text = i.TextIntrebare;

            string[] variante = i.GetVariante();
            TbVariantaA.Text = variante.Length > 0 ? variante[0] : "-";
            TbVariantaB.Text = variante.Length > 1 ? variante[1] : "-";
            TbVariantaC.Text = variante.Length > 2 ? variante[2] : "-";
            TbVariantaD.Text = variante.Length > 3 ? variante[3] : "-";

            // Marchează răspunsul corect cu fundal verde
            ResetVarianteBorder();
            char litera = (char)('A' + i.RaspunsCorect);
            TbRaspunsCorect.Text = litera.ToString();
            HighlightRaspunsCorect(i.RaspunsCorect);

            TbTipCunostinte.Text = i.TipCunostinte.ToString();

            // Badge dificultate
            TbDificultate.Text = i.Dificultate.ToString();
            switch (i.Dificultate)
            {
                case Dificultate.Usor:
                    BadgeDificultate.Background = new SolidColorBrush(Color.FromRgb(212, 237, 218));
                    TbDificultate.Foreground = new SolidColorBrush(Color.FromRgb(21, 87, 36));
                    break;
                case Dificultate.Mediu:
                    BadgeDificultate.Background = new SolidColorBrush(Color.FromRgb(255, 243, 205));
                    TbDificultate.Foreground = new SolidColorBrush(Color.FromRgb(133, 100, 4));
                    break;
                case Dificultate.Greu:
                    BadgeDificultate.Background = new SolidColorBrush(Color.FromRgb(248, 215, 218));
                    TbDificultate.Foreground = new SolidColorBrush(Color.FromRgb(114, 28, 36));
                    break;
            }

            CardIntrebare.Visibility = Visibility.Visible;
        }

        private void ResetVarianteBorder()
        {
            var defaultBrush = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            var defaultBg = new SolidColorBrush(Colors.White);
            foreach (var b in new[] { BorderA, BorderB, BorderC, BorderD })
            {
                b.BorderBrush = defaultBrush;
                b.Background = defaultBg;
                b.BorderThickness = new Thickness(1);
            }
        }

        private void HighlightRaspunsCorect(int index)
        {
            var greenBorder = new SolidColorBrush(Color.FromRgb(40, 167, 69));
            var greenBg = new SolidColorBrush(Color.FromRgb(212, 237, 218));
            var border = index switch
            {
                0 => BorderA,
                1 => BorderB,
                2 => BorderC,
                3 => BorderD,
                _ => null
            };
            if (border != null)
            {
                border.BorderBrush = greenBorder;
                border.Background = greenBg;
                border.BorderThickness = new Thickness(2);
            }
        }

        private void AfiseazaMesaj(string mesaj)
        {
            TbMesaj.Text = mesaj;
            CardMesaj.Visibility = Visibility.Visible;
        }

        // Deschide fereastra de adăugare
        private void OnAdaugaIntrebare(object sender, RoutedEventArgs e)
        {
            var fereastra = new AdaugaIntrebareWindow(_adminIntrebari);
            fereastra.Owner = this;
            fereastra.ShowDialog();
        }
    }
}
