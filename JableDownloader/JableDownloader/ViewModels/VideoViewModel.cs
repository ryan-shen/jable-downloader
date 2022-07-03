using System;
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
    public class VideoViewModel : ViewModelBase
    {
        private IFileService _fileService = DependencyService.Get<IFileService>();
        private double _downloadProgress;
        private bool _isDownloading;

        public VideoViewModel()
        {
            Play = new Command(async () =>
            {
                await PopupNavigation.Instance.PopAsync();
                await Application.Current.MainPage.Navigation.PushModalAsync(new HlsPlayerPage(await GetVideoUrl(Url)));
            });

            Download = new Command(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("", "Download this video?", "OK", "Cancel");

                IsDownloading = true;
                //test url: https://jable.tv/videos/heyzo-2556/
                await new HlsDownloader(await GetVideoUrl(Url)).SequenceDownloadAsync(Path.Combine(_fileService.GetDownloadDirectory(), $"{Name}.mp4"), (index, total) =>
                {
                    DownloadProgress = index * 1.0 / total;
                });
                IsDownloading = false;
                await Application.Current.MainPage.DisplayAlert("", $"{Name} has been successfully downloaded", "OK");
            });
        }

        public ICommand Play { get; set; }
        public ICommand Download { get; set; }
        public Func<string, Task<string>> GetVideoUrl { get; set; }
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
