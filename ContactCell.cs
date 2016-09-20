using Foundation;
using System;
using UIKit;

namespace VidiOS
{
    public partial class ContactCell : UITableViewCell
    {
    	public static readonly NSString Id = new NSString ("ContactCell");

   		public Contact Contact {
   			get { return contact; }
   			set {
   				if (contact != value) {
   					contact = value;
   					Name.Text = contact.Name;
   					CallButton.Contact = contact;
   				}
   			}
   		}
   		Contact contact;

        public ContactCell (IntPtr handle) : base (handle)
        {
        }
    }
}