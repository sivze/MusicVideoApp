using HMV.Droid.Util;
using HMV.Shared.Data;
using HMV.Shared.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HMV.Shared.Service
{
    public class YoutubeService
    {
        public static string BASE_PLAYLIST_URL = "https://www.googleapis.com/youtube/v3/playlistItems?&part=snippet&maxResults=15";
        //public static string BASE_SEARCH_URL = "https://www.googleapis.com/youtube/v3/search?part=snippet&type=video&order=relevance&maxResults=50";
        //public static string BASE_CHART_URL = "https://www.googleapis.com/youtube/v3/videos?part=snippet&maxResults=50&chart=mostpopular&videoCategoryId=10";

        public static string PLAYLIST_ID = "PLDcnymzs18LVXfO_x0Ei0R24qDbVtyy66";

        static VideoDatabase videoDb = App.VideoDb;
        public static async Task<List<Video>> loadDataAsync()
        {
            try
            {
                string url = string.Format("{0}&playlistId={1}&key={2}",
                    BASE_PLAYLIST_URL, PLAYLIST_ID, KeyKeeper.YOUTUBE_API_KEY);

                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                    request.ContentType = "application/json";
                    request.Method = "GET";

                    string stringResponse;
                    // Send the request to the server and wait for the response:
                    using (WebResponse response = await request.GetResponseAsync())
                    {
                        // Get a stream representation of the HTTP web response:
                        using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                        {
                            stringResponse = await stream.ReadToEndAsync();

                        }
                    }

                    YoutubeResponse channel = JsonConvert.DeserializeObject<YoutubeResponse>(stringResponse);

                    if (channel != null && channel.items.Count > 0)
                    {
                        foreach (YoutubeItem item in channel.items)
                        {
                            if (item.snippet != null)
                            {
                                Video video = new Video().PrepareVideo(
                                    item.snippet.resourceId.videoId, item.snippet.title,
                                    item.snippet.publishedAt.Substring(0, 10), item.snippet.description,
                                    item.snippet.thumbnails.medium != null ? item.snippet.thumbnails.medium.url : "",
                                    item.snippet.thumbnails.maxres != null ? item.snippet.thumbnails.maxres.url : "",
                                    item.snippet.playlistId);

                                //adding to database
                                videoDb.SaveVideo(video);
                            }
                        }
                    }
                }

                
                if(App.VideosList!=null && App.VideosList.Count>0)
                    App.VideosList.Clear();

                //getting data from database
                App.VideosList = videoDb.GetVideos().ToList();

                if (App.VideosList.Count>0)
                    return App.VideosList;
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
