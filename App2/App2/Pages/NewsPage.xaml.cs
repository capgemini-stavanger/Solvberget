using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Xamarin.Forms;

namespace App2.Pages
{
    public partial class NewsPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public NewsPage()
        {
            InitializeComponent();

            var url = Properties.Resources.ServiceUrl + Properties.Resources.ServiceUrl_News;

            var request = WebRequest.Create(new Uri(url));

            var responseAsync = request.GetResponseAsync();

            var result = responseAsync.Result;

            var streamReader = new StreamReader(result.GetResponseStream(), Encoding.UTF8).ReadToEnd();

            var news = JsonConvert.DeserializeObject<List<NewsStoryDto>>(streamReader);

            listView.ItemsSource = news;

            listView.ItemTapped += OnItemTapped;
        }

        void OnItemTapped(object sender, ItemTappedEventArgs args)
        {
            var item = args.Item as NewsStoryDto;
            Device.OpenUri(item.Link);
        }
    }
}
