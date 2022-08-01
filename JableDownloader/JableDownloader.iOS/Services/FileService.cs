using System;
using JableDownloader.iOS.Services;
using JableDownloader.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace JableDownloader.iOS.Services
{
    /// <summary>
    /// 用來存取手機檔案系統的介面
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// 取得檔案下載路徑
        /// </summary>
        /// <returns></returns>
        public string GetDownloadDirectory()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //string libFolder = System.IO.Path.Combine(docFolder, "..", "Library");
            
            return docFolder;
        }
    }
}