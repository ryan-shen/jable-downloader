using JableDownloader.Models;
using JableDownloader.Pages;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 網站設定
    /// </summary>
    internal class SettingViewModel : ViewModelBase
    {
        #region Private Fields
        private bool _showOtherSites = Global.ShowOtherSites;
        #endregion

        public bool ShowOtherSites
        {
            get { return _showOtherSites; }
            set
            {
                if (_showOtherSites == value)
                {
                    return;
                }

                SetProperty(ref _showOtherSites, value);
                Global.ShowOtherSites = _showOtherSites;
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }
    }
}
