using System.Windows.Input;

namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 分頁物件上的一顆按鈕
    /// </summary>
    public class PageUnit
    {
        /// <summary>
        /// 顯示文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 所代表的頁數
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 點擊後的觸發動作
        /// </summary>
        public ICommand Action { get; set; }
    }
}
