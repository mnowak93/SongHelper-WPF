using Autofac;
using SpotifyHandlingLibrary;
using SpotifyHandlingLibrary.Models;
using YoutubeHandlingLibrary;

namespace MvxStarter.Core
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<YotubeApiHandling>().As<IYotubeApiHandling>();
            builder.RegisterType<SpotifyApiHandling>().As<ISpotifyApiHandling>();
            builder.RegisterType<SpotifyToken>().As<ISpotifyToken>();

            return builder.Build();
        }
    }
}
