using System.Collections.ObjectModel;
using JableDownloader.Models;
using JableDownloader.Pages;
using JableDownloader.Services;

namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 頁面左側 Menu 選單
    /// </summary>
    internal class MenuItemViewModel : ViewModelBase
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public MenuItemViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>(!Global.ShowOtherSites ? 
                new[]
            {
                new MenuItem { Id = 0, Title = "按女優", TargetType = typeof(ActressListTabbedPage) },
                new MenuItem { Id = 1, Title = "新片優先", TargetType = typeof(RecentVideoListPage) },
                new MenuItem { Id = 2, Title = "熱度優先", TargetType = typeof(PopularVideoListTabbedPage) },
                new MenuItem { Id = 3, Title = "設定", TargetType = typeof(SettingPage) },
            } : new[]
            {
                new MenuItem { Id = 0, Title = "Jable", TargetType = typeof(VideoCrawlerTabbedPage), ServiceType = typeof(JableService) },
                new MenuItem { Id = 1, Title = "JavFull", TargetType = typeof(VideoCrawlerTabbedPage), ServiceType = typeof(JavFullService) },
                new MenuItem { Id = 2, Title = "設定", TargetType = typeof(SettingPage) },
            });
        }
    }
}
