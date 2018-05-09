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
    using System.ComponentModel.DataAnnotations;
    using static MetaData.Seance;

    public partial class Seance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Seance()
        {
            this.Facture = new HashSet<Facture>();
            this.Photo = new HashSet<Photo>();
        }
    
        public int Seance_ID { get; set; }
        public Nullable<int> Photographe_ID { get; set; }
        public int Agent_ID { get; set; }
        public string Adresse { get; set; }
        public System.DateTime DateSeance { get; set; }
        public string Ville { get; set; }
        public string Statut { get; set; }
        public Nullable<System.DateTime> DateFinSeance { get; set; }
        public Nullable<decimal> Prix { get; set; }
        public string Commentaire { get; set; }
    
        public virtual Agent Agent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Facture> Facture { get; set; }
        public virtual Photographe Photographe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Photo> Photo { get; set; }
    }
}
