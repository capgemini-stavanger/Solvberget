using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Solvberget.Core.DTOs;

namespace Solvberget.Search8.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultsView : Page
    {
        public ResultsView()
        {
            InitializeComponent();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SemanticZoom.ToggleActiveView();
        }
        
        private void FilterOption_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var vm = (ResultsViewModel)DataContext;
            vm.SelectedFilter = ((FrameworkElement)sender).DataContext as FilterOptionVm;
        }

        private void Result_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var vm = (ResultsViewModel)DataContext;
            vm.ShowDetails(((FrameworkElement)sender).DataContext as DocumentDto);
            
        }
    }
}
