using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.OS;
using Android.Support.V7.App;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;

using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using HMV.Droid.Fragments;
using HMV.Droid.Activities;
using HMV.Droid.Adapters;

namespace HMV.Droid
{
    [Activity(Label = "Music Videos", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity, VideosFragment.ICallback
    {
        DrawerLayout drawerLayout;
        public static Boolean isTwoPane = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            if (FindViewById(Resource.Id.activity_player_fragment_container) != null)
            {
                isTwoPane = true;

                FragmentManager.BeginTransaction()
                    .Replace(Resource.Id.activity_player_fragment_container, new PlayerFragment())
                    .Commit();
            }
            else
            {
                isTwoPane = false;
            }

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.activity_main_drawer_layout);

            var navigationView = FindViewById<NavigationView>(Resource.Id.activity_main_nav_view);
            if (navigationView != null)
                setupDrawerContent(navigationView);

            var viewPager = FindViewById<Android.Support.V4.View.ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
                setupViewPager(viewPager);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_actions, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        void setupViewPager(Android.Support.V4.View.ViewPager viewPager)
        {
            var adapter = new MainActivityPagerAdapter(SupportFragmentManager);
            adapter.AddFragment(new VideosFragment(), "Trending");
            //adapter.AddFragment(new VideosFragment(), "Hot 50");
            //adapter.AddFragment(new VideosFragment(), "Most Famous");
            viewPager.Adapter = adapter;
        }

        void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }

        public void OnItemSelected(int position)
        {
            if (isTwoPane)
            {
                Bundle arguments = new Bundle();
                arguments.PutInt(PlayerFragment.FRAGMENT_PLAYER_EXTRA, position);

                PlayerFragment fragment = new PlayerFragment();
                fragment.Arguments = arguments;

                FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.activity_player_fragment_container, fragment, "")
                        .Commit();
            }
            else
            {
                var intent = new Intent(this, typeof(PlayerActivity));
                //intent.PutExtra(PlayerActivity.PLAYER_ACTIVITY_EXTRA, position);
                StartActivity(intent);
            }
        }
    }
}


