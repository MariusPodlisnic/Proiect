using LibraryUserAndIntrebari;
using StocareData;
using System;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuizWPF
{
    public partial class AdaugaIntrebareWindow : Window
    {
        // ─── Constante validare ──────────────────────────────────────────────
        private const int DOMENIU_MIN = 2, DOMENIU_MAX = 50;
        private const int TEXT_MIN = 10, TEXT_MAX = 300;
        private const int VARIANTA_MIN = 1, VARIANTA_MAX = 150;

        private readonly AdministrareIntrebariFisierText _adminIntrebari;

        // Perii reutilizabile
        private static readonly SolidColorBrush BrushLabelNormal = new(Color.FromRgb(58, 58, 92));
        private static readonly SolidColorBrush BrushLabelError = new(Color.FromRgb(220, 53, 69));
        private static readonly SolidColorBrush BrushBorderNormal = new(Color.FromRgb(204, 204, 221));
        private static readonly SolidColorBrush BrushBorderError = new(Color.FromRgb(220, 53, 69));
        private static readonly SolidColorBrush BrushBgNormal = new(Colors.White);
        private static readonly SolidColorBrush BrushBgError = new(Color.FromRgb(255, 245, 245));

        public AdaugaIntrebareWindow(AdministrareIntrebariFisierText adminIntrebari)
        {
            InitializeComponent();
            _adminIntrebari = adminIntrebari;
            DpDataCreare.SelectedDate = DateTime.Today;
        }

        // ─── Salvare ─────────────────────────────────────────────────────────
        private void OnSalveaza(object sender, RoutedEventArgs e)
        {
            PanelSucces.Visibility = Visibility.Collapsed;

            if (!ValidareFormular()) return;

            int raspunsCorect = RbA.IsChecked == true ? 0
                              : RbB.IsChecked == true ? 1
                              : RbC.IsChecked == true ? 2
                              : 3; // RbD

            Dificultate dif = CmbDificultate.SelectedIndex switch
            {
                1 => Dificultate.Mediu,
                2 => Dificultate.Greu,
                _ => Dificultate.Usor
            };

            TipCunostinte tip = TipCunostinte.Niciuna;
            if (ChkTeorie.IsChecked == true) tip |= TipCunostinte.Teorie;
            if (ChkPractica.IsChecked == true) tip |= TipCunostinte.Practica;
            if (ChkSintaxa.IsChecked == true) tip |= TipCunostinte.Sintaxa;
            if (ChkAlgoritmi.IsChecked == true) tip |= TipCunostinte.Algoritmi;

            var intrebare = new Intrebare(0,
                TxtDomeniu.Text.Trim(),
                TxtTextIntrebare.Text.Trim(),
                new[] { TxtVarA.Text.Trim(), TxtVarB.Text.Trim(),
                        TxtVarC.Text.Trim(), TxtVarD.Text.Trim() },
                raspunsCorect, dif, tip);

            _adminIntrebari.AddIntrebare(intrebare);
            PanelSucces.Visibility = Visibility.Visible;
            ResetFormular();
        }

        // ─── Validare ────────────────────────────────────────────────────────
        private bool ValidareFormular()
        {
            bool ok = true;

            ok &= ValidareCamp(TxtDomeniu, LblDomeniu, ErrDomeniu,
                v => v.Length >= DOMENIU_MIN && v.Length <= DOMENIU_MAX,
                $"Domeniu invalid ({DOMENIU_MIN}–{DOMENIU_MAX} caractere).");

            ok &= ValidareCampMultiRand(TxtTextIntrebare, LblText, ErrText,
                v => v.Length >= TEXT_MIN && v.Length <= TEXT_MAX,
                $"Textul trebuie să aibă {TEXT_MIN}–{TEXT_MAX} caractere.");

            ok &= ValidareCamp(TxtVarA, LblVarA, ErrVarA,
                v => v.Length >= VARIANTA_MIN && v.Length <= VARIANTA_MAX,
                $"Varianta trebuie să aibă {VARIANTA_MIN}–{VARIANTA_MAX} caractere.");
            ok &= ValidareCamp(TxtVarB, LblVarB, ErrVarB,
                v => v.Length >= VARIANTA_MIN && v.Length <= VARIANTA_MAX,
                $"Varianta trebuie să aibă {VARIANTA_MIN}–{VARIANTA_MAX} caractere.");
            ok &= ValidareCamp(TxtVarC, LblVarC, ErrVarC,
                v => v.Length >= VARIANTA_MIN && v.Length <= VARIANTA_MAX,
                $"Varianta trebuie să aibă {VARIANTA_MIN}–{VARIANTA_MAX} caractere.");
            ok &= ValidareCamp(TxtVarD, LblVarD, ErrVarD,
                v => v.Length >= VARIANTA_MIN && v.Length <= VARIANTA_MAX,
                $"Varianta trebuie să aibă {VARIANTA_MIN}–{VARIANTA_MAX} caractere.");

            // Validare RadioButton răspuns corect
            bool raspunsSelectat = RbA.IsChecked == true || RbB.IsChecked == true
                                || RbC.IsChecked == true || RbD.IsChecked == true;
            if (!raspunsSelectat)
            {
                LblRaspuns.Foreground = BrushLabelError;
                ErrRaspuns.Visibility = Visibility.Visible;
                ok = false;
            }
            else
            {
                LblRaspuns.Foreground = BrushLabelNormal;
                ErrRaspuns.Visibility = Visibility.Collapsed;
            }

            return ok;
        }

        private bool ValidareCamp(TextBox txt, Label lbl, TextBlock err,
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

        private bool ValidareCampMultiRand(TextBox txt, Label lbl, TextBlock err,
            Func<string, bool> regula, string mesaj)
            => ValidareCamp(txt, lbl, err, regula, mesaj);

        // ─── Reset ───────────────────────────────────────────────────────────
        private void ResetFormular()
        {
            TxtDomeniu.Clear();
            TxtTextIntrebare.Clear();
            TxtVarA.Clear(); TxtVarB.Clear();
            TxtVarC.Clear(); TxtVarD.Clear();
            RbA.IsChecked = RbB.IsChecked = RbC.IsChecked = RbD.IsChecked = false;
            CmbDificultate.SelectedIndex = 0;
            ChkTeorie.IsChecked = ChkPractica.IsChecked =
            ChkSintaxa.IsChecked = ChkAlgoritmi.IsChecked = false;
            DpDataCreare.SelectedDate = DateTime.Today;
        }

        private void OnAnuleaza(object sender, RoutedEventArgs e) => Close();
    }
}