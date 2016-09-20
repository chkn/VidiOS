using System;
using System.ComponentModel;

namespace VidiOS {

	public class Call : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		public Contact Contact {
			get { return contact; }
			set {
				if (contact != value) {
					contact = value;
					PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Contact)));
				}
			}
		}
		Contact contact;

		
	}
}
