using System;
using System.Globalization;
using System.Text.RegularExpressions;
using JableDownloader.ViewModels;
using Xamarin.Forms;

namespace JableDownloader.Converters
{
    /// <summary>
    /// 用來強調當前所在的頁數顏色
    /// </summary>
    internal class PageUnitToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var pageUnit = values[0] as PageUnit;
            var currentPage = (int)values[1];

            return pageUnit != null 
                && Regex.IsMatch(pageUnit.Text, @"^\d+$") 
                && pageUnit.Page == currentPage 
                ? Color.Pink : Color.Gray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
