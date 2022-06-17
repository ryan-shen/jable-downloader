using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 用來方便在設定屬性後主動通知前端的基底類別
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 實作 INotifyPropertyChanged 所需的事件，用來通知 XAML 更新資料
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 設定屬性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">舊值</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">ViewModel 屬性名稱</param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// 通知 XAML 更新資料 (就算 Binding Mode 是 TwoWay 也要透過此方式通知，但如果是 OneWayToSource 就會無效)
        /// </summary>
        /// <param name="propertyName">ViewModel 屬性名稱</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
