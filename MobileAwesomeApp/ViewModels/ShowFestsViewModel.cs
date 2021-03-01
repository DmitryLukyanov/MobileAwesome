using Xamarin.Forms;

namespace MobileAwesomeApp.ViewModels
{
    public class ShowFestsViewModel
    {
        public HtmlWebViewSource Html
        {
            get
            {
                // TODO: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/webview?tabs=windows#ios
                var source = @"<iframe style=""background: #FFFFFF;border: none;border-radius: 2px;box-shadow: 0 2px 10px 0 rgba(70, 76, 79, .2);"" width=""640"" height=""480"" src=""https://charts.mongodb.com/charts-mobileawesome-ldwiw/embed/charts?id=1a56c5d5-0a18-4f5f-9adf-2aef9b453e8d&theme=light""></iframe>";
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = source;
                return htmlSource;
            }
        }
    }
}
