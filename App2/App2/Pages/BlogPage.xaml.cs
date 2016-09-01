using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Xamarin.Forms;

namespace App2.Pages
{
    public partial class BlogPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public BlogPage(int id)
        {
            InitializeComponent();

            var url = Properties.Resources.ServiceUrl + "/blogs/" + id;

            var request = WebRequest.Create(new Uri(url));

            var responseAsync = request.GetResponseAsync();

            var result = responseAsync.Result;

            var streamReader = new StreamReader(result.GetResponseStream(), Encoding.UTF8).ReadToEnd();

            var blergs = JsonConvert.DeserializeObject<BlogWithPostsDto>(streamReader);

            listView.ItemsSource = blergs.Posts;
        }
    }
}
