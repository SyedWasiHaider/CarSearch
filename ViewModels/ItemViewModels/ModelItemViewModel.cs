using System;
using System.Linq;

namespace CarSearch
{
	public class ModelItemViewModel : BaseImageItemViewModel, ISelectable
	{
		public ModelItemViewModel()
		{
		}

		private Model _model;
		public Model Model
		{
			get
			{
				return _model;
			}

			set
			{
				_model = value;
				RaisePropertyChanged();
			}
		}


		public string DescriptiveName
		{
			get
			{
				if (Model == null)
				{
					return String.Empty;
				}
				return (Model.id + " " + year).Replace("_", " ");
			}
		}

		public int year
		{
			get
			{
				if (Model == null)
				{
					return 0;
				}
				return Model.years.FirstOrDefault().year;
			}
		}


		private Engine _engine;
		public Engine engine
		{
			get
			{
				return _engine;
			}

			set
			{
				_engine = value;
				RaisePropertyChanged();
			}
		}

		private Mileage _mpg;
		public Mileage Mpg
		{
			get
			{
				return _mpg;
			}

			set
			{
				_mpg = value;
				RaisePropertyChanged();
			}
		}


		private int _msrp;
		public int MSRP
		{
			get
			{
				return _msrp;
			}
			set
			{
				_msrp = value;
				RaisePropertyChanged();
			}
		}

		public bool IsSelected
		{
			get; set;
		}

		public int index
		{
			get; set;
		}
	}
}

