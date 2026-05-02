using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LibraryUserAndIntrebari;
using StocareData;

namespace QuizWPF
{
    // ─── ViewModel pentru ListBox ────────────────────────────────────────────
    public class IntrebareViewModel
    {
        public Intrebare Sursa { get; set; }

        public string DisplayId => $"#{Sursa.IdIntrebare}";
        public string DomeniuText => $"📂 {Sursa.Domeniu}";
        public string TextScurt => Sursa.TextIntrebare?.Length > 60
                                      ? Sursa.TextIntrebare.Substring(0, 60) + "…"
                                      : Sursa.TextIntrebare ?? "";
        public string DificultateText => Sursa.Dificultate.ToString();

        public string DificultateColor => Sursa.Dificultate switch
        {
            Dificultate.Usor => "#D4EDDA",
            Dificultate.Mediu => "#FFF3CD",
            Dificultate.Greu => "#F8D7DA",
            _ => "#EEEEEE"
        };

        public string DificultateTextColor => Sursa.Dificultate switch
        {
            Dificultate.Usor => "#155724",
            Dificultate.Mediu => "#856404",
            Dificultate.Greu => "#721C24",
            _ => "#333333"
        };
    }

    // ─── Code-behind MainWindow ──────────────────────────────────────────────
    public partial class MainWindow : Window
    {
        private readonly AdministrareIntrebariFisierText _adminIntrebari;
        private List<Intrebare> _toateIntrebarile = new();
        private Intrebare? _intrebareSelectata;

        // Culori validare
        private static readonly SolidColorBrush BrushLabelNormal = new(Color.FromRgb(58, 58, 92));
        private static readonly SolidColorBrush BrushLabelError = new(Color.FromRgb(220, 53, 69));
        private static readonly SolidColorBrush BrushBorderNormal = new(Color.FromRgb(204, 204, 221));
        private static readonly SolidColorBrush BrushBorderError = new(Color.FromRgb(220, 53, 69));
        private static readonly SolidColorBrush BrushBgNormal = new(Colors.White);
        private static readonly SolidColorBrush BrushBgError = new(Color.FromRgb(255, 245, 245));

        private static string GetBasePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public MainWindow()
        {
            InitializeComponent();

            string caleIntrebari = System.IO.Path.Combine(GetBasePath(), "Data", "intrebari.txt");
            string caleUtilizatori = System.IO.Path.Combine(GetBasePath(), "Data", "utilizatori.txt");

            System.IO.Directory.CreateDirectory(
                System.IO.Path.Combine(GetBasePath(), "Data"));

            _adminIntrebari = new AdministrareIntrebariFisierText(caleIntrebari);
            IncarcaIntrebari();
        }

        // ════════════════════════════════════════════════════════════════════
        //  Încărcare și afișare listă
        // ════════════════════════════════════════════════════════════════════
        private void IncarcaIntrebari()
        {
            _toateIntrebarile = _adminIntrebari.GetIntrebari();
            AplicaFiltre();
        }

        private void AplicaFiltre()
        {
            if (RbUsor == null) return;
            string cautare = TxtCautare.Text.Trim().ToLower();

            // Filtru dificultate din RadioButton
            Dificultate? filtruDif = null;
            if (RbUsor.IsChecked == true) filtruDif = Dificultate.Usor;
            if (RbMediu.IsChecked == true) filtruDif = Dificultate.Mediu;
            if (RbGreu.IsChecked == true) filtruDif = Dificultate.Greu;

            // Filtru tip cunoștințe din CheckBox
            TipCunostinte filtruTip = TipCunostinte.Niciuna;
            if (ChkFiltruTeorie.IsChecked == true) filtruTip |= TipCunostinte.Teorie;
            if (ChkFiltruPractica.IsChecked == true) filtruTip |= TipCunostinte.Practica;
            if (ChkFiltruSintaxa.IsChecked == true) filtruTip |= TipCunostinte.Sintaxa;
            if (ChkFiltruAlgoritmi.IsChecked == true) filtruTip |= TipCunostinte.Algoritmi;

            var filtrat = _toateIntrebarile.AsEnumerable();

            // Text liber (ID, domeniu, text)
            if (!string.IsNullOrEmpty(cautare))
            {
                filtrat = filtrat.Where(q =>
                    q.IdIntrebare.ToString().Contains(cautare) ||
                    (q.Domeniu?.ToLower().Contains(cautare) ?? false) ||
                    (q.TextIntrebare?.ToLower().Contains(cautare) ?? false));
            }

            // Dificultate
            if (filtruDif.HasValue)
                filtrat = filtrat.Where(q => q.Dificultate == filtruDif.Value);

            // Tip cunoștințe (cel puțin unul din cele selectate)
            if (filtruTip != TipCunostinte.Niciuna)
                filtrat = filtrat.Where(q => (q.TipCunostinte & filtruTip) != TipCunostinte.Niciuna);

            var viewModels = filtrat.Select(q => new IntrebareViewModel { Sursa = q }).ToList();
            LstIntrebari.ItemsSource = viewModels;
            TbNrIntrebari.Text = $"{viewModels.Count} întrebări";
        }

        // ════════════════════════════════════════════════════════════════════
        //  Selecție ListBox
        // ════════════════════════════════════════════════════════════════════
        private void OnIntrebareSelectata(object sender, SelectionChangedEventArgs e)
        {
            if (LstIntrebari.SelectedItem is IntrebareViewModel vm)
            {
                _intrebareSelectata = vm.Sursa;
                AfiseazaDetalii(_intrebareSelectata);
                PrecompletezaFormularEdit(_intrebareSelectata);
            }
        }

        private void AfiseazaDetalii(Intrebare q)
        {
            PanelGol.Visibility = Visibility.Collapsed;
            PanelDetalii.Visibility = Visibility.Visible;

            TbIdIntrebare.Text = q.IdIntrebare.ToString();
            TbDomeniu.Text = q.Domeniu;
            TbTextIntrebare.Text = q.TextIntrebare;

            string[] v = q.GetVariante();
            TbVariantaA.Text = v.Length > 0 ? v[0] : "-";
            TbVariantaB.Text = v.Length > 1 ? v[1] : "-";
            TbVariantaC.Text = v.Length > 2 ? v[2] : "-";
            TbVariantaD.Text = v.Length > 3 ? v[3] : "-";

            // Evidențiază răspunsul corect
            var borders = new[] { BorderA, BorderB, BorderC, BorderD };
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].BorderBrush = i == q.RaspunsCorect
                    ? new SolidColorBrush(Color.FromRgb(40, 167, 69))
                    : new SolidColorBrush(Color.FromRgb(221, 221, 221));
                borders[i].BorderThickness = i == q.RaspunsCorect
                    ? new Thickness(2)
                    : new Thickness(1);
            }

            TbRaspunsCorect.Text = ((char)('A' + q.RaspunsCorect)).ToString();
            TbTipCunostinte.Text = q.TipCunostinte == TipCunostinte.Niciuna
                ? "—"
                : q.TipCunostinte.ToString();

            // Badge dificultate
            TbDificultate.Text = q.Dificultate.ToString();
            (BadgeDificultate.Background, TbDificultate.Foreground) = q.Dificultate switch
            {
                Dificultate.Usor => (new SolidColorBrush(Color.FromRgb(212, 237, 218)),
                                      new SolidColorBrush(Color.FromRgb(21, 87, 36))),
                Dificultate.Mediu => (new SolidColorBrush(Color.FromRgb(255, 243, 205)),
                                      new SolidColorBrush(Color.FromRgb(133, 100, 4))),
                Dificultate.Greu => (new SolidColorBrush(Color.FromRgb(248, 215, 218)),
                                      new SolidColorBrush(Color.FromRgb(114, 28, 36))),
                _ => (new SolidColorBrush(Colors.LightGray),
                                      new SolidColorBrush(Colors.Black))
            };
        }

        // ════════════════════════════════════════════════════════════════════
        //  Formular Editare
        // ════════════════════════════════════════════════════════════════════
        private void PrecompletezaFormularEdit(Intrebare q)
        {
            PanelEditGol.Visibility = Visibility.Collapsed;
            PanelEdit.Visibility = Visibility.Visible;
            PanelEditSucces.Visibility = Visibility.Collapsed;
            PanelEditEroare.Visibility = Visibility.Collapsed;

            TxtEditDomeniu.Text = q.Domeniu;
            TxtEditText.Text = q.TextIntrebare;

            string[] v = q.GetVariante();
            TxtEditVarA.Text = v.Length > 0 ? v[0] : "";
            TxtEditVarB.Text = v.Length > 1 ? v[1] : "";
            TxtEditVarC.Text = v.Length > 2 ? v[2] : "";
            TxtEditVarD.Text = v.Length > 3 ? v[3] : "";

            CmbEditRaspuns.SelectedIndex = q.RaspunsCorect;
            CmbEditDificultate.SelectedIndex = (int)q.Dificultate;

            ChkEditTeorie.IsChecked = (q.TipCunostinte & TipCunostinte.Teorie) != 0;
            ChkEditPractica.IsChecked = (q.TipCunostinte & TipCunostinte.Practica) != 0;
            ChkEditSintaxa.IsChecked = (q.TipCunostinte & TipCunostinte.Sintaxa) != 0;
            ChkEditAlgoritmi.IsChecked = (q.TipCunostinte & TipCunostinte.Algoritmi) != 0;

            ResetValidareEdit();
        }

        private void OnSalveazaEdit(object sender, RoutedEventArgs e)
        {
            if (_intrebareSelectata == null) return;
            PanelEditSucces.Visibility = Visibility.Collapsed;
            PanelEditEroare.Visibility = Visibility.Collapsed;

            if (!ValidareFormularEdit()) return;

            _intrebareSelectata.Domeniu = TxtEditDomeniu.Text.Trim();
            _intrebareSelectata.TextIntrebare = TxtEditText.Text.Trim();
            _intrebareSelectata.SetVariante(new[]
            {
                TxtEditVarA.Text.Trim(),
                TxtEditVarB.Text.Trim(),
                TxtEditVarC.Text.Trim(),
                TxtEditVarD.Text.Trim()
            });
            _intrebareSelectata.RaspunsCorect = CmbEditRaspuns.SelectedIndex;
            _intrebareSelectata.Dificultate = CmbEditDificultate.SelectedIndex switch
            {
                1 => Dificultate.Mediu,
                2 => Dificultate.Greu,
                _ => Dificultate.Usor
            };

            TipCunostinte tip = TipCunostinte.Niciuna;
            if (ChkEditTeorie.IsChecked == true) tip |= TipCunostinte.Teorie;
            if (ChkEditPractica.IsChecked == true) tip |= TipCunostinte.Practica;
            if (ChkEditSintaxa.IsChecked == true) tip |= TipCunostinte.Sintaxa;
            if (ChkEditAlgoritmi.IsChecked == true) tip |= TipCunostinte.Algoritmi;
            _intrebareSelectata.TipCunostinte = tip;

            bool ok = _adminIntrebari.UpdateIntrebare(_intrebareSelectata);
            if (ok)
            {
                PanelEditSucces.Visibility = Visibility.Visible;
                IncarcaIntrebari();
                AfiseazaDetalii(_intrebareSelectata);
            }
            else
            {
                PanelEditEroare.Visibility = Visibility.Visible;
            }
        }

        private void OnAnuleazaEdit(object sender, RoutedEventArgs e)
        {
            if (_intrebareSelectata != null)
                PrecompletezaFormularEdit(_intrebareSelectata);
        }

        // ─── Validare formular editare ────────────────────────────────────
        private bool ValidareFormularEdit()
        {
            bool valid = true;
            valid &= ValidareCampEdit(TxtEditDomeniu, LblEditDomeniu, ErrEditDomeniu,
                v => v.Length >= 2 && v.Length <= 50, "Minim 2 caractere.");
            valid &= ValidareCampEdit(TxtEditText, LblEditText, ErrEditText,
                v => v.Length >= 10 && v.Length <= 300, "Minim 10 caractere.");
            valid &= ValidareCampEdit(TxtEditVarA, LblEditVarA, ErrEditVarA,
                v => v.Length >= 1, "Obligatorie.");
            valid &= ValidareCampEdit(TxtEditVarB, LblEditVarB, ErrEditVarB,
                v => v.Length >= 1, "Obligatorie.");
            valid &= ValidareCampEdit(TxtEditVarC, LblEditVarC, ErrEditVarC,
                v => v.Length >= 1, "Obligatorie.");
            valid &= ValidareCampEdit(TxtEditVarD, LblEditVarD, ErrEditVarD,
                v => v.Length >= 1, "Obligatorie.");

            if (CmbEditRaspuns.SelectedIndex < 0)
            {
                LblEditRaspuns.Foreground = BrushLabelError;
                ErrEditRaspuns.Visibility = Visibility.Visible;
                valid = false;
            }
            else
            {
                LblEditRaspuns.Foreground = BrushLabelNormal;
                ErrEditRaspuns.Visibility = Visibility.Collapsed;
            }
            return valid;
        }

        private bool ValidareCampEdit(TextBox txt, Label lbl, TextBlock err,
            Func<string, bool> regula, string mesaj)
        {
            string val = txt.Text.Trim();
            if (!regula(val))
            {
                lbl.Foreground = BrushLabelError;
                err.Text = mesaj;
                err.Visibility = Visibility.Visible;
                txt.BorderBrush = BrushBorderError;
                txt.BorderThickness = new Thickness(2);
                txt.Background = BrushBgError;
                return false;
            }
            lbl.Foreground = BrushLabelNormal;
            err.Visibility = Visibility.Collapsed;
            txt.BorderBrush = BrushBorderNormal;
            txt.BorderThickness = new Thickness(1);
            txt.Background = BrushBgNormal;
            return true;
        }

        private void ResetValidareEdit()
        {
            foreach (var txt in new[] { TxtEditDomeniu, TxtEditText,
                TxtEditVarA, TxtEditVarB, TxtEditVarC, TxtEditVarD })
            {
                txt.BorderBrush = BrushBorderNormal;
                txt.BorderThickness = new Thickness(1);
                txt.Background = BrushBgNormal;
            }
            foreach (var lbl in new[] { LblEditDomeniu, LblEditText,
                LblEditVarA, LblEditVarB, LblEditVarC, LblEditVarD, LblEditRaspuns })
                lbl.Foreground = BrushLabelNormal;
            foreach (var err in new[] { ErrEditDomeniu, ErrEditText,
                ErrEditVarA, ErrEditVarB, ErrEditVarC, ErrEditVarD, ErrEditRaspuns })
                err.Visibility = Visibility.Collapsed;
        }

        // ════════════════════════════════════════════════════════════════════
        //  Ștergere
        // ════════════════════════════════════════════════════════════════════
        private void OnStergeIntrebare(object sender, RoutedEventArgs e)
        {
            if (_intrebareSelectata == null)
            {
                MessageBox.Show("Selectați mai întâi o întrebare.", "Atenție",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string textScurt = _intrebareSelectata.TextIntrebare ?? "";
            if (textScurt.Length > 50)
                textScurt = textScurt.Substring(0, 50) + "...";

            MessageBoxResult result = MessageBox.Show(
                "Sigur doriti sa stergeti intrebarea #" + _intrebareSelectata.IdIntrebare + "?\n\n" + textScurt,
                "Confirmare stergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            // Ștergere: rescrie fișierul fără această întrebare
            var toate = _adminIntrebari.GetIntrebari();
            toate.RemoveAll(q => q.IdIntrebare == _intrebareSelectata.IdIntrebare);

            // Rescriem tot fișierul
            var tip = typeof(AdministrareIntrebariFisierText);
            var field = tip.GetField("numeFisier",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            string cale = (string)field!.GetValue(_adminIntrebari)!;

            System.IO.File.WriteAllLines(cale,
                toate.Select(q => q.ConversieLaSirPentruFisier()));

            _intrebareSelectata = null;
            PanelDetalii.Visibility = Visibility.Collapsed;
            PanelGol.Visibility = Visibility.Visible;
            PanelEdit.Visibility = Visibility.Collapsed;
            PanelEditGol.Visibility = Visibility.Visible;

            IncarcaIntrebari();
            MessageBox.Show("Întrebarea a fost ștearsă.", "Succes",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ════════════════════════════════════════════════════════════════════
        //  Filtre și căutare — event handlers
        // ════════════════════════════════════════════════════════════════════
        private void OnCautareTextChanged(object sender, TextChangedEventArgs e) => AplicaFiltre();
        private void OnFiltruDificultateChanged(object sender, RoutedEventArgs e) => AplicaFiltre();
        private void OnFiltruTipChanged(object sender, RoutedEventArgs e) => AplicaFiltre();

        private void OnResetFiltre(object sender, RoutedEventArgs e)
        {
            TxtCautare.Clear();
            RbToate.IsChecked = true;
            ChkFiltruTeorie.IsChecked = ChkFiltruPractica.IsChecked =
            ChkFiltruSintaxa.IsChecked = ChkFiltruAlgoritmi.IsChecked = false;
            AplicaFiltre();
        }

        // ════════════════════════════════════════════════════════════════════
        //  Meniu
        // ════════════════════════════════════════════════════════════════════
        private void OnAdaugaIntrebare(object sender, RoutedEventArgs e)
        {
            var win = new AdaugaIntrebareWindow(_adminIntrebari) { Owner = this };
            win.ShowDialog();
            IncarcaIntrebari();
        }

        private void OnEditeazaIntrebare(object sender, RoutedEventArgs e)
        {
            if (_intrebareSelectata == null)
            {
                MessageBox.Show("Selectați mai întâi o întrebare din listă.", "Atenție",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            TabPrincipal.SelectedItem = TabEditare;
        }

        private void OnReincarcaLista(object sender, RoutedEventArgs e) => IncarcaIntrebari();

        private void OnCautaDupaId(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Introduceți ID-ul întrebării:", "Caută după ID", "");
            if (string.IsNullOrEmpty(input)) return;
            TxtCautare.Text = input.Trim();
            RbToate.IsChecked = true;
        }

        private void OnCautaDupaDomeniu(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Introduceți domeniul:", "Caută după Domeniu", "");
            if (string.IsNullOrEmpty(input)) return;
            TxtCautare.Text = input.Trim();
            RbToate.IsChecked = true;
        }

        private void OnCautaDupaDificultate(object sender, RoutedEventArgs e)
        {
            // Activează tab-ul de filtrare și focusează pe RadioButton
            TxtCautare.Clear();
            MessageBox.Show("Utilizați butoanele RadioButton din panoul de filtrare din stânga pentru a filtra după dificultate.",
                "Filtrare după Dificultate", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnAfiseazaTot(object sender, RoutedEventArgs e)
        {
            OnResetFiltre(sender, e);
        }

        private void OnCautaIntrebare(object sender, RoutedEventArgs e) => AplicaFiltre();

        private void OnDespre(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Quiz App — Aplicație pentru gestionarea întrebărilor de quiz.\n\n" +
                "Funcționalități:\n" +
                "  • Adăugare întrebări cu validare\n" +
                "  • Editare și ștergere întrebări\n" +
                "  • Căutare și filtrare avansată\n" +
                "  • Persistență în fișier text\n\n" +
                "Tehnologii: C# / WPF / .NET",
                "Despre aplicație", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}