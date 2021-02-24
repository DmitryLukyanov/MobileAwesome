using Autofac;
using MobileAwesomeApp.Infrastructure.Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MobileAwesomeApp.Infrastructure
{
    public class DI
    {
        private static IContainer __container;
        public static IContainer Container => __container;

        static DI()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<MobileAwesomeAppModule>();
            __container = builder.Build();
            // TODO: ?
            //DependencyResolver.ResolveUsing(type => __container.IsRegistered(type) ? __container.Resolve(type) : null);
        }
    }
}
