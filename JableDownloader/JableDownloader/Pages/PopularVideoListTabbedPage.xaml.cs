using System.Threading.Tasks;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopularVideoListTabbedPage : TabbedPage
    {
        public PopularVideoListTabbedPage()
        {
            InitializeComponent();

            _ = SetBindingContext();
        }

        public async Task SetBindingContext()
        {
            var service = new JableService();

            AllPopularVideoListPage.BindingContext = new VideoListViewModel
            {
                Pager = await service.GetPopularVideos("video_viewed")
            };

            MonthlyPopularVideoListPage.BindingContext = new VideoListViewModel
            {
                Pager = await service.GetPopularVideos("video_viewed_month")
            };

            WeeklyPopularVideoListPage.BindingContext = new VideoListViewModel
            {
                Pager = await service.GetPopularVideos("video_viewed_week")
            };

            DailyPopularVideoListPage.BindingContext = new VideoListViewModel
            {
                Pager = await service.GetPopularVideos("video_viewed_today")
            };
        }
    }
}