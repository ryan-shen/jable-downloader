using System;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using JableDownloader.Services.Interfaces;

namespace JableDownloader.Pages
{
    /// <summary>
    /// APP 的主頁面框架
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();

            //因為 Flyout Page 已經有自己的 Navigation Bar 了，所以把 Navigation Page 的 Navigation Bar 移除
            NavigationPage.SetHasNavigationBar(this, false);

            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
            FlyoutPage.ListView.SelectedItem = FlyoutPage.ListView.ItemsSource.Cast<MenuItem>().First();
        }

        /// <summary>
        /// 選擇 MenuItem 選項時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MenuItem item = e.SelectedItem as MenuItem;
            if (item == null)
            {
                return;
            }

            //更換「搜尋」按鈕的觸發動作
            Type searchServiceType = item.ServiceType ?? typeof(JableService);
            IVideoCrawlerService searchService = (IVideoCrawlerService)Activator.CreateInstance(searchServiceType);
            SearchIcon.Command = new Command(async () => await Navigation.PushPopupAsync(new SearchPopupPage(searchService)));

            //new 出所選擇的頁面
            Page page = item.ServiceType == null 
                ? (Page)Activator.CreateInstance(item.TargetType) 
                : (Page)Activator.CreateInstance(item.TargetType, Activator.CreateInstance(item.ServiceType));
            page.Title = item.Title;

            //塞入頁面內容
            Detail = new NavigationPage(page);

            IsPresented = false;
            FlyoutPage.ListView.SelectedItem = null;
        }
    }
}