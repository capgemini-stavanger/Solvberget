using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Solvberget.Search8.Pages
{
    public sealed partial class ResultDetailsView : Page
    {
        public ResultDetailsView()
        {
            this.InitializeComponent();
        }

        private void Availability_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var branchName = (sender as TextBlock).Text;

            var vm = (ResultDetailsViewModel)DataContext;
            vm.ShowOtherAvailability(branchName);
        }
    }
}
