using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarSearch
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private bool _isBusy = false;
		public bool IsBusy
		{
			get
			{
				return _isBusy;
			}
			set
			{
				_isBusy = value;
				RaisePropertyChanged();
			}

		}


		public void RaisePropertyChanged([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}

		public BaseViewModel()
		{

		}

	}
}

