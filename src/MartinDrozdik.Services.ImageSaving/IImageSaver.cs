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
        Task SaveAsync(string path, Stream imageData);

        /// <summary>
        /// Saves an image
        /// </summary>
        /// <param name="path">The target image path</param>
        /// <param name="imageData">The image to save</param>
        /// <param name="config">Save configuration</param>
        Task SaveAsync(string path, Stream imageData, IImageConfiguration config);

        /// <summary>
        /// Saves one image into multiple paths with multiple configs.
        /// The process is as efficient as possible, scaling and modifiing one image. 
        /// It is recommended that the configs are from the highest quality to the lower quality.
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        Task SaveAsync(Stream imageData, params (string path, IImageConfiguration config)[] targets);
    }
}
