using Foundation;
using UIKit;

namespace VidiOS {
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class App : UIApplicationDelegate {
		// class-level declarations
		
		// FIXME: These data should not be hardcoded in here..

		public static readonly Contact [] Contacts = new [] {
			new Contact { Name = "Alex", Online = true },
			new Contact { Name = "Amy", Online = true },
			new Contact { Name = "Alan", Online = true },
			new Contact { Name = "Tony", Online = false }		
		};

		public static readonly Novelty [] Novelties = new [] {
			new Novelty (NoveltyType.Hat, UIImage.FromBundle ("hat1.png")),
			new Novelty (NoveltyType.Hat, UIImage.FromBundle ("hat2.png")),
			new Novelty (NoveltyType.Hat, UIImage.FromBundle ("hat3.png")),
			new Novelty (NoveltyType.Mustache, UIImage.FromBundle ("mustache1.png")),
			new Novelty (NoveltyType.Mustache, UIImage.FromBundle ("mustache2.png"))
		};

		public override UIWindow Window {
			get;
			set;
		}

		public static UIViewController TopViewController {
			get {
				UIViewController presented;
				var vc = UIApplication.SharedApplication.KeyWindow?.RootViewController;
				while ((presented = vc?.PresentedViewController) != null)
					vc = presented;
				return vc;
			}
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			return true;
		}
	}
}

