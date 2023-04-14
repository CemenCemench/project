//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Books.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Readers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Readers()
        {
            this.Readings = new HashSet<Readings>();
        }
    
        public int ReaId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }
        public string FullName { get; set; }
        public Nullable<int> PspSer { get; set; }
        public Nullable<int> PspNum { get; set; }
        public Nullable<int> FullPspNum { get; set; }
        public Nullable<int> GenId { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
    
        public virtual Gender Gender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Readings> Readings { get; set; }
    }
}