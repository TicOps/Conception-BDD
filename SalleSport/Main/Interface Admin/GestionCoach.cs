using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Models;

namespace Main
{
    public class GestionCoach
    {
        private string connectionString =
            "Server=localhost;" +
            "Database=salle_sport;" +
            "Uid=root;" +
            "Pwd=root;";

        public List<Coach> ObtenirTousLesCoachs()
        {
            List<Coach> liste = new List<Coach>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = "SELECT * FROM Coach ORDER BY nom, prenom";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    MySqlDataReader lecteur = cmd.ExecuteReader();

                    while (lecteur.Read())
                    {
                        Coach coach = new Coach
                        {
                            IdCoach = lecteur.GetInt32("idCoach"),
                            Email = lecteur.IsDBNull(lecteur.GetOrdinal("email")) ? "" : lecteur.GetString("email"),
                            Nom = lecteur.IsDBNull(lecteur.GetOrdinal("nom")) ? "" : lecteur.GetString("nom"),
                            Prenom = lecteur.IsDBNull(lecteur.GetOrdinal("prenom")) ? "" : lecteur.GetString("prenom"),
                            Specialite = lecteur.IsDBNull(lecteur.GetOrdinal("specialite")) ? "" : lecteur.GetString("specialite"),
                            Formations = lecteur.IsDBNull(lecteur.GetOrdinal("formations")) ? "" : lecteur.GetString("formations"),
                            Telephone = lecteur.IsDBNull(lecteur.GetOrdinal("telephone")) ? "" : lecteur.GetString("telephone")
                        };
                        liste.Add(coach);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lecture coachs : " + ex.Message);
                }
            }

            return liste;
        }

        public bool AjouterCoach(Coach nouveauCoach)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"INSERT INTO Coach 
                                      (email, nom, prenom, specialite, formations, telephone, idGerant) 
                                      VALUES 
                                      (@email, @nom, @prenom, @specialite, @formations, @tel, 1)";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@email", nouveauCoach.Email);
                    cmd.Parameters.AddWithValue("@nom", nouveauCoach.Nom);
                    cmd.Parameters.AddWithValue("@prenom", nouveauCoach.Prenom);
                    cmd.Parameters.AddWithValue("@specialite", nouveauCoach.Specialite);
                    cmd.Parameters.AddWithValue("@formations", nouveauCoach.Formations);
                    cmd.Parameters.AddWithValue("@tel", nouveauCoach.Telephone);

                    int resultat = cmd.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur ajout coach : " + ex.Message);
                    return false;
                }
            }
        }

        public bool ModifierCoach(Coach coach)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"UPDATE Coach 
                                      SET email = @email, nom = @nom, prenom = @prenom,
                                          specialite = @specialite, formations = @formations,
                                          telephone = @tel
                                      WHERE idCoach = @id";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@email", coach.Email);
                    cmd.Parameters.AddWithValue("@nom", coach.Nom);
                    cmd.Parameters.AddWithValue("@prenom", coach.Prenom);
                    cmd.Parameters.AddWithValue("@specialite", coach.Specialite);
                    cmd.Parameters.AddWithValue("@formations", coach.Formations);
                    cmd.Parameters.AddWithValue("@tel", coach.Telephone);
                    cmd.Parameters.AddWithValue("@id", coach.IdCoach);

                    int resultat = cmd.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur modification coach : " + ex.Message);
                    return false;
                }
            }
        }

        public bool SupprimerCoach(int idCoach)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = "DELETE FROM Coach WHERE idCoach = @id";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@id", idCoach);

                    int resultat = cmd.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur suppression coach : " + ex.Message);
                    return false;
                }
            }
        }
    }
}
