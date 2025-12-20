using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Models;

namespace Main
{
    public class GestionMembre
    {
        private string connectionString =
            "Server=localhost;" +
            "Database=salle_sport;" +
            "Uid=root;" +
            "Pwd=root;";

        public List<Membre> ObtenirTousLesMembres()
        {
            List<Membre> liste = new List<Membre>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = "SELECT * FROM Membre ORDER BY nom, prenom";
                    MySqlCommand commande = new MySqlCommand(requete, connection);
                    MySqlDataReader lecteur = commande.ExecuteReader();

                    while (lecteur.Read())
                    {
                        Membre m = new Membre
                        {
                            IdMembre = lecteur.GetInt32("idMembre"),
                            Email = lecteur.GetString("email"),
                            Prenom = lecteur.GetString("prenom"),
                            Nom = lecteur.GetString("nom"),
                            Adresse = lecteur.GetString("adresse"),
                            Telephone = lecteur.GetString("telephone"),
                            DateDebut = lecteur.IsDBNull(lecteur.GetOrdinal("dateDebut")) 
                                ? null 
                                : (DateTime?)lecteur.GetDateTime("dateDebut"),
                            StatutInscription = lecteur.IsDBNull(lecteur.GetOrdinal("statutInscription")) 
                                ? "EN_ATTENTE" 
                                : lecteur.GetString("statutInscription")
                        };
                        liste.Add(m);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lecture : " + ex.Message);
                }
            }

            return liste;
        }

        public List<Membre> ObtenirMembresEnAttente()
        {
            List<Membre> liste = new List<Membre>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"SELECT * FROM Membre 
                                      WHERE statutInscription = 'EN_ATTENTE'
                                      ORDER BY dateDebut ASC";

                    MySqlCommand commande = new MySqlCommand(requete, connection);
                    MySqlDataReader lecteur = commande.ExecuteReader();

                    while (lecteur.Read())
                    {
                        Membre m = new Membre
                        {
                            IdMembre = lecteur.GetInt32("idMembre"),
                            Email = lecteur.GetString("email"),
                            Prenom = lecteur.GetString("prenom"),
                            Nom = lecteur.GetString("nom"),
                            Telephone = lecteur.GetString("telephone"),
                            DateDebut = lecteur.IsDBNull(lecteur.GetOrdinal("dateDebut")) 
                                ? null 
                                : (DateTime?)lecteur.GetDateTime("dateDebut"),
                            StatutInscription = lecteur.GetString("statutInscription")
                        };
                        liste.Add(m);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }

            return liste;
        }

        public bool ValiderInscription(int idMembre)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"UPDATE Membre 
                                      SET statutInscription = 'VALIDE'
                                      WHERE idMembre = @id 
                                      AND statutInscription = 'EN_ATTENTE'";

                    MySqlCommand commande = new MySqlCommand(requete, connection);
                    commande.Parameters.AddWithValue("@id", idMembre);

                    int resultat = commande.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur validation : " + ex.Message);
                    return false;
                }
            }
        }

        public bool ModifierMembre(Membre membre)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string requete = @"UPDATE Membre 
                                      SET nom = @nom, prenom = @prenom, adresse = @adresse, 
                                          telephone = @tel, email = @email
                                      WHERE idMembre = @id";

                    MySqlCommand commande = new MySqlCommand(requete, connection);
                    commande.Parameters.AddWithValue("@nom", membre.Nom);
                    commande.Parameters.AddWithValue("@prenom", membre.Prenom);
                    commande.Parameters.AddWithValue("@adresse", membre.Adresse);
                    commande.Parameters.AddWithValue("@tel", membre.Telephone);
                    commande.Parameters.AddWithValue("@email", membre.Email);
                    commande.Parameters.AddWithValue("@id", membre.IdMembre);

                    int resultat = commande.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur modification : " + ex.Message);
                    return false;
                }
            }
        }

        public bool SupprimerMembre(int idMembre)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string requete1 = "DELETE FROM Reserve WHERE idMembre = @id";
                    MySqlCommand cmd1 = new MySqlCommand(requete1, connection);
                    cmd1.Parameters.AddWithValue("@id", idMembre);
                    cmd1.ExecuteNonQuery();

                    string requete2 = "DELETE FROM Valide WHERE idMembre = @id";
                    MySqlCommand cmd2 = new MySqlCommand(requete2, connection);
                    cmd2.Parameters.AddWithValue("@id", idMembre);
                    cmd2.ExecuteNonQuery();

                    string requete3 = "DELETE FROM Membre WHERE idMembre = @id";
                    MySqlCommand cmd3 = new MySqlCommand(requete3, connection);
                    cmd3.Parameters.AddWithValue("@id", idMembre);

                    int resultat = cmd3.ExecuteNonQuery();
                    return resultat > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur suppression : " + ex.Message);
                    return false;
                }
            }
        }
    }
}
