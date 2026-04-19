using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LibraryUserAndIntrebari;
using StocareData;

namespace QuizWPF
{
    public partial class AdaugaIntrebareWindow : Window
    {
        // ─── Constante pentru validare ───────────────────────────────────────
        private const int DOMENIU_MIN_LUNGIME    = 2;
        private const int DOMENIU_MAX_LUNGIME    = 50;
        private const int TEXT_MIN_LUNGIME       = 10;
        private const int TEXT_MAX_LUNGIME       = 300;
        private const int VARIANTA_MIN_LUNGIME   = 1;
        private const int VARIANTA_MAX_LUNGIME   = 150;
        // ─────────────────────────────────────────────────────────────────────

        private readonly AdministrareIntrebariFisierText _adminIntrebari;

        // Culori reutilizabile
        private static readonly SolidColorBrush BrushLabelNormal =
            new SolidColorBrush(Color.FromRgb(58, 58, 92));   // #3A3A5C
        private static readonly SolidColorBrush BrushLabelError =
            new SolidColorBrush(Color.FromRgb(220, 53, 69));  // #DC3545
        private static readonly SolidColorBrush BrushBorderNormal =
            new SolidColorBrush(Color.FromRgb(204, 204, 204));
        private static readonly SolidColorBrush BrushBorderError =
            new SolidColorBrush(Color.FromRgb(220, 53, 69));
        private static readonly SolidColorBrush BrushBgNormal =
            new SolidColorBrush(Colors.White);
        private static readonly SolidColorBrush BrushBgError =
            new SolidColorBrush(Color.FromRgb(255, 245, 245));

        public AdaugaIntrebareWindow(AdministrareIntrebariFisierText adminIntrebari)
        {
            InitializeComponent();
            _adminIntrebari = adminIntrebari;
        }

        // ─── Salvare ─────────────────────────────────────────────────────────
        private void OnSalveaza(object sender, RoutedEventArgs e)
        {
            PanelSucces.Visibility = Visibility.Collapsed;

            if (!ValidareFormular())
                return;

            // Construiește obiectul Intrebare
            string domeniu = TxtDomeniu.Text.Trim();
            string text    = TxtTextIntrebare.Text.Trim();
            string[] variante = {
                TxtVarA.Text.Trim(),
                TxtVarB.Text.Trim(),
                TxtVarC.Text.Trim(),
                TxtVarD.Text.Trim()
            };

            int raspunsCorect = CmbRaspuns.SelectedIndex; // 0=A, 1=B, 2=C, 3=D

            Dificultate dificultate = CmbDificultate.SelectedIndex switch
            {
                1 => Dificultate.Mediu,
                2 => Dificultate.Greu,
                _ => Dificultate.Usor
            };

            TipCunostinte tip = TipCunostinte.Niciuna;
            if (ChkTeorie.IsChecked   == true) tip |= TipCunostinte.Teorie;
            if (ChkPractica.IsChecked == true) tip |= TipCunostinte.Practica;
            if (ChkSintaxa.IsChecked  == true) tip |= TipCunostinte.Sintaxa;
            if (ChkAlgoritmi.IsChecked== true) tip |= TipCunostinte.Algoritmi;

            Intrebare intrebare = new Intrebare(0, domeniu, text, variante,
                                                raspunsCorect, dificultate, tip);
            _adminIntrebari.AddIntrebare(intrebare);

            PanelSucces.Visibility = Visibility.Visible;
            ResetFormular();
        }

