using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Models;

namespace Main
{
    public class GestionReservation
    {
        private string connectionString =
            "Server=localhost;" +
            "Database=salle_sport;" +
            "Uid=root;" +
            "Pwd=root;";

        public bool ReserverCours(int idMembre, int idCours)
        {
            // À implémenter plus tard
            return true;
        }
    }
}
