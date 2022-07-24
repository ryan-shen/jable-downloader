namespace JableDownloader.Services.Interfaces
{
    /// <summary>
    /// 用來存取手機檔案系統的介面
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// 取得檔案下載路徑
        /// </summary>
        /// <returns></returns>
        string GetDownloadDirectory();
    }
}
