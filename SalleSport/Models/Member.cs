namespace Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public DateTime DateInscription { get; set; }
        public bool IsValide { get; set; }   // True si validée par un admin
    }
}
