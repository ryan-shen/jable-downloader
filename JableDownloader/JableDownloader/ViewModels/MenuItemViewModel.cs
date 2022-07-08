using System.Collections.ObjectModel;
using JableDownloader.Pages;
using JableDownloader.Services;

namespace JableDownloader.ViewModels
{
    internal class MenuItemViewModel : ViewModelBase
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public MenuItemViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>(new[]
            {
                new MenuItem { Id = 0, Title = "按女優", TargetType = typeof(ActressListTabbedPage) },
                new MenuItem { Id = 1, Title = "新片優先", TargetType = typeof(VideoListPage) },
                new MenuItem { Id = 2, Title = "熱度優先", TargetType = typeof(VideoListPage) },
                new MenuItem { Id = 3, Title = "設定", TargetType = typeof(SettingPage) },
                new MenuItem { Id = 4, Title = "Jable", TargetType = typeof(VideoCrawlerTabbedPage), ServiceType = typeof(JableService) },
                new MenuItem { Id = 4, Title = "JavFull", TargetType = typeof(VideoCrawlerTabbedPage), ServiceType = typeof(JableService) },
                new MenuItem { Id = 4, Title = "JavHDPorn", TargetType = typeof(VideoCrawlerTabbedPage), ServiceType = typeof(JableService) },
            });
        }
    }
}
