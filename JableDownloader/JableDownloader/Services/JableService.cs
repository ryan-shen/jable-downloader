using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using JableDownloader.ViewModels;

namespace JableDownloader.Services
{
    public class JableService : IVideoCrawlerService
    {
        public HttpClient Client { get; set; }

        public JableService()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://jable.tv/")
            };

            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36");
            //不知為啥要設定第二次才有用
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/84.0.4147.135");

            Client = client;
        }

        public async Task<Pager<ActressViewModel>> GetActresses(string sortBy, int page = 1)
        {
            var uriBuilder = new UriBuilder(new Uri(Client.BaseAddress, "models/"));
            NameValueCollection queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            queryString["mode"] = "async";
            queryString["function"] = "get_block";
            queryString["block_id"] = "list_models_models_list";
            queryString["sort_by"] = sortBy; //熱度 avg_videos_popularity，最多影片 total_videos，標題 title
            queryString["from"] = page.ToString(); //第幾頁
            queryString["_"] = DateTime.Now.Ticks.ToString();
            uriBuilder.Query = queryString.ToString();
            var result = await Client.GetAsync(uriBuilder.ToString());

            var htmlContent = await result.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var actressNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@id='list_models_models_list']/div[@class='container']/section/div")
                .SelectNodes("div");

            HtmlNode pageNode = htmlDocument.DocumentNode.SelectSingleNode("(//ul[@class='pagination']/li[@class='page-item']/a[@data-parameters])[last()]");
            string dataParameters = pageNode.Attributes["data-parameters"].Value;
            int pageCount = Convert.ToInt32(Regex.Match(dataParameters, @"(?<=from:)\d+").Value);

            return new Pager<ActressViewModel>(actressNodes.Select(node => new ActressViewModel
            {
                Name = node.SelectSingleNode(".//h6[@class='title']").InnerText,
                VideoCountText = node.SelectSingleNode(".//span").InnerText,
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("src", ""),
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", "")
            }).ToList(), page, pageCount, (p) => GetActresses(sortBy, p));
        }

        public async Task<Pager<VideoViewModel>> GetVideos(string url, int page = 1)
        {
            var result = await Client.GetAsync(url);

            var htmlContent = await result.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var videoNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@id='list_videos_common_videos_list']/div[@class='container']/section/div")
                .SelectNodes("div");

            HtmlNode pageNode = htmlDocument.DocumentNode.SelectSingleNode("(//ul[@class='pagination']/li[@class='page-item']/a[@data-parameters])[last()]");
            string dataParameters = pageNode?.Attributes["data-parameters"].Value;
            int pageCount = string.IsNullOrEmpty(dataParameters) ? 
                1 : Convert.ToInt32(Regex.Match(dataParameters, @"(?<=from:)\d+").Value);
            
            return new Pager<VideoViewModel>(videoNodes.Select(node => new VideoViewModel
            {
                Name = node.SelectSingleNode(".//div[@class='detail']/h6[@class='title']").InnerText,
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("data-src", ""),
                PreviewUrl = node.SelectSingleNode(".//img").GetAttributeValue("data-preview", ""),
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", ""),
                Duration = node.SelectSingleNode(".//span[@class='label']").InnerText,
                WatchCountText = node.SelectSingleNode(".//p[@class='sub-title']/svg[1]").NextSibling.InnerText,
                HeartCountText = node.SelectSingleNode(".//p[@class='sub-title']/svg[2]").NextSibling.InnerText,
                GetVideoUrl = GetVideoUrl
            }).ToList(), page, pageCount, (p) => GetVideos(url, p));
        }

        public async Task<Pager<VideoViewModel>> GetRecentVideos(int page = 1)
        {
            var uriBuilder = new UriBuilder(new Uri(Client.BaseAddress, "latest-updates/"));
            NameValueCollection queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            queryString["mode"] = "async";
            queryString["function"] = "get_block";
            queryString["block_id"] = "list_videos_latest_videos_list";
            queryString["sort_by"] = "post_date";
            queryString["from"] = page.ToString(); //第幾頁
            queryString["_"] = DateTime.Now.Ticks.ToString();
            uriBuilder.Query = queryString.ToString();
            var result = await Client.GetAsync(uriBuilder.ToString());

            var htmlContent = await result.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var videoNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@id='list_videos_latest_videos_list']/div[@class='container']/section/div")
                .SelectNodes("div");

            HtmlNode pageNode = htmlDocument.DocumentNode.SelectSingleNode("(//ul[@class='pagination']/li[@class='page-item']/a[@data-parameters])[last()]");
            string dataParameters = pageNode.Attributes["data-parameters"].Value;
            int pageCount = Convert.ToInt32(Regex.Match(dataParameters, @"(?<=from:)\d+").Value);

            return new Pager<VideoViewModel>(videoNodes.Select(node => new VideoViewModel
            {
                Name = node.SelectSingleNode(".//h6[@class='title']").InnerText,
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("data-src", ""),
                PreviewUrl = node.SelectSingleNode(".//img").GetAttributeValue("data-preview", ""),
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", ""),
                Duration = node.SelectSingleNode(".//span[@class='label']").InnerText,
                WatchCountText = node.SelectSingleNode(".//p[@class='sub-title']/svg[1]").NextSibling.InnerText,
                HeartCountText = node.SelectSingleNode(".//p[@class='sub-title']/svg[2]").NextSibling.InnerText,
                GetVideoUrl = GetVideoUrl
            }).ToList(), page, pageCount, GetRecentVideos);
        }

        public async Task<Pager<VideoViewModel>> GetPopularVideos(int page = 1)
        {
            var uriBuilder = new UriBuilder(new Uri(Client.BaseAddress, "hot/"));
            NameValueCollection queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            queryString["mode"] = "async";
            queryString["function"] = "get_block";
            queryString["block_id"] = "list_videos_common_videos_list";
            queryString["sort_by"] = "video_viewed_week";
            queryString["from"] = page.ToString(); //第幾頁
            queryString["_"] = DateTime.Now.Ticks.ToString();
            uriBuilder.Query = queryString.ToString();
            var result = await Client.GetAsync(uriBuilder.ToString());

            var htmlContent = await result.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var videoNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@id='list_videos_common_videos_list']/div[@class='container']/section/div")
                .SelectNodes("div");

            HtmlNode pageNode = htmlDocument.DocumentNode.SelectSingleNode("(//ul[@class='pagination']/li[@class='page-item']/a[@data-parameters])[last()]");
            string dataParameters = pageNode.Attributes["data-parameters"].Value;
            int pageCount = Convert.ToInt32(Regex.Match(dataParameters, @"(?<=from:)\d+").Value);

            return new Pager<VideoViewModel>(videoNodes.Select(node => new VideoViewModel
            {
                Name = node.SelectSingleNode(".//h6[@class='title']").InnerText,
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("data-src", ""),
                PreviewUrl = node.SelectSingleNode(".//img").GetAttributeValue("data-preview", ""),
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", ""),
                Duration = node.SelectSingleNode(".//span[@class='label']").InnerText,
                WatchCountText = node.SelectSingleNode(".//p[@class='sub-title']/svg[1]").NextSibling.InnerText,
                HeartCountText = node.SelectSingleNode(".//p[@class='sub-title']/svg[2]").NextSibling.InnerText,
                GetVideoUrl = GetVideoUrl
            }).ToList(), page, pageCount, GetPopularVideos);
        }

        public async Task<string> GetVideoUrl(string url)
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();

            return Regex.Match(content, @"https?://([\w\-\.]+/)+[\w\-\.]+\.m3u8").Value;
        }
    }
}
