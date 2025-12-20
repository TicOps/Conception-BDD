namespace Models
{
    ppublic class Reserve
{
    public int IdMembre { get; set; }
    public int IdCours { get; set; }
    public string StatutReservation { get; set; }
    public DateTime DateHeureDebut { get; set; }
    public DateTime DateHeureFin { get; set; }
}

}
