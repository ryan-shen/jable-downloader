using System.Windows.Input;
using JableDownloader.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 影片清單
    /// </summary>
    internal class VideoListViewModel : ViewModelBase
    {
        #region Private Fields
        private Pager<VideoViewModel> _pager;
        #endregion

        public VideoListViewModel()
        {
            ClickCommand = new Command(async (parameter) =>
            {
                var video = parameter as VideoViewModel;

                //固定寫死 Push 到 MainPage 即可，因為 MainPage 本身就是一個 Stack
                await Application.Current.MainPage.Navigation.PushPopupAsync(new VideoPreviewerPopupPage(video));
            });
        }

        /// <summary>
        /// 點擊事件
        /// </summary>
        public ICommand ClickCommand { get; private set; }

        /// <summary>
        /// 分頁物件
        /// </summary>
        public Pager<VideoViewModel> Pager
        {
            get { return _pager; }
            set { SetProperty(ref _pager, value); }
        }
    }
}
