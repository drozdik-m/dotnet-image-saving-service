namespace MartinDrozdik.Services.ImageSaving.Configuration;

public interface IImageConfiguration
{
    /// <summary>
    /// Fixed height of the image [px]
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Fix width of the image [px]
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Maximum height of the image [px]
    /// </summary>
    int MaxHeight { get;    }

    /// <summary>
    /// Maximum width of the image [px]
    /// </summary>
    int MaxWidth { get; }

    /// <summary>
    /// Quality of the image [%]
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The quality must be between 0 % and 100 %</exception>
    int Quality { get; }
}
