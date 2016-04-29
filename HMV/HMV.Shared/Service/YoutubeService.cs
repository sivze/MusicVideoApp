using HMV.Droid.Util;
using HMV.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HMV.Shared.Service
{
    public class YoutubeService
    {
        public static string BASE_PLAYLIST_URL = "https://www.googleapis.com/youtube/v3/playlistItems?&part=snippet&maxResults=15";
        public static string BASE_SEARCH_URL = "https://www.googleapis.com/youtube/v3/search?part=snippet&type=video&order=relevance&maxResults=50";
        public static string BASE_CHART_URL = "https://www.googleapis.com/youtube/v3/videos?part=snippet&maxResults=50&chart=mostpopular&videoCategoryId=10";

        public static string PLAYLIST_ID = "PLOWGD2mptK9cRgOprwj35uTYxG70sCMpH";

        //public static List<YoutubeService> videosYTList = new List<YoutubeService>();
        //public string title { get; set; }
        //public string imageUrl { get; set; }
        //public string publishedOn { get; set; }
        //public YoutubeService(string title, string imageUrl, string publishedOn)
        //{
        //    this.title = title;
        //    this.imageUrl = imageUrl;
        //    this.publishedOn = publishedOn;

        //}

        public static async Task<List<YoutubeItem>> loadDataAsync()
        {
            try
            {
                string url = string.Format("{0}&playlistId={1}&key={2}",
                    BASE_PLAYLIST_URL, PLAYLIST_ID, KeyKeeper.YOUTUBE_API_KEY);

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

                //foreach (YoutubeItem item in channel.items)
                //{
                //    videosYTList.Add(new YoutubeService(item.snippet.title, item.snippet.thumbnails.medium.url
                //        , item.snippet.publishedAt));
                //}

                if (channel != null)
                    return channel.items;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
