using System.Threading.Tasks;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    /// <summary>
    /// 「新片優先」頁面
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentVideoListPage : VideoListPage
    {
        public RecentVideoListPage()
        {
            InitializeComponent();

            _ = SetBindingContext();
        }

        /// <summary>
        /// 綁定資料來源
        /// </summary>
        /// <returns></returns>
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