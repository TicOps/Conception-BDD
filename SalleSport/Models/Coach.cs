namespace Models
{
    public class Coach
    {
        public int IdCoach { get; set; }
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Specialite { get; set; }
        public string Formations { get; set; }
        public string Telephone { get; set; }
        public bool EstActif { get; set; }
    }
}
