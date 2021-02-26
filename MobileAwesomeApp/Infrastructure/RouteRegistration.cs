using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MobileAwesomeApp.Infrastructure
{
    public class RouteRegistration
    {
        public static void RegisterRoutes()
        {
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(type => type.Name.EndsWith("Page"))
                    .ForEach(type => Routing.RegisterRoute(type.Name, type));
        }
    }
}
