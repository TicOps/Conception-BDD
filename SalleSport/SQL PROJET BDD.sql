-- ================================
-- RESET BASE
-- ================================
DROP DATABASE IF EXISTS salle_sport;
CREATE DATABASE salle_sport;
USE salle_sport;

-- ================================
-- TABLE MEMBRE
-- ================================
CREATE TABLE Membre (
   idMembre INT AUTO_INCREMENT,
   email VARCHAR(50) NOT NULL,
   prenom VARCHAR(50) NOT NULL,
   nom VARCHAR(50) NOT NULL,
   adresse VARCHAR(100) NOT NULL,
   mdp VARCHAR(50) NOT NULL,
   telephone CHAR(10) NOT NULL,
   dateDebut DATE,
   statutInscription VARCHAR(20),
   dateFin DATE,
   PRIMARY KEY (idMembre),
   UNIQUE (email),
   CONSTRAINT chk_telephone_membre
     CHECK (telephone REGEXP '^[0-9]{10}$')
);

-- ================================
-- TABLE SALLE
-- ================================
CREATE TABLE Salle (
   idSalle INT AUTO_INCREMENT,
   capaciteMax INT NOT NULL,
   localisation VARCHAR(50),
   nomSalle VARCHAR(50),
   estDisponible BOOL,
   PRIMARY KEY (idSalle),
   CONSTRAINT chk_capacite_salle
     CHECK (capaciteMax > 0)
);

-- ================================
-- TABLE GERANT
-- ================================
CREATE TABLE Gerant (
   idGerant INT AUTO_INCREMENT,
   nom VARCHAR(50) NOT NULL,
   prenom VARCHAR(50) NOT NULL,
   adresse VARCHAR(100) NOT NULL,
   telephone CHAR(10) NOT NULL,
   mdp VARCHAR(50) NOT NULL,
   email VARCHAR(50) NOT NULL,
   PRIMARY KEY (idGerant),
   UNIQUE (email)
);

-- ================================
-- TABLE ADMINISTRATEUR 1
-- ================================
CREATE TABLE Administrateur_1 (
   idGerant INT,
   niveauPrivilege VARCHAR(20),
   PRIMARY KEY (idGerant),
   FOREIGN KEY (idGerant) REFERENCES Gerant(idGerant)
);

-- ================================
-- TABLE ADMINISTRATEUR 2
-- ================================
CREATE TABLE Administrateur_2 (
   idGerant INT,
   niveauPrivilege VARCHAR(20),
   PRIMARY KEY (idGerant),
   FOREIGN KEY (idGerant) REFERENCES Gerant(idGerant)
);

-- ================================
-- TABLE COACH
-- ================================
CREATE TABLE Coach (
   idCoach INT AUTO_INCREMENT,
   email VARCHAR(50),
   nom VARCHAR(50),
   prenom VARCHAR(50),
   specialite VARCHAR(50),
   formations VARCHAR(50),
   telephone CHAR(10),
   idGerant INT NOT NULL,
   PRIMARY KEY (idCoach),
   UNIQUE (email),
   FOREIGN KEY (idGerant) REFERENCES Gerant(idGerant)
);

-- ================================
-- TABLE COURS
-- ================================
CREATE TABLE Cours (
   idCours INT AUTO_INCREMENT,
   nomCours VARCHAR(50),
   description VARCHAR(100),
   duree VARCHAR(20),
   intensite VARCHAR(20),
   niveauDifficulte VARCHAR(20),
   horaire VARCHAR(50),
   capaciteMaxCours INT,
   idGerant INT NOT NULL,
   idSalle INT NOT NULL,
   idCoach INT NOT NULL,
   PRIMARY KEY (idCours),
   FOREIGN KEY (idGerant) REFERENCES Gerant(idGerant),
   FOREIGN KEY (idSalle) REFERENCES Salle(idSalle),
   FOREIGN KEY (idCoach) REFERENCES Coach(idCoach),
   CONSTRAINT chk_capacite_cours
     CHECK (capaciteMaxCours > 0)
);

