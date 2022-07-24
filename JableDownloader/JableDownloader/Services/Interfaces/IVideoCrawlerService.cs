using System.Threading.Tasks;
using JableDownloader.ViewModels;

namespace JableDownloader.Services.Interfaces
{
    /// <summary>
    /// 爬蟲介面
    /// </summary>
    public interface IVideoCrawlerService
    {
        /// <summary>
        /// 取得網站名稱
        /// </summary>
        /// <returns></returns>
        string GetSiteName();

        /// <summary>
        /// 取得最新影片清單
        /// </summary>
        /// <param name="page">第幾頁</param>
        /// <returns></returns>
        Task<Pager<VideoViewModel>> GetRecentVideos(int page = 1);

        /// <summary>
        /// 取得熱門影片清單
        /// </summary>
        /// <param name="page">第幾頁</param>
        /// <returns></returns>
        Task<Pager<VideoViewModel>> GetPopularVideos(int page = 1);

        /// <summary>
        /// 搜尋影片
        /// </summary>
        /// <param name="query">搜尋字串</param>
        /// <param name="page">第幾頁</param>
        /// <returns></returns>
        Task<Pager<VideoViewModel>> SearchVideos(string query, int page = 1);

        /// <summary>
        /// 爬出頁面內影片檔案所在的 URL
        /// </summary>
        /// <param name="url">影片播放頁面網址</param>
        /// <returns></returns>
        Task<string> GetVideoUrl(string url);
    }
}
