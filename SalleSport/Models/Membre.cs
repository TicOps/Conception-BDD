namespace Models
{
    public class Membre
    {
        public int IdMembre { get; set; }
        public string Email { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Mdp { get; set; }
        public string Telephone { get; set; }
        public DateTime? DateDebut { get; set; }
        public string StatutInscription { get; set; }
        public DateTime? DateFin { get; set; }
    }
}
