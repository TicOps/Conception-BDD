using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===== Salle de Sport =====");
        Console.WriteLine("1. Connexion");
        Console.WriteLine("2. Mode évaluation");
        Console.WriteLine("3. Quitter");
        Console.Write("Votre choix: ");

        var choix = Console.ReadLine();

        switch(choix)
        {
            case "1":
                Console.WriteLine("Connexion (à implémenter)...");
                break;
            case "2":
                Console.WriteLine("Mode évaluation (à implémenter)...");
                break;
            case "3":
                Console.WriteLine("Au revoir !");
                break;
            default:
                Console.WriteLine("Choix invalide.");
                break;
        }
    }
}
