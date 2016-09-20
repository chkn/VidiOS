
using System;
using System.Linq;

using UIKit;
using Foundation;

namespace VidiOS
{
    public partial class SplitViewController : UISplitViewController, IUISplitViewControllerDelegate
    {
		public SplitViewController (IntPtr handle) : base (handle)
		{
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			Delegate = this;
			PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
		}

		[Export ("splitViewController:collapseSecondaryViewController:ontoPrimaryViewController:")]
		public new bool CollapseSecondViewController (UISplitViewController splitViewController, UIViewController secondaryViewController, UIViewController primaryViewController)
		{
			return true;
		}
    }
}