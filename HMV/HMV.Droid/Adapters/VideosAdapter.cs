using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Com.Lilarcor.Cheeseknife;
using Square.Picasso;
using HMV.Shared.Models;
using Android.Util;
using HMV.Droid.Activities;
using Android.Graphics;
using Android.Content.Res;

namespace HMV.Droid.Adapters
{
    public class VideosAdapter : RecyclerView.Adapter
    {
        private List<YoutubeItem> videosList;
        private Activity context;
        private int selected_position;

        public event EventHandler<int> ItemClick;

        public VideosAdapter(Activity context, List<YoutubeItem> videosList)
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
            var videoHolder = holder as VideoHolder;

            YoutubeItem youTubeItem = videosList.ElementAt(position);

            if (youTubeItem != null)
            {
                string videoID = youTubeItem.snippet.resourceId.videoId;
                string videoTitle = youTubeItem.snippet.title;
                string videoThumbnailUrl = youTubeItem.snippet.thumbnails.medium.url;
                string videoPublishedDate = youTubeItem.snippet.publishedAt.Substring(0, 10);

                if (selected_position == position)
                    videoHolder.titleLayout.SetBackgroundColor(context.Resources.GetColor(Resource.Color.colorPrimaryTransparent));
                else
                    videoHolder.titleLayout.SetBackgroundColor(Color.White);

                if (videoHolder.ClickHandler != null)
                    videoHolder.View.Click -= videoHolder.ClickHandler;

                videoHolder.ClickHandler = new EventHandler((sender, e) =>
                {
                    // Updating old as well as new positions
                    NotifyItemChanged(selected_position);
                    selected_position = position;
                    NotifyItemChanged(selected_position);

                    var context = videoHolder.View.Context;
                    var intent = new Intent(context, typeof(PlayerActivity));
                    intent.PutExtra(PlayerActivity.PLAYER_ACTIVITY_EXTRA, videoID);

                    context.StartActivity(intent);
                });
                videoHolder.View.Click += videoHolder.ClickHandler;

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
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public void addVideos(List<YoutubeItem> data)
        {
            if (data == null)
            {
                data = new List<YoutubeItem>();
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
            public EventHandler ClickHandler { get; set; }


            public VideoHolder(View view, Action<int> listener) : base(view)
            {
                View = view;
                Cheeseknife.Inject(this, view);

                view.Click += (sender, e) => listener(base.Position);
            }

        }
    }
}