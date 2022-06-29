using System.Windows.Input;
using JableDownloader.Pages;
using JableDownloader.Services;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    public class ActressListViewModel : ViewModelBase
    {
        private Pager<ActressViewModel> _pager;

        public ActressListViewModel()
        {
            ClickActressCommand = new Command(async (parameter) =>
            {
                var actress = parameter as ActressViewModel;

                //固定寫死 Push 到 MainPage 即可，因為 MainPage 本身就是一個 Stack
                await Application.Current.MainPage.Navigation.PushAsync(new MovieListPage(actress.Url));
            });

            new JableService().GetActresses().ContinueWith((data) =>
            {
                Pager = data.Result;
            });
        }

        public ICommand ClickActressCommand { get; private set; }

        public Pager<ActressViewModel> Pager
        {
            get { return _pager; }
            set { SetProperty(ref _pager, value); }
        }
    }
}
