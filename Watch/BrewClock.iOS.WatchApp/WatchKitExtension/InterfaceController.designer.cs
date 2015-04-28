// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace WatchKitExtension
{
	[Register ("InterfaceController")]
	partial class InterfaceController
	{
		[Outlet]
		WatchKit.WKInterfaceLabel BrewsLabel { get; set; }

		[Outlet]
		WatchKit.WKInterfaceLabel BrewTimeLabel { get; set; }

		[Outlet]
		WatchKit.WKInterfaceSlider BrewTimeSlider { get; set; }

		[Outlet]
		WatchKit.WKInterfaceButton StartButton { get; set; }

		[Action ("SliderValueChanged:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SliderValueChanged (WatchKit.WKInterfaceSlider sender);

		[Action ("StartButton_Activated:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void StartButton_Activated (WatchKit.WKInterfaceButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
