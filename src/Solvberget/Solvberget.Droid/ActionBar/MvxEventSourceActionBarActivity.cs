using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Droid.Views;
using System;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Solvberget.Droid.ActionBar
{
    public class MvxEventSourceActionBarActivity : MvxAppCompatActivity, IMvxEventSourceActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            CreateWillBeCalled.Raise(this, bundle);
            base.OnCreate(bundle);
            CreateCalled.Raise(this, bundle);
        }

        protected override void OnDestroy()
        {
            DestroyCalled.Raise(this);
            base.OnDestroy();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            NewIntentCalled.Raise(this, intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ResumeCalled.Raise(this);
        }

        protected override void OnPause()
        {
            PauseCalled.Raise(this);
            base.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            StartCalled.Raise(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            RestartCalled.Raise(this);
        }

        protected override void OnStop()
        {
            StopCalled.Raise(this);
            base.OnStop();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            SaveInstanceStateCalled.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResultCalled.Raise(this, new MvxStartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResultCalled.Raise(this, new MvxActivityResultParameters(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public new event EventHandler DisposeCalled;
        public new event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;
        public new event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;
        public new event EventHandler DestroyCalled;
        public new event EventHandler<MvxValueEventArgs<Intent>> NewIntentCalled;
        public new event EventHandler ResumeCalled;
        public new event EventHandler PauseCalled;
        public new event EventHandler StartCalled;
        public new event EventHandler RestartCalled;
        public new event EventHandler StopCalled;
        public new event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;
        public new event EventHandler<MvxValueEventArgs<MvxStartActivityForResultParameters>> StartActivityForResultCalled;
        public new event EventHandler<MvxValueEventArgs<MvxActivityResultParameters>> ActivityResultCalled;
    }
}