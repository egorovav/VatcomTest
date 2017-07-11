using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VatcomTest.Model;
using System.Data.Entity;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace VatcomTest
{
	public class SimpleViewModel : BaseViewModel
	{
		public SimpleViewModel()
		{
			this.FReceiptModel = new ReceiptModel();
			this.FReceiptModel.Receipt.Load();


			this.SaveCommand = new Command(() => this.Save(), true);
			this.AddReceiptCommand = new Command(() => this.AddNewReceipt(), true);
			this.DeleteReceiptCommand = new Command(() => this.FReceiptModel.Receipt.Local.Remove(this.SelectedReceipt), false);

			this.AddItemCommand = new Command(() => this.AddNewItem() , true);
			this.DeleteItemCommand = new Command(() => this.ReceiptProductItems.Remove(this.SelectedItem), false);

			this.FReceiptModel.Receipt.Local.CollectionChanged += Local_CollectionChanged;
			this.FReceiptProductItems.CollectionChanged += FReceiptProductItems_CollectionChanged;
		}

		private async void Save()
		{
			int c = 0;
			try
			{
				this.OperationName = "Save to Data Base.";
				this.IsBusy = true;
				c = await this.FReceiptModel.SaveChangesAsync();
				this.IsBusy = false;
			}
			catch(Exception exc)
			{
				this.ExceptionString = GetExceptionStirng(exc);
			}
		}

		private void AddNewReceipt()
		{
			var _receipt = this.FReceiptModel.Receipt.Create();
			this.FReceiptModel.Receipt.Local.Add(_receipt);
		}

		private void AddNewItem()
		{
			var _item = this.FReceiptModel.ProductItem.Create();
			this.ReceiptProductItems.Add(_item);
		}

		void Local_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var _item in e.NewItems)
				{
					var _receipt = (Receipt)_item;
					_receipt.ReceiptId = Guid.NewGuid();
				}
			}
		}

		private ReceiptModel FReceiptModel;

		public ReceiptModel ReceiptModel
		{
			get { return this.FReceiptModel; }
		}

		public static string SelectedReceiptPropertyName = "SelectedReceipt";
		private Receipt FSelectedReceipt;
		public Receipt SelectedReceipt
		{
			get { return this.FSelectedReceipt; }
			set
			{
				this.FSelectedReceipt = value;
				this.DeleteReceiptCommand.CanExecuteCommand = (this.FSelectedReceipt != null);
				NotifyPropertyChanged(SelectedReceiptPropertyName);

				this.FReceiptProductItems.CollectionChanged -= FReceiptProductItems_CollectionChanged;
				this.FReceiptProductItems.Clear();
				if (this.FSelectedReceipt != null)
				{
					foreach (var _pi in this.SelectedReceipt.ProductItem)
						this.FReceiptProductItems.Add(_pi);
				}
				this.FReceiptProductItems.CollectionChanged += FReceiptProductItems_CollectionChanged;

				NotifyPropertyChanged(ReceiptProductItemsPropertyName);
			}
		}

		public static string ReceiptProductItemsPropertyName = "ReceiptProductItems";
		private ObservableCollection<ProductItem> FReceiptProductItems = new ObservableCollection<ProductItem>();
		public ObservableCollection<ProductItem> ReceiptProductItems
		{
			get { return this.FReceiptProductItems; }
		}


		void FReceiptProductItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.SelectedReceipt == null)
				return;

			if (e.NewItems != null)
			{
				foreach (var item in e.NewItems)
				{
					var _pi = (ProductItem)item;
					this.SelectedReceipt.ProductItem.Add(_pi);
					_pi.ProductItemId = Guid.NewGuid();
					_pi.ReceiptId = this.SelectedReceipt.ReceiptId;
				}
			}

			if (e.OldItems != null)
			{
				foreach (var _item in e.OldItems)
					this.FReceiptModel.ProductItem.Remove((ProductItem)_item);
			}
		}

		public static string SelectedItemPropertyName = "SelectedItem";
		private ProductItem FSelectedItem;
		public ProductItem SelectedItem
		{
			get { return this.FSelectedItem; }
			set
			{
				this.FSelectedItem = value;
				this.DeleteItemCommand.CanExecuteCommand = (this.FSelectedItem != null);
				NotifyPropertyChanged(SelectedItemPropertyName);
			}
		}
		

		public Command SaveCommand
		{
			get;
			protected set;
		}

		public Command AddReceiptCommand
		{
			get;
			protected set;
		}

		public Command DeleteReceiptCommand
		{
			get;
			protected set;
		}

		public Command AddItemCommand
		{
			get;
			protected set;
		}

		public Command DeleteItemCommand
		{
			get;
			protected set;
		}
	}
}
