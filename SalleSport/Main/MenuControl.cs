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
            Console.WriteLine("=== Mode Évaluation ===");
            Console.WriteLine("1. Rapports généraux");
            Console.WriteLine("2. Coachs les plus suivis");
            Console.WriteLine("3. Occupation des cours");
            Console.WriteLine("4. Retour");
            Console.Write("Votre choix : ");

            var choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                case "2":
                case "3":
                    Console.WriteLine("(Rapport non implémenté)");
                    Console.ReadKey();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Choix invalide.");
                    Console.ReadKey();
                    break;
            }
        }

// ---------------------------------------------------------------------------------//
// Affichage du menu admin

    private void ShowAdminMenu(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Menu Administrateur ===");
                Console.WriteLine($"Connecté : {user.Username}");
                Console.WriteLine("1. Gérer les membres");
                Console.WriteLine("2. Gérer les coachs");
                Console.WriteLine("3. Gérer les cours");
                Console.WriteLine("4. Statistiques / Rapports");
                Console.WriteLine("5. Retour");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        ManageMembers();
                        break;
                    case "2":
                        ManageCoachs();
                        break;
                    case "3":
                        ManageCourses();
                        break;
                    case "4":
                        ShowAdminStats();
                        break;
                    case "5":
                        return; // retour au menu précédent
                    default:
                        Console.WriteLine("Choix invalide.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // ----- sous-menus d'administration (placeholders) -----
        private void ManageMembers()
        {
            Console.Clear();
            Console.WriteLine("=== Gérer les membres ===");
            Console.WriteLine("1. Ajouter un membre (non implémenté)");
            Console.WriteLine("2. Valider une inscription (non implémenté)");
            Console.WriteLine("3. Supprimer une adhésion (non implémenté)");
            Console.WriteLine("4. Retour");
            Console.ReadKey();
        }

        private void ManageCoachs()
        {
            Console.Clear();
            Console.WriteLine("=== Gérer les coachs ===");
            Console.WriteLine("1. Ajouter un coach (non implémenté)");
            Console.WriteLine("2. Modifier un coach (non implémenté)");
            Console.WriteLine("3. Supprimer un coach (non implémenté)");
            Console.WriteLine("4. Retour");
            Console.ReadKey();
        }

        private void ManageCourses()
        {
            Console.Clear();
            Console.WriteLine("=== Gérer les cours ===");
            Console.WriteLine("1. Ajouter un cours (non implémenté)");
            Console.WriteLine("2. Modifier un cours (non implémenté)");
            Console.WriteLine("3. Annuler un cours (non implémenté)");
            Console.WriteLine("4. Retour");
            Console.ReadKey();
        }

        private void ShowAdminStats()
        {
            Console.Clear();
            Console.WriteLine("=== Statistiques administrateur ===");
            Console.WriteLine("(Non implémenté)");
            Console.ReadKey();
        }

// ------------------------------------------------------------------------------------------------------------------
    
// Affichage du menu membre
    private void ShowMemberMenu(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Menu Membre ===");
                Console.WriteLine($"Connecté : {user.Username}");
                Console.WriteLine("1. Réserver un cours");
                Console.WriteLine("2. Annuler une réservation");
                Console.WriteLine("3. Voir mon historique");
                Console.WriteLine("4. Modifier mes informations");
                Console.WriteLine("5. Retour");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        Console.WriteLine("(Réservation non implémentée)");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("(Annulation non implémentée)");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.WriteLine("(Historique non implémenté)");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.WriteLine("(Modification non implémentée)");
                        Console.ReadKey();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Choix invalide.");
                        Console.ReadKey();
                        break;
                }
            }
        }


}



