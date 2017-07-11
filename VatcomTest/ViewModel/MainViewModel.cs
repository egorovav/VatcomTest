using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VatcomTest.Model;
using System.Data.Entity;

namespace VatcomTest
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel()
		{
			this.FReceiptModel = new ReceiptModel();
			this.FReceiptModel.Receipt.Load();
			this.Receipts = new ReceiptCollection(this.FReceiptModel);

			this.SaveCommand = new Command(() => this.Save(), true);
			this.DeleteReceiptCommand = new Command(() => this.DeleteReceipt(), false);
			this.AddReceiptCommand = new Command(() => this.AddReceipt(), true);
		}

		private ReceiptModel FReceiptModel;		

		public ReceiptCollection Receipts
		{
			get;
			protected set;
		}

		public static string SelectedReceiptPropertyName = "SelectedReceipt";
		private ReceiptViewModel FSelectedReceipt;
		public ReceiptViewModel SelectedReceipt
		{
			get { return this.FSelectedReceipt; }
			set
			{
				this.FSelectedReceipt = value;
				this.DeleteReceiptCommand.CanExecuteCommand = value != null;
				NotifyPropertyChanged(SelectedReceiptPropertyName);
			}
		}

		public Command SaveCommand
		{
			get;
			protected set;
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
			catch (Exception exc)
			{
				this.ExceptionString = GetExceptionStirng(exc);
			}
		}

		public Command AddReceiptCommand
		{
			get;
			protected set;
		}

		private void AddReceipt()
		{
			var _newReceipt = new ReceiptViewModel();
			this.Receipts.Add(_newReceipt);
		}

		public Command DeleteReceiptCommand
		{
			get;
			protected set;
		}

		private void DeleteReceipt()
		{
			this.Receipts.Remove(this.SelectedReceipt);
		}

	}
}
