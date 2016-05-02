using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Content;

using Com.Lilarcor.Cheeseknife;

using HMV.Droid.Adapters;
using HMV.Shared.Models;
using HMV.Shared.Service;
using HMV.Shared;

namespace HMV.Droid.Fragments
{
    public class VideosFragment : Android.Support.V4.App.Fragment
    {
        [InjectView(Resource.Id.fragment_videos_recyclerview)]
        RecyclerView recylerView;

        [InjectView(Resource.Id.fragment_videos_progress_bar)]
        ProgressBar progressBar;

        public static List<Video> videosList;

        private VideosAdapter videosAdapter;
        private Context context;
        private View view;
        public static int selectedItemPosition = RecyclerView.InvalidType;
        private static string SELECTED_ITEM_KEY = "selected_item_position";

        public interface ICallback
        {
            /**
             * FragmentCallback for when an item has been selected.
             */
            void OnItemSelected(int position);
        }
        public VideosFragment()
        {
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            videosList = new List<Video>();
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

            if (savedInstanceState != null && savedInstanceState.ContainsKey(SELECTED_ITEM_KEY))
            {
                selectedItemPosition = savedInstanceState.GetInt(SELECTED_ITEM_KEY);
            }
            progressBar.Visibility = ViewStates.Visible;
            loadAndBindVideos();
            return view;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            if (selectedItemPosition != RecyclerView.InvalidType)
            {
                outState.PutInt(SELECTED_ITEM_KEY, selectedItemPosition);
            }
        }

        void OnItemClick(object sender, int position)
        {
            ((ICallback)this.Activity).OnItemSelected(position);
        }

        private async void loadAndBindVideos()
        {
            if(App.VideosList==null)
                videosList = await YoutubeService.loadDataAsync();

            videosList = App.VideosList;
            videosAdapter.addVideos(videosList);

            progressBar.Visibility = ViewStates.Invisible;

            if (selectedItemPosition != RecyclerView.InvalidType)
                recylerView.SmoothScrollToPosition(selectedItemPosition);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Cheeseknife.Reset(this);
        }

    }
}