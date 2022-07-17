using System.Threading.Tasks;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentVideoListPage : VideoListPage
    {
        public RecentVideoListPage()
        {
            InitializeComponent();

            _ = SetBindingContext();
        }

        public async Task SetBindingContext()
        {
            var service = new JableService();

            BindingContext = new VideoListViewModel
            {
                Pager = await service.GetRecentVideos()
            };
        }
    }
}