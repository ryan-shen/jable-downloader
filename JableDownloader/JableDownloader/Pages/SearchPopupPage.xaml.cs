using System;
using System.Threading.Tasks;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPopupPage : PopupPage
    {
        private readonly IVideoCrawlerService _videoCrawlerService;

        public SearchPopupPage(IVideoCrawlerService service)
        {
            InitializeComponent();

            _videoCrawlerService = service;

            SearchTitle.Text = $"在 {_videoCrawlerService.GetSiteName()} 中搜尋";
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = (SearchBar)sender;

            var result = await _videoCrawlerService.SearchVideos(searchBar.Text);

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