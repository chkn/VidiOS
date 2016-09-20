using System;
using System.ComponentModel;

using UIKit;
using Foundation;
using CoreGraphics;

namespace VidiOS {

	public enum CallButtonType {
		PlaceCall,
		EndCall
	}

	[Register ("CallButton"), DesignTimeVisible (true)]
	public class CallButton : UIButton, IComponent {

		public event EventHandler Disposed;

		[Export ("type"), Browsable (true)]
		public CallButtonType Type {
			get { return type; }
			set {
				if (type != value) {
					type = value;
					UpdateColors ();
				}
			}
		}
		CallButtonType type;

		public Contact Contact {
			get { return contact; }
			set {
				if (contact != value) {
					contact = value;
					Enabled = contact.Online;
				}
			}
		}
		Contact contact;

		public override bool Enabled {
			get { return base.Enabled; }
			set {
				base.Enabled = value;
				UpdateColors ();
			}
		}
		
		public override CGRect Frame {
			get { return base.Frame; }
			set {
				// Lock the size
				base.Frame = new CGRect (value.Location, IntrinsicContentSize);
			}
		}

		public ISite Site { get; set; }

		public override CGSize IntrinsicContentSize => new CGSize (50, 50);
		public UIStoryboard Storyboard { get; private set; }

		public CallButton ()
		{
			// Called when created from code.
			Initialize ();
		}

		public CallButton (IntPtr handle): base (handle)
		{
		}

		public override void AwakeFromNib ()
		{
			// Called when created from XIB or storyboard.
			base.AwakeFromNib ();
			Initialize ();
		}

		void Initialize ()
		{
			UpdateColors ();
			Layer.CornerRadius = 25;
			TintColor = UIColor.White;
			AdjustsImageWhenDisabled = false;
			SetTitle (null, UIControlState.Normal);
			SetImage (UIImage.FromBundle ("vidcam"), UIControlState.Normal);
			
			TouchUpInside += OnTapped;
			Storyboard = (Site?.DesignMode ?? false)? null : UIStoryboard.FromName ("Main", null);
			Frame = Frame; // reset size
		}

		void UpdateColors ()
		{
			switch (Type) {

			case CallButtonType.PlaceCall:
				BackgroundColor = UIColor.Green;
				break;

			case CallButtonType.EndCall:
				BackgroundColor = UIColor.Red;
				break;
			}
			if (!Enabled)
				BackgroundColor = UIColor.LightGray;
		}

		void OnTapped (object sender, EventArgs e)
		{
			switch (Type) {
			
			case CallButtonType.PlaceCall: {
				var vc = (CallViewController)Storyboard.InstantiateViewController ("CallViewController");
				vc.Call = new Call { Contact = Contact };
				App.TopViewController.PresentViewController (vc, true, null);
			}
			break;

			case CallButtonType.EndCall: {
				var vc = App.TopViewController as CallViewController;
				//FIXME: Actually end the call..
				vc?.DismissViewController (true, null);
			}
			break;
			
			default:
				throw new NotSupportedException (Type.ToString ());
			}
		}
	}
}
