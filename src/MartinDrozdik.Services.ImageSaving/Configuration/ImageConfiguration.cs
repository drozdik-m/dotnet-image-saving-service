namespace MartinDrozdik.Services.ImageSaving.Configuration;

public class ImageConfiguration : IImageConfiguration
{
    /// <inheritdoc/>
    public int Height { get; set; } = default;

    /// <inheritdoc/>
    public int Width { get; set; } = default;

    /// <inheritdoc/>
    public int MaxHeight { get; set; } = default;

    /// <inheritdoc/>
    public int MaxWidth { get; set; } = default;

    /// <inheritdoc/>
    public int Quality
    {
        get => quality;
        set
        {
            if (value < 0 && value > 100)
                throw new ArgumentOutOfRangeException(nameof(value));

            quality = value;
        }
    }
    int quality = 75;
}
