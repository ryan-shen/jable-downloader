using System.Windows.Input;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    public class MovieViewModel : ViewModelBase
    {
        public MovieViewModel()
        {
            Download = new Command(() =>
            {

            });
        }

        public ICommand Download { get; set; }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string WatchCountText { get; set; }
        public string HeartCountText { get; set; }
    }
}
