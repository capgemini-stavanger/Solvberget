using CoreGraphics;
using Solvberget.Core.ViewModels;
using System;
using UIKit;

namespace Solvberget.iOS
{
    public class MyPagePersonaliaView : NamedViewController
    {
        public new MyPagePersonaliaViewModel ViewModel
        {
            get
            {
                return base.ViewModel as MyPagePersonaliaViewModel;
            }
        }

        UIScrollView scrollView;

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            if (null != scrollView)
                scrollView.RemoveFromSuperview();

            scrollView = new UIScrollView(new CGRect(CGPoint.Empty, View.Frame.Size));
            scrollView.BackgroundColor = UIColor.White;

            Add(scrollView);

            var boxes = new BoxRenderer(scrollView);

            var box = boxes.StartBox();

            new LabelAndValue(box, "Navn", ViewModel.Name);
            new LabelAndValue(box, "Fødselsdato", ViewModel.DateOfBirth);

            box = boxes.StartBox();
            new LabelAndValue(box, "Addresse", ViewModel.StreetAdress + Environment.NewLine + ViewModel.CityAdress);
            new LabelAndValue(box, "Epost", ViewModel.Email);
            new LabelAndValue(box, "Mobiltelefon", ViewModel.CellPhoneNumber);

            box = boxes.StartBox();
            new LabelAndValue(box, "Bibliotek", ViewModel.HomeLibrary);
            new LabelAndValue(box, "Balanse", ViewModel.Balance);
            new LabelAndValue(box, "Kredittgrense", ViewModel.Credit);

            UIHelpers.SetContentSize(scrollView);
        }
    }

}
