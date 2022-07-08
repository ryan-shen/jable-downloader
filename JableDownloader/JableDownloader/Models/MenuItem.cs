using System;
using JableDownloader.Services;

namespace JableDownloader.Pages
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Type TargetType { get; set; }

        public Type ServiceType { get; set; }
    }
}