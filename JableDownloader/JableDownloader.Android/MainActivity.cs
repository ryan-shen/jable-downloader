using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using MediaManager;
using Plugin.LocalNotification;
using Android.Content;

namespace JableDownloader.Droid
{
    [Activity(Label = "JableDownloader", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //啟用 FFImageLoading，用來顯示 SVG 圖片
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            //啟用 Plugin.MediaManager，用來顯示 HLS 影片
            CrossMediaManager.Current.Init(this);

            //啟用 Popup Page
            Rg.Plugins.Popup.Popup.Init(this);

            //啟用 Plugin.LocalNotification 的自訂事件功能
            NotificationCenter.CreateNotificationChannel();
            NotificationCenter.NotifyNotificationTapped(Intent);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            //啟用 Plugin.LocalNotification 的自訂事件功能
            NotificationCenter.NotifyNotificationTapped(intent);

            base.OnNewIntent(intent);
        }

        public override void OnBackPressed()
        {
            //Rg.Plugins.Popup.Popup.SendBackPressed 回傳 True 代表正在 Popup Page 內，否則就不是
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }
    }
}