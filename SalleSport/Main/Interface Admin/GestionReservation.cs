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
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Vérifier si le membre a déjà réservé ce cours
                    string checkRequete = @"SELECT COUNT(*) FROM Reserve 
                                           WHERE idMembre = @idMembre 
                                           AND idCours = @idCours";
                    
                    MySqlCommand checkCmd = new MySqlCommand(checkRequete, connection);
                    checkCmd.Parameters.AddWithValue("@idMembre", idMembre);
                    checkCmd.Parameters.AddWithValue("@idCours", idCours);
                    
                    int existe = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (existe > 0)
                    {
                        Console.WriteLine("Vous êtes déjà inscrit à ce cours.");
                        return false;
                    }

                    // Vérifier la capacité du cours
                    string capaciteRequete = @"SELECT c.capaciteMaxCours, COUNT(r.idMembre) as nbInscrits
                                              FROM Cours c
                                              LEFT JOIN Reserve r ON c.idCours = r.idCours
                                              WHERE c.idCours = @idCours
                                              GROUP BY c.idCours, c.capaciteMaxCours";
                    
                    MySqlCommand capaciteCmd = new MySqlCommand(capaciteRequete, connection);
                    capaciteCmd.Parameters.AddWithValue("@idCours", idCours);
                    
                    MySqlDataReader lecteur = capaciteCmd.ExecuteReader();
                    
                    if (lecteur.Read())
                    {
                        int capaciteMax = lecteur.GetInt32("capaciteMaxCours");
                        int nbInscrits = lecteur.GetInt32("nbInscrits");
                        
                        lecteur.Close();
                        
                        if (nbInscrits >= capaciteMax)
                        {
                            Console.WriteLine("Le cours est complet.");
                            return false;
                        }
                    }
                    else
                    {
                        lecteur.Close();
                        Console.WriteLine("Cours introuvable.");
                        return false;
                    }

                    // Insérer la réservation
                    string insertRequete = @"INSERT INTO Reserve 
                                            (idMembre, idCours, statutReservation, dateHeureDebut, dateHeureFin) 
                                            VALUES 
                                            (@idMembre, @idCours, 'CONFIRMEE', NOW(), DATE_ADD(NOW(), INTERVAL 1 HOUR))";
                    
                    MySqlCommand insertCmd = new MySqlCommand(insertRequete, connection);
                    insertCmd.Parameters.AddWithValue("@idMembre", idMembre);
                    insertCmd.Parameters.AddWithValue("@idCours", idCours);

                    int resultat = insertCmd.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur réservation : " + ex.Message);
                    return false;
                }
            }
        }

        public List<Reservation> ObtenirReservationsMembre(int idMembre)
        {
            List<Reservation> liste = new List<Reservation>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"SELECT r.*, c.nomCours, c.horaire
                                      FROM Reserve r
                                      INNER JOIN Cours c ON r.idCours = c.idCours
                                      WHERE r.idMembre = @idMembre
                                      ORDER BY r.dateHeureDebut DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@idMembre", idMembre);
                    
                    MySqlDataReader lecteur = cmd.ExecuteReader();

                    while (lecteur.Read())
                    {
                        Reservation reservation = new Reservation
                        {
                            IdMembre = lecteur.GetInt32("idMembre"),
                            IdCours = lecteur.GetInt32("idCours"),
                            StatutReservation = lecteur.GetString("statutReservation"),
                            DateHeureDebut = lecteur.GetDateTime("dateHeureDebut"),
                            DateHeureFin = lecteur.GetDateTime("dateHeureFin"),
                            NomCours = lecteur.GetString("nomCours")
                        };
                        liste.Add(reservation);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lecture réservations : " + ex.Message);
                }
            }

            return liste;
        }

        public bool AnnulerReservation(int idMembre, int idCours)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"DELETE FROM Reserve 
                                      WHERE idMembre = @idMembre 
                                      AND idCours = @idCours";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@idMembre", idMembre);
                    cmd.Parameters.AddWithValue("@idCours", idCours);

                    int resultat = cmd.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur annulation : " + ex.Message);
                    return false;
                }
            }
        }
    }
}