using HMV.Shared.Models;
using Plugin.Connectivity;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace HMV.Shared.Data
{
    public class VideoDatabase
    {
        static object locker = new object();

        SQLiteConnection database;
       
        public VideoDatabase()
        {
            //Xamarin forms needs to be initiated before this
            database = DependencyService.Get<ISQLite>().GetConnection();

            if(CrossConnectivity.Current.IsConnected)
                database.DropTable<Video>();

            // create the table
            database.CreateTable<Video>();
        }
       
        public IEnumerable<Video> GetVideos()
        {
            lock (locker)
            {
                return (from i in database.Table<Video>() select i).ToList();
            }
        }

        public Video GetVideo(int id)
        {
            lock (locker)
            {
                return database.Table<Video>().FirstOrDefault(x => x.Id == id);
            }
        }

        public int SaveVideo(Video item)
        {
            lock (locker)
            {
                if (item.Id!=0)
                {
                    database.Update(item);
                    return item.Id;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        public int DeleteVideo(int id)
        {
            lock (locker)
            {
                return database.Delete<Video>(id);
            }
        }
    }
}
