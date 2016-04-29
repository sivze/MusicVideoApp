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

namespace HMV.Droid.Fragments
{
    public class VideoInfoFragment : Fragment
    {
        [InjectView(Resource.Id.fragment_player_video_title_text_view)]
        public TextView videoTitle;

        public static string PLAYER_FRAGMENT_EXTRA = "player_fragment_extra";
        private Context context;
        private View view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.fragment_video_info, container, false);
            context = view.Context;

            Cheeseknife.Inject(this, view);
            
            if (Arguments != null)
            {
                //videoTitle.Text = Arguments.GetBundle(PLAYER_FRAGMENT_EXTRA).ToString();
            }

            return view;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Cheeseknife.Reset(this);
        }
    }
}