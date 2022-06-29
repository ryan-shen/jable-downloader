namespace JableDownloader.ViewModels
{
    public class ActressViewModel : ViewModelBase
    {
        private string _name;
        private string _movieCountText;
        private string _imageUrl;
        private string _url;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string MovieCountText
        {
            get { return _movieCountText; }
            set { SetProperty(ref _movieCountText, value); }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }

        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }
    }
}
