namespace Models
{
    public class Coach
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Specialite { get; set; }
        public string Telephone { get; set; }
        public string Formations { get; set; }   // optionnel
    }
}
