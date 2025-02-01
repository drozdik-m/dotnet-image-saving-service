using MartinDrozdik.Services.ImageSaving.Configuration;
using MartinDrozdik.Services.ImageSaving.Dimension;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics.CodeAnalysis;
using Path = System.IO.Path;

namespace MartinDrozdik.Services.ImageSaving;

public class ImageSaver : IImageSaver
{
    /// <inheritdoc/>
    public async Task SaveAsync(string path, Stream imageData, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(imageData);

        path = Path.GetFullPath(path);

        await CopyToFileAsync(path, imageData, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SaveAsync(string path, Stream imageData, IImageConfiguration config, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(imageData);
        ArgumentNullException.ThrowIfNull(config);

        path = Path.GetFullPath(path);

        // Default behaviour
        if (await CheckForDefaultBehaviourAsync(path, imageData, config, cancellationToken))
            return;

        // Load
        using Image image = await Image.LoadAsync(imageData, cancellationToken);

        // Auto rotate (exif)
        image.Mutate(e =>
        {
            e.AutoOrient();
        });

        // Resize
        if (!(config.Width == default && config.Height == default
            && config.MaxWidth == default && config.MaxHeight == default))
        {
            var size = DimensionsCalculator.GetImageSize(image.Width, image.Height, config);
            image.Mutate(x => x.Resize(size.Width, size.Height));
        }

        // Save using an encoder if possible
        if (TryGetEncoder(image, config, out var encoder))
        {
            await image.SaveAsync(path, encoder, cancellationToken);
        }
        else
        {
            await image.SaveAsync(path, cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task SaveAsync(Stream imageData, IEnumerable<ImageTarget> targets, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(imageData);
        ArgumentNullException.ThrowIfNull(targets);

        Image? image = null;

        foreach(var target in targets)
        {
            ArgumentNullException.ThrowIfNull(target.Path);
            ArgumentNullException.ThrowIfNull(target.Configuration);

            imageData.Seek(0, SeekOrigin.Begin);

            var path = Path.GetFullPath(target.Path);

            // Default behaviour
            if (await CheckForDefaultBehaviourAsync(path, imageData, target.Configuration, cancellationToken))
                continue;

            // Load the image if not already loaded
            if (image is null)
            {
                image = await Image.LoadAsync(imageData, cancellationToken);

                // Auto rotate (exif)
                image.Mutate(e =>
                {
                    e.AutoOrient();
                });
            }

            // Resize
            if (!(target.Configuration.Width == default && target.Configuration.Height == default
                && target.Configuration.MaxWidth == default && target.Configuration.MaxHeight == default))
            {
                var size = DimensionsCalculator.GetImageSize(image.Width, image.Height, target.Configuration);
                image.Mutate(x => x.Resize(size.Width, size.Height));
            }

            // Save using an encoder if possible
            if (TryGetEncoder(image, target.Configuration, out var encoder))
            {
                await image.SaveAsync(path, encoder, cancellationToken);
            }
            else
            {
                await image.SaveAsync(path, cancellationToken);
            }
        }

        image?.Dispose();
    }

    /// <summary>
    /// Tries to get an encoder for the image
    /// </summary>
    private static bool TryGetEncoder(Image image, IImageConfiguration config, [NotNullWhen(true)] out IImageEncoder? encoder)
    {
        var format = image.Metadata.DecodedImageFormat;

        if (format is null)
        {
            encoder = null;
            return false;
        }

        ImageFormatManager formatManager = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager;
        encoder = formatManager.GetEncoder(format);

        if (encoder is JpegEncoder jpeg)
        {
            encoder = new JpegEncoder
            {
                Quality = config.Quality,
                ColorType = jpeg.ColorType,
                Interleaved = jpeg.Interleaved,
                SkipMetadata = jpeg.SkipMetadata,
            };
        }

        return true;
    }

    /// <summary>
    /// Check if the file should be simply saved, without any coversion.
    /// </summary>
    /// <returns>True if the image needs no further process. Else false.</returns>
    private static async Task<bool> CheckForDefaultBehaviourAsync(string fullPath, Stream imageData, IImageConfiguration config, CancellationToken cancellationToken)
    {
        // No width or quality? Simply save it!
        if (config.Quality == default &&
            config.Width == default && config.Height == default
            && config.MaxWidth == default && config.MaxHeight == default)
        {
            await CopyToFileAsync(fullPath, imageData, cancellationToken);
            return true;
        }

        // SVG image? Simply save it!
        var extention = Path.GetExtension(fullPath);
        if (extention != null && extention.Equals(".svg", StringComparison.InvariantCultureIgnoreCase))
        {
            await CopyToFileAsync(fullPath, imageData, cancellationToken);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Simply outputs the stream into a file
    /// </summary>
    private static async Task CopyToFileAsync(string fullPath, Stream imageData, CancellationToken cancellationToken)
    {
        using var outputStream = File.Create(fullPath);
        imageData.Seek(0, SeekOrigin.Begin);
        await imageData.CopyToAsync(outputStream, cancellationToken);
    }
}
