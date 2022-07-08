using System.Threading.Tasks;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoCrawlerTabbedPage : TabbedPage
    {
        private readonly IVideoCrawlerService _videoCrawlerService;

        public VideoCrawlerTabbedPage(IVideoCrawlerService service)
        {
            InitializeComponent();

            _videoCrawlerService = service;

            _ = SetBindingContext();

            //service.GetRecentVideos().ContinueWith((result) =>
            //{
            //    A.BindingContext = result.Result;
            //    System.Diagnostics.Debug.WriteLine(A.BindingContext);
            //});
        }

        public async Task SetBindingContext()
        {
            RecentVideoListPage.BindingContext = new VideoListViewModel
            {
                Pager = await _videoCrawlerService.GetRecentVideos()
            };

            PopularVideoListPage.BindingContext = new VideoListViewModel
            {
                Pager = await _videoCrawlerService.GetPopularVideos()
            };
        }
    }
}