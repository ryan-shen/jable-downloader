using System.Threading.Tasks;
using JableDownloader.Services.Interfaces;
using JableDownloader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    /// <summary>
    /// 開啟「顯示所有網站」選項後，每個網站共同用來顯示影片清單的 Tabbed Page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoCrawlerTabbedPage : TabbedPage
    {
        /// <summary>
        /// 執行查詢影片清單的服務提供者
        /// </summary>
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

        /// <summary>
        /// 注入資料來源到底下的各個頁面
        /// </summary>
        /// <returns></returns>
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