using Foundation;
using System;
using UIKit;

namespace VidiOS
{
    public partial class ContactViewController : UIViewController
    {
    	public Contact Contact {
    		get { return contact; }
    		set {
    			if (contact != value) {
    				contact = value;
    				UpdateViews ();
    			}
    		}
    	}
    	Contact contact;
    
        public ContactViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			UpdateViews ();
		}

		void UpdateViews ()
		{
			if (!IsViewLoaded)
				return;
			if (contact == null) {
				Name.Hidden = true;
				CallButton.Hidden = true;
			} else {
				Name.Hidden = false;
				Name.Text = contact.Name;
				CallButton.Hidden = false;
				CallButton.Contact = contact;
			}
		}
    }
}