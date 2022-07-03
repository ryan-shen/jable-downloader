using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JableDownloader.ViewModels;

namespace JableDownloader.Services
{
    public interface IVideoCrawlerService
    {
        Task<Pager<VideoViewModel>> GetRecentVideos(int page = 1);

        Task<Pager<VideoViewModel>> GetPopularVideos(int page = 1);

        Task<string> GetVideoUrl(string url);
    }
}
