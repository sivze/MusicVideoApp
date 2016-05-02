using HMV.Shared.Data;
using HMV.Shared.Models;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HMV.Shared
{
    public class App : Application
    {
        private static VideoDatabase videoDb;
        public static List<Video> VideosList;
        public App()
        {
        }

        //return instance of VideoDatabase
        public static VideoDatabase VideoDb
        {
            get
            {
                if (videoDb == null)
                {
                    videoDb = new VideoDatabase();
                }
                return videoDb;
            }
        }
    }
}
