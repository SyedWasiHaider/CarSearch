using System;
namespace CarSearch
{
	public class MakeItemViewModel : BaseViewModel
	{
		public MakeItemViewModel()
		{
		}

		private string _imageUrl = "genericcar.png";
		public string imageUrl
		{
			get
			{
				return _imageUrl;
			}
			set
			{
				_imageUrl = value;
				RaisePropertyChanged();
			}
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

