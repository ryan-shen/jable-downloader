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
    public class JableService
    {
        public async Task<Pager<ActressViewModel>> GetActresses(int page = 1)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36");
            //不知為啥要設定第二次才有用
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/84.0.4147.135");
            //第幾頁
            int from = page;
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

            
            HtmlNode pageNode = htmlDocument.DocumentNode.SelectSingleNode("(//ul[@class='pagination']/li[@class='page-item']/a[@data-parameters])[last()]");
            string dataParameters = pageNode.Attributes["data-parameters"].Value;
            int pageCount = Convert.ToInt32(Regex.Match(dataParameters, @"(?<=from:)\d+").Value);

            return new Pager<ActressViewModel>(GetActresses)
            {
                CurrentPage = page,
                PageCount = pageCount,
                Datas = actressNodes.Select(node => new ActressViewModel
                {
                    Name = node.SelectSingleNode(".//h6[@class='title']").InnerText,
                    MovieCountText = node.SelectSingleNode(".//span").InnerText,
                    ImageUrl = node.SelectSingleNode(".//img").GetAttributeValue("src", ""),
                    Url = node.SelectSingleNode(".//a").GetAttributeValue("href", "")
                }).ToList()
            };
        }
    }
}
