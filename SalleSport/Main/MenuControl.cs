using Microsoft.VisualBasic;

public class MenuControl
{

    //Fonction start pour lancer l'appel du programme
    public void Start()
    {
        ShowMainMenu();
    }

    // Fonction ShowMainMenu pour pouvoir afficher le menu principal
    private void ShowMainMenu()
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("===== Salle de Sport =====");
            Console.WriteLine("1. Connexion");
            Console.WriteLine("2. Mode évaluation");
            Console.WriteLine("3. Quitter");
            Console.Write("Votre choix : ");

            var choix = Console.ReadLine();

            switch(choix)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    ShowEvaluationMenu();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Choix invalide, appuyez sur une touche...");
                    Console.ReadKey();
                    break;
            }
        }
    }

// ---------------------------------------------------------------------------------//
    // Passage au menu de connexion
    private void Login()
    {
        Console.Clear();
        Console.WriteLine("=== Connexion ===");

        Console.WriteLine("Nom d'utilisateur :");
        var username = Console.ReadLine();

        Console.WriteLine("Mot de passe :");
        var password = Console.ReadLine();

        var user = CanLogIn(username, password);

        if (user == null)
        {
            Console.WriteLine("Identifiants incorrects.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"==== Bienvenue {user.Username} ====");
        Console.ReadKey();

        // Redirection selon le rôle
        if (user.Role == "ADMIN")
            ShowAdminMenu(user);
        else if (user.Role == "MEMBER")
            ShowMemberMenu(user);
    }


    private User CanLogIn(string username, string password)
    {
        // ---- Fake users for testing ----
        if (username == "admin" && password == "admin")
            return new User { Username = username, Role = "ADMIN" };

        if (username == "member" && password == "1234")
            return new User { Username = username, Role = "MEMBER" };

        return null;
    }


// ---------------------------------------------------------------------------------//

    // Passage au menu d'évaluation
    private void ShowEvaluationMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Mode Evaluation ===");
        Console.WriteLine("(non implémenté)");
        Console.ReadKey();
    }

// ---------------------------------------------------------------------------------//
// Affichage du menu admin

    private void ShowAdminMenu(User user)
    {
        Console.Clear();
        Console.WriteLine("=== Menu Administrateur ===");
        Console.WriteLine($"Connecté en tant que {user.Username}");
        Console.WriteLine("(non implémenté)");
        Console.ReadKey();
    }

// Affichage du menu membre
private void ShowMemberMenu(User user)
{
    Console.Clear();
    Console.WriteLine("=== Menu Membre ===");
    Console.WriteLine($"Connecté en tant que {user.Username}");
    Console.WriteLine("(non implémenté)");
    Console.ReadKey();
}


}



