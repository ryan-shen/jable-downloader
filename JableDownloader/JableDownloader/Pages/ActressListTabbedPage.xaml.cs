using System.Threading.Tasks;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActressListTabbedPage : TabbedPage
    {
        public ActressListTabbedPage()
        {
            InitializeComponent();

            _ = SetBindingContext();
        }

        private async Task SetBindingContext()
        {
            var service = new JableService();

            PageOrderByName.BindingContext = new ActressListViewModel
            {
                Pager = await service.GetActresses("title", 1)
            };

            PageOrderByMostPopular.BindingContext = new ActressListViewModel
            {
                Pager = await service.GetActresses("avg_videos_popularity", 1)
            };

            PageOrderByMostVideos.BindingContext = new ActressListViewModel
            {
                Pager = await service.GetActresses("total_videos", 1)
            };
        }
    }
}