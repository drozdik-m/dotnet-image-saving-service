using MartinDrozdik.Services.ImageSaving.Configuration;
using MartinDrozdik.Services.ImageSaving.Properties;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading.Tasks;

namespace MartinDrozdik.Services.ImageSaving
{
    public class ImageSaver : IImageSaver
    {
        static readonly ResourceManager resources = new ResourceManager(typeof(Resources));

        /// <inheritdoc/>
        public async Task SaveAsync(string path, Stream imageData)
        {
            if (imageData == null)
                throw new ArgumentNullException(nameof(imageData));

            path = Path.GetFullPath(path);

            //Load
            using Image image = Image.Load(imageData/*, out IImageFormat format*/);

            //Save
            await image.SaveAsync(path);
        }

        /// <inheritdoc/>
        public async Task SaveAsync(string path, Stream imageData, IImageConfiguration config)
        {
            if (imageData == null)
                throw new ArgumentNullException(nameof(imageData));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            path = Path.GetFullPath(path);

            ///Load
            using Image image = Image.Load(imageData, out IImageFormat format);

            //Resize
            var size = GetImageSize(image.Width, image.Height, config);
            image.Mutate(x => x.Resize(size.Width, size.Height));

            //Quality
            ImageFormatManager formatManager = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager;
            var encoder = formatManager.FindEncoder(format);

            //if (encoder is BmpEncoder bmp)
            //if (encoder is GifEncoder gif)
            if (encoder is JpegEncoder jpeg)
                jpeg.Quality = config.Quality;
            //if (encoder is PbmEncoder pbm)
            //if (encoder is PngEncoder png)
            //if (encoder is TgaEncoder tga)
            //if (encoder is TiffEncoder tiff)
            if (encoder is WebpEncoder webp)
                webp.Quality = config.Quality;

            //Save
            await image.SaveAsync(path, encoder);
        }

        /// <summary>
        /// Calculates appropriate image size
        /// </summary>
        /// <param name="currentSize">Current image size</param>
        /// <param name="config">Image config</param>
        /// <returns>Image size</returns>
        public static (int Width, int Height) GetImageSize(int currentWidth, int currentHeight, IImageConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            (int newWidth, int newHeight) newSize = (currentWidth, currentHeight);

            //Height or width is defined
            if (config.Width != default || config.Height != default)
            {
                //Width and heigth are defined
                if (config.Width != default && config.Height != default)
                    newSize = (config.Width, config.Height);

                //Only height is defined
                else if (config.Height != default)
                    newSize = GetSizeByHeight(currentWidth, currentHeight, config.Height);

                //Only width is defined
                else if (config.Width != default)
                    newSize = GetSizeByWidth(currentWidth, currentHeight, config.Width);
            }

            //Max height or mag width is defined
            if (config.MaxWidth != default || config.MaxHeight != default)
            {
                //Max width and max heigth are defined
                if (config.MaxWidth != default && config.MaxHeight != default)
                {
                    if (newSize.newHeight > config.MaxHeight)
                        newSize = (newSize.newWidth, config.MaxHeight);
                    if (newSize.newWidth > config.MaxWidth)
                        newSize = (config.MaxWidth, newSize.newHeight);
                }

                //Only max height is defined
                else if (config.MaxHeight != default)
                {
                    if (newSize.newHeight > config.MaxHeight)
                        newSize = GetSizeByHeight(currentWidth, currentHeight, config.MaxHeight);
                }

                //Only width is defined
                else if (config.MaxWidth != default)
                {
                    if (newSize.newWidth > config.MaxWidth)
                        newSize = GetSizeByWidth(currentWidth, currentHeight, config.MaxWidth);
                }
            }

            return newSize;
        }

        /// <summary>
        /// Returns image size based on the current size and new width.
        /// The calculation preserves the aspect ratio.
        /// </summary>
        /// <param name="currentSize">The current size</param>
        /// <param name="newWidth">The desired width</param>
        /// <returns></returns>
        public static (int Width, int Height) GetSizeByWidth(int currentWidth, int currentHeight, int newWidth)
        {
            if (currentWidth == 0)
                throw new ArgumentException(resources.GetString("CurrentSize.Width can not be zero", CultureInfo.CurrentCulture));

            var newHeight = currentHeight * newWidth / (double)currentWidth;
            return (newWidth, (int)newHeight);
        }

        /// <summary>
        /// Returns image size based on the current size and new height.
        /// The calculation preserves the aspect ratio.
        /// </summary>
        /// <param name="currentSize">The current size</param>
        /// <param name="newHeight">The desired height</param>
        /// <returns></returns>
        public static (int Width, int Height) GetSizeByHeight(int currentWidth, int currentHeight, int newHeight)
        {
            if (currentHeight == 0)
                throw new ArgumentException(resources.GetString("CurrentSize.Height can not be zero", CultureInfo.CurrentCulture));

            if (newHeight == 0)
                throw new ArgumentException(resources.GetString("newHeight can not be zero", CultureInfo.CurrentCulture));

            var newWidth = currentWidth / (currentHeight / (double)newHeight);
            return ((int)newWidth, newHeight);
        }

        /*/// <summary>
        /// Returns an image format based on an extension (.jpg, .png, ...)
        /// </summary>
        /// <param name="extension">The extension with dot</param>
        /// <returns>ISupportedImageFormat</returns>
        static ISupportedImageFormat GetImageFormat(string extension)
        {
            switch (extension)
            {
                case ".bmp":
                    return new BitmapFormat();
                case ".png":
                case ".png8":
                    return new PngFormat();
                case ".gif":
                    return new GifFormat();
                case ".tif":
                case ".tiff":
                    return new TiffFormat();
                default:
                    return new JpegFormat();
            }
        }*/

        /*public IEnumerable<string> SupportedExtensions { get; } = new string[] {
            ".bmp",
            ".png",
            ".png8",
            ".gif",
            ".tif",
            ".tiff",
            ".jpg",
            ".jpeg",

            ".bmp",
            ".png",
            ".png8",
            ".gif",
            ".tif",
            ".tiff",
            ".jpg",
            ".jpeg"
        };*/
    }
}
