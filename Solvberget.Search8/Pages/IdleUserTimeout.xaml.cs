using System;
using System.Diagnostics;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;

namespace Solvberget.Search8.Pages
{
    public sealed partial class IdleUserTimeout : UserControl
    {
        private Timer _warnTimer;

        public IdleUserTimeout()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _warnTimer = new Timer(OnTimeout, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            Window.Current.CoreWindow.PointerPressed += ResetIdle;
            Window.Current.CoreWindow.PointerMoved += ResetIdle;
            Window.Current.CoreWindow.KeyDown += ResetIdle;
            Window.Current.CoreWindow.PointerWheelChanged += ResetIdle;
        }

        private void ResetIdle(CoreWindow sender, object args)
        {
            _seconds = 0;
            HideWarning.Begin();
        }
        
        private int _seconds = 0;
        private const int WarnAfter = 50;
        private const int NavigateAfter = 60;

        private void OnTimeout(object state)
        {
            _seconds++;

            if (_seconds > WarnAfter)
            {
                var remaining = NavigateAfter - _seconds;
                
                Execute.OnUIThread(() =>
                {
                    SecondsLeft.Text = remaining.ToString();
                    ShowWarning.Begin();
                });

                if (remaining == 0)
                {
                    var nav = IoC.Get<INavigationService>();
                    
                    Execute.OnUIThread(() => 
                    {
                        while(nav.BackStack.Count > 1) nav.BackStack.RemoveAt(1);
                        nav.GoBack();
                    });
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _warnTimer.Dispose();
            ResetIdle(null, null);

            Window.Current.CoreWindow.PointerPressed -= ResetIdle;
            Window.Current.CoreWindow.PointerMoved -= ResetIdle;
            Window.Current.CoreWindow.KeyDown -= ResetIdle;
            Window.Current.CoreWindow.PointerWheelChanged -= ResetIdle;
        }
    }
}
