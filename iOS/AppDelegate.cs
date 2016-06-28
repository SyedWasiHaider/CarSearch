using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FFImageLoading.Forms.Touch;
using Foundation;
using RoundedBoxView.Forms.Plugin.iOSUnified;
using UIKit;

namespace CarSearch.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			RoundedBoxViewRenderer.Init();
			CachedImageRenderer.Init();

			// Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}

