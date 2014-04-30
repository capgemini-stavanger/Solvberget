using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
            this.InitializeComponent();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if(SemanticZoom.IsZoomedInViewActive) SemanticZoom.ToggleActiveView();
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
