using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    /// <summary>
    /// 分頁設定
    /// </summary>
    /// <typeparam name="T">資料內容的型別</typeparam>
    public class Pager<T> : ViewModelBase
    {
        private IEnumerable<T> _datas;

        public Pager(IEnumerable<T> datas, int currentPage, int pageCount, Func<int, Task<Pager<T>>> loadPage)
        {
            Datas = datas;
            CurrentPage = currentPage;
            PageCount = pageCount;
            PageUnits = GetPageUnits(loadPage);
        }

        /// <summary>
        /// 當前頁數
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 分頁物件上的每顆按鈕
        /// </summary>
        public IEnumerable<PageUnit> PageUnits { get; set; }

        /// <summary>
        /// 資料來源
        /// </summary>
        public IEnumerable<T> Datas 
        {
            get { return _datas; }
            set { SetProperty(ref _datas, value); }
        }

        /// <summary>
        /// 取得要顯示的頁數按鈕們
        /// </summary>
        /// <param name="loadPage">可以根據不同頁數載入資料來源的方法</param>
        /// <returns></returns>
        public IEnumerable<PageUnit> GetPageUnits(Func<int, Task<Pager<T>>> loadPage)
        {
            //按下頁數按鈕時要執行的動作
            ICommand command = new Command(async (parameter) =>
            {
                int page = Convert.ToInt32(parameter);
                CurrentPage = page;
                Datas = (await loadPage(page)).Datas;
                PageUnits = GetPageUnits(loadPage);
            });

            //始頁與末頁
            int start = CurrentPage - 2 > 0 ? CurrentPage - 2 : 1;
            int end = PageCount;

            //組出要顯示的頁數按鈕們
            var list = new List<PageUnit>();
            list.Add(new PageUnit { Text = "<<", Page = 1, Action = command });
            list.AddRange(Enumerable
                .Range(start, Math.Min(5, end - start + 1))
                .Select(x => new PageUnit
            {
                Text = x.ToString().PadLeft(2, '0'),
                Page = x,
                Action = command
            }));
            list.Add(new PageUnit { Text = ">>", Page = PageCount, Action = command });

            return list;
        }
    }
}
