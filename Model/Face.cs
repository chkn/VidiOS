using System;
using System.ComponentModel;

using CoreGraphics;

namespace VidiOS {

	public class FacesEventArgs : EventArgs {

		public Face [] Faces { get; private set; }

		public FacesEventArgs (Face [] faces)
		{
			Faces = faces;
		}
	}

	public class Face : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		public int Id {
			get;
			private set;
		}

		public CGRect Bounds {
			get { return bounds; }
			set {
				if (bounds != value) {
					bounds = value;
					PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Bounds)));
				}
			}
		}
		CGRect bounds;

		public Face (int id, CGRect bounds)
		{
			Id = id;
			Bounds = bounds;
		}
	}
}
