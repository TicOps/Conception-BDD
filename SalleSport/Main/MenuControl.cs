using System;
using System.Threading;
using Models;
using MySql.Data.MySqlClient;


namespace Main
{
    public class MenuControl
    {

        string connectionString =
            "Server=localhost;" +
            "Database=salle_sport;" +
            "Uid=root;" +
            "Pwd=root;";


        // ===== POINT D'ENTRÉE =====
        public void Start()
        {
            ShowMainMenu();
        }

        // ===== HEADER GLOBAL =====
        private void DrawHeader(string title)
        {
            Console.Clear();

            // Logo ASCII simple
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║        SALLE DE SPORT        ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();

            // Titre du menu
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"► {title}");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------");
        }

        // ===== MENU PRINCIPAL =====
        private void ShowMainMenu()
        {
            while (true)
            {
                DrawHeader("Menu Principal");

                Console.WriteLine("[1] Connexion");
                Console.WriteLine("[2] Mode Évaluation");
                Console.WriteLine("[3] Quitter");
                Console.WriteLine("--------------------------------------------");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        ShowEvaluationMenu();
                        break;
                    case "3":
                        Goodbye();
                        return;
                    default:
                        Error("Choix invalide !");
                        break;
                }
            }
        }

        // ===== SIMULATION LOGIN =====
        private void Login()
        {
            DrawHeader("Connexion");

            Console.Write("Nom d'utilisateur : ");
            var username = Console.ReadLine();

            Console.Write("Mot de passe : ");
            var password = Console.ReadLine();

            Loading("Connexion");

            var user = CanLogIn(username, password);

            if (user == null)
            {
                Error("Identifiants incorrects !");
                return;
            }

            Success($"Bienvenue {user.Username} ({user.Role})");

            if (user.Role == "ADMIN")
                ShowAdminMenu(user);
            else
                ShowMemberMenu(user);
        }

        private User CanLogIn(string email, string password)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                // 1️⃣ Ouvrir connexion
                connection.Open();

                // 2️⃣ Requête SQL (membre)
                string sql = "SELECT idMembre, email FROM Membre WHERE email = @email AND mdp = @mdp";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@mdp", password);

                // 3️⃣ Exécution
                MySqlDataReader reader = cmd.ExecuteReader();

                // 4️⃣ Lecture résultat
                if (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetString(0);
                    user.Username = reader.GetString(1);
                    user.Role = "MEMBER";

                    reader.Close();
                    connection.Close();
                    return user;
                }

                reader.Close();

                // 5️⃣ Test admin (gérant)
                sql = "SELECT idGerant, email FROM Gerant WHERE email = @email AND mdp = @mdp";
                cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@mdp", password);

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetString(0);
                    user.Username = reader.GetString(1);
                    user.Role = "ADMIN";

                    reader.Close();
                    connection.Close();
                    return user;
                }

                reader.Close();
                connection.Close();
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur SQL : " + e.Message);
                connection.Close();
                return null;
            }
        }


        // ===== MENU ADMIN =====
        private void ShowAdminMenu(User user)
        {
            while (true)
            {
                DrawHeader($"Espace Administrateur : {user.Username}");

                Console.WriteLine("[1] Gérer les membres");
                Console.WriteLine("[2] Gérer les coachs");
                Console.WriteLine("[3] Gérer les cours");
                Console.WriteLine("[4] Statistiques");
                Console.WriteLine("[5] Retour");
                Console.WriteLine("--------------------------------------------");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        Placeholder("Gestion membres");
                        break;
                    case "2":
                        Placeholder("Gestion coachs");
                        break;
                    case "3":
                        Placeholder("Gestion cours");
                        break;
                    case "4":
                        Placeholder("Statistiques");
                        break;
                    case "5":
                        return;
                    default:
                        Error("Choix invalide !");
                        break;
                }
            }
        }

        // ===== MENU MEMBRE =====
        private void ShowMemberMenu(User user)
        {
            while (true)
            {
                DrawHeader($"Espace Membre : {user.Username}");

                Console.WriteLine("[1] Réserver un cours");
                Console.WriteLine("[2] Annuler une réservation");
                Console.WriteLine("[3] Voir mon historique");
                Console.WriteLine("[4] Modifier mes informations");
                Console.WriteLine("[5] Retour");
                Console.WriteLine("--------------------------------------------");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        Placeholder("Réservations");
                        break;
                    case "2":
                        Placeholder("Annulation");
                        break;
                    case "3":
                        Placeholder("Historique");
                        break;
                    case "4":
                        Placeholder("Modification infos");
                        break;
                    case "5":
                        return;
                    default:
                        Error("Choix invalide !");
                        break;
                }
            }
        }

        // ===== MODE EVALUATION =====
        private void ShowEvaluationMenu()
        {
            while (true)
            {
                DrawHeader("Mode Évaluation");

                Console.WriteLine("[1] Rapports généraux");
                Console.WriteLine("[2] Coachs les plus suivis");
                Console.WriteLine("[3] Occupation des cours");
                Console.WriteLine("[4] Retour");
                Console.WriteLine("--------------------------------------------");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                    case "2":
                    case "3":
                        Placeholder("Rapports");
                        break;
                    case "4":
                        return;
                    default:
                        Error("Choix invalide !");
                        break;
                }
            }
        }

        // ===== UTILITAIRES VISUELS =====
        private void Placeholder(string title)
        {
            DrawHeader(title);
            Console.WriteLine("(Cette fonctionnalité n'est pas encore implémentée)");
            Pause();
        }

        private void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
            Pause();
        }

        private void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
            Pause();
        }

        private void Loading(string msg)
        {
            Console.Write($"{msg}");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(250);
                Console.Write(".");
            }
            Console.WriteLine();
        }

        private void Pause()
        {
            Console.WriteLine();
            Console.Write("Appuyez sur une touche...");
            Console.ReadKey();
        }

        private void Goodbye()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Fermeture de l'application...");
            Console.ResetColor();
            Thread.Sleep(500);
        }
    }
}
