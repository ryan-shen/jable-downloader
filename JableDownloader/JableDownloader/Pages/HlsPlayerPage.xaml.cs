using MediaManager;
using MediaManager.Forms;
using MediaManager.Playback;
using MediaManager.Player;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    /// <summary>
    /// HLS 影片播放頁面
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HlsPlayerPage : ContentPage
    {
        public HlsPlayerPage(string m3u8Url)
        {
            InitializeComponent();

            VideoView.Source = m3u8Url;

            //必須強制在 UI Thread 執行，用來否則 iOS 在設定 VideoIndicator.IsRunning 時會噴 UIKitThreadAccessException
            CrossMediaManager.Current.StateChanged += (object sender, StateChangedEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    switch (e.State)
                    {
                        case MediaPlayerState.Buffering:
                            VideoIndicator.IsRunning = true;
                            break;
                        case MediaPlayerState.Failed:
                            await DisplayAlert("Error", "Loading video failed", "OK");
                            break;
                        default:
                            VideoIndicator.IsRunning = false;
                            break;
                    }
                });
            };
        }

        protected override async void OnDisappearing()
        {
            //在關閉頁面時停止播放器，不然下次開啟時可能會看到上次播放的最後一個畫面
            await CrossMediaManager.Current.Stop();

            base.OnDisappearing();
        }
    }
}