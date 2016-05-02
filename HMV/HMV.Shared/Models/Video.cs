using SQLite;

namespace HMV.Shared.Models
{
    public class Video
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string PublishedAt { get; set; }
        public string Description { get; set; }
        public string ThumbnailMedium { get; set; }
        public string ThumbnailMaxres { get; set; }
        public string PlaylistId { get; set; }

        public Video PrepareVideo( string videoId, string title, string publishedAt, string description,
                     string thumbnailMedium, string thumbnailMaxres, string playlistId)
        {
            this.VideoId = videoId;
            this.Title = title;
            this.PublishedAt = publishedAt;
            this.Description = description;
            this.ThumbnailMedium = thumbnailMedium;
            this.ThumbnailMaxres = thumbnailMaxres;
            this.PlaylistId = playlistId;

            return this;
        }
    }
}
