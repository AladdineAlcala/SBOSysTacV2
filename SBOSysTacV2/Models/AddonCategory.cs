//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SBOSysTacV2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddonCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AddonCategory()
        {
            this.AddonDetails = new HashSet<AddonDetail>();
        }
    
        public int addoncatId { get; set; }
        public string addoncatdesc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddonDetail> AddonDetails { get; set; }
    }
}
