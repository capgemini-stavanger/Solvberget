using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App2.Pages
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public MasterPage()
        {
            InitializeComponent();

            var masterPageItems = new List<MasterPageItem>();

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Min Side",
                IconSource = "todo.png",
                TargetType = typeof(MyPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Arrangementer",
                IconSource = "todo.png",
                TargetType = typeof(EventsPage)
            });
            //masterPageItems.Add(new MasterPageItem
            //{
            //    Title = "Søk",
            //    IconSource = "todo.png",
            //    TargetType = typeof(SearchPage)
            //});
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Blogger",
                IconSource = "todo.png",
                TargetType = typeof(BlogsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Nyheter",
                IconSource = "todo.png",
                TargetType = typeof(NewsPage)
            });
            //masterPageItems.Add(new MasterPageItem
            //{
            //    Title = "Anbefalinger",
            //    IconSource = "todo.png",
            //    TargetType = typeof(SuggestionsPage)
            //});
            //masterPageItems.Add(new MasterPageItem
            //{
            //    Title = "Åpningstider",
            //    IconSource = "todo.png",
            //    TargetType = typeof(OpeningHoursPage)
            //});
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Kontakt oss",
                IconSource = "todo.png",
                TargetType = typeof(ContactsPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }

    public class MasterPageItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}
