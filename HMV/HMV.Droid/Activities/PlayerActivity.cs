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
using Google.YouTube.Player;
using HMV.Droid.Util;
using HMV.Droid.Fragments;

namespace HMV.Droid.Activities
{
    [Activity(Label = "YoutubePlayerActivity")]
    public class PlayerActivity : YouTubeFailureRecoveryActivity
    {
        public static string PLAYER_ACTIVITY_EXTRA = "player_activity_extra";
        private static string videoId; 

        #region implemented abstract members of YouTubeFailureRecoveryActivity

        protected override IYouTubePlayerProvider GetYouTubePlayerProvider()
        {
            return FindViewById<YouTubePlayerView>(Resource.Id.activity_player_youtube);
        }

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_player);

            videoId = Intent.GetStringExtra(PLAYER_ACTIVITY_EXTRA);

            YouTubePlayerView youTubeView = FindViewById<YouTubePlayerView>(Resource.Id.activity_player_youtube);
            youTubeView.Initialize(KeyKeeper.YOUTUBE_API_KEY, this);


            Bundle arguments = new Bundle();
            arguments.PutString(VideoInfoFragment.PLAYER_FRAGMENT_EXTRA, videoId);

            VideoInfoFragment fragment = new VideoInfoFragment();
            fragment.Arguments = arguments;

            FragmentManager.BeginTransaction()
                .Add(Resource.Id.activity_player_video_info_container, fragment)
                .Commit();
        }

        public override void OnInitializationSuccess(IYouTubePlayerProvider provider, IYouTubePlayer player, bool wasRestored)
        {
            if (!wasRestored)
            {
                player.LoadVideo(videoId);
                player.SetShowFullscreenButton(false);
            }
        }
    }
}