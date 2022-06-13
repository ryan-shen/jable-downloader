using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using HtmlAgilityPack;
using JableDownloader.Pages;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    internal class ActressListViewModel : ViewModelBase
    {
        private IList<ActressViewModel> _actresses;

        public ActressListViewModel()
        {
            ClickActressCommand = new Command(async (parameter) =>
            {
                var actress = parameter as ActressViewModel;

                //固定寫死 Push 到 MainPage 即可，因為 MainPage 本身就是一個 Stack
                await Application.Current.MainPage.Navigation.PushAsync(new MovieListPage(actress.Url));
            });

            GetActresses().ContinueWith((data) =>
            {
                Actresses = data.Result;
            });
        }

        public async Task<IList<ActressViewModel>> GetActresses()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36");
            //不知為啥要設定第二次才有用
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/84.0.4147.135");
            //第幾頁
            int from = 1;
            //熱度 avg_videos_popularity，最多影片 total_videos，標題 title
            string sortBy = "total_videos";

            var uriBuilder = new UriBuilder("https://jable.tv/models/");
            NameValueCollection queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            queryString["mode"] = "async";
            queryString["function"] = "get_block";
            queryString["block_id"] = "list_models_models_list";
            queryString["sort_by"] = sortBy;
            queryString["from"] = from.ToString();
            queryString["_"] = DateTime.Now.Ticks.ToString();
            uriBuilder.Query = queryString.ToString();
            var result = await client.GetAsync(uriBuilder.ToString());

            var htmlContent = await result.Content.ReadAsStringAsync();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var actressNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@id='list_models_models_list']/div[@class='container']/section/div")
                .SelectNodes("div");

            return actressNodes.Select(node => new ActressViewModel
            {
                Name = node.SelectSingleNode(".//h6[@class='title']").InnerText,
                MovieCountText = node.SelectSingleNode(".//span").InnerText,
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("src", ""),
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", "")
            }).ToList();
        }

        public ICommand ClickActressCommand { get; private set; }

        public string Name { get; set; }
        public IList<ActressViewModel> Actresses
        {
            get { return _actresses; }
            set { SetProperty(ref _actresses, value); }
        }
    }
}
