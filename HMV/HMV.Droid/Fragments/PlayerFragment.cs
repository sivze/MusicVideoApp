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
using Com.Lilarcor.Cheeseknife;
using Google.YouTube.Player;
using HMV.Droid.Util;
using HMV.Droid.Activities;
using Square.Picasso;
using HMV.Shared.Models;
using Android.Support.V7.Widget;
using Android.Content.Res;

namespace HMV.Droid.Fragments
{
    public class PlayerFragment : Fragment, IYouTubePlayerOnInitializedListener
    {
        //[InjectView(Resource.Id.fragment_video_info_thumbnail_image_view)]
        //public ImageView videoThumbnail;

        [InjectView(Resource.Id.fragment_player_youtube_container)]
        public FrameLayout playerContainer;

        [InjectView(Resource.Id.fragment_player_title_text_view)]
        public TextView videoTitle;

        [InjectView(Resource.Id.fragment_player_desc_text_view)]
        public TextView videoDesc;

        [InjectView(Resource.Id.fragment_player_divider)]
        public View divider;

        public static string FRAGMENT_PLAYER_EXTRA = "fragment_player_extra";

        private Context context;
        private View view;
        private static int RECOVERY_DIALOG_REQUEST = 1;
        private YoutubeItem youtubeItem;
        YouTubePlayerFragment youTubePlayerFragment;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.fragment_player, container, false);
            context = view.Context;

            Cheeseknife.Inject(this, view);

            int item_position = VideosFragment.selectedItemPosition;
            //if (Arguments != null)
            //{
            //    item_position = Arguments.GetInt(FRAGMENT_PLAYER_EXTRA);
            //}

            if (item_position != RecyclerView.InvalidType)
            {
                youtubeItem = VideosFragment.videosList.ElementAt(item_position);
                youTubePlayerFragment = new YouTubePlayerFragment();
                youTubePlayerFragment.Initialize(KeyKeeper.YOUTUBE_API_KEY, this);

                if (savedInstanceState == null || MainActivity.isTwoPane)
                {
                    FragmentManager.BeginTransaction()
                                   .Replace(Resource.Id.fragment_player_youtube_container, youTubePlayerFragment)
                                   .Commit();
                }
                //try
                //{
                //    Picasso.With(context)
                //        .Load(youtubeItem.snippet.thumbnails.standard.url)
                //        .Into(videoThumbnail);
                //}
                //catch(Exception e)
                //{

                //}

                videoTitle.Text = youtubeItem.snippet.title;

                playerContainer.Visibility = ViewStates.Visible;
                divider.Visibility = ViewStates.Visible;

                videoDesc.Text = youtubeItem.snippet.description;
            }
            return view;
        }

        public void OnInitializationFailure(IYouTubePlayerProvider provider, YouTubeInitializationResult errorReason)
        {
            if (errorReason.IsUserRecoverableError)
            {
                errorReason.GetErrorDialog(this.Activity, RECOVERY_DIALOG_REQUEST).Show();
            }
            else
            {
                String errorMessage = String.Format(GetString(Resource.String.error_player), errorReason.ToString());
                Toast.MakeText(context, errorMessage, ToastLength.Long).Show();
            }
        }

        public void OnInitializationSuccess(IYouTubePlayerProvider provider, IYouTubePlayer player, bool wasRestored)
        {
            if (!wasRestored)
            {
                player.LoadVideo(youtubeItem.snippet.resourceId.videoId);
                player.SetShowFullscreenButton(false);
            }
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }

        public override void OnDestroyView()
        {
            if (youTubePlayerFragment != null)
                youTubePlayerFragment.Dispose();
            Cheeseknife.Reset(this);
            base.OnDestroyView();
        }
    }
}