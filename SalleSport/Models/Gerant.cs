using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Models
{
    // ========== MODÈLES DE DONNÉES ==========
    
    public class Administrateur
    {
        public string IdGerant { get; set; }
        public string Username { get; set; }
        public string MotDePasse { get; set; }
        public string NiveauPrivilege { get; set; } // "Principal" ou "Secondaire"
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public DateTime DateCreation { get; set; }
        public bool EstActif { get; set; }
    }
}