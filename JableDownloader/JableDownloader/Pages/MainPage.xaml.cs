using System;
using JableDownloader.Services;
using JableDownloader.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
            FlyoutPage.ListView.SelectedItem = FlyoutPage.ListView.ItemsSource.Cast<MenuItem>().First();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuItem;
            if (item == null)
            {
                return;
            }

            var searchServiceType = item.ServiceType ?? typeof(JableService);
            IVideoCrawlerService searchService = (IVideoCrawlerService)Activator.CreateInstance(searchServiceType);
            SearchIcon.Command = new Command(async () => await Navigation.PushPopupAsync(new SearchPopupPage(searchService)));

            var page = item.ServiceType == null 
                ? (Page)Activator.CreateInstance(item.TargetType) 
                : (Page)Activator.CreateInstance(item.TargetType, Activator.CreateInstance(item.ServiceType));
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            FlyoutPage.ListView.SelectedItem = null;
        }
    }
}