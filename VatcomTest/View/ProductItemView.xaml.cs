﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace VatcomTest
{
	/// <summary>
	/// Interaction logic for ProductItemView.xaml
	/// </summary>
	public partial class ProductItemView : Window
	{
		public ProductItemView()
		{
			InitializeComponent();
		}

		public ReceiptViewModel ViewModel
		{
			get { return (ReceiptViewModel)this.DataContext; }
			set { this.DataContext = value; }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}
	}
}
