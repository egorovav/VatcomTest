using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VatcomTest
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public BaseViewModel()
		{
			this.FTokenSource = new CancellationTokenSource();
			this.CancelCommand = new Command(() => this.Cancel(), true);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(string aPropertyName)
		{
			var _handler = this.PropertyChanged;
			if (_handler != null && !String.IsNullOrEmpty(aPropertyName))
			{
				_handler(this.PropertyChanged, new PropertyChangedEventArgs(aPropertyName));
			}
		}

		private void Cancel()
		{
			this.FTokenSource.Cancel();
			this.FTokenSource = new CancellationTokenSource();
			this.IsBusy = false;
		}

		public static string ExceptionStringPropertyName = "ExceptionString";
		private string FExceptionString;
		public string ExceptionString
		{
			get { return this.FExceptionString; }
			set
			{
				this.FExceptionString = value;
				NotifyPropertyChanged(ExceptionStringPropertyName);
			}
		}

		public static string IsBusyPropertyName = "IsBusy";
		private bool FIsBusy = false;
		public bool IsBusy
		{
			get { return this.FIsBusy; }
			set
			{
				this.FIsBusy = value;
				NotifyPropertyChanged(IsBusyPropertyName);
			}
		}

		public static string OperationNamePropertyName = "OperationName";
		private string FOperationName;
		public string OperationName
		{
			get { return this.FOperationName; }
			set
			{
				this.FOperationName = value;
				NotifyPropertyChanged(OperationNamePropertyName);
			}
		}

		private CancellationTokenSource FTokenSource;
		public CancellationToken Token
		{
			get
			{
				return this.FTokenSource.Token;
			}
		}

		public Command CancelCommand { get; protected set; }

		public string GetExceptionStirng(Exception exc)
		{
			var _sb = new StringBuilder();
			var _innerExc = exc;
			while (_innerExc != null)
			{
				_sb.AppendLine(_innerExc.Message);
				_innerExc = _innerExc.InnerException;
			}
			this.ExceptionString = _sb.ToString();
			return _sb.ToString();
		}
	}
}
