using Xamarin.Forms;
using System.IO;
using HMV.Shared.Data;
using HMV.Droid.Data;
using SQLite;

[assembly: Dependency(typeof(DroidVideosDbHelper))]

namespace HMV.Droid.Data
{
    public class DroidVideosDbHelper : ISQLite
    {
        public DroidVideosDbHelper()
        {
        }

        #region ISQLite implementation
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "HMV.db3";
            string documentsPath = System.Environment.GetFolderPath(
                                   System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            var conn = new SQLite.SQLiteConnection(path);

            return conn;
        }
        #endregion

    }
}