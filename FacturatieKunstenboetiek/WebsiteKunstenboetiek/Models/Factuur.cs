//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebsiteKunstenboetiek.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Factuur
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Factuur()
        {
            this.FactuurRegels = new HashSet<FactuurRegel>();
        }
    
        public int FactuurNr { get; set; }
        public int KlantNr { get; set; }
        public string Datum { get; set; }
    
        public virtual Klant Klant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactuurRegel> FactuurRegels { get; set; }
    }
}