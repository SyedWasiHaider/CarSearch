using System;
namespace CarSearch
{
	public class BaseImageItemViewModel : BaseViewModel
	{
		public BaseImageItemViewModel()
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
	}
}

