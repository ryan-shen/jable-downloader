using System.Threading.Tasks;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    /// <summary>
    /// 女優清單的 Tabbed Page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActressListTabbedPage : TabbedPage
    {
        public ActressListTabbedPage()
        {
            InitializeComponent();

            _ = SetBindingContext();
        }

        /// <summary>
        /// 注入資料來源到底下的各個頁面
        /// </summary>
        /// <returns></returns>
        private async Task SetBindingContext()
        {
            var service = new JableService();

            PageOrderByName.BindingContext = new ActressListViewModel
            {
                Pager = await service.GetActresses("title")
            };

            PageOrderByMostPopular.BindingContext = new ActressListViewModel
            {
                Pager = await service.GetActresses("avg_videos_popularity")
            };

            PageOrderByMostVideos.BindingContext = new ActressListViewModel
            {
                Pager = await service.GetActresses("total_videos")
            };
        }
    }
}