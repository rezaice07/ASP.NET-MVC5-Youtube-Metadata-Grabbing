using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeApi.Web.ViewModels
{
    public class YouTubeData
    {
        public string Descriptions { get; set; }
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public bool IsValid { get; set; }

        public string VideoId { get; set; }
    }
}