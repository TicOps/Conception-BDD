CREATE DATABASE salle_sport;
USE salle_sport;

CREATE TABLE Membre(
   idMembre VARCHAR(50),
   email VARCHAR(50) NOT NULL,
   prenom VARCHAR(50) NOT NULL,
   nom VARCHAR(50) NOT NULL,
   adresse VARCHAR(50) NOT NULL,
   mdp VARCHAR(50) NOT NULL,
   telephone VARCHAR(50) NOT NULL,
   dateDebut DATE,
   statutInscription VARCHAR(50),
   dateFin DATE,
   PRIMARY KEY(idMembre),
   UNIQUE(email)
);

CREATE TABLE Salle(
   idSalle VARCHAR(50),
   capacitéMax VARCHAR(50),
   localisation VARCHAR(50),
   nomSalle VARCHAR(50),
   PRIMARY KEY(idSalle)
);

CREATE TABLE Gerant(
   idGerant VARCHAR(50),
   nom VARCHAR(50) NOT NULL,
   adresse VARCHAR(50) NOT NULL,
   telephone VARCHAR(50) NOT NULL,
   prenom VARCHAR(50) NOT NULL,
   mdp VARCHAR(50) NOT NULL,
   email VARCHAR(50) NOT NULL,
   PRIMARY KEY(idGerant),
   UNIQUE(email)
);

CREATE TABLE Administrateur_1(
   idGerant VARCHAR(50),
   niveauPrivliege VARCHAR(50),
   PRIMARY KEY(idGerant),
   FOREIGN KEY(idGerant) REFERENCES Gerant(idGerant)
);

CREATE TABLE Administrateur_2(
   idGerant VARCHAR(50),
   niveauPrivliege VARCHAR(50),
   PRIMARY KEY(idGerant),
   FOREIGN KEY(idGerant) REFERENCES Gerant(idGerant)
);

CREATE TABLE Coach(
   idCoach VARCHAR(50),
   email VARCHAR(50),
   nom VARCHAR(50),
   prénom VARCHAR(50),
   spécialité VARCHAR(50),
   formations VARCHAR(50),
   téléphone VARCHAR(50),
   idGerant VARCHAR(50) NOT NULL,
   PRIMARY KEY(idCoach),
   UNIQUE(email),
   FOREIGN KEY(idGerant) REFERENCES Gerant(idGerant)
);

CREATE TABLE Cours(
   idCours VARCHAR(50),
   nomCours VARCHAR(50),
   description VARCHAR(50),
   durée VARCHAR(50),
   intensité VARCHAR(50),
   niveauDiffuclté VARCHAR(50),
   horaire VARCHAR(50),
   capacitéMaxCours VARCHAR(50),
   idGerant VARCHAR(50) NOT NULL,
   idSalle VARCHAR(50) NOT NULL,
   idCoach VARCHAR(50) NOT NULL,
   PRIMARY KEY(idCours),
   FOREIGN KEY(idGerant) REFERENCES Gerant(idGerant),
   FOREIGN KEY(idSalle) REFERENCES Salle(idSalle),
   FOREIGN KEY(idCoach) REFERENCES Coach(idCoach)
);

CREATE TABLE Reserve(
   idMembre VARCHAR(50),
   idCours VARCHAR(50),
   statutReservation VARCHAR(50),
   dateHeureFin DATETIME,
   dateHeureDebut DATETIME,
   PRIMARY KEY(idMembre, idCours),
   FOREIGN KEY(idMembre) REFERENCES Membre(idMembre),
   FOREIGN KEY(idCours) REFERENCES Cours(idCours)
);

CREATE TABLE Valide(
   idMembre VARCHAR(50),
   idGerant VARCHAR(50),
   PRIMARY KEY(idMembre, idGerant),
   FOREIGN KEY(idMembre) REFERENCES Membre(idMembre),
   FOREIGN KEY(idGerant) REFERENCES Gerant(idGerant)
);

