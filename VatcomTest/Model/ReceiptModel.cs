namespace VatcomTest.Model
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class ReceiptModel : DbContext
	{
		public ReceiptModel()
			: base("name=ReceiptModel")
		{
		}

		public virtual DbSet<ProductItem> ProductItem { get; set; }
		public virtual DbSet<Receipt> Receipt { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductItem>()
				.Property(e => e.Price)
				.HasPrecision(19, 4);

			modelBuilder.Entity<ProductItem>()
				.Property(e => e.PriceWithTax)
				.HasPrecision(19, 4);

			modelBuilder.Entity<ProductItem>()
				.Property(e => e.Tax)
				.HasPrecision(19, 4);

			modelBuilder.Entity<ProductItem>()
				.Property(e => e.Amount)
				.HasPrecision(19, 4);

			modelBuilder.Entity<Receipt>()
				.Property(e => e.ShiftNumber)
				.IsUnicode(false);

			modelBuilder.Entity<Receipt>()
				.Property(e => e.CashRegisterId)
				.IsUnicode(false);

			modelBuilder.Entity<Receipt>()
				.HasMany(e => e.ProductItem)
				.WithRequired(e => e.Receipt)
				.WillCascadeOnDelete(false);
		}
	}
}
