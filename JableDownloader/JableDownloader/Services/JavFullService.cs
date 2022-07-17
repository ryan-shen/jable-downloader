using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using JableDownloader.ViewModels;

namespace JableDownloader.Services
{
    internal class JavFullService : IVideoCrawlerService
    {
        private readonly HttpClient _client;

        public JavFullService()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://javfull.net/")
            };

            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36");

            _client = client;
        }

        public string GetSiteName()
        {
            return "JavFull";
        }

        public async Task<Pager<VideoViewModel>> GetRecentVideos(int page = 1)
        {
            var result = await _client.GetAsync($"page/{page}/");

            var htmlContent = await result.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var videoNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@id='renderTemp']")
                .SelectNodes("div");

            HtmlNode pageNode = htmlDocument.DocumentNode.SelectSingleNode("(//ul[@class='pagination justify-content-center']/li/a[name(*) != 'span'])[last()]");
            int pageCount = pageNode == null ? 1 : Convert.ToInt32(pageNode.InnerText);

            return new Pager<VideoViewModel>(videoNodes.Select(node => new VideoViewModel
            {
                Name = HttpUtility.HtmlDecode(node.SelectSingleNode(".//div[@class='video-caption']/h4").InnerText),
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("src", ""),
                PreviewUrl = null,
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", ""),
                Duration = "-",
                WatchCountText = Regex.Match(node.SelectSingleNode(".//div[@class='video-caption']/div").InnerText, @"[\d,]+(?= view)").Value,
                HeartCountText = "-",
                GetVideoUrl = GetVideoUrl
            }).ToList(), page, pageCount, GetRecentVideos);
        }

        public async Task<Pager<VideoViewModel>> GetPopularVideos(int page = 1)
        {
            var result = await _client.GetAsync($"top-jav-movies-this-month/");

            var htmlContent = await result.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var videoNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@id='renderTemp']")
                .SelectNodes("div");

            return new Pager<VideoViewModel>(videoNodes.Select(node => new VideoViewModel
            {
                Name = HttpUtility.HtmlDecode(node.SelectSingleNode(".//div[@class='video-caption']/h4").InnerText),
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("src", ""),
                PreviewUrl = null,
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", ""),
                Duration = "-",
                WatchCountText = Regex.Match(node.SelectSingleNode(".//div[@class='video-caption']/div").InnerText, @"[\d,]+(?= view)").Value,
                HeartCountText = "-",
                GetVideoUrl = GetVideoUrl
            }).ToList(), page, 1, GetRecentVideos);
        }

        public async Task<Pager<VideoViewModel>> SearchVideos(string query, int page = 1)
        {
            var result = await _client.GetAsync($"search/{query}");

            var htmlContent = await result.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            var videoNodes = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@class='left-content']/div[@class='row']")
                .SelectNodes("div");

            HtmlNode pageNode = htmlDocument.DocumentNode.SelectSingleNode("(//ul[@class='pagination justify-content-center']/li/a[name(*) != 'span'])[last()]");
            int pageCount = pageNode == null ? 1 : Convert.ToInt32(pageNode.InnerText);

            return new Pager<VideoViewModel>(videoNodes.Select(node => new VideoViewModel
            {
                Name = HttpUtility.HtmlDecode(node.SelectSingleNode(".//div[@class='video-caption']/h4").InnerText),
                ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("src", ""),
                PreviewUrl = null,
                Url = node.SelectSingleNode(".//a").GetAttributeValue("href", ""),
                Duration = "-",
                WatchCountText = Regex.Match(node.SelectSingleNode(".//div[@class='video-caption']/div").InnerText, @"[\d,]+(?= view)").Value,
                HeartCountText = "-",
                GetVideoUrl = GetVideoUrl
            }).ToList(), page, pageCount, (p) => SearchVideos(query, p));
        }

        public async Task<string> GetVideoUrl(string url)
        {
            throw new NotImplementedException("Can not get the video url because this website is a SPA");

            HttpClient client = new HttpClient();
            var result = await client.GetAsync(url);

            var htmlContent = await result.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            string iFrameSource = htmlDocument.DocumentNode
                .SelectSingleNode("//iframe")
                .GetAttributeValue("src", "");

            client = new HttpClient();
            result = await client.GetAsync(iFrameSource);
            htmlContent = await result.Content.ReadAsStringAsync();
            htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            string redirectUrl = htmlDocument.DocumentNode
                .SelectSingleNode("//video")
                .GetAttributeValue("src", "");

            result = await client.GetAsync(redirectUrl);
            htmlContent = await result.Content.ReadAsStringAsync();
            htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            string videoUrl = htmlDocument.DocumentNode
                .SelectSingleNode("//video")
                .GetAttributeValue("src", "");

            return Regex.Match(htmlContent, @"https?://([\w\-\.]+/)+[\w\-\.]+\.m3u8").Value;
        }
    }
}
