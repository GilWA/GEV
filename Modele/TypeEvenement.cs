using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modele
{
    public class TypeEvenement
    {
        public int TypeEvenementId { get; set; }
        [StringLength(100)]
        public string Libelle { get; set; }
    }
}
