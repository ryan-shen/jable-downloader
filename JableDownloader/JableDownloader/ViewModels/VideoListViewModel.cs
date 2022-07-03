using System.Windows.Input;
using JableDownloader.Pages;
using JableDownloader.Services;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    internal class VideoListViewModel : ViewModelBase
    {
        private Pager<VideoViewModel> _pager;

        public VideoListViewModel()
        {
            ClickVideoCommand = new Command(async (parameter) =>
            {
                var video = parameter as VideoViewModel;

                //固定寫死 Push 到 MainPage 即可，因為 MainPage 本身就是一個 Stack
                await Application.Current.MainPage.Navigation.PushPopupAsync(new VideoPreviewerPopupPage(video));
            });
        }

        public ICommand ClickVideoCommand { get; private set; }

        public IVideoCrawlerService VideoCrawlerService { get; set; }

        public Pager<VideoViewModel> Pager
        {
            get { return _pager; }
            set { SetProperty(ref _pager, value); }
        }
    }
}
