using MobileAwesomeApp.Models;
using Xamarin.Forms;

namespace MobileAwesomeApp.ViewModels
{
    public class RestaurantViewModel
    {
        public string Cuisine { get; private set; }
        public string Name { get; private set; }

        public RestaurantViewModel(Restaurant restaurant)
        {
            Cuisine = restaurant.Cuisine;
            Name = restaurant.Name;
        }
    }

    public class VM
    {
        public HtmlWebViewSource Html
        {
            get
            {
                var source = @"<iframe style=""background: #FFFFFF;border: none;border-radius: 2px;box-shadow: 0 2px 10px 0 rgba(70, 76, 79, .2);"" width=""640"" height=""480"" src=""https://charts.mongodb.com/charts-mobileawesome-ldwiw/embed/charts?id=1a56c5d5-0a18-4f5f-9adf-2aef9b453e8d&theme=light""></iframe>";
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = source;
                return htmlSource;
            }
        }

    }
}