-- Admin 1 (plus de privilèges)
CREATE USER 'admin1'@'%' IDENTIFIED BY 'PwdAdmin1!';
GRANT ALL PRIVILEGES ON salle_sport.* TO 'admin1'@'%';

-- Admin 2 (moins de privilèges)
CREATE USER 'admin2'@'%' IDENTIFIED BY 'PwdAdmin2!';
GRANT SELECT, INSERT, UPDATE ON salle_sport.* TO 'admin2'@'%';
-- par exemple pas le droit de DROP/ALTER/etc.

-- Utilisateur membre (lecture seule sur les cours, réservation)
CREATE USER 'membre_user'@'%' IDENTIFIED BY 'PwdMembre123!';
GRANT SELECT ON salle_sport.Cours   TO 'membre_user'@'%';
GRANT INSERT ON salle_sport.Reserve TO 'membre_user'@'%';

FLUSH PRIVILEGES;

ALTER TABLE Salle
ADD CONSTRAINT chk_capacite_salle
CHECK (capacitéMax > 0);

ALTER TABLE Cours
ADD CONSTRAINT chk_capacite_cours
CHECK (capacitéMaxCours > 0);

ALTER TABLE Reserve
ADD CONSTRAINT chk_date_reservation
CHECK (dateHeureDebut < dateHeureFin);

ALTER TABLE Membre
MODIFY telephone CHAR(10),
ADD CONSTRAINT chk_telephone_membre
CHECK (telephone >= '0000000000'AND telephone <= '9999999999');

------------------- Peuplement de la BDD -----------------------------------------
INSERT INTO Gerant (idGerant, nom, prenom, adresse, telephone, email, mdp) VALUES
('G1', 'Durand', 'Paul', '12 rue de Paris', '0612345678', 'paul.durand@salle.fr', 'admin'),
('G2', 'Martin', 'Claire', '8 avenue Lyon', '0623456789', 'claire.martin@salle.fr', 'admin');

INSERT INTO Administrateur_1 (idGerant, niveauPrivliege) VALUES
('G1', 'TOTAL');

INSERT INTO Administrateur_2 (idGerant, niveauPrivliege) VALUES
('G2', 'LIMITÉ');


INSERT INTO Salle (idSalle, capacitéMax, localisation, nomSalle) VALUES
('S1', '20', 'Rez-de-chaussée', 'Salle Cardio'),
('S2', '30', '1er étage', 'Salle Musculation'),
('S3', '15', 'Sous-sol', 'Salle Yoga');

INSERT INTO Membre (
    idMembre, email, prenom, nom, adresse, mdp, telephone,
    dateDebut, statutInscription, dateFin
) VALUES
('M1', 'alice@mail.com', 'Alice', 'Dupont', '10 rue Lille', '1234', '0611111111', '2024-01-10', 'VALIDE', NULL),
('M2', 'bob@mail.com', 'Bob', 'Martin', '22 rue Lyon', '1234', '0622222222', '2024-02-05', 'EN_ATTENTE', NULL),
('M3', 'charles@mail.com', 'Charles', 'Petit', '5 avenue Nice', '1234', '0633333333', '2024-03-01', 'VALIDE', NULL);

INSERT INTO Valide (idMembre, idGerant) VALUES
('M1', 'G1'),
('M3', 'G2');


INSERT INTO Coach (
    idCoach, email, nom, prénom, spécialité, formations, téléphone, idGerant
) VALUES
('C1', 'coach.john@salle.fr', 'Doe', 'John', 'Cardio', 'BPJEPS', '0644444444', 'G1'),
('C2', 'coach.emma@salle.fr', 'Smith', 'Emma', 'Yoga', 'Diplôme Yoga', '0655555555', 'G2');


