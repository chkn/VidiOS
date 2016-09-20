using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

using UIKit;
using Foundation;
using CoreGraphics;
using AVFoundation;
using CoreAnimation;

namespace VidiOS
{
    public partial class CallViewController : UIViewController,
		IUINavigationBarDelegate, IUICollectionViewSource
    {
		bool adornmentsVisible = true;
		bool noveltiesTrayVisible = false;

		class NoveltyAttachment {
			public Novelty Novelty;
			public CALayer Layer;
		}
		Dictionary<Face,NoveltyAttachment> noveltyLayers = new Dictionary<Face,NoveltyAttachment> ();

		public Call Call {
			get { return call; }
			set {
				if (call != value) {
					call = value;
					UpdateViews ();
				}
			}
		}
		Call call;

		public CallViewController (IntPtr handle) : base (handle)
        {
        }

		public override UIStatusBarStyle PreferredStatusBarStyle () => UIStatusBarStyle.LightContent;

		[Export ("positionForBar:")]
		public UIBarPosition GetPositionForBar (IUIBarPositioning barPositioning) => UIBarPosition.TopAttached;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NavBar.Delegate = this;
			NoveltyList.Delegate = this;
			NoveltyList.DataSource = this;
			SelfView.FacesDetected += OnFacesDetected;
			SelfView.FacesRemoved += OnFacesRemoved;
			CallView.AddGestureRecognizer (TapRecognizer);
			UpdateViews ();
		}

		public override async void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			// Only start video if we're not in the designer
			SelfView.Start ();
			// Give a second and then hide adornments
			await Task.Delay (TimeSpan.FromSeconds (1.5));
			OnTapped (null);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			SelfView.Stop ();
		}

		public override void WillTransitionToTraitCollection (UITraitCollection traitCollection, IUIViewControllerTransitionCoordinator coordinator)
		{
			base.WillTransitionToTraitCollection (traitCollection, coordinator);
			UpdateViews (traitCollection);
		}

		public override void WillRotate (UIInterfaceOrientation orientation, double duration)
		{
			base.WillRotate (orientation, duration);
			var connection = SelfView.Layer.Connection;
			if (connection == null || !connection.SupportsVideoOrientation)
				return;

			switch (orientation) {

            case UIInterfaceOrientation.LandscapeRight:
                connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeRight;
                break;
            case UIInterfaceOrientation.LandscapeLeft:
                connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeLeft;
                break;
            case UIInterfaceOrientation.PortraitUpsideDown:
                connection.VideoOrientation = AVCaptureVideoOrientation.PortraitUpsideDown;
                break;

            default:
                connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
                break;
			}
		}

		partial void OnNoveltyShowHide (UIBarButtonItem sender)
		{
			noveltiesTrayVisible = !noveltiesTrayVisible;
			View.LayoutIfNeeded ();
			UIView.Animate (0.25, () => {
				if (TraitCollection.HorizontalSizeClass == UIUserInterfaceSizeClass.Compact) {
					NoveltyBottomConstraint.Constant = noveltiesTrayVisible? 0 : -200;
				} else {
					NoveltyLeadingConstraint.Constant = noveltiesTrayVisible? 0 : -200;
				}
				View.LayoutIfNeeded ();
			});
		}

		partial void OnTapped (UITapGestureRecognizer sender)
		{
			adornmentsVisible = !adornmentsVisible;
			UIView.Animate (0.25, () => {
				var alpha = adornmentsVisible? 1 : 0;
				NavBar.Alpha = alpha;
				CallButton.Alpha = alpha;
			}, null);
		}

		void UpdateViews ()
		{
			UpdateViews (TraitCollection);
		}
		void UpdateViews (UITraitCollection traitCollection)
		{
			if (!IsViewLoaded || Call == null)
				return;
			NavBar.TopItem.Title = Call.Contact.Name;
			
			if (traitCollection.HorizontalSizeClass == UIUserInterfaceSizeClass.Compact) {
				NoveltyList.ContentInset = UIEdgeInsets.Zero;
				NoveltyLeadingConstraint.Constant = 0;
				NoveltyBottomConstraint.Constant = noveltiesTrayVisible? 0 : -200;
			} else {
				NoveltyList.ContentInset = new UIEdgeInsets { Top = 100 };
				NoveltyLeadingConstraint.Constant = noveltiesTrayVisible? 0 : -200;
				NoveltyBottomConstraint.Constant = 0;
			}
			View.LayoutIfNeeded ();
		}

		void OnFacesDetected (object sender, FacesEventArgs e)
		{
			foreach (var face in e.Faces) {
				noveltyLayers.Add (face, new NoveltyAttachment ());
				face.PropertyChanged += Face_PropertyChanged;
			}
		}

		void OnFacesRemoved (object sender, FacesEventArgs e)
		{
			foreach (var face in e.Faces) {
				face.PropertyChanged -= Face_PropertyChanged;
				var att = noveltyLayers [face];
				if (att.Layer != null)
					att.Layer.RemoveFromSuperLayer ();
				noveltyLayers.Remove (face);
			}
		}

		void Face_PropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			var face = (Face)sender;
			var att = noveltyLayers [face];
			if (att.Layer != null)
				att.Layer.Frame = att.Novelty.GetLocation (face.Bounds);
		}

		// Novelties:

		public nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return App.Novelties.Length;
		}

		public UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (NoveltyCell)collectionView.DequeueReusableCell ("NoveltyCell", indexPath);
			cell.Novelty = App.Novelties [indexPath.Row];
			return cell;
		}

		[Export ("collectionView:shouldSelectItemAtIndexPath:")]
		public bool ShouldSelectItem (UICollectionView collectionView, NSIndexPath indexPath)
		{
			return true;
		}

		[Export ("collectionView:didSelectItemAtIndexPath:")]
		public void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var novelty = ((NoveltyCell)collectionView.CellForItem (indexPath)).Novelty;
			foreach (var kv in noveltyLayers) {
				var layer = kv.Value.Layer;
				if (layer == null) {
					layer = new CALayer ();
					layer.ContentsGravity = CALayer.GravityResizeAspect;
					SelfView.Layer.AddSublayer (layer);
				}
				layer.Contents = novelty.Image.CGImage;
				kv.Value.Layer = layer;
				kv.Value.Novelty = novelty;
			}
		}
	}
}