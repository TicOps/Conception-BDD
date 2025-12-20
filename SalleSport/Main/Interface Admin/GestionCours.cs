using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Models;

namespace Main
{
    public class GestionCours
    {
        private string connectionString =
            "Server=localhost;" +
            "Database=salle_sport;" +
            "Uid=root;" +
            "Pwd=root;";

        public List<Cours> ObtenirTousLesCours()
        {
            List<Cours> liste = new List<Cours>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"SELECT * FROM Cours ORDER BY nomCours";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    MySqlDataReader lecteur = cmd.ExecuteReader();

                    while (lecteur.Read())
                    {
                        Cours cours = new Cours
                        {
                            IdCours = lecteur.GetInt32("idCours"),
                            NomCours = lecteur.GetString("nomCours"),
                            Description = lecteur.IsDBNull(lecteur.GetOrdinal("description")) ? "" : lecteur.GetString("description"),
                            Duree = lecteur.IsDBNull(lecteur.GetOrdinal("duree")) ? "" : lecteur.GetString("duree"),
                            Intensite = lecteur.IsDBNull(lecteur.GetOrdinal("intensite")) ? "" : lecteur.GetString("intensite"),
                            NiveauDifficulte = lecteur.IsDBNull(lecteur.GetOrdinal("niveauDifficulte")) ? "" : lecteur.GetString("niveauDifficulte"),
                            Horaire = lecteur.IsDBNull(lecteur.GetOrdinal("horaire")) ? "" : lecteur.GetString("horaire"),
                            CapaciteMaxCours = lecteur.GetInt32("capaciteMaxCours"),
                            IdCoach = lecteur.GetInt32("idCoach"),
                            IdSalle = lecteur.GetInt32("idSalle")
                        };
                        liste.Add(cours);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lecture cours : " + ex.Message);
                }
            }

            return liste;
        }

        public bool AjouterCours(Cours nouveauCours)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"INSERT INTO Cours 
                                      (nomCours, description, duree, intensite, niveauDifficulte, 
                                       horaire, capaciteMaxCours, idGerant, idSalle, idCoach) 
                                      VALUES 
                                      (@nom, @desc, @duree, @intensite, @niveau, @horaire, @capacite, 1, @salle, @coach)";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@nom", nouveauCours.NomCours);
                    cmd.Parameters.AddWithValue("@desc", nouveauCours.Description);
                    cmd.Parameters.AddWithValue("@duree", nouveauCours.Duree);
                    cmd.Parameters.AddWithValue("@intensite", nouveauCours.Intensite);
                    cmd.Parameters.AddWithValue("@niveau", nouveauCours.NiveauDifficulte);
                    cmd.Parameters.AddWithValue("@horaire", nouveauCours.Horaire);
                    cmd.Parameters.AddWithValue("@capacite", nouveauCours.CapaciteMaxCours);
                    cmd.Parameters.AddWithValue("@salle", nouveauCours.IdSalle);
                    cmd.Parameters.AddWithValue("@coach", nouveauCours.IdCoach);

                    int resultat = cmd.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur ajout cours : " + ex.Message);
                    return false;
                }
            }
        }

        public bool ModifierCours(Cours cours)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"UPDATE Cours 
                                      SET nomCours = @nom, description = @desc, horaire = @horaire,
                                          duree = @duree, intensite = @intensite, 
                                          niveauDifficulte = @niveau, capaciteMaxCours = @capacite,
                                          idSalle = @salle, idCoach = @coach
                                      WHERE idCours = @id";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@nom", cours.NomCours);
                    cmd.Parameters.AddWithValue("@desc", cours.Description);
                    cmd.Parameters.AddWithValue("@horaire", cours.Horaire);
                    cmd.Parameters.AddWithValue("@duree", cours.Duree);
                    cmd.Parameters.AddWithValue("@intensite", cours.Intensite);
                    cmd.Parameters.AddWithValue("@niveau", cours.NiveauDifficulte);
                    cmd.Parameters.AddWithValue("@capacite", cours.CapaciteMaxCours);
                    cmd.Parameters.AddWithValue("@salle", cours.IdSalle);
                    cmd.Parameters.AddWithValue("@coach", cours.IdCoach);
                    cmd.Parameters.AddWithValue("@id", cours.IdCours);

                    int resultat = cmd.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur modification cours : " + ex.Message);
                    return false;
                }
            }
        }

        public bool SupprimerCours(int idCours)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Supprimer d'abord les réservations liées
                    string requete1 = "DELETE FROM Reserve WHERE idCours = @id";
                    MySqlCommand cmd1 = new MySqlCommand(requete1, connection);
                    cmd1.Parameters.AddWithValue("@id", idCours);
                    cmd1.ExecuteNonQuery();

                    // Supprimer le cours
                    string requete2 = "DELETE FROM Cours WHERE idCours = @id";
                    MySqlCommand cmd2 = new MySqlCommand(requete2, connection);
                    cmd2.Parameters.AddWithValue("@id", idCours);

                    int resultat = cmd2.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur suppression cours : " + ex.Message);
                    return false;
                }
            }
        }
    }
}