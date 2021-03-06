//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetails>();
            this.Reviews = new HashSet<Reviews>();
        }
    
        public int Pro_Id { get; set; }
        public string Pro_Name { get; set; }
        public decimal Pro_Price { get; set; }
        public string Pro_Image { get; set; }
        public string Pro_Description { get; set; }
        public Nullable<int> Pro_Category { get; set; }
        public Nullable<int> Pro_Brand { get; set; }
        public Nullable<int> Pro_Status { get; set; }
        public Nullable<int> Pro_QuantityOnStock { get; set; }
    
        public virtual Brands Brands { get; set; }
        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}
