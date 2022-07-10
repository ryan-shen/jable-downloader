using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace JableDownloader.Models
{
    internal static class Global
    {
        #region Common getter & setter
        private static T Get<T>([CallerMemberName] string name = null)
        {
            if (Application.Current.Properties.ContainsKey(name))
            {
                return (T)Application.Current.Properties[name];
            }

            return default;
        }

        private static void Set<T>(T value, [CallerMemberName] string name = null)
        {
            Application.Current.Properties[name] = value;
        }
        #endregion

        public static bool ShowOtherSites
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }
}
