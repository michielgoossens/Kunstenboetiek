//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FacturatieKunstenboetiek
{
    using System;
    using System.Collections.Generic;
    
    public partial class Artikel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Artikel()
        {
            this.FactuurRegels = new HashSet<FactuurRegel>();
        }
    
        public int ArtikelNr { get; set; }
        public string Naam { get; set; }
        public Nullable<double> Prijs { get; set; }
        public string Soort { get; set; }
        public string Kleur { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactuurRegel> FactuurRegels { get; set; }
    }
}