using JableDownloader.ViewModels;
using MediaManager;
using MediaManager.Playback;
using MediaManager.Player;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    /// <summary>
    /// 影片預覽頁面
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPreviewerPopupPage : PopupPage
    {
        public VideoPreviewerPopupPage(VideoViewModel video)
        {
            InitializeComponent();

            BindingContext = video;

            CrossMediaManager.Current.StateChanged += (object sender, StateChangedEventArgs e) =>
            {
                //必須強制在 UI Thread 執行，用來否則 iOS 在設定 VideoIndicator.IsRunning 時會噴 UIKitThreadAccessException
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