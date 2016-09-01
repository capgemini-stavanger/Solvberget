using System.Collections.Generic;
using Xamarin.Forms;

namespace App2.Pages
{
    public partial class EventsPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public EventsPage()
        {
            InitializeComponent();

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Contacts",
                IconSource = "contacts.png",
                TargetType = typeof(ContactsPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}
