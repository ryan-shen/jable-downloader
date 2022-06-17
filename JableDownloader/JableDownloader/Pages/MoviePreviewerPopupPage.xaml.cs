using JableDownloader.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviePreviewerPopupPage : PopupPage
    {
        public MoviePreviewerPopupPage(MovieViewModel movie)
        {
            InitializeComponent();

            BindingContext = movie;
        }
    }
}