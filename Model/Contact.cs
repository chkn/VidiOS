using System;
using System.ComponentModel;

namespace VidiOS {

	public class Contact : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		public bool Online {
			get { return online; }
			set {
				if (online != value) {
					online = value;
					PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Online)));
				}
			}
		}
		bool online;

		public string Name {
			get { return name; }
			set {
				if (name != value) {
					name = value;
					PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Name)));
				}
			}
		}
		string name;
	}
}
