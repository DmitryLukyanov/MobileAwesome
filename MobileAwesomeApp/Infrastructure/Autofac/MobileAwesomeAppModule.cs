using System.Linq;
using System.Reflection;
using Autofac;
using MobileAwesomeApp.Infrastructure.Mongo;
using MobileAwesomeApp.Services;
using MongoDB.Driver;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Module = Autofac.Module;

namespace MobileAwesomeApp.Infrastructure.Autofac
{
    public class MobileAwesomeAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register lower modules
            // builder.RegisterModule<...>();

            builder
                .RegisterTypes(
                    typeof(HardcodedSettingsProvider),
                    typeof(RestaurantService),
                    typeof(GeoService))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<MongoClientFactory>().AsSelf(); // TODO: interface?
            builder.Register<IMongoClient>(
                (context) =>
                {
                    var clientFactory = context.Resolve<MongoClientFactory>();
                    var client = clientFactory.CreateMongoClient();
                    return client;
                })
                .SingleInstance();
            builder.RegisterType<MongoClientWrapper>().AsSelf().SingleInstance();
            builder.RegisterTypes(typeof(Page)).AsSelf();
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(type => type.FullName.EndsWith("ViewModel"))
                    .ForEach(type => builder.RegisterType(type).AsSelf());
        }
    }
}
