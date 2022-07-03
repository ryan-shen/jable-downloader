using JableDownloader.ViewModels;
using MediaManager;
using MediaManager.Playback;
using MediaManager.Player;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPreviewerPopupPage : PopupPage
    {
        public VideoPreviewerPopupPage(VideoViewModel video)
        {
            InitializeComponent();

            BindingContext = video;

            CrossMediaManager.Current.StateChanged += async (object sender, StateChangedEventArgs e) =>
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
            };
        }

        protected override async void OnDisappearing()
        {
            //Because of MediaManager is singleton, it must be stopped before the next play for fear of the afterimage
            await CrossMediaManager.Current.Stop();

            base.OnDisappearing();
        }
    }
}