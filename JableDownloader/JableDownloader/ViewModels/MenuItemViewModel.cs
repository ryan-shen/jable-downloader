using System.Collections.ObjectModel;
using JableDownloader.Pages;

namespace JableDownloader.ViewModels
{
    internal class MenuItemViewModel : ViewModelBase
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public MenuItemViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>(new[]
            {
                new MenuItem { Id = 0, Title = "按主題", TargetType = typeof(ActressListTabbedPage) },
                new MenuItem { Id = 1, Title = "按女優", TargetType = typeof(ActressListTabbedPage) },
                new MenuItem { Id = 2, Title = "新片優先", TargetType = typeof(ActressListTabbedPage) },
                new MenuItem { Id = 3, Title = "熱度優先", TargetType = typeof(ActressListTabbedPage) },
            });
        }
    }
}
