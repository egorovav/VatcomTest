using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VatcomTest.Model;

namespace VatcomTest
{
	public class ReceiptViewModel : BaseViewModel
	{
		public ReceiptViewModel(Receipt aReceipt)
		{
			this.FReceipt = aReceipt;
			this.ProductItem = new ProductItemCollection(aReceipt);
			this.DeleteItemCommand = new Command(() => this.DeleteItem(), false);
			this.AddItemCommand = new Command(() => this.AddItem(), true);
		}

		public ReceiptViewModel() : this(new Receipt())
		{
			this.FReceipt.ReceiptId = Guid.NewGuid();
		}

		private Receipt FReceipt;
		public Receipt Receipt
		{
			get { return this.FReceipt; }
		}

		public static string OpenDatePropertyName = "OpenDate";
		public DateTime OpenDate 
		{
			get { return this.FReceipt.OpenDate; } 
			set
			{
				this.FReceipt.OpenDate = value;
				NotifyPropertyChanged(OpenDatePropertyName);
			}
		}

		public static string CloseDatePropertyName = "CloseDate";
		public DateTime CloseDate 
		{
			get { return this.FReceipt.CloseDate; } 
			set
			{
				this.FReceipt.CloseDate = value;
				NotifyPropertyChanged(CloseDatePropertyName);
			}
		}

		public static string ShiftNumberPropertyName = "ShiftNumber";
		public string ShiftNumber 
		{
			get { return this.FReceipt.ShiftNumber; } 
			set
			{
				this.FReceipt.ShiftNumber = value;
				NotifyPropertyChanged(ShiftNumberPropertyName);
			}
		}

		public static string CashierNamePropertyName = "CashierName";
		public string CashierName 
		{
			get { return this.FReceipt.CashierName; } 
			set
			{
				this.FReceipt.CashierName = value;
				NotifyPropertyChanged(CashierNamePropertyName);
			}
		}

		public static string CashRegisterIdPropertyName = "CashRegisterId";
		public string CashRegisterId 
		{
			get { return this.FReceipt.CashRegisterId; }
			set
			{
				this.FReceipt.CashRegisterId = value;
				NotifyPropertyChanged(CashRegisterIdPropertyName);
			}
		}

		public ProductItemCollection ProductItem 
		{
			get;
			protected set;
		}

		public static string SelectedItemPropertyName = "SelectedItem";
		private ItemViewModel FSelectedItem;
		public ItemViewModel SelectedItem
		{
			get { return this.FSelectedItem; }
			set
			{
				this.FSelectedItem = value;
				this.DeleteItemCommand.CanExecuteCommand = value != null;
				NotifyPropertyChanged(SelectedItemPropertyName);
			}
		}

		private Receipt FBuckup = new Receipt();

		public void BackupState()
		{
			this.FBuckup.CashierName = this.CashierName;
			this.FBuckup.CashRegisterId = this.CashRegisterId;
			this.FBuckup.CloseDate = this.CloseDate;
			this.FBuckup.OpenDate = this.OpenDate;
			this.FBuckup.ShiftNumber = this.ShiftNumber;
		}

		public void RestoreState()
		{
			this.CashierName = this.FBuckup.CashierName;
			this.CashRegisterId = this.FBuckup.CashRegisterId;
			this.CloseDate = this.FBuckup.CloseDate;
			this.OpenDate = this.FBuckup.OpenDate;
			this.ShiftNumber = this.FBuckup.ShiftNumber;
		}

		public Command AddItemCommand
		{
			get;
			protected set;
		}

		public void AddItem()
		{
			var _newItem = new ItemViewModel();
			this.ProductItem.Add(_newItem);
		}

		public Command DeleteItemCommand
		{
			get;
			protected set;
		}

		private void DeleteItem()
		{
			this.ProductItem.Remove(this.SelectedItem);
		}
	}

	public class ReceiptCollection : ObservableCollection<ReceiptViewModel>
	{
		public ReceiptCollection(ReceiptModel aModel)
		{
			foreach (var _rec in aModel.Receipt)
			{
				var _rvm = new ReceiptViewModel(_rec);
				this.Items.Add(_rvm);
				_rvm.PropertyChanged += _rvm_PropertyChanged;
				_rvm.ProductItem.CollectionChanged += ProductItem_CollectionChanged;
			}

			this.FModel = aModel;
		}

		private ReceiptModel FModel;

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var _rec in e.NewItems)
				{
					var _rvm = (ReceiptViewModel)_rec;
					this.FModel.Receipt.Add(_rvm.Receipt);
					_rvm.PropertyChanged += _rvm_PropertyChanged;
					_rvm.ProductItem.CollectionChanged += ProductItem_CollectionChanged;
				}
			}

			if (e.OldItems != null)
			{
				foreach (var _rec in e.OldItems)
				{
					var _rvm = (ReceiptViewModel)_rec;
					foreach (var _item in _rvm.ProductItem)
						this.FModel.ProductItem.Remove(_item.Item);
					this.FModel.Receipt.Remove(_rvm.Receipt);
					_rvm.PropertyChanged -= _rvm_PropertyChanged;
					_rvm.ProductItem.CollectionChanged -= ProductItem_CollectionChanged;
				}
			}

			base.OnCollectionChanged(e);
		}

		void ProductItem_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var _item in e.NewItems)
				{
					var _ivm = (ItemViewModel)_item;
					this.FModel.ProductItem.Add(_ivm.Item);
				}
			}

			if (e.OldItems != null)
			{
				foreach (var _item in e.OldItems)
				{
					var _ivm = (ItemViewModel)_item;
					this.FModel.ProductItem.Remove(_ivm.Item);
				}
			}
		}

		void _rvm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
