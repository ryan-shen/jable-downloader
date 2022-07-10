using JableDownloader.Models;
using JableDownloader.Pages;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    internal class SettingViewModel : ViewModelBase
    {
        private bool _showOtherSites = Global.ShowOtherSites;

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
