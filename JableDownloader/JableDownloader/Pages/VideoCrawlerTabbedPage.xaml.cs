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
        public VideoCrawlerTabbedPage(IVideoCrawlerService service)
        {
            InitializeComponent();

            _ = SetBindingContext(service);

            //service.GetRecentVideos().ContinueWith((result) =>
            //{
            //    A.BindingContext = result.Result;
            //    System.Diagnostics.Debug.WriteLine(A.BindingContext);
            //});
        }

        public async Task SetBindingContext(IVideoCrawlerService service)
        {
            RecentVideoListPage.BindingContext = new VideoListViewModel
            {
                Pager = await service.GetRecentVideos()
            };

            PopularVideoListPage.BindingContext = new VideoListViewModel
            {
                Pager = await service.GetPopularVideos()
            };
        }
    }
}