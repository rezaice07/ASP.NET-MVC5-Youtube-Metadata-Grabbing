using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Extensions.AppControl;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using YoutubeApi.Web.ViewModels;

//reference: 
//https://forums.asp.net/t/2114826.aspx?Youtube+Video+Data+API+and+MVC+Razor+Coding+checkpoint+
//https://www.youtube.com/watch?v=AcUauzCn7RE

namespace YoutubeApi.Web.Controllers
{
    public class YoutubeController : Controller
    {
        [HttpGet]
        public ActionResult GetChannelInfo()
        {
            var model = new YouTubeViewModel {
                YoutubeApiKey= "YOUR_YOUTUBE_APIKEY"
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult GetChannelInfo(YouTubeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Message = "You must fill all the required fields.";
                return View(model);
            }


            List<YouTubeData> youTubeDataList = new List<YouTubeData>();
            
            try
            {
                var yt = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = model.YoutubeApiKey

                });

                var channelsListRequest = yt.Channels.List("contentDetails");
                channelsListRequest.Id = model.ChannelId;
                var channelsListResponse = channelsListRequest.Execute();   //< -----this is where trouble starts
                foreach (var channel in channelsListResponse.Items)
                {
                    // of videos uploaded to the authenticated user's channel.
                    var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                    var nextPageToken = "";
                    while (nextPageToken != null)
                    {
                        var playlistItemsListRequest = yt.PlaylistItems.List("snippet");
                        playlistItemsListRequest.PlaylistId = uploadsListId;
                        playlistItemsListRequest.MaxResults = 8;
                        playlistItemsListRequest.PageToken = nextPageToken;
                        // Retrieve the list of videos uploaded to the authenticated user's channel.
                        var playlistItemsListResponse = playlistItemsListRequest.Execute();
                        foreach (var playlistItem in playlistItemsListResponse.Items)
                        {
                            YouTubeData objYouTubeData = new YouTubeData();
                            objYouTubeData.VideoId = "https://www.youtube.com/embed/" + playlistItem.Snippet.ResourceId.VideoId;
                            objYouTubeData.Title = playlistItem.Snippet.Title;
                            objYouTubeData.Descriptions = playlistItem.Snippet.Description;
                            objYouTubeData.ImageUrl = playlistItem.Snippet.Thumbnails.High.Url;
                            objYouTubeData.IsValid = true;
                            youTubeDataList.Add(objYouTubeData);

                        }
                        nextPageToken = playlistItemsListResponse.NextPageToken;
                    }
                }               
            }
            catch (Exception e)
            {
                string errorMessage = "Some exception occured" + e.Data.ToString() + e.Message.ToString() + e.GetBaseException().ToString();
                model.Message = errorMessage;
                return View(model);
            }

            model.YouTubeDataList = youTubeDataList;

            return View(model);
        }        
    }

}