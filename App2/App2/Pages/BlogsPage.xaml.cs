using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Xamarin.Forms;

namespace App2.Pages
{
    public partial class BlogsPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public BlogsPage()
        {
            InitializeComponent();

            var url = Properties.Resources.ServiceUrl + Properties.Resources.ServiceUrl_BlogListing;

            var request = WebRequest.Create(new Uri(url));

            var responseAsync = request.GetResponseAsync();

            var result = responseAsync.Result;

            var streamReader = new StreamReader(result.GetResponseStream(), Encoding.UTF8).ReadToEnd();

            var blogs = JsonConvert.DeserializeObject<List<BlogDto>>(streamReader);

            listView.ItemsSource = blogs;

            listView.ItemTapped += (sender, args) =>
            {
                var item = args.Item as BlogDto;
                Navigation.PushAsync(new BlogPage((int) item.Id));
            };
        }
    }
}
