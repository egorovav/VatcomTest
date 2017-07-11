namespace VatcomTest.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductItem")]
    public partial class ProductItem
    {
        public Guid ProductItemId { get; set; }

        public Guid ReceiptId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        public byte Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Column(TypeName = "money")]
        public decimal PriceWithTax { get; set; }

        [Column(TypeName = "money")]
        public decimal Tax { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public virtual Receipt Receipt { get; set; }
    }
}
