using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JableDownloader.Pages
{
    /// <summary>
    /// 用來顯示影片清單的模板頁面
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoListPage : ContentPage
    {
        public VideoListPage()
        {
            InitializeComponent();
        }
    }
}