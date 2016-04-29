using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Views;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Runtime;

using Com.Lilarcor.Cheeseknife;

using Java.Lang;
using HMV.Droid.Adapters;
using HMV.Shared.Models;
using HMV.Shared.Service;

namespace HMV.Droid.Fragments
{
    public class VideosFragment : Android.Support.V4.App.Fragment
    {

        [InjectView(Resource.Id.fragment_videos_recyclerview)]
        RecyclerView recylerView;

        private List<YoutubeItem> videosList;
        private VideosAdapter videosAdapter;
        private Context context;
        private View view;
        private int item_position=RecyclerView.InvalidType;

        public VideosFragment()
        {
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            videosList = new List<YoutubeItem>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.fragment_videos, container, false);
            context = view.Context;
            Cheeseknife.Inject(this, view);
            
            LinearLayoutManager layoutManager = new LinearLayoutManager(context);

            recylerView.SetLayoutManager(layoutManager);

            videosAdapter = new VideosAdapter(Activity, videosList);
            videosAdapter.ItemClick += OnItemClick;

            recylerView.SetAdapter(videosAdapter);

            loadAndBindVideos();

            return view;
        }

        void OnItemClick(object sender, int position)
        {
            item_position = position;
        }

        private async void loadAndBindVideos()
        {
            List<YoutubeItem> videosList = await YoutubeService.loadDataAsync();
            videosAdapter.addVideos(videosList);

            if (item_position != RecyclerView.InvalidType)
                recylerView.SmoothScrollToPosition(item_position);
        }
       
        public override void OnDestroy()
        {
            base.OnDestroy();
            Cheeseknife.Reset(this);
        }

    }
}