namespace Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public DateTime Horaire { get; set; }     // date + heure du cours
        public int CapaciteMax { get; set; }
        public int CoachId { get; set; }          // FK vers Coach
        public string Intensite { get; set; }     // ex: "Faible", "Moyenne", "Forte"
        public string Niveau { get; set; }        // ex: "Débutant", "Intermédiaire", "Expert"
    }
}
