using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using Xamarin.Forms;

namespace CarSearch
{
	public static class StyleLoader
	{
		public static void Load(Type style)
		{
			if (style == null)
			{
				return;
			}

			if (Application.Current.Resources == null)
			{
				Application.Current.Resources = new ResourceDictionary();
			}


			var fields = style.GetRuntimeFields();

			foreach (var f in fields)
			{
				var name = f.Name;
				var fieldValue = f.GetValue(style);
				Application.Current.Resources.Add(name, fieldValue);
			}
		}
	}
}

