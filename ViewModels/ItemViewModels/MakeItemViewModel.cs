using System;
namespace CarSearch
{
	public class MakeItemViewModel : BaseImageItemViewModel
	{
		public MakeItemViewModel()
		{
		}


		private Make _make;
		public Make Make
		{
			get
			{
				return _make;
			}

			set
			{
				_make = value;
				RaisePropertyChanged();
			}
		}
	}
}