-- ================================
-- TABLE RESERVATION
-- ================================
CREATE TABLE Reserve (
   idMembre INT,
   idCours INT,
   statutReservation VARCHAR(20),
   dateHeureDebut DATETIME,
   dateHeureFin DATETIME,
   PRIMARY KEY (idMembre, idCours),
   FOREIGN KEY (idMembre) REFERENCES Membre(idMembre),
   FOREIGN KEY (idCours) REFERENCES Cours(idCours),
   CONSTRAINT chk_date_reservation
     CHECK (dateHeureDebut < dateHeureFin)
);

-- ================================
-- TABLE VALIDATION
-- ================================
CREATE TABLE Valide (
   idMembre INT,
   idGerant INT,
   PRIMARY KEY (idMembre, idGerant),
   FOREIGN KEY (idMembre) REFERENCES Membre(idMembre),
   FOREIGN KEY (idGerant) REFERENCES Gerant(idGerant)
);

-- ================================
-- PEUPLEMENT
-- ================================

-- GERANTS
INSERT INTO Gerant (nom, prenom, adresse, telephone, mdp, email)
VALUES
('Martin', 'Paul', '1 rue Admin', '0600000001', 'admin123', 'admin@mail.com'),
('Durand', 'Julie', '2 rue Admin', '0600000002', 'admin456', 'admin2@mail.com');

-- ADMINS
INSERT INTO Administrateur_1 (idGerant, niveauPrivilege)
VALUES (1, 'TOTAL');

INSERT INTO Administrateur_2 (idGerant, niveauPrivilege)
VALUES (2, 'LIMITE');

-- SALLES
INSERT INTO Salle (capaciteMax, localisation, nomSalle)
VALUES
(30, 'Rez-de-chaussée', 'Salle Musculation'),
(20, '1er étage', 'Salle Cardio'),
(15, 'Sous-sol', 'Salle Yoga');

-- MEMBRES
INSERT INTO Membre (
    email, prenom, nom, adresse, mdp, telephone,
    dateDebut, statutInscription, dateFin
)
VALUES
('alice@mail.com', 'Alice', 'Dupont', '10 rue Lille', '1234', '0611111111',
 '2024-01-10', 'VALIDE', NULL),

('bob@mail.com', 'Bob', 'Martin', '20 rue Paris', 'abcd', '0622222222',
 '2024-02-01', 'EN_ATTENTE', NULL);

-- VALIDATION
INSERT INTO Valide (idMembre, idGerant)
VALUES (1, 1);

-- COACHS
INSERT INTO Coach (email, nom, prenom, specialite, formations, telephone, idGerant)
VALUES
('coach1@mail.com', 'Leroy', 'Maxime', 'Musculation', 'BPJEPS', '0611111111', 1),
('coach2@mail.com', 'Bernard', 'Emma', 'Yoga', 'Yoga Certifie', '0622222222', 1);

-- COURS
INSERT INTO Cours (
    nomCours, description, duree, intensite, niveauDifficulte,
    horaire, capaciteMaxCours, idGerant, idSalle, idCoach
)
VALUES
('Muscu Debutant', 'Renforcement musculaire', '1h', 'Moyenne', 'Debutant',
 'Lundi 18h', 20, 1, 1, 1),

('Yoga Relax', 'Detente et respiration', '1h', 'Faible', 'Tous niveaux',
 'Mardi 17h', 15, 1, 3, 2);

-- RESERVATION
INSERT INTO Reserve (
    idMembre, idCours, statutReservation, dateHeureDebut, dateHeureFin
)
VALUES
(1, 1, 'CONFIRMEE', '2024-03-10 18:00:00', '2024-03-10 19:00:00');

-- ================================
-- TESTS
-- ================================
SELECT * FROM Membre;
SELECT * FROM Cours;
SELECT * FROM Reserve;
