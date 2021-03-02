using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modele;

namespace GestionEvenements.Services.Dal
{
    public class GevDbContext : IdentityDbContext
    {
        public GevDbContext(DbContextOptions<GevDbContext> options) : base(options)
        {
        }

        public string Migrer()
        {
            try
            {
                var migrations = Database.GetPendingMigrations().ToList();
                if (migrations.Count == 0)
                {
                    return "Aucune migration en attente.";
                }
                else
                {
                    Database.Migrate();
                    return "Migration terminée !";
                }
            }
            catch
            {
                return "Echec de la migration !";
            }
        }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Evenement> Evenements { get; set; }
        public DbSet<Adresse> Adresses { get; set; }
        public DbSet<TypeEvenement> TypesEvenement { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Interet> Interets { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Appel obligatoire pour configurer les entité du IdentityDbContext
            base.OnModelCreating(builder);

            builder.Entity<Participation>().HasKey(it => new { it.ParticipantId, it.EvenementId });
            builder.Entity<Participation>()
                .HasOne(x => x.Participant)
                .WithMany(x => x.Participations)
                .HasForeignKey(x => x.ParticipantId);
            builder.Entity<Participation>()
                .HasOne(x => x.Evenement)
                .WithMany(x => x.Participations)
                .HasForeignKey(x => x.EvenementId);

            builder.Entity<TypeEvenement>().HasData(
                new { TypeEvenementId = 1, Libelle = "Scientifique" },
                new { TypeEvenementId = 2, Libelle = "IT" },
                new { TypeEvenementId = 3, Libelle = "Culturel" },
                new { TypeEvenementId = 4, Libelle = "Invention" });

            builder.Entity<Participant>().HasData(
                new { ParticipantId = 1, Civilite = Civilite.Monsieur, Nom = "Gutenberg", AnneeNaissance = 1395, LieuNaissance = "Mayence en Allemagne", AnneeDeces = 1468, Activites = "L'inventeur de l'imprimerie.", Url = "https://www.histoire-pour-tous.fr/inventions/307-invention-de-imprimerie.html" },
                new { ParticipantId = 2, Civilite = Civilite.Monsieur, Nom = "Pasteur", AnneeNaissance = 1822, LieuNaissance = "Dole", AnneeDeces = 1895, Activites = "Inventeur du premier vaccin contre la rage.", Url = "https://fr.wikipedia.org/wiki/Louis_Pasteur" },
                new { ParticipantId = 3, Civilite = Civilite.Monsieur, Nom = "Berners-Lee", Prenom = "Tim", AnneeNaissance = 1955, LieuNaissance = "Londres", Activites = "L'un des principaux inventeurs du Web, fondateur du W3C.", Url = "https://fr.wikipedia.org/wiki/Tim_Berners-Lee", },
                new { ParticipantId = 4, Civilite = Civilite.Monsieur, Nom = "Gates", Prenom = "Bill", AnneeNaissance = 1955, LieuNaissance = "Seattle", Activites = "Fondateur de Microsoft avec Paul Allen.", Url = "https://fr.wikipedia.org/wiki/Bill_Gates" });

            builder.Entity<Evenement>().HasData(
                new
                {
                    EvenementId = 1,
                    TypeEvenementId = 4,
                    Annee = 1455,
                    Url = "https://www.histoire-pour-tous.fr/inventions/307-invention-de-imprimerie.html",
                    Titre = "Invention de l'imprimerie",
                    Description = "L'invention de l'imprimerie commence par la publication de la Bible en 1455."
                },
                new
                {
                    EvenementId = 2,
                    TypeEvenementId = 4,
                    Annee = 1769,
                    Titre = "Invention de l'automobile",
                    Description = "Le premier véhicule automobile a été créé en 1769 par Joseph Cugnot. Le premier moteur à essence a été créé en 1883 à Rouen par Edouard Delamare-Deboutteville.",
                    Url = "https://www.histoire-pour-tous.fr/dossiers/168-histoire-des-inventions-les-transports-terrestres.html"
                },
                new
                {
                    EvenementId = 3,
                    TypeEvenementId = 1,
                    Titre = "Invention du téléphone par Alexander Graham Bell",
                    Annee = 1876,
                    Url = "https://fr.wikipedia.org/wiki/Téléphone",
                },
                new
                {
                    EvenementId = 4,
                    TypeEvenementId = 1,
                    Titre = "Invention du vaccin",
                    Annee = 1885,
                    AdresseId = 1,
                    Url = "https://fr.wikipedia.org/wiki/Louis_Pasteur",
                    Description = "Invention du premier vaccin contre la rage."
                },
                new
                {
                    EvenementId = 5,
                    TypeEvenementId = 4,
                    Annee = 1897,
                    Titre = "Le premier tube cathodique",
                    Description = "Inventé par Karl Ferdinand Braun, la télévision couleur voit le jour en 1928.",
                    Url = "https://fr.wikipedia.org/wiki/Télévision"
                },
                     new
                     {
                         EvenementId = 6,
                         TypeEvenementId = 4,
                         Titre = "Début de l'aviation",
                         Annee = 1903,
                         Description = "Le premier vol motorisé.",
                         Url = "https://fr.wikipedia.org/wiki/Histoire_de_laviation"
                     },
                new
                {
                    EvenementId = 7,
                    TypeEvenementId = 4,
                    Annee = 1969,
                    Date = new DateTime(1969, 7, 24),
                    Url = "https://fr.wikipedia.org/wiki/Apollo_11",
                    Titre = "Le premier pas sur la lune",
                    Description = "Neil Armstrong pose le pied sur la lune lors de la mission Apollo 11."
                },
                new
                {
                    EvenementId = 8,
                    TypeEvenementId = 4,
                    Titre = "Le lancement du PC",
                    Annee = 1975,
                    Url = "https://fr.wikipedia.org/wiki/Microsoft",
                    Description = "MS est créé par Bill Gates et Paul Allen qui lance MS-DOS, Windows, Office puis .NET en 2002. Apple est créé l'année suivante par Steve Jobs."
                },
                new
                {
                    EvenementId = 9,
                    TypeEvenementId = 4,
                    Titre = "Le lancement d'Internet",
                    Annee = 1990,
                    Url = "https://fr.wikipedia.org/wiki/Internet",
                    Description = "Les bases sont créées dans le cadre du projet ARPANET dans les années 60. Le protocole HTTP est développé au CERN par Tim Berners-Lee et Robert Cailliau dans les années 90"
                },
                new
                {
                    EvenementId = 10,
                    TypeEvenementId = 2,
                    Annee = 1998,
                    AdresseId = 3,
                    Titre = "Le lancement de Google",
                    Description = "Google est créé dans la Silicon Valleypar Larry Page et Sergey Brin.",
                    Url = "https://fr.wikipedia.org/wiki/Google"
                }, new
                {
                    EvenementId = 11,
                    TypeEvenementId = 2,
                    Titre = "Les réseaux sociaux",
                    Annee = 2006,
                    AdresseId = 4,
                    Description = "Mark Zuckerberg développe son réseau qui s'ouvre au public qui atteint 2 mds d'utilisdateurs en 2017.",
                    Url = "https://fr.wikipedia.org/wiki/Facebook"
                },
                new
                {
                    EvenementId = 12,
                    TypeEvenementId = 2,
                    Titre = "La mobilité",
                    Annee = 2007,
                    Description = "L'iPhone est lancé avec une interface tactile et un ensemble des services connectés qui vont devenir Apple Store.",
                    Url = "https://fr.wikipedia.org/wiki/Apple"
                },
                new
                {
                    EvenementId = 13,
                    TypeEvenementId = 2,
                    Titre = ".NET Core",
                    Annee = 2016,
                    AdresseId = 2,
                    Description = ".NET devient multi-plateformes et Open Source.",
                    Url = "https://dotnet.microsoft.com/platform/support/policy/dotnet-core"
                },
                new
                {
                    EvenementId = 14,
                    TypeEvenementId = 2,
                    Annee = 2020,
                    Titre = ".NET 5",
                    AdresseId = 2,
                    Description = ".NET 5 fusionne le Framework .NET et Core avec un sous-ensemble commun dénommé .NET Standard.",
                    Url = "https://dotnet.microsoft.com/platform/support/policy/dotnet-core"
                });

            builder.Entity<Participation>().HasData(
                new { ParticipationId = 1, ParticipantId = 1, EvenementId = 1 },
                new { ParticipationId = 2, ParticipantId = 2, EvenementId = 4 },
                new { ParticipationId = 3, ParticipantId = 3, EvenementId = 9 },
                new { ParticipationId = 4, ParticipantId = 4, EvenementId = 8 });

            builder.Entity<Adresse>().HasData(
                new
                {
                    AdresseId = 1,
                    Titre = "Institut Pasteur",
                    Ville = "Paris",
                    Rue = "25-28 Rue du Dr Roux",
                    CodePostal = "75015",
                    Region = "Ile de France",
                    Pays = "France",
                    Notes = "Fondation à but non lucratif consacrée à l'étude de la biologie et des micros-organismes."

                }, new
                {
                    AdresseId = 2,
                    Titre = "Campus Microsoft",
                    Ville = "Redmond",
                    Pays = "USA",
                    Region = "Washington",
                    Notes = "Construit en 1986 sur 2 Ha pour plus de 40 000 salariés.",
                    Latitude = 47.6463889,
                    Longitude = -122.135,
                }, new
                {
                    AdresseId = 3,
                    Titre = "Googleplex",
                    Ville = "Zee-Town",
                    Pays = "Santa Clara",
                    Region = "Californie",
                    Notes = "Site (parmi 23 aux USA et autant en Europe) principal de la firme.",
                    Latitude = 37.4219,
                    Longitude = -122.0838
                }, new
                {
                    AdresseId = 4,
                    Titre = "Facebook City",
                    Ville = "Zee-Town",
                    Pays = "USA",
                    Region = "Californie",
                    Notes = "En cours de construction depuis 2012 sur 80 Ha.",
                    Latitude = 37.4811,
                    Longitude = -122.1538
                });
        }

    }
}

