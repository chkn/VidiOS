using System;
using System.ComponentModel;

using UIKit;
using CoreGraphics;

namespace VidiOS {

	public enum NoveltyType {
		Hat,
		Mustache
	}

	public class Novelty {

		public NoveltyType Type {
			get;
			private set;
		}

		public UIImage Image {
			get;
			private set;
		}

		public Novelty (NoveltyType type, UIImage image)
		{
			Type = type;
			Image = image;
		}

		public CGRect GetLocation (CGRect faceBounds)
		{
			var imgSize = Image.Size;
			switch (Type) {

			case NoveltyType.Mustache:
				return new CGRect (faceBounds.X, faceBounds.GetMinY (), faceBounds.Width, imgSize.Height);

			case NoveltyType.Hat:
				return new CGRect (faceBounds.X, faceBounds.Y - imgSize.Height, faceBounds.Width, imgSize.Height);
			}
			return new CGRect (faceBounds.Location, imgSize);
		}
	}
}
