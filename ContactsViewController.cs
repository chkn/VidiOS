using Foundation;
using System;
using UIKit;

namespace VidiOS
{
    public partial class ContactsViewController : UITableViewController
    {
    	
    
        public ContactsViewController (IntPtr handle) : base (handle)
        {
        }

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableView, nint section)
		{
			return App.Contacts.Length;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (ContactCell)TableView.DequeueReusableCell (ContactCell.Id, indexPath);
			cell.Contact = App.Contacts [indexPath.Row];
			return cell;
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
			var cell = sender as ContactCell;
			if (cell == null)
				return;

			var dest = (ContactViewController)segue.DestinationViewController;
			dest.Contact = cell.Contact;	
		}
    }
}