        // ─── Validare completă ───────────────────────────────────────────────
        private bool ValidareFormular()
        {
            bool valid = true;

            // Domeniu
            valid &= ValidareCamp(
                TxtDomeniu, LblDomeniu, ErrDomeniu,
                val => val.Length >= DOMENIU_MIN_LUNGIME && val.Length <= DOMENIU_MAX_LUNGIME,
                $"Domeniul trebuie să aibă între {DOMENIU_MIN_LUNGIME} și {DOMENIU_MAX_LUNGIME} caractere.");

            // Text întrebare
            valid &= ValidareCampTextBox(
                TxtTextIntrebare, LblText, ErrText,
                val => val.Length >= TEXT_MIN_LUNGIME && val.Length <= TEXT_MAX_LUNGIME,
                $"Textul trebuie să aibă între {TEXT_MIN_LUNGIME} și {TEXT_MAX_LUNGIME} caractere.");

            // Variante
            valid &= ValidareCamp(TxtVarA, LblVarA, ErrVarA,
                val => val.Length >= VARIANTA_MIN_LUNGIME && val.Length <= VARIANTA_MAX_LUNGIME,
                $"Varianta trebuie să aibă între {VARIANTA_MIN_LUNGIME} și {VARIANTA_MAX_LUNGIME} caractere.");
            valid &= ValidareCamp(TxtVarB, LblVarB, ErrVarB,
                val => val.Length >= VARIANTA_MIN_LUNGIME && val.Length <= VARIANTA_MAX_LUNGIME,
                $"Varianta trebuie să aibă între {VARIANTA_MIN_LUNGIME} și {VARIANTA_MAX_LUNGIME} caractere.");
            valid &= ValidareCamp(TxtVarC, LblVarC, ErrVarC,
                val => val.Length >= VARIANTA_MIN_LUNGIME && val.Length <= VARIANTA_MAX_LUNGIME,
                $"Varianta trebuie să aibă între {VARIANTA_MIN_LUNGIME} și {VARIANTA_MAX_LUNGIME} caractere.");
            valid &= ValidareCamp(TxtVarD, LblVarD, ErrVarD,
                val => val.Length >= VARIANTA_MIN_LUNGIME && val.Length <= VARIANTA_MAX_LUNGIME,
                $"Varianta trebuie să aibă între {VARIANTA_MIN_LUNGIME} și {VARIANTA_MAX_LUNGIME} caractere.");

            // Răspuns corect
            if (CmbRaspuns.SelectedIndex < 0)
            {
                LblRaspuns.Foreground = BrushLabelError;
                ErrRaspuns.Visibility = Visibility.Visible;
                valid = false;
            }
            else
            {
                LblRaspuns.Foreground = BrushLabelNormal;
                ErrRaspuns.Visibility = Visibility.Collapsed;
            }

            return valid;
        }

        /// <summary>Validare pentru TextBox cu un singur rând.</summary>
        private bool ValidareCamp(TextBox txtBox, Label label, TextBlock errMsg,
                                   Func<string, bool> regula, string mesajEroare)
        {
            string val = txtBox.Text.Trim();
            if (!regula(val))
            {
                label.Foreground      = BrushLabelError;
                errMsg.Text           = mesajEroare;
                errMsg.Visibility     = Visibility.Visible;
                txtBox.BorderBrush    = BrushBorderError;
                txtBox.BorderThickness= new Thickness(2);
                txtBox.Background     = BrushBgError;
                return false;
            }
            label.Foreground      = BrushLabelNormal;
            errMsg.Visibility     = Visibility.Collapsed;
            txtBox.BorderBrush    = BrushBorderNormal;
            txtBox.BorderThickness= new Thickness(1);
            txtBox.Background     = BrushBgNormal;
            return true;
        }

        /// <summary>Validare pentru TextBox multi-rând (fără stilul inline).</summary>
        private bool ValidareCampTextBox(TextBox txtBox, Label label, TextBlock errMsg,
                                          Func<string, bool> regula, string mesajEroare)
        {
            string val = txtBox.Text.Trim();
            if (!regula(val))
            {
                label.Foreground      = BrushLabelError;
                errMsg.Text           = mesajEroare;
                errMsg.Visibility     = Visibility.Visible;
                txtBox.BorderBrush    = BrushBorderError;
                txtBox.BorderThickness= new Thickness(2);
                txtBox.Background     = BrushBgError;
                return false;
            }
            label.Foreground      = BrushLabelNormal;
            errMsg.Visibility     = Visibility.Collapsed;
            txtBox.BorderBrush    = BrushBorderNormal;
            txtBox.BorderThickness= new Thickness(1);
            txtBox.Background     = BrushBgNormal;
            return true;
        }

        // ─── Reset formular după salvare ─────────────────────────────────────
        private void ResetFormular()
        {
            TxtDomeniu.Clear();
            TxtTextIntrebare.Clear();
            TxtVarA.Clear(); TxtVarB.Clear();
            TxtVarC.Clear(); TxtVarD.Clear();
            CmbRaspuns.SelectedIndex    = -1;
            CmbDificultate.SelectedIndex = 0;
            ChkTeorie.IsChecked   = false;
            ChkPractica.IsChecked = false;
            ChkSintaxa.IsChecked  = false;
            ChkAlgoritmi.IsChecked= false;
        }

        private void OnAnuleaza(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
