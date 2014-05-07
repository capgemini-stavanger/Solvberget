using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Windows.UI.Xaml.Input;

namespace Solvberget.Search8.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchView : Page
    {
        public SearchView()
        {
            this.InitializeComponent();
        }

        private void Query_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Query.SelectAll();
            Query.PlaceholderText = "";
        }
        
        private void Query_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Query.PlaceholderText = "Trykk her for å starte et søk.";
        }

        private void Query_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Search || e.Key == VirtualKey.Enter)
            {
                ((SearchViewModel) DataContext).Search();
            }
        }
    }
}
