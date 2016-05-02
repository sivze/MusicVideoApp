using SQLite;

namespace HMV.Shared.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
