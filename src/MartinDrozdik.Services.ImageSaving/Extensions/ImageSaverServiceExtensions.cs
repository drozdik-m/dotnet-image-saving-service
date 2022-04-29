using Microsoft.Extensions.DependencyInjection;

namespace MartinDrozdik.Services.ImageSaving.Extensions
{
    public static class ImageSaverServiceExtensions
    {
        public static IServiceCollection AddImageSaver(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IImageSaver, ImageSaver>();

            return serviceCollection;
        }
    }
}

