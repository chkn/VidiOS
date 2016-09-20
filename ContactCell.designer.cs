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
    [Register ("ContactCell")]
    partial class ContactCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        VidiOS.CallButton CallButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Name { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CallButton != null) {
                CallButton.Dispose ();
                CallButton = null;
            }

            if (Name != null) {
                Name.Dispose ();
                Name = null;
            }
        }
    }
}