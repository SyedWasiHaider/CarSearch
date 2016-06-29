using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading.Forms;
using Xamarin.Forms;
//HorizontalImageList
namespace CarSearch
{
	public class HorizontalImageList : Grid
	{
		protected ScrollView ScrollView;
		protected readonly ICommand SelectedCommand;
		protected readonly StackLayout ItemsStackLayout;
		protected Boolean Wait = false;


		private Boolean _isScrollAutomaticInitialized;

		public HorizontalImageList()
		{
			ScrollView = new ScrollView
			{
				Orientation = ScrollOrientation.Horizontal
			};

			ItemsStackLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(0),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			ScrollView.Content = ItemsStackLayout;
			Children.Add(ScrollView);

			SelectedCommand = new Command<object>(item =>
			{
				var selectable = item as ISelectable;
				if (selectable == null) return;

				SetSelected(selectable);
				SelectedItem = selectable.IsSelected ? selectable : null;
			});

			SelectedItemChanged += (sender, e) =>
			{
				ActualElementIndex = (e as SelectedItemsChangedEventArgs).itemChanged.index;
			};

			PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == "Orientation")
				{
					ItemsStackLayout.Orientation = ScrollView.Orientation == ScrollOrientation.Horizontal ? StackOrientation.Horizontal : StackOrientation.Vertical;
				}
				else if (e.PropertyName == "Width")
				{
					foreach (var child in ItemsStackLayout.Children)
					{
						child.WidthRequest = Width / ItemsOnFullPage;
					}
				}
			};
		}



		public int ItemsCount
		{
			get { return this.ItemsStackLayout.Children.Count; }
		}

		protected virtual void SetSelected(ISelectable selectable)
		{
			selectable.IsSelected = true;
		}

		public View ActualElement
		{
			get
			{
				return ItemsStackLayout.Children[ActualElementIndex];
			}
		}



		public bool ScrollToStartOnSelected { get; set; }

		public int ItemsOnFullPage { get; set; } = 3;

		public event EventHandler SelectedItemChanged;

        public int ActualElementIndex
		{
			get { return (int)GetValue(ActualElementIndexProperty); }
			set { SetValue(ActualElementIndexProperty, value); }
		}

		public static readonly BindableProperty ActualElementIndexProperty =
			BindableProperty.Create<HorizontalImageList, int>(p => p.ActualElementIndex, 0, BindingMode.TwoWay, null, OnIndexSelectedChanged);

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create<HorizontalImageList, IEnumerable>(p => p.ItemsSource, default(IEnumerable<object>), BindingMode.TwoWay, null, ItemsSourceChanged);

		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create<HorizontalImageList, object>(p => p.SelectedItem, default(object), BindingMode.TwoWay, null, OnSelectedItemChanged);

		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly BindableProperty ItemTemplateProperty =
			BindableProperty.Create<HorizontalImageList, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		private static void ItemsSourceChanged(BindableObject bindable, IEnumerable oldValue, IEnumerable newValue)
		{
			var itemsLayout = (HorizontalImageList)bindable;
			itemsLayout.SetItems();
		}

		protected virtual void SetItems()
		{
			ItemsStackLayout.Children.Clear();

			if (ItemsSource == null )
				return;


			foreach (var item in ItemsSource)
			{
				var selectable = item as ISelectable;
				selectable.index = ItemsStackLayout.Children.Count;
				ItemsStackLayout.Children.Add(CreateItemView(selectable));
			}

			SelectedItem = ItemsSource.OfType<ISelectable>().FirstOrDefault(x => x.IsSelected);
		}

		protected virtual View CreateItemView(ISelectable item)
		{
			var content = ItemTemplate.CreateContent();
			var view = content as View;

			if (view == null) return null;

			view.BindingContext = item;

			var gesture = new TapGestureRecognizer
			{
				Command = SelectedCommand,
				CommandParameter = item
			};

			AddGesture(view, gesture);

			return view;
		}

		protected void AddGesture(View view, TapGestureRecognizer gesture)
		{
			view.GestureRecognizers.Add(gesture);

			var layout = view as Layout<View>;

			if (layout == null)
				return;

			foreach (var child in layout.Children)
				AddGesture(child, gesture);
		}

		private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var itemsView = (HorizontalImageList)bindable;
			if (newValue == oldValue)
				return;

			var selectable = newValue as ISelectable;
			itemsView.SetSelectedItem(selectable ?? oldValue as ISelectable);
		}

		private async static void OnIndexSelectedChanged(BindableObject bindable, int oldValue, int newValue)
		{
			var itemsView = (HorizontalImageList)bindable;
			if (newValue == oldValue)
				return;
			await itemsView.ScrollToActualAsync();
		}

		protected virtual void SetSelectedItem(ISelectable selectedItem)
		{
			var items = ItemsSource;

			foreach (var item in items.OfType<ISelectable>())
				item.IsSelected = selectedItem != null && item == selectedItem && selectedItem.IsSelected;

			var itemChanged = new SelectedItemsChangedEventArgs() { itemChanged = selectedItem };
			var handler = SelectedItemChanged;
			if (handler != null)
				handler(this, itemChanged);
		}

		class SelectedItemsChangedEventArgs : EventArgs
		{
			public ISelectable itemChanged;
		}

		protected virtual async void ScrollAutomaticAsync()
		{
			while (!_isScrollAutomaticInitialized)
			{
				_isScrollAutomaticInitialized = true;
				if (Wait)
				{
					Wait = false;
					await Task.Delay(5000);
				}
				await Task.Delay(5000);
				this.ActualElementIndex++;
				await ScrollToActualAsync();
				_isScrollAutomaticInitialized = false;

			}
		}

		private async Task ScrollToActualAsync()
		{
			if (this.ActualElementIndex == this.ItemsCount)
				this.ActualElementIndex = 0;

			if (this.ActualElementIndex < 0)
				this.ActualElementIndex = 0;

			try
			{
				await this.ScrollView.ScrollToAsync(this.ActualElement.X - Width/ItemsOnFullPage, 0, false);
			}
			catch
			{
				//invalid scroll: sometimes happen
			}
		}
	}

	public interface ISelectable
	{
		bool IsSelected { get; set; }
		int index { get; set; }
	}
}