using System;
using System.Collections.Generic;
using System.Text;

namespace HMV.Shared.Models
{

    public class YoutubeResponse
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<YoutubeItem> items { get; set; }
    }

}
