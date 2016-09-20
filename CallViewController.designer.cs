// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace VidiOS
{
    [Register ("CallViewController")]
    partial class CallViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        VidiOS.CallButton CallButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        VidiOS.CallView CallView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationBar NavBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint NoveltyBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint NoveltyLeadingConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView NoveltyList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        VidiOS.SelfView SelfView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITapGestureRecognizer TapRecognizer { get; set; }

        [Action ("OnNoveltyShowHide:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnNoveltyShowHide (UIKit.UIBarButtonItem sender);

        [Action ("OnTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnTapped (UIKit.UITapGestureRecognizer sender);

        void ReleaseDesignerOutlets ()
        {
            if (CallButton != null) {
                CallButton.Dispose ();
                CallButton = null;
            }

            if (CallView != null) {
                CallView.Dispose ();
                CallView = null;
            }

            if (NavBar != null) {
                NavBar.Dispose ();
                NavBar = null;
            }

            if (NoveltyBottomConstraint != null) {
                NoveltyBottomConstraint.Dispose ();
                NoveltyBottomConstraint = null;
            }

            if (NoveltyLeadingConstraint != null) {
                NoveltyLeadingConstraint.Dispose ();
                NoveltyLeadingConstraint = null;
            }

            if (NoveltyList != null) {
                NoveltyList.Dispose ();
                NoveltyList = null;
            }

            if (SelfView != null) {
                SelfView.Dispose ();
                SelfView = null;
            }

            if (TapRecognizer != null) {
                TapRecognizer.Dispose ();
                TapRecognizer = null;
            }
        }
    }
}