using MartinDrozdik.Services.ImageSaving.Configuration;

namespace MartinDrozdik.Services.ImageSaving;

public interface IImageSaver
{
    /// <summary>
    /// Saves an image
    /// </summary>
    /// <param name="path">The target image path</param>
    /// <param name="imageData">The image to save</param>
    /// <exception cref="ArgumentNullException">Any of the arguments are null</exception>
    Task SaveAsync(string path, Stream imageData, CancellationToken cancellationToken);

    /// <summary>
    /// Saves an image
    /// </summary>
    /// <param name="path">The target image path</param>
    /// <param name="imageData">The image to save</param>
    /// <param name="config">Save configuration</param>
    /// <exception cref="ArgumentNullException">Any of the arguments are null</exception>
    Task SaveAsync(string path, Stream imageData, IImageConfiguration config, CancellationToken cancellationToken);

    /// <summary>
    /// Saves one image into multiple paths with multiple configs.
    /// The process is as efficient as possible, scaling and modifiing one image. 
    /// It is recommended that the configs are from the highest quality to the lower quality.
    /// </summary>
    /// <exception cref="ArgumentNullException">Any of the arguments are null</exception>
    Task SaveAsync(Stream imageData, IEnumerable<ImageTarget> targets, CancellationToken cancellationToken);
}
