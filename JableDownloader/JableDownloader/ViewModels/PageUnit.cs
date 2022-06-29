using System.Windows.Input;

namespace JableDownloader.ViewModels
{
    public class PageUnit
    {
        public string Text { get; set; }
        public int Page { get; set; }
        public ICommand Action { get; set; }
    }
}
