using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using Solvberget.Core.DTOs;
using Solvberget.Core.ViewModels;
using UIKit;

namespace Solvberget.iOS
{
    public class MyPageMessagesView : NamedTableViewController
    {
        public new MyPageMessagesViewModel ViewModel
        {
            get
            {
                return base.ViewModel as MyPageMessagesViewModel;
            }
        }

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            var source = new SimpleTableViewSource<NotificationDto>(TableView, CellBindings.Messages);
            TableView.Source = source;

            var set = this.CreateBindingSet<MyPageMessagesView, MyPageMessagesViewModel>();
            set.Bind(source).To(vm => vm.Notifications);

            set.Apply();


            TableView.ReloadData();

            _noRows.RemoveFromSuperview();

            if (ViewModel.Notifications.Count == 0)
            {
                _noRows = new UILabel(new CGRect(10, 10, 300, 30)) { Text = "Du har ingen meldinger.", Font = Application.ThemeColors.DefaultFont };
                Add(_noRows);
            }
        }

        UILabel _noRows = new UILabel();
    }

}
