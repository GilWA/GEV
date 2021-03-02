using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modele
{
    public class Adresse
    {
        public int AdresseId { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage ="{0} requis")]
        public string Titre { get; set; }
        [StringLength(80)]
        public string Rue { get; set; }
        [StringLength(10)]
        public string CodePostal { get; set; }
        [StringLength(30)]
        [Required(ErrorMessage = "{0} requis")] 
        public string Ville { get; set; }
        [StringLength(80)]
        public string Region { get; set; }
        [StringLength(30)]
        [Required(ErrorMessage = "{0} requis")] 
        public string Pays{ get; set; }
        [StringLength(200)]
        public string Notes { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
