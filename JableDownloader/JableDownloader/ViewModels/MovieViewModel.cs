using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using JableDownloader.Pages;
using JableDownloader.Services;
using MediaManager;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    public class MovieViewModel : ViewModelBase
    {
        private IFileService _fileService = DependencyService.Get<IFileService>();
        private double _downloadProgress;
        private bool _isDownloading;

        public MovieViewModel()
        {
            Play = new Command(async () =>
            {
                HttpClient client = new HttpClient();
                var content = client.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;

                string m3u8Url = Regex.Match(content, @"https?://([\w\-\.]+/)+[\w\-\.]+\.m3u8").Value;

                await PopupNavigation.Instance.PopAsync();
                await Application.Current.MainPage.Navigation.PushModalAsync(new MoviePlayerPage(m3u8Url));
            });

            Download = new Command(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("", "Download this video?", "OK", "Cancel");

                HttpClient client = new HttpClient();
                //var content = client.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;
                var content = client.GetAsync(@"https://jable.tv/videos/heyzo-2556/").Result.Content.ReadAsStringAsync().Result;

                //string m3u8Url = Regex.Match(content, @"https?://([\w\-\.]+/)+[\w\-\.]+\.m3u8").Value;
                string m3u8Url = "https://ev-h.phncdn.com/hls/videos/202112/18/399841761/,1080P_4000K,720P_4000K,480P_2000K,240P_1000K,_399841761.mp4.urlset/index-f2-v1-a1.m3u8?validfrom=1655919086&validto=1655926286&ipa=150.116.255.89&hdl=-1&hash=NpLBdJo2dJWxiucDqoHIBMoLJw4%3D&&";

                IsDownloading = true;
                await new HlsDownloader(m3u8Url).SequenceDownloadAsync(Path.Combine(_fileService.GetDownloadDirectory(), $"{Name}.mp4"), (index, total) =>
                {
                    DownloadProgress = index * 1.0 / total;
                });
                IsDownloading = false;
                await Application.Current.MainPage.DisplayAlert("", $"{Name} has been successfully downloaded", "OK");
            });
        }

        public ICommand Play { get; set; }
        public ICommand Download { get; set; }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string WatchCountText { get; set; }
        public string HeartCountText { get; set; }

        public double DownloadProgress
        {
            get { return _downloadProgress; }
            set { SetProperty(ref _downloadProgress, value); }
        }

        public bool IsDownloading
        {
            get { return _isDownloading; }
            set { SetProperty(ref _isDownloading, value); }
        }
    }
}
