using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VatcomTest.Model;

namespace VatcomTest
{
	public class ItemViewModel : BaseViewModel
	{
		public ItemViewModel(ProductItem aItem)
		{
			this.FItem = aItem;
		}

		public ItemViewModel() : this(new ProductItem())
		{
			this.FItem.ProductItemId = Guid.NewGuid();
		}

		private ProductItem FItem;
		public ProductItem Item
		{
			get { return this.FItem; }
		}

		public Guid ReceiptId
		{
			get { return this.FItem.ReceiptId; }
			set { this.FItem.ReceiptId = value; }
		}

		public static string ProductNamePropertyName = "ProductName";
		public string ProductName 
		{
			get { return this.FItem.ProductName; } 
			set
			{
				this.FItem.ProductName = value;
				NotifyPropertyChanged(ProductNamePropertyName);
			}
		}

		public static string QuantityPropertyName = "Quantity";
		public byte Quantity 
		{
			get { return this.FItem.Quantity; } 
			set
			{
				this.FItem.Quantity = value;
				NotifyPropertyChanged(QuantityPropertyName);
			}
		}

		public static string PricePropertyName = "Price";
		public decimal Price 
		{
			get { return this.FItem.Price; } 
			set
			{
				this.FItem.Price = value;
				NotifyPropertyChanged(PricePropertyName);
			}
		}

		public static string PriceWithTaxPropertyName = "PriceWithTax";
		public decimal PriceWithTax 
		{
			get { return this.FItem.PriceWithTax; }
			set
			{
				this.FItem.PriceWithTax = value;
				NotifyPropertyChanged(PriceWithTaxPropertyName);
			}
		}

		public static string TaxPropertyName = "Tax";
		public decimal Tax 
		{
			get { return this.FItem.Tax; }
			set
			{
				this.FItem.Tax = value;
				NotifyPropertyChanged(TaxPropertyName);
			}
		}

		public static string AmountPropertyName = "Amount";
		public decimal Amount 
		{
			get { return this.FItem.Amount; } 
			set
			{
				this.FItem.Amount = value;
				NotifyPropertyChanged(AmountPropertyName);
			}
		}

		private ProductItem FBackup = new ProductItem();

		public void BackupState()
		{
			this.FBackup.ProductName = this.ProductName;
			this.FBackup.Quantity = this.Quantity;
			this.FBackup.Price = this.Price;
			this.FBackup.PriceWithTax = this.PriceWithTax;
			this.FBackup.Tax = this.Tax;
			this.FBackup.Amount = this.Amount;
		}

		public void RestoreState()
		{
			this.ProductName = this.FBackup.ProductName;
			this.Quantity = this.FBackup.Quantity;
			this.Price = this.FBackup.Price;
			this.PriceWithTax = this.FBackup.PriceWithTax;
			this.Tax = this.FBackup.Tax;
			this.Amount = this.FBackup.Amount;
		}
	}

	public class ProductItemCollection : ObservableCollection<ItemViewModel>
	{
		public ProductItemCollection(Receipt aReceipt)
		{
			foreach (var _item in aReceipt.ProductItem)
			{
				var _ivm = new ItemViewModel(_item);
				this.Items.Add(_ivm);
				_ivm.PropertyChanged += _ivm_PropertyChanged;
			}

			this.FReceipt = aReceipt;
		}

		private Receipt FReceipt;

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var _item in e.NewItems)
				{
					var _ivm = (ItemViewModel)_item;
					_ivm.ReceiptId = this.FReceipt.ReceiptId;
					_ivm.PropertyChanged += _ivm_PropertyChanged;
				}
			}

			if (e.OldItems != null)
			{
				foreach (var _item in e.OldItems)
				{
					var _ivm = (ItemViewModel)_item;
					_ivm.PropertyChanged -= _ivm_PropertyChanged;
				}
			}

			base.OnCollectionChanged(e);
		}

		void _ivm_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
