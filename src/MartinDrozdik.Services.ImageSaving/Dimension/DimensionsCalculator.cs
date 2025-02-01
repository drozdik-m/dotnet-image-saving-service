using MartinDrozdik.Services.ImageSaving.Configuration;
using MartinDrozdik.Services.ImageSaving.Properties;
using System.Globalization;
using System.Resources;

namespace MartinDrozdik.Services.ImageSaving.Dimension;

public static class DimensionsCalculator
{
    static readonly ResourceManager resources = new(typeof(Resources));

    /// <summary>
    /// Calculates appropriate image size
    /// </summary>
    /// <param name="currentWidth">Current image width</param>
    /// <param name="currentHeight">Current image height</param>
    /// <param name="config">Desired image configucation</param>
    /// <returns>Image size</returns>
    public static Dimensions GetImageSize(int currentWidth, int currentHeight, IImageConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        Dimensions newSize = new(currentWidth, currentHeight);

        //Height or width is defined
        if (config.Width != default || config.Height != default)
        {
            //Width and heigth are defined
            if (config.Width != default && config.Height != default)
                newSize = new Dimensions(config.Width, config.Height);

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
                if (newSize.Height > config.MaxHeight)
                    newSize = new Dimensions(newSize.Width, config.MaxHeight);
                if (newSize.Width > config.MaxWidth)
                    newSize = new Dimensions(config.MaxWidth, newSize.Height);
            }

            //Only max height is defined
            else if (config.MaxHeight != default)
            {
                if (newSize.Height > config.MaxHeight)
                    newSize = GetSizeByHeight(currentWidth, currentHeight, config.MaxHeight);
            }

            //Only width is defined
            else if (config.MaxWidth != default && newSize.Width > config.MaxWidth)
                newSize = GetSizeByWidth(currentWidth, currentHeight, config.MaxWidth);
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
    public static Dimensions GetSizeByWidth(int currentWidth, int currentHeight, int newWidth)
    {
        if (currentWidth == 0)
            throw new ArgumentException(resources.GetString("CurrentSize.Width can not be zero", CultureInfo.CurrentCulture));

        var newHeight = currentHeight * newWidth / (double)currentWidth;
        return new Dimensions(newWidth, (int)newHeight);
    }

    /// <summary>
    /// Returns image size based on the current size and new height.
    /// The calculation preserves the aspect ratio.
    /// </summary>
    /// <param name="currentWidth">Current image width</param>
    /// <param name="currentHeight">Current image height</param>
    /// <param name="newHeight">The desired height</param>
    public static Dimensions GetSizeByHeight(int currentWidth, int currentHeight, int newHeight)
    {
        if (currentHeight == 0)
            throw new ArgumentException(resources.GetString("CurrentSize.Height can not be zero", CultureInfo.CurrentCulture));

        if (newHeight == 0)
            throw new ArgumentException(resources.GetString("newHeight can not be zero", CultureInfo.CurrentCulture));

        var newWidth = currentWidth / (currentHeight / (double)newHeight);
        return new Dimensions((int)newWidth, newHeight);
    }
}
