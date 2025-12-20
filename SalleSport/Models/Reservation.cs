namespace Models
{
    public class Reservation  // Renommé de Reserve à Reservation
    {
        public int IdMembre { get; set; }
        public int IdCours { get; set; }
        public string StatutReservation { get; set; }
        public DateTime DateHeureDebut { get; set; }
        public DateTime DateHeureFin { get; set; }
        public string NomCours { get; set; }  // Ajouté pour l'affichage
    }
}