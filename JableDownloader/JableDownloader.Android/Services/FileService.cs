﻿using JableDownloader.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(JableDownloader.Droid.Services.FileService))]
namespace JableDownloader.Droid.Services
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
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMovies).AbsolutePath;
        }
    }
}