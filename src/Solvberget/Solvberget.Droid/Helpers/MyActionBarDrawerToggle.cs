using Android.App;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using System;

namespace Solvberget.Droid.Helpers
{
    public class DrawerEventArgs : EventArgs
    {
        public View DrawerView { get; set; }
        public float SlideOffset { get; set; }
        public int NewState { get; set; }
    }

    public delegate void ActionBarDrawerChangedEventHandler(object s, DrawerEventArgs e);

    public class MyActionBarDrawerToggle : ActionBarDrawerToggle
    {
        public MyActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, int drawerImageRes, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, drawerImageRes, openDrawerContentDescRes, closeDrawerContentDescRes)
        { }

        public event ActionBarDrawerChangedEventHandler DrawerClosed;
        public event ActionBarDrawerChangedEventHandler DrawerOpened;
        public event ActionBarDrawerChangedEventHandler DrawerSlide;
        public event ActionBarDrawerChangedEventHandler DrawerStateChanged;

        public override void OnDrawerClosed(View drawerView)
        {
            if (null != DrawerClosed)
                DrawerClosed(this, new DrawerEventArgs { DrawerView = drawerView });
            base.OnDrawerClosed(drawerView);
        }

        public override void OnDrawerOpened(View drawerView)
        {
            if (null != DrawerOpened)
                DrawerOpened(this, new DrawerEventArgs { DrawerView = drawerView });
            base.OnDrawerOpened(drawerView);
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            if (null != DrawerSlide)
                DrawerSlide(this, new DrawerEventArgs
                {
                    DrawerView = drawerView,
                    SlideOffset = slideOffset
                });
            base.OnDrawerSlide(drawerView, slideOffset);
        }

        public override void OnDrawerStateChanged(int newState)
        {
            if (null != DrawerStateChanged)
                DrawerStateChanged(this, new DrawerEventArgs
                {
                    NewState = newState
                });
            base.OnDrawerStateChanged(newState);
        }
    }
}