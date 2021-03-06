using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Com.Lilarcor.Cheeseknife;
using Square.Picasso;
using HMV.Shared.Models;
using Android.Graphics;
using HMV.Droid.Fragments;

namespace HMV.Droid.Adapters
{
    public class VideosAdapter : RecyclerView.Adapter
    {
        private List<Video> videosList;
        private Activity context;
        private VideoHolder videoHolder;

        public event EventHandler<int> ItemClick;

        public VideosAdapter(Activity context, List<Video> videosList)
        {
            this.videosList = videosList;
            this.context = context;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.grid_item_video_template, parent, false);

            return new VideoHolder(view, OnClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            videoHolder = holder as VideoHolder;

            Video video = videosList.ElementAt(position);

            if (video != null)
            {
                string videoID = video.VideoId;
                string videoTitle = video.Title;
                string videoThumbnailUrl = video.ThumbnailMedium;
                string videoPublishedDate = video.PublishedAt;

                if (VideosFragment.selectedItemPosition == position)
                    videoHolder.titleLayout.SetBackgroundColor(context.Resources.GetColor(Resource.Color.colorPrimaryTransparent));
                else
                    videoHolder.titleLayout.SetBackgroundColor(Color.White);

                videoHolder.videoTitle.Text = videoTitle;
                videoHolder.videoPublishedDate.Text = videoPublishedDate;

                Picasso.With(context)
                    .Load(videoThumbnailUrl)
                    .Into(videoHolder.videoThumbnail);
            }
        }

        public override int ItemCount
        {
            get
            {
                if (videosList != null)
                    return videosList.Count;
                else
                    return 0;
            }
        }

        void OnClick(int position)
        {
            // Updating old as well as new positions

            if (VideosFragment.selectedItemPosition != RecyclerView.InvalidType)
                NotifyItemChanged(VideosFragment.selectedItemPosition);

            VideosFragment.selectedItemPosition = position;

            NotifyItemChanged(VideosFragment.selectedItemPosition);

            if (ItemClick != null)
                ItemClick(this, position);
        }

        public void addVideos(List<Video> data)
        {
            if (data == null)
            {
                data = new List<Video>();
            }
            videosList = data;
            NotifyDataSetChanged();
        }

        public class VideoHolder : RecyclerView.ViewHolder
        {
            [InjectView(Resource.Id.grid_item_video_thumbnail_image_view)]
            public ImageView videoThumbnail;

            [InjectView(Resource.Id.grid_item_video_title_text_view)]
            public TextView videoTitle;

            [InjectView(Resource.Id.grid_item_video_published_at_text_view)]
            public TextView videoPublishedDate;

            [InjectView(Resource.Id.grid_item_video_title_layout)]
            public LinearLayout titleLayout;

            public View View { get; set; }

            public VideoHolder(View view, Action<int> listener) : base(view)
            {
                View = view;
                Cheeseknife.Inject(this, view);

                view.Click += (sender, e) => listener(base.Position);
            }

        }
    }
}