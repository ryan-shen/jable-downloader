using System;
using System.Collections.Generic;
using System.Text;

namespace JableDownloader.ViewModels
{
    internal class MovieViewModel : ViewModelBase
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string WatchCountText { get; set; }
        public string HeartCountText { get; set; }
    }
}
