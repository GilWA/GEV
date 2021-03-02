# GEV
Cette application est une Gestion d'événements avec participants réalisée en ASP CORE NET 5 comprenant les éléments suivants :

* Une interface Web responsive basée sur Bootstrap.
* Un modèle de données avec des relations 1-N et N-M.
* Une persitance des données réalisée avec des repository afin de centraliser les accès à une base SQL Server LocalDb via EF Core.
* Des Vues/Controlleurs de chaque entité principale, avec validations des saises dans les opérations de création/mise à jour.
* Des contrôleurs API appelés par jQuery.
* Une pagination/filtrage/tri d'une liste de données.
* Une gestion de sessions pour mémoriser les choix utilisateurs.
* Un controle d'accès aux pages par le système d'authentification Identity, avec création de comptes utilisateurs.
* Une gestion d'erreurs par log dans le journal d'événement système et dans un fichier avec un filtre.

Cet application est utilisable librement telle quelle, sans support, à titre d'exemple de réalisations possibles avec ASP NET 5. Elle peut être adaptée / intégrée en partie ou en totalité à condition : 

1. que l'usage soit NON commercial.
2. de mentionner explicitement dans le code de démarrage (éventuellement dans la page d'acceuil), la source et l'auteur Gilles NICOT - Web Applications.

Pour tout autre usage, merci de me contacter sur web-applications.com.

Bon Dev ;)
