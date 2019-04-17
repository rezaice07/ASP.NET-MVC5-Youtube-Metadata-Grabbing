using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YoutubeApi.Web.ViewModels
{
    public class YouTubeViewModel
    {
        public string Message { get; set; }

        [Required]
        public string YoutubeApiKey { get; set; }

        [Required]
        public string ChannelId { get; set; }

        public List<YouTubeData> YouTubeDataList { get; set; }
    }
}