namespace VatcomTest.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Receipt")]
    public partial class Receipt
    {
        public Receipt()
        {
            ProductItem = new HashSet<ProductItem>();
        }

        public Guid ReceiptId { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime CloseDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ShiftNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string CashierName { get; set; }

        [Required]
        [StringLength(50)]
        public string CashRegisterId { get; set; }

        public virtual ICollection<ProductItem> ProductItem { get; set; }
    }
}
