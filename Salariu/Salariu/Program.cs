// See https://aka.ms/new-console-template for more information
int ore;
double tarif;
Console.WriteLine("Introduceti numarul de ore lucrate: ");
string ore_ca_string= Console.ReadLine();
int.TryParse(ore_ca_string, out ore);
Console.WriteLine("Introduceti tariful pe ora: ");
string tarif_ca_string = Console.ReadLine();
double.TryParse(tarif_ca_string, out tarif);
double salariu = ore * tarif;
Console.WriteLine($"Salariul este: {salariu}");
if (salariu > 3000)
{
    Console.WriteLine("Salariul este mare");
}
else
{
       Console.WriteLine("Salariul este mic");
}

