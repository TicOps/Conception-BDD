// ===== RÉSERVER UN COURS =====
private void ReserverCoursMembre(int idMembre)
{
    DrawHeader("Réserver un Cours");

    // Vérifier le statut du membre
    var membres = gestionMembre.ObtenirTousLesMembres();
    var membre = membres.Find(m => m.IdMembre == idMembre);

    if (membre == null || membre.StatutInscription != "VALIDE")
    {
        Error("Votre inscription doit être validée par un administrateur avant de réserver un cours.");
        return;
    }

    // Afficher les cours disponibles
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
        Console.WriteLine($"    Coach: {c.NomCoach}");
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

// ===== ANNULER UNE RÉSERVATION =====
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

// ===== MES RÉSERVATIONS (HISTORIQUE) =====
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

// ===== MODIFIER MES INFORMATIONS =====
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

    Loading("Mise à jour");

    if (gestionMembre.ModifierMembre(membre))
        Success("Vos informations ont été mises à jour !");
    else
        Error("Erreur lors de la mise à jour.");
}
