using Foundation;
using MediaManager;
using UIKit;

namespace JableDownloader.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            //啟用 FFImageLoading，用來顯示 SVG 圖片
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            //啟用 Plugin.MediaManager，用來顯示 HLS 影片
            CrossMediaManager.Current.Init();

            //啟用 Popup Page
            Rg.Plugins.Popup.Popup.Init();

            //要求發送推播的權限，沒有要求的話會在第一次推播時要求
            Plugin.LocalNotification.NotificationCenter.AskPermission();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            //啟用 Plugin.LocalNotification 的自訂事件功能
            Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }
    }
}
