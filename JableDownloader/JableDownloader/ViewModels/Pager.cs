using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JableDownloader.ViewModels
{
    public class Pager<T> : ViewModelBase
    {
        private IEnumerable<T> _datas;

        public Pager(Func<int, Task<Pager<T>>> loadPage)
        {
            PageUnits = GetPageUnits(loadPage);

            PageUnits.First().Action.Execute(1);
        }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }

        public IEnumerable<PageUnit> PageUnits { get; set; }
        public IEnumerable<T> Datas 
        {
            get { return _datas; }
            set { SetProperty(ref _datas, value); }
        }

        public IEnumerable<PageUnit> GetPageUnits(Func<int, Task<Pager<T>>> loadPage)
        {
            ICommand command = new Command(async (parameter) =>
            {
                int page = Convert.ToInt32(parameter);
                CurrentPage = page;
                Datas = (await loadPage(page)).Datas;
                PageUnits = GetPageUnits(loadPage);
            });

            int start = CurrentPage - 2 > 0 ? CurrentPage - 2 : 1;
            int end = PageCount;

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
