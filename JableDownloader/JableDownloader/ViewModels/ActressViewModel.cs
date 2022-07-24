namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 女優
    /// </summary>
    public class ActressViewModel : ViewModelBase
    {
        #region Private Fields
        private string _name;
        private string _videoCountText;
        private string _imageUrl;
        private string _url;
        #endregion

        /// <summary>
        /// 女優名稱
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// 拍過的影片數量
        /// </summary>
        public string VideoCountText
        {
            get { return _videoCountText; }
            set { SetProperty(ref _videoCountText, value); }
        }

        /// <summary>
        /// 大頭照網址
        /// </summary>
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }

        /// <summary>
        /// 點擊後觸發的網址
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }
    }
}
