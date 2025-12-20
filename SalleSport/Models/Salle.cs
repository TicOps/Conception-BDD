 namespace Models
    { 
        public class Salle
        {
            public string IdSalle { get; set; }
            public string NomSalle { get; set; }
            public int Capacite { get; set; }
            public string Equipements { get; set; }
            public bool EstDisponible { get; set; }
        }
    }