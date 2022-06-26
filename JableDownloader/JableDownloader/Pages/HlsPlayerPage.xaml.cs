using MediaManager;
using MediaManager.Forms;
using MediaManager.Playback;
using MediaManager.Player;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviePlayerPage : ContentPage
    {
        public MoviePlayerPage(string m3u8Url)
        {
            InitializeComponent();
            
            VideoView.Source = m3u8Url;

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