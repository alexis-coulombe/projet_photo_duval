//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace projet_photo_duval.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Facture
    {
        public int Facture_ID { get; set; }
        public int Seance_ID { get; set; }
        public decimal Prix { get; set; }
        public int EstPayee { get; set; }
    
        public virtual Seance Seance { get; set; }
    }
}