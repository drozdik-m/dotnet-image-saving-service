using MartinDrozdik.Services.ImageSaving.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace MartinDrozdik.Services.ImageSaving
{
    public interface IImageSaver
    {
        /// <summary>
        /// Saves an image
        /// </summary>
        /// <param name="path">The target image path</param>
        /// <param name="imageData">The image to save</param>
        Task Save(string path, Stream imageData);

        /// <summary>
        /// Saves an image
        /// </summary>
        /// <param name="path">The target image path</param>
        /// <param name="imageData">The image to save</param>
        /// <param name="config">Save configuration</param>
        Task Save(string path, Stream imageData, IImageConfiguration config);
    }
}
