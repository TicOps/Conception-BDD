namespace Models
{
    public class Cours
{
    public int IdCours { get; set; }
    public string NomCours { get; set; }
    public string Description { get; set; }
    public string Duree { get; set; }
    public string Intensite { get; set; }
    public string NiveauDifficulte { get; set; }
    public string Horaire { get; set; }
    public int CapaciteMaxCours { get; set; }
    public int IdGerant { get; set; }
    public int IdSalle { get; set; }
    public int IdCoach { get; set; }
}

}