INSERT INTO Cours (
    idCours, nomCours, description, durée, intensité,
    niveauDiffuclté, horaire, capacitéMaxCours,
    idGerant, idSalle, idCoach
) VALUES
('CR1', 'Cardio Training', 'Cardio intense', '1h', 'Forte', 'Intermédiaire', 'Lundi 18h', '20', 'G1', 'S1', 'C1'),
('CR2', 'Yoga Zen', 'Détente et respiration', '1h', 'Faible', 'Débutant', 'Mardi 10h', '15', 'G2', 'S3', 'C2'),
('CR3', 'Renforcement', 'Musculation légère', '1h', 'Moyenne', 'Intermédiaire', 'Jeudi 19h', '25', 'G1', 'S2', 'C1');

INSERT INTO Reserve (
    idMembre, idCours, statutReservation, dateHeureDebut, dateHeureFin
) VALUES
('M1', 'CR1', 'CONFIRMEE', '2024-04-01 18:00:00', '2024-04-01 19:00:00'),
('M1', 'CR2', 'CONFIRMEE', '2024-04-02 10:00:00', '2024-04-02 11:00:00'),
('M3', 'CR3', 'CONFIRMEE', '2024-04-04 19:00:00', '2024-04-04 20:00:00');





-- -- Tous les emails utilisés dans le site :
-- -- soit par un membre, soit par un gérant
-- SELECT email FROM Membre
-- UNION
-- SELECT email FROM admin1
-- UNION
-- SELECT email FROM admin2;

-- -- 1) Les membres qui ont réservé au moins un cours
-- SELECT * FROM Membre WHERE idMembre IN (SELECT DISTINCT idMembre FROM Reserve);

-- -- 2) Les cours dont la capacité max est supérieure
-- --     à la capacité moyenne des salles
-- SELECT *
-- FROM Cours
-- WHERE capacitéMaxCours > (
--     SELECT AVG(capacitéMax)
--     FROM Salle
-- );

-- -- sql1 : lister les réservations avec nom du membre et nom du cours
-- SELECT M.nom, M.prenom, C.nomCours, R.dateHeureDebut, R.dateHeureFin
-- FROM Reserve R
-- JOIN Membre M ON R.idMembre = M.idMembre
-- JOIN Cours  C ON R.idCours  = C.idCours;

-- -- sql2 : lister les cours avec leur coach
-- SELECT C.idCours, C.nomCours, Co.nom AS nomCoach, Co.prénom AS prenomCoach
-- FROM Cours C
-- JOIN Coach Co ON C.idCoach = Co.idCoach;

-- -- LEFT JOIN : tous les cours, même sans réservations
-- SELECT C.idCours, C.nomCours, R.idMembre, R.statutReservation
-- FROM Cours C
-- LEFT JOIN Reserve R ON C.idCours = R.idCours;

-- -- RIGHT JOIN : toutes les réservations, même si cours absent côté gauche
-- SELECT C.idCours, C.nomCours, R.idMembre, R.statutReservation
-- FROM Cours C
-- RIGHT JOIN Reserve R ON C.idCours = R.idCours;

-- -- COUNT : nombre total de membres
-- SELECT COUNT(*) AS nbMembres
-- FROM Membre;

-- -- SUM : somme des capacités max de toutes les salles
-- SELECT SUM(capacitéMax) AS capaciteTotaleSalles
-- FROM Salle;

-- -- AVG : capacité moyenne des cours
-- SELECT AVG(capacitéMaxCours) AS capaciteMoyCours
-- FROM Cours;

-- -- MIN : plus petite capacité de salle
-- SELECT MIN(capacitéMax) AS capaciteMinSalle
-- FROM Salle;

-- -- MAX : plus grande capacité de cours
-- SELECT MAX(capacitéMaxCours) AS capaciteMaxCours
-- FROM Cours;

-- -- GROUP_CONCAT
-- SELECT idCoach, GROUP_CONCAT(nomCours SEPARATOR ', ') AS coursDuCoach
-- FROM Cours
-- GROUP BY idCoach;


