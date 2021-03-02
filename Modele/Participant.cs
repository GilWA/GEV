using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Modele
{
    public class Participant
    {
        public int ParticipantId { get; set; }

        [Required(ErrorMessage = "{0} requis")]
        [DisplayName("Civilité")]
        public Civilite Civilite { get; set; }
        [Required(ErrorMessage = "{0} requise")]
        [StringLength(50, ErrorMessage = "{0} caractères max")]
        public string Nom { get; set; }
        [StringLength(50, ErrorMessage = "{0} caractères max")]
        [DisplayName("Prénom")]
        public string Prenom { get; set; }
        [DisplayName("Année naissance")]
        public int? AnneeNaissance { get; set; }
        [DisplayName("Année décès")]
        public int? AnneeDeces { get; set; }
        [StringLength(80)]
        [DisplayName("Lieu naissance")]
        public string LieuNaissance { get; set; }
        [StringLength(1000, ErrorMessage = "{0} caractères max")]
        [DisplayName("Activités")]
        public string Activites { get; set; }
        [StringLength(200, ErrorMessage = "{0} caractères max")]
        [DisplayName("Fiche Internet")]
        public string Url { get; set; }
        public List<Participation> _participations;
        public List<Participation> Participations
        {
            get { return _participations ??= new List<Participation>(); }
        }

        public override string ToString()
        {
            return $"{Civilite} {Prenom} {Nom}";
        }
    }
    //public class Participant
    //{
    //    public enum Sexe
    //    {
    //        Masculin, Feminin
    //    }
    //    public int ParticipantId { get; set; }
    //    [Required]
    //    public Civilite Civilite { get; set; }

    //    [StringLength(50)]
    //    [Required]
    //    public string Nom { get; set; }
    //    [StringLength(50)]

    //    public string Prenom { get; set; }
    //    public int? AnneeNaissance { get; set; }
    //    public int? AnneeDeces { get; set; }
    //    [StringLength(80)] 
    //    public string LieuNaissance { get; set; }

    //    [StringLength(500)]
    //    public string Activites { get; set; }
    //    [StringLength(200)]
    //    public string Url { get; set; }

    //    public List<Participation> Participations { get; set; }
    //}
}
