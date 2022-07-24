using System.Threading.Tasks;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    /// <summary>
    /// 「熱度優先」頁面
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopularVideoListTabbedPage : TabbedPage
    {
        public PopularVideoListTabbedPage()
        {
            InitializeComponent();

            _ = SetBindingContext();
        }

        /// <summary>
        /// 注入資料來源到底下的各個頁面
        /// </summary>
        /// <returns></returns>
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