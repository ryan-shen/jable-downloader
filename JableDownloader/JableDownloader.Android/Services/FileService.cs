using JableDownloader.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(JableDownloader.Droid.Services.FileService))]
namespace JableDownloader.Droid.Services
{
    public class FileService : IFileService
    {
        public string GetDownloadDirectory()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMovies).AbsolutePath;
        }
    }
}