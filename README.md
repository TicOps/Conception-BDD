# Conception-BDD
Projet de création de base de données

# Commandes Git
git add .
git commit -m"message"
git push

pour récupérer -> git pull

git status -> permet de vérifier si les fichiers ont bien été ajoutés

# Commandes C#
Pour créer un projet C# -> dotnet new console -o SalleSport

Pour lancer un programme -> dotnet run

# à implémenter : 
🔹 Interface Administrateur

Ajouter un membre

Valider une inscription

Modifier un membre

Supprimer une adhésion

Afficher infos d’un membre

Ajouter / modifier / supprimer un cours

Ajouter / gérer les coachs

Gérer les salles

Gestion différente admin principal / secondaire

🔹 Interface Membre

Inscription (formulaire → en attente de validation)

Réserver un cours

Annuler une réservation

Voir l’historique des réservations

Modifier ses informations

🔹 Règles métier

Bloquer réservation si capacité atteinte

Bloquer réservation si inscription non validée

Gestion annulation cours (admin)

📊 INTERFACE ÉVALUATION (REQUÊTES SQL)

À implémenter dans l’application :

Membres ayant réservé au moins un cours (sous-requête)

Requête ensembliste (UNION)

2 jointures classiques

1 LEFT JOIN

1 RIGHT JOIN

Fonctions d’agrégation :

COUNT

SUM

AVG

MIN

MAX

GROUP_CONCAT (ou équivalent vu en cours)