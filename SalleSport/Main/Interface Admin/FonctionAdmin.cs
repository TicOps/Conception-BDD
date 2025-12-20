
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Models;

namespace Main
{
    public class FonctionAdmin
    {
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
                string prenom = Console.ReadLine();
                if (!string.IsNullOrEmpty(prenom)) membre.Prenom = prenom;

                Console.Write($"Nouveau nom [{membre.Nom}] : ");
                string nom = Console.ReadLine();
                if (!string.IsNullOrEmpty(nom)) membre.Nom = nom;

                Console.Write($"Nouvelle adresse [{membre.Adresse}] : ");
                string adresse = Console.ReadLine();
                if (!string.IsNullOrEmpty(adresse)) membre.Adresse = adresse;

                Console.Write($"Nouveau téléphone [{membre.Telephone}] : ");
                string tel = Console.ReadLine();
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
                    Console.WriteLine($"    Coach: {c.NomCoach}");
                    Console.WriteLine($"    Horaire: {c.Horaire}");
                    Console.WriteLine($"    Durée: {c.Duree} | Intensité: {c.Intensite} | Niveau: {c.NiveauDifficulte}");
                    Console.WriteLine($"    Capacité: {c.CapaciteMaxCours} places");
                    Console.WriteLine($"    {c.Description}");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        private void AjouterCours()
        {
            DrawHeader("Ajouter un Cours");

            // Afficher les coachs disponibles
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
            nouveau.NomCours = Console.ReadLine();

            Console.Write("Description : ");
            nouveau.Description = Console.ReadLine();

            Console.Write("Horaire (ex: Lundi 18h) : ");
            nouveau.Horaire = Console.ReadLine();

            Console.Write("Durée (ex: 1h) : ");
            nouveau.Duree = Console.ReadLine();

            Console.Write("Intensité (Faible/Moyenne/Forte) : ");
            nouveau.Intensite = Console.ReadLine();

            Console.Write("Niveau (Debutant/Intermediaire/Avance) : ");
            nouveau.NiveauDifficulte = Console.ReadLine();

            Console.Write("Capacité maximale : ");
            nouveau.CapaciteMaxCours = int.Parse(Console.ReadLine());

            Console.Write("ID du coach : ");
            nouveau.IdCoach = int.Parse(Console.ReadLine());

            Console.Write("ID de la salle (1-3) : ");
            nouveau.IdSalle = int.Parse(Console.ReadLine());

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
                string nom = Console.ReadLine();
                if (!string.IsNullOrEmpty(nom)) cours.NomCours = nom;

                Console.Write($"Nouvelle description [{cours.Description}] : ");
                string desc = Console.ReadLine();
                if (!string.IsNullOrEmpty(desc)) cours.Description = desc;

                Console.Write($"Nouvel horaire [{cours.Horaire}] : ");
                string horaire = Console.ReadLine();
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
            nouveau.Email = Console.ReadLine();

            Console.Write("Prénom : ");
            nouveau.Prenom = Console.ReadLine();

            Console.Write("Nom : ");
            nouveau.Nom = Console.ReadLine();

            Console.Write("Spécialité : ");
            nouveau.Specialite = Console.ReadLine();

            Console.Write("Formations : ");
            nouveau.Formations = Console.ReadLine();

            Console.Write("Téléphone : ");
            nouveau.Telephone = Console.ReadLine();

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
                string prenom = Console.ReadLine();
                if (!string.IsNullOrEmpty(prenom)) coach.Prenom = prenom;

                Console.Write($"Nouveau nom [{coach.Nom}] : ");
                string nom = Console.ReadLine();
                if (!string.IsNullOrEmpty(nom)) coach.Nom = nom;

                Console.Write($"Nouvelle spécialité [{coach.Specialite}] : ");
                string spec = Console.ReadLine();
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
      
    }
}
