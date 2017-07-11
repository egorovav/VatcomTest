using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VatcomTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			this.ViewModel = new MainViewModel();
			this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
		}

		void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == BaseViewModel.ExceptionStringPropertyName)
			{
				MessageBox.Show(this.ViewModel.ExceptionString, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			if (e.PropertyName == MainViewModel.SelectedReceiptPropertyName)
			{
				this.spItems.DataContext = this.ViewModel.SelectedReceipt;
			}
		}

		public MainViewModel ViewModel
		{
			get { return (MainViewModel)this.DataContext; }
			set { this.DataContext = value; }
		}

		private void EditReceiptButton_Click(object sender, RoutedEventArgs e)
		{
			var _receiptEditor = new ReceiptView();
			this.ViewModel.SelectedReceipt.BackupState();
			_receiptEditor.ViewModel = this.ViewModel.SelectedReceipt;
			if (_receiptEditor.ShowDialog() == false)
			{
				this.ViewModel.SelectedReceipt.RestoreState();
			}
		}

		private void EditItemButton_Click(object sender, RoutedEventArgs e)
		{
			var _itemEditor = new ProductItemView();
			this.ViewModel.SelectedReceipt.SelectedItem.BackupState();
			_itemEditor.ViewModel = this.ViewModel.SelectedReceipt;
			if (_itemEditor.ShowDialog() == false)
			{
				this.ViewModel.SelectedReceipt.SelectedItem.RestoreState();
			}
		}
	}
}
