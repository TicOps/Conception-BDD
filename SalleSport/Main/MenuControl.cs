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

        // ===== INSTANCES DES CLASSES DE GESTION =====
        private GestionMembre gestionMembre;
        private GestionCours gestionCours;
        private GestionCoach gestionCoach;
        private GestionReservation gestionReservation;

        // ===== CONSTRUCTEUR =====
        public MenuControl()
        {
            gestionMembre = new GestionMembre();
            gestionCours = new GestionCours();
            gestionCoach = new GestionCoach();
            gestionReservation = new GestionReservation();
        }

        // ===== POINT D'ENTRÉE =====
        public void Start()
        {
            ShowMainMenu();
        }

        // ===== HEADER GLOBAL =====
        private void DrawHeader(string title)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║        SALLE DE SPORT        ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();

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
                Console.WriteLine("[2] Inscription");
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
                        Register();
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

            var user = CanLogIn(username ?? "", password ?? "");

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

        private User? CanLogIn(string email, string password)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                // Test membre
                string sql = "SELECT idMembre, email FROM Membre WHERE email = @email AND mdp = @mdp";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@mdp", password);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetInt32(0);
                    user.Username = reader.GetString(1);
                    user.Role = "MEMBER";

                    reader.Close();
                    connection.Close();
                    return user;
                }

                reader.Close();

                // Test admin
                sql = "SELECT idGerant, email FROM Gerant WHERE email = @email AND mdp = @mdp";
                cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@mdp", password);

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetInt32(0);
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
                        MenuGestionMembres();
                        break;
                    case "2":
                        MenuGestionCoachs();
                        break;
                    case "3":
                        MenuGestionCours();
                        break;
                    case "4":
                        AfficherStatistiques();
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
                        ReserverCoursMembre(user.Id);
                        break;
                    case "2":
                        AnnulerReservationMembre(user.Id);
                        break;
                    case "3":
                        AfficherMesReservations(user.Id);
                        break;
                    case "4":
                        ModifierMesInformations(user.Id);
                        break;
                    case "5":
                        return;
                    default:
                        Error("Choix invalide !");
                        break;
                }
            }
        }

        // ===== INSCRIPTION =====
        private void Register()
        {
            DrawHeader("Inscription Membre");

            Console.Write("Email : ");
            string? email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string? mdp = Console.ReadLine();

            Console.Write("Prénom : ");
            string? prenom = Console.ReadLine();

            Console.Write("Nom : ");
            string? nom = Console.ReadLine();

            Console.Write("Adresse : ");
            string? adresse = Console.ReadLine();

            Console.Write("Téléphone (10 chiffres) : ");
            string? telephone = Console.ReadLine();

            bool success = CreatePendingMember(
                email ?? "", mdp ?? "", prenom ?? "", nom ?? "", adresse ?? "", telephone ?? ""
            );

            if (success)
                Success("Inscription enregistrée ! En attente de validation par un administrateur.");
            else
                Error("Erreur lors de l'inscription (email déjà utilisé ?)");
        }

        private bool CreatePendingMember(string email, string mdp, string prenom, string nom, string adresse, string telephone)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = @"
                    INSERT INTO Membre (
                        email, prenom, nom, adresse, mdp, telephone,
                        dateDebut, statutInscription, dateFin
                    )
                    VALUES (
                        @email, @prenom, @nom, @adresse, @mdp, @telephone,
                        CURDATE(), 'EN_ATTENTE', NULL
                    );
                ";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                cmd.Parameters.AddWithValue("@telephone", telephone);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERREUR SQL :");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return false;
            }
        }

        // ===== UTILITAIRES VISUELS =====
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

        // ===== GESTION MEMBRES =====
        private void MenuGestionMembres()
        {
            while (true)
            {
                DrawHeader("Gestion des Membres");
                Console.WriteLine("[1] Voir tous les membres");
                Console.WriteLine("[2] Valider une inscription");
                Console.WriteLine("[3] Modifier un membre");
                Console.WriteLine("[4] Supprimer un membre");
                Console.WriteLine("[5] Retour");
                Console.WriteLine("--------------------------------------------");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherTousMembres();
                        break;
                    case "2":
                        ValiderInscription();
                        break;
                    case "3":
                        ModifierMembre();
                        break;
                    case "4":
                        SupprimerMembre();
                        break;
                    case "5":
                        return;
                    default:
                        Error("Choix invalide !");
                        break;
                }
            }
        }

        private void AfficherTousMembres()
        {
            DrawHeader("Liste des Membres");
            var membres = gestionMembre.ObtenirTousLesMembres();

            if (membres.Count == 0)
            {
                Console.WriteLine("Aucun membre enregistré.");
            }
            else
            {
                Console.WriteLine($"\n{"ID",-5} {"Nom",-20} {"Email",-30} {"Statut",-15}");
                Console.WriteLine(new string('-', 75));

                foreach (var m in membres)
                {
                    Console.WriteLine($"{m.IdMembre,-5} {m.Prenom + " " + m.Nom,-20} {m.Email,-30} {m.StatutInscription,-15}");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        private void ValiderInscription()
        {
            DrawHeader("Valider une Inscription");
            var enAttente = gestionMembre.ObtenirMembresEnAttente();

            if (enAttente.Count == 0)
            {
                Error("Aucune inscription en attente.");
                return;
            }

            Console.WriteLine("\nMembres en attente :\n");
            Console.WriteLine($"{"ID",-5} {"Nom",-20} {"Email",-30} {"Date",-15}");
            Console.WriteLine(new string('-', 75));

            foreach (var m in enAttente)
            {
                string date = m.DateDebut.HasValue ? m.DateDebut.Value.ToString("dd/MM/yyyy") : "N/A";
                Console.WriteLine($"{m.IdMembre,-5} {m.Prenom + " " + m.Nom,-20} {m.Email,-30} {date,-15}");
            }

            Console.Write("\nID du membre à valider (0 pour annuler) : ");
            if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
            {
                if (gestionMembre.ValiderInscription(id))
                    Success("Inscription validée avec succès !");
                else
                    Error("Erreur lors de la validation.");
            }
        }

        private void ModifierMembre()
        {
            DrawHeader("Modifier un Membre");
            Console.Write("ID du membre : ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var membres = gestionMembre.ObtenirTousLesMembres();
                var membre = membres.Find(m => m.IdMembre == id);

                if (membre == null)
                {
                    Error("Membre introuvable !");
                    return;
                }

                Console.WriteLine($"\nModification de : {membre.Prenom} {membre.Nom}");
                Console.WriteLine("(Appuyez sur Entrée pour garder la valeur actuelle)\n");

                Console.Write($"Nouveau prénom [{membre.Prenom}] : ");
                string? prenom = Console.ReadLine();
                if (!string.IsNullOrEmpty(prenom)) membre.Prenom = prenom;

                Console.Write($"Nouveau nom [{membre.Nom}] : ");
                string? nom = Console.ReadLine();
                if (!string.IsNullOrEmpty(nom)) membre.Nom = nom;

                Console.Write($"Nouvelle adresse [{membre.Adresse}] : ");
                string? adresse = Console.ReadLine();
                if (!string.IsNullOrEmpty(adresse)) membre.Adresse = adresse;

                Console.Write($"Nouveau téléphone [{membre.Telephone}] : ");
                string? tel = Console.ReadLine();
                if (!string.IsNullOrEmpty(tel)) membre.Telephone = tel;

                if (gestionMembre.ModifierMembre(membre))
                    Success("Membre modifié avec succès !");
                else
                    Error("Erreur lors de la modification.");
            }
        }

        private void SupprimerMembre()
        {
            DrawHeader("Supprimer un Membre");
            Console.Write("ID du membre à supprimer : ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Êtes-vous sûr ? (O/N) : ");
                if (Console.ReadLine()?.ToUpper() == "O")
                {
                    if (gestionMembre.SupprimerMembre(id))
                        Success("Membre supprimé !");
                    else
                        Error("Erreur lors de la suppression.");
                }
            }
        }

        // ===== GESTION COURS =====
        private void MenuGestionCours()
        {
            while (true)
            {
                DrawHeader("Gestion des Cours");
                Console.WriteLine("[1] Voir tous les cours");
                Console.WriteLine("[2] Ajouter un cours");
                Console.WriteLine("[3] Modifier un cours");
                Console.WriteLine("[4] Supprimer un cours");
                Console.WriteLine("[5] Retour");
                Console.WriteLine("--------------------------------------------");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherTousCours();
                        break;
                    case "2":
                        AjouterCours();
                        break;
                    case "3":
                        ModifierCours();
                        break;
                    case "4":
                        SupprimerCours();
                        break;
                    case "5":
                        return;
                    default:
                        Error("Choix invalide !");
                        break;
                }
            }
        }

        private void AfficherTousCours()
        {
            DrawHeader("Liste des Cours");
            var cours = gestionCours.ObtenirTousLesCours();

            if (cours.Count == 0)
            {
                Console.WriteLine("Aucun cours enregistré.");
            }
            else
            {
                foreach (var c in cours)
                {
                    Console.WriteLine($"\n[{c.IdCours}] {c.NomCours}");
                    Console.WriteLine($"    Horaire: {c.Horaire}");
                    Console.WriteLine($"    Durée: {c.Duree} | Intensité: {c.Intensite} | Niveau: {c.NiveauDifficulte}");
                    Console.WriteLine($"    Capacité: {c.CapaciteMaxCours} places");
                    Console.WriteLine($"    Description: {c.Description}");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        private void AjouterCours()
        {
            DrawHeader("Ajouter un Cours");

            var coachs = gestionCoach.ObtenirTousLesCoachs();
            if (coachs.Count == 0)
            {
                Error("Aucun coach disponible. Ajoutez d'abord un coach.");
                return;
            }

            Console.WriteLine("\nCoachs disponibles :");
            foreach (var coach in coachs)
            {
                Console.WriteLine($"[{coach.IdCoach}] {coach.Prenom} {coach.Nom} - {coach.Specialite}");
            }

            Cours nouveau = new Cours();

            Console.Write("\nNom du cours : ");
            nouveau.NomCours = Console.ReadLine() ?? "";

            Console.Write("Description : ");
            nouveau.Description = Console.ReadLine() ?? "";

            Console.Write("Horaire (ex: Lundi 18h) : ");
            nouveau.Horaire = Console.ReadLine() ?? "";

            Console.Write("Durée (ex: 1h) : ");
            nouveau.Duree = Console.ReadLine() ?? "";

            Console.Write("Intensité (Faible/Moyenne/Forte) : ");
            nouveau.Intensite = Console.ReadLine() ?? "";

            Console.Write("Niveau (Debutant/Intermediaire/Avance) : ");
            nouveau.NiveauDifficulte = Console.ReadLine() ?? "";

            Console.Write("Capacité maximale : ");
            nouveau.CapaciteMaxCours = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("ID du coach : ");
            nouveau.IdCoach = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("ID de la salle (1-3) : ");
            nouveau.IdSalle = int.Parse(Console.ReadLine() ?? "0");

            Loading("Création du cours");

            if (gestionCours.AjouterCours(nouveau))
                Success("Cours ajouté avec succès !");
            else
                Error("Erreur lors de l'ajout du cours.");
        }

        private void ModifierCours()
        {
            DrawHeader("Modifier un Cours");
            AfficherTousCours();

            Console.Write("\nID du cours à modifier : ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var coursList = gestionCours.ObtenirTousLesCours();
                var cours = coursList.Find(c => c.IdCours == id);

                if (cours == null)
                {
                    Error("Cours introuvable !");
                    return;
                }

                Console.WriteLine($"\nModification de : {cours.NomCours}");
                Console.WriteLine("(Appuyez sur Entrée pour garder la valeur actuelle)\n");

                Console.Write($"Nouveau nom [{cours.NomCours}] : ");
                string? nom = Console.ReadLine();
                if (!string.IsNullOrEmpty(nom)) cours.NomCours = nom;

                Console.Write($"Nouvelle description [{cours.Description}] : ");
                string? desc = Console.ReadLine();
                if (!string.IsNullOrEmpty(desc)) cours.Description = desc;

                Console.Write($"Nouvel horaire [{cours.Horaire}] : ");
                string? horaire = Console.ReadLine();
                if (!string.IsNullOrEmpty(horaire)) cours.Horaire = horaire;

                if (gestionCours.ModifierCours(cours))
                    Success("Cours modifié avec succès !");
                else
                    Error("Erreur lors de la modification.");
            }
        }

        private void SupprimerCours()
        {
            DrawHeader("Supprimer un Cours");
            AfficherTousCours();

            Console.Write("\nID du cours à supprimer : ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Êtes-vous sûr ? (O/N) : ");
                if (Console.ReadLine()?.ToUpper() == "O")
                {
                    if (gestionCours.SupprimerCours(id))
                        Success("Cours supprimé !");
                    else
                        Error("Erreur lors de la suppression.");
                }
            }
        }

        // ===== GESTION COACHS =====
        private void MenuGestionCoachs()
        {
            while (true)
            {
                DrawHeader("Gestion des Coachs");
                Console.WriteLine("[1] Voir tous les coachs");
                Console.WriteLine("[2] Ajouter un coach");
                Console.WriteLine("[3] Modifier un coach");
                Console.WriteLine("[4] Supprimer un coach");
                Console.WriteLine("[5] Retour");
                Console.WriteLine("--------------------------------------------");
                Console.Write("Votre choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherTousCoachs();
                        break;
                    case "2":
                        AjouterCoach();
                        break;
                    case "3":
                        ModifierCoach();
                        break;
                    case "4":
                        SupprimerCoach();
                        break;
                    case "5":
                        return;
                    default:
                        Error("Choix invalide !");
                        break;
                }
            }
        }

        private void AfficherTousCoachs()
        {
            DrawHeader("Liste des Coachs");
            var coachs = gestionCoach.ObtenirTousLesCoachs();

            if (coachs.Count == 0)
            {
                Console.WriteLine("Aucun coach enregistré.");
            }
            else
            {
                Console.WriteLine($"\n{"ID",-5} {"Nom",-20} {"Spécialité",-20} {"Email",-30}");
                Console.WriteLine(new string('-', 80));

                foreach (var c in coachs)
                {
                    Console.WriteLine($"{c.IdCoach,-5} {c.Prenom + " " + c.Nom,-20} {c.Specialite,-20} {c.Email,-30}");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        private void AjouterCoach()
        {
            DrawHeader("Ajouter un Coach");

            Coach nouveau = new Coach();

            Console.Write("Email : ");
            nouveau.Email = Console.ReadLine() ?? "";

            Console.Write("Prénom : ");
            nouveau.Prenom = Console.ReadLine() ?? "";

            Console.Write("Nom : ");
            nouveau.Nom = Console.ReadLine() ?? "";

            Console.Write("Spécialité : ");
            nouveau.Specialite = Console.ReadLine() ?? "";

            Console.Write("Formations : ");
            nouveau.Formations = Console.ReadLine() ?? "";

            Console.Write("Téléphone : ");
            nouveau.Telephone = Console.ReadLine() ?? "";

            Loading("Ajout du coach");

            if (gestionCoach.AjouterCoach(nouveau))
                Success("Coach ajouté avec succès !");
            else
                Error("Erreur lors de l'ajout.");
        }

        private void ModifierCoach()
        {
            DrawHeader("Modifier un Coach");
            AfficherTousCoachs();

            Console.Write("\nID du coach : ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var coachs = gestionCoach.ObtenirTousLesCoachs();
                var coach = coachs.Find(c => c.IdCoach == id);

                if (coach == null)
                {
                    Error("Coach introuvable !");
                    return;
                }

                Console.WriteLine($"\nModification de : {coach.Prenom} {coach.Nom}");
                Console.WriteLine("(Appuyez sur Entrée pour garder la valeur actuelle)\n");

                Console.Write($"Nouveau prénom [{coach.Prenom}] : ");
                string? prenom = Console.ReadLine();
                if (!string.IsNullOrEmpty(prenom)) coach.Prenom = prenom;

                Console.Write($"Nouveau nom [{coach.Nom}] : ");
                string? nom = Console.ReadLine();
                if (!string.IsNullOrEmpty(nom)) coach.Nom = nom;

                Console.Write($"Nouvelle spécialité [{coach.Specialite}] : ");
                string? spec = Console.ReadLine();
                if (!string.IsNullOrEmpty(spec)) coach.Specialite = spec;

                if (gestionCoach.ModifierCoach(coach))
                    Success("Coach modifié avec succès !");
                else
                    Error("Erreur lors de la modification.");
            }
        }

        private void SupprimerCoach()
        {
            DrawHeader("Supprimer un Coach");
            AfficherTousCoachs();

            Console.Write("\nID du coach à supprimer : ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Êtes-vous sûr ? (O/N) : ");
                if (Console.ReadLine()?.ToUpper() == "O")
                {
                    if (gestionCoach.SupprimerCoach(id))
                        Success("Coach supprimé !");
                    else
                        Error("Erreur : ce coach a peut-être des cours assignés.");
                }
            }
        }

        // ===== STATISTIQUES =====
        private void AfficherStatistiques()
        {
            DrawHeader("Statistiques");

            var membres = gestionMembre.ObtenirTousLesMembres();
            var cours = gestionCours.ObtenirTousLesCours();
            var coachs = gestionCoach.ObtenirTousLesCoachs();

            Console.WriteLine($"\n📊 Nombre total de membres : {membres.Count}");
            Console.WriteLine($"   - Validés : {membres.FindAll(m => m.StatutInscription == "VALIDE").Count}");
            Console.WriteLine($"   - En attente : {membres.FindAll(m => m.StatutInscription == "EN_ATTENTE").Count}");

            Console.WriteLine($"\n📅 Nombre de cours : {cours.Count}");
            Console.WriteLine($"👨‍🏫 Nombre de coachs : {coachs.Count}");

            Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        // ===== FONCTIONS MEMBRE =====
        private void ReserverCoursMembre(int idMembre)
        {
            DrawHeader("Réserver un Cours");

            var membres = gestionMembre.ObtenirTousLesMembres();
            var membre = membres.Find(m => m.IdMembre == idMembre);

            if (membre == null || membre.StatutInscription != "VALIDE")
            {
                Error("Votre inscription doit être validée par un administrateur avant de réserver un cours.");
                return;
            }

            var cours = gestionCours.ObtenirTousLesCours();

            if (cours.Count == 0)
            {
                Error("Aucun cours disponible.");
                return;
            }

            Console.WriteLine("\nCours disponibles :\n");
            foreach (var c in cours)
            {
                Console.WriteLine($"\n[{c.IdCours}] {c.NomCours}");
                Console.WriteLine($"    Horaire: {c.Horaire}");
                Console.WriteLine($"    Durée: {c.Duree} | Intensité: {c.Intensite}");
                Console.WriteLine($"    Capacité: {c.CapaciteMaxCours} places");
            }

            Console.Write("\n\nID du cours à réserver (0 pour annuler) : ");
            if (int.TryParse(Console.ReadLine(), out int idCours) && idCours > 0)
            {
                Loading("Réservation en cours");

                if (gestionReservation.ReserverCours(idMembre, idCours))
                    Success("Cours réservé avec succès !");
                else
                    Error("Erreur : Le cours est peut-être complet ou vous êtes déjà inscrit.");
            }
        }

        private void AnnulerReservationMembre(int idMembre)
        {
            DrawHeader("Annuler une Réservation");

            var reservations = gestionReservation.ObtenirReservationsMembre(idMembre);

            if (reservations.Count == 0)
            {
                Error("Vous n'avez aucune réservation à annuler.");
                return;
            }

            Console.WriteLine("\nVos réservations :\n");
            Console.WriteLine($"{"ID",-5} {"Cours",-25} {"Date",-20}");
            Console.WriteLine(new string('-', 60));

            foreach (var r in reservations)
            {
                Console.WriteLine($"{r.IdCours,-5} {r.NomCours,-25} {r.DateHeureDebut.ToString("dd/MM/yyyy HH:mm"),-20}");
            }

            Console.Write("\nID du cours à annuler (0 pour revenir) : ");
            if (int.TryParse(Console.ReadLine(), out int idCours) && idCours > 0)
            {
                Console.Write("Êtes-vous sûr ? (O/N) : ");
                if (Console.ReadLine()?.ToUpper() == "O")
                {
                    if (gestionReservation.AnnulerReservation(idMembre, idCours))
                        Success("Réservation annulée !");
                    else
                        Error("Erreur lors de l'annulation.");
                }
            }
        }

        private void AfficherMesReservations(int idMembre)
        {
            DrawHeader("Mon Historique de Réservations");

            var reservations = gestionReservation.ObtenirReservationsMembre(idMembre);

            if (reservations.Count == 0)
            {
                Console.WriteLine("Vous n'avez aucune réservation.");
            }
            else
            {
                Console.WriteLine($"\n{"ID",-5} {"Cours",-30} {"Statut",-15} {"Date",-20}");
                Console.WriteLine(new string('-', 80));

                foreach (var r in reservations)
                {
                    Console.WriteLine($"{r.IdCours,-5} {r.NomCours,-30} {r.StatutReservation,-15} {r.DateHeureDebut.ToString("dd/MM/yyyy HH:mm"),-20}");
                }
            }

            Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        private void ModifierMesInformations(int idMembre)
        {
            DrawHeader("Modifier mes Informations");

            var membres = gestionMembre.ObtenirTousLesMembres();
            var membre = membres.Find(m => m.IdMembre == idMembre);

            if (membre == null)
            {
                Error("Erreur : membre introuvable.");
                return;
            }

            Console.WriteLine($"\nInformations actuelles :");
            Console.WriteLine($"  Prénom   : {membre.Prenom}");
            Console.WriteLine($"  Nom      : {membre.Nom}");
            Console.WriteLine($"  Email    : {membre.Email}");
            Console.WriteLine($"  Adresse  : {membre.Adresse}");
            Console.WriteLine($"  Téléphone: {membre.Telephone}");

            Console.WriteLine("\n(Appuyez sur Entrée pour garder la valeur actuelle)\n");

            Console.Write($"Nouveau prénom [{membre.Prenom}] : ");
            string? prenom = Console.ReadLine();
            if (!string.IsNullOrEmpty(prenom)) membre.Prenom = prenom;

            Console.Write($"Nouveau nom [{membre.Nom}] : ");
            string? nom = Console.ReadLine();
            if (!string.IsNullOrEmpty(nom)) membre.Nom = nom;

            Console.Write($"Nouvelle adresse [{membre.Adresse}] : ");
            string? adresse = Console.ReadLine();
            if (!string.IsNullOrEmpty(adresse)) membre.Adresse = adresse;

            Console.Write($"Nouveau téléphone [{membre.Telephone}] : ");
            string? tel = Console.ReadLine();
            if (!string.IsNullOrEmpty(tel)) membre.Telephone = tel;

            Loading("Mise à jour");

            if (gestionMembre.ModifierMembre(membre))
                Success("Vos informations ont été mises à jour !");
            else
                Error("Erreur lors de la mise à jour.");
        }
    }
}