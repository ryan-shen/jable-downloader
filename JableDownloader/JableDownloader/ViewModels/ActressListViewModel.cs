using System.Windows.Input;
using JableDownloader.Pages;
using JableDownloader.Services;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 女優清單
    /// </summary>
    public class ActressListViewModel : ViewModelBase
    {
        private Pager<ActressViewModel> _pager;

        public ActressListViewModel()
        {
            ClickCommand = new Command(async (parameter) =>
            {
                var actress = parameter as ActressViewModel;

                //固定寫死 Push 到 MainPage 即可，因為 MainPage 本身就是一個 Stack
                await Application.Current.MainPage.Navigation.PushAsync(new VideoListPage
                {
                    BindingContext = new VideoListViewModel
                    {
                        Pager = await new JableService().GetVideos(actress.Url)
                    }
                });
            });
        }

        /// <summary>
        /// 點擊事件
        /// </summary>
        public ICommand ClickCommand { get; private set; }

        /// <summary>
        /// 分頁物件
        /// </summary>
        public Pager<ActressViewModel> Pager
        {
            get { return _pager; }
            set { SetProperty(ref _pager, value); }
        }
    }
}
