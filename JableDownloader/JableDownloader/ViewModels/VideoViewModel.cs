using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using JableDownloader.Pages;
using JableDownloader.Services;
using JableDownloader.Services.Interfaces;
using Plugin.LocalNotification;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 影片
    /// </summary>
    public class VideoViewModel : ViewModelBase
    {
        private readonly IFileService _fileService = DependencyService.Get<IFileService>();

        public VideoViewModel()
        {
            Play = new Command(async () =>
            {
                await PopupNavigation.Instance.PopAsync();
                await Application.Current.MainPage.Navigation.PushModalAsync(new HlsPlayerPage(await GetVideoUrl(Url)));
            });

            Download = new Command(async () =>
            {
                try
                {
                    bool download = await Application.Current.MainPage.DisplayAlert("", "確定下載？", "OK", "Cancel");

                    if (!download)
                    {
                        return;
                    }

                    await new HlsDownloader(await GetVideoUrl(Url)).SequenceDownloadAsync(Path.Combine(_fileService.GetDownloadDirectory(), $"{Title}.mp4"), (index, total) =>
                    {
                        var notification = new NotificationRequest
                        {
                            NotificationId = Title.GetHashCode(), //推播 ID，一樣的 ID 在重複推播時會覆蓋前一個，
                            Title = Title,
                            Description = $"{index}/{total}",
                            CategoryType = NotificationCategoryType.Progress, //推播類別，給作業系統看的
                            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                            {
                                Ongoing = true, //推播位置在更上面且無法被取消
                                ProgressBarMax = total, //進度條最大值 (iOS 沒有進度條)，當進度值滿了不會自動取消，必須手動取消
                                ProgressBarProgress = index, //進度條當前進度，如果要讓進度條變動就必須再執行一次 Show()
                                IsProgressBarIndeterminate = false //是否為不確定進度的進度條，必須設 false 進度值才有效
                            }
                        };

                        //執行推播
                        NotificationCenter.Current.Show(notification);
                    });

                    NotificationCenter.Current.Clear(Title.GetHashCode());
                    await Application.Current.MainPage.DisplayAlert("", $"{Title} 下載完成！", "OK");
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    NotificationCenter.Current.Clear(Title.GetHashCode());
                    await Application.Current.MainPage.DisplayAlert("", ex.Message, "OK");
                }
            });
        }

        /// <summary>
        /// 影片播放事件
        /// </summary>
        public ICommand Play { get; set; }

        /// <summary>
        /// 影片下載事件
        /// </summary>
        public ICommand Download { get; set; }

        /// <summary>
        /// 爬出頁面內影片檔案所在的 URL (來自 IVideoCrawlerService)
        /// </summary>
        public Func<string, Task<string>> GetVideoUrl { get; set; }

        /// <summary>
        /// 影片標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 縮圖照網址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 預覽影片的檔案網址
        /// </summary>
        public string PreviewUrl { get; set; }

        /// <summary>
        /// 點擊影片後觸發的網址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 影片長度
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// 觀看數
        /// </summary>
        public string WatchCountText { get; set; }

        /// <summary>
        /// 愛心數
        /// </summary>
        public string HeartCountText { get; set; }
    }
}
