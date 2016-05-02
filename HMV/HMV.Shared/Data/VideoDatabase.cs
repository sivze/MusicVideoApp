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

            CreateTable();
        }
       
        public void CreateTable()
        {
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
        public void DeleteAllVideos()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                lock (locker)
                {
                    database.Query<Video>("DELETE FROM [Video]");
                }
            }
        }

        public void DropTable()
        {
            database.DropTable<Video>();
        }
    }
}
