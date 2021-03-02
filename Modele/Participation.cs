using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modele
{
    public class Participation
    {
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }
        public int EvenementId { get; set; }
        public Evenement Evenement { get; set; }
        [StringLength(1000)]
        public string Commentaires { get; set; }
    }
}
