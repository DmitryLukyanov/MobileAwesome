using Autofac;
using MobileAwesomeApp.Infrastructure.Autofac;
using Xamarin.Forms.Internals;

namespace MobileAwesomeApp.Infrastructure
{
    public class DI
    {
        private static IContainer __container;

        public static void Initialize()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<MobileAwesomeAppModule>();
            __container = builder.Build();

            DependencyResolver.ResolveUsing(type => __container.IsRegistered(type) ? __container.Resolve(type) : null);
        }
    }
}
