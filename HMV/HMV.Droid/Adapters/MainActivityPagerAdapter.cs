using System;
using System.Collections.Generic;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;

namespace HMV.Droid.Adapters
{
    public class MainActivityPagerAdapter : Android.Support.V4.App.FragmentPagerAdapter
    {
        List<V4Fragment> fragments = new List<V4Fragment>();
        List<string> fragmentTitles = new List<string>();

        public MainActivityPagerAdapter(V4FragmentManager fm) : base(fm)
        {
        }

        public void AddFragment(V4Fragment fragment, String title)
        {
            fragments.Add(fragment);
            fragmentTitles.Add(title);
        }

        public override V4Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override int Count
        {
            get { return fragments.Count; }
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(fragmentTitles[position]);
        }

    }
}