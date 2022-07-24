using System;

namespace JableDownloader.Pages
{
    /// <summary>
    /// 頁面左側的 Menu 選項
    /// </summary>
    internal class MenuItem
    {
        /// <summary>
        /// 頁面代碼
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 頁面名稱
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 要開啟的頁面的類別
        /// </summary>
        public Type TargetType { get; set; }

        /// <summary>
        /// 服務提供者
        /// </summary>
        public Type ServiceType { get; set; }
    }
}