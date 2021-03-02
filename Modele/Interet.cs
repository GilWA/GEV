using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Modele
{
    public class Interet
    {
        public int InteretId { get; set; }
        [Required]
        [StringLength(450, ErrorMessage = "{0} caractères max")] 
        public string UserId { get; set; }
        [Range(1,5,ErrorMessage ="Entrez une valeur entre {1} et {2}")]
        public int Rating { get; set; }
        [DisplayName("Evénement")]
        public int EvenementId { get; set; }
        // Mémoriser le Titre de l'événement indépendamment de la Pté de nav Evenement
        [NotMapped]
        public string TitreEvenement { get; set; }
        public Evenement Evenement { get; set; }
        [StringLength(1000, ErrorMessage = "{0} caractères max")]
        public string Commentaires { get; set; }
    }
}
