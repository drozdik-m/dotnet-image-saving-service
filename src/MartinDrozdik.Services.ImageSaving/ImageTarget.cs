using MartinDrozdik.Services.ImageSaving.Configuration;

namespace MartinDrozdik.Services.ImageSaving;

public record struct ImageTarget(string Path, IImageConfiguration Configuration);
