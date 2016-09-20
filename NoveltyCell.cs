using Foundation;
using System;
using UIKit;

namespace VidiOS
{
    public partial class NoveltyCell : UICollectionViewCell
    {
    	public static readonly NSString Id = new NSString ("NoveltyCell");

    	public Novelty Novelty {
    		get { return novelty; }
    		set {
    			if (novelty != value) {
    				novelty = value;
    				ImageView.Image = novelty.Image;
    			}
    		}
    	}
    	Novelty novelty;

        public NoveltyCell (IntPtr handle) : base (handle)
        {
        }
    }
}