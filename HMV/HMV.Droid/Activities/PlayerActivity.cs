using Android.App;
using Android.Content;
using Android.OS;
using HMV.Droid.Fragments;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Support.V7.App;

namespace HMV.Droid.Activities
{
    [Activity(Label = "YoutubePlayerActivity",
        ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class PlayerActivity : AppCompatActivity
    {
        public static string PLAYER_ACTIVITY_EXTRA = "player_activity_extra";
        private static string videoId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_player);

            //int position = Intent.GetIntExtra(PLAYER_ACTIVITY_EXTRA, RecyclerView.InvalidType);

            //Bundle arguments = new Bundle();
            //arguments.PutInt(PlayerFragment.FRAGMENT_PLAYER_EXTRA, position);

            //PlayerFragment fragment = new PlayerFragment();
            //fragment.Arguments = arguments;

            FragmentManager.BeginTransaction()
                .Add(Resource.Id.activity_player_fragment_container, new PlayerFragment(), "")
                .Commit();
        }
    }
}