using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using System.Threading;
using System;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using System.Threading.Tasks;

namespace Solvberget.Core.ViewModels.Base
{
    public class BaseViewModel : MvxViewModel
    {
		public static bool AddEmptyItemForEmptyLists = true;

        private IAnalyticsService _analytics;
        public IAnalyticsService Analytics
        {
            get { return _analytics ?? (_analytics = Mvx.Resolve<IAnalyticsService>() ?? new VoidAnalyticsService()); }
            set { _analytics = value; }
        }

        readonly ManualResetEvent _viewModelReady = new ManualResetEvent(false);

		public void WaitForReady(Action onReady)
		{
			Task stuff = Task.Run(() => {
				_viewModelReady.WaitOne ();
				onReady ();
			});
		}

        public override void Start()
        {
            base.Start();
            Analytics.LogEvent("VM_" + GetType().Name);
        }

        protected void NotifyViewModelReady()
		{
			_viewModelReady.Set();
		}

		public virtual void OnViewReady()
		{
		}

        private long _id;
        /// <summary>
        /// Gets or sets the unique ID for the menu
        /// </summary>
        public long Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(() => Id); }
        }

        private string _title = string.Empty;
        /// <summary>
        /// Gets or sets the name of the menu
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        private bool _isLoading;
        public bool IsLoading 
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged(() => IsLoading);}
        }
    }
}
