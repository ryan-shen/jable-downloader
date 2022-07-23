using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using JableDownloader.Pages;
using JableDownloader.Services;
using Plugin.LocalNotification;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
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
                    bool download = await Application.Current.MainPage.DisplayAlert("", "Download this video?", "OK", "Cancel");

                    if (!download)
                    {
                        return;
                    }

                    await new HlsDownloader(await GetVideoUrl(Url)).SequenceDownloadAsync(Path.Combine(_fileService.GetDownloadDirectory(), $"{Name}.mp4"), (index, total) =>
                    {
                        var notification = new NotificationRequest
                        {
                            NotificationId = Name.GetHashCode(), //推播 ID，一樣的 ID 在重複推播時會覆蓋前一個，
                            Title = Name,
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

                    NotificationCenter.Current.Clear(Name.GetHashCode());
                    await Application.Current.MainPage.DisplayAlert("", $"{Name} has been successfully downloaded", "OK");
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    NotificationCenter.Current.Clear(Name.GetHashCode());
                    await Application.Current.MainPage.DisplayAlert("", ex.Message, "OK");
                }
            });
        }

        public ICommand Play { get; set; }
        public ICommand Download { get; set; }
        public Func<string, Task<string>> GetVideoUrl { get; set; }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string WatchCountText { get; set; }
        public string HeartCountText { get; set; }
    }
}
