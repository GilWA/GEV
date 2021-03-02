using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Modele
{
    public class Evenement
    {
        public int EvenementId { get; set; }
        [Required(ErrorMessage = "{0} non renseigné")]
        [Range(1, 1000, ErrorMessage = "{0} non renseigné")]
        [DisplayName("Type d'événement")]
        public int TypeEvenementId { get; set; }
        public  TypeEvenement TypeEvenement { get; set; }
        [Required(ErrorMessage = "{0} non renseignée")]
        [DisplayName("Année")]
        [Display(Prompt = "Entrez l'année sur 4 chiffres")]
        public int Annee { get; set; }
        [DisplayName("Date")] 
        public DateTime? DateEvenement { get; set; }
        [StringLength(100, ErrorMessage = "{0} caractères max")]
        [Required(ErrorMessage = "{0} non renseigné")]
        public string Titre { get; set; }
        [StringLength(1000, ErrorMessage = "{0} caractères max")]
        public string Description { get; set; }
        [StringLength(200, ErrorMessage = "{0} caractères max")]
        [DisplayName("Fiche Internet")]
        public string Url { get; set; }
        public int? AdresseId { get; set; }
        public Adresse Adresse { get; set; }

        public List<Participation> _participations;
        public List<Participation> Participations
        {
            get { return _participations ??= new List<Participation>(); }
        }
    }
    //public class Evenement
    //{
    //    public int EvenementId { get; set; }
    //    public int TypeEvenementId { get; set; }
    //    public TypeEvenement TypeEvenement { get; set; }
    //    [Required]
    //    public int Annee { get; set; }
    //    public DateTime? DateEvenement { get; set; }
    //    [StringLength(100)]
    //    public string Titre { get; set; }
    //    [StringLength(1000)]
    //    public string Description { get; set; }
    //    [StringLength(200)]
    //    public string Url { get; set; }
    //    public int? AdresseId { get; set; }
    //    public Adresse Adresse { get; set; }
    //    public List<Participation> Participations{ get; set; }
    //}

}
