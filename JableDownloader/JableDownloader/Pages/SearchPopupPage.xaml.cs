using System;
using System.Threading.Tasks;
using JableDownloader.Services.Interfaces;
using JableDownloader.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    /// <summary>
    /// 「搜尋」頁面
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPopupPage : PopupPage
    {
        /// <summary>
        /// 執行搜尋的服務提供者
        /// </summary>
        private readonly IVideoCrawlerService _videoCrawlerService;

        public SearchPopupPage(IVideoCrawlerService service)
        {
            InitializeComponent();

            _videoCrawlerService = service;

            SearchTitle.Text = $"在 {_videoCrawlerService.GetSiteName()} 中搜尋";
        }

        /// <summary>
        /// 執行查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = (SearchBar)sender;

            //搜尋結果
            var result = await _videoCrawlerService.SearchVideos(searchBar.Text);

            //關閉當前「搜尋」頁面，開啟「搜尋結果」頁面
            await Task.WhenAll(Navigation.PopPopupAsync(), Navigation.PushAsync(new VideoListPage
            {
                BindingContext = new VideoListViewModel
                {
                    Pager = result
                }
            }));
        }
    }
}