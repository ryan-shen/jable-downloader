using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace JableDownloader.ViewModels
{
    internal class MovieListViewModel : ViewModelBase
    {
        private IList<MovieViewModel> _movies;

        public MovieListViewModel(string url)
        {
            GetMovies(url).ContinueWith((data) =>
            {
                Movies = data.Result;
            });
        }

        public async Task<IList<MovieViewModel>> GetMovies(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36");
            //不知為啥要設定第二次才有用
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/84.0.4147.135");

            //var url = $"https://jable.tv/models/{}";
            var result = await client.GetAsync(url);

            var htmlContent = await result.Content.ReadAsStringAsync();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var movieNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@id='list_videos_common_videos_list']/div[@class='container']/section/div")
                .SelectNodes("div");

            return movieNodes.Select(node => new MovieViewModel
            {
                Name = node.SelectSingleNode(".//div[@class='detail']/h6[@class='title']").InnerText,
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("data-src", ""),
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", ""),
                Duration = node.SelectSingleNode(".//span[@class='label']").InnerText,
                WatchCountText = node.SelectSingleNode(".//p[@class='sub-title']/svg[1]").NextSibling.InnerText,
                HeartCountText = node.SelectSingleNode(".//p[@class='sub-title']/svg[2]").NextSibling.InnerText,
            }).ToList();
        }

        public IList<MovieViewModel> Movies
        {
            get { return _movies; }
            set { SetProperty(ref _movies, value); }
        }
    }
}
