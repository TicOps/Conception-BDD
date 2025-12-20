using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Models
{
    // ========== MODÈLES DE DONNÉES ==========
    
    public class Gerant
{
    public int IdGerant { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Adresse { get; set; }
    public string Telephone { get; set; }
    public string Mdp { get; set; }
    public string Email { get; set; }
}

}