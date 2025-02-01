using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MartinDrozdik.Services.ImageSaving;
using MartinDrozdik.Services.ImageSaving.Configuration;
using NUnit.Framework;
using SixLabors.ImageSharp;

namespace MartinDrozdik.Services.ImageSaving.Tests;

public class ImageSaveTests
{
    private static MemoryStream GetExampleImage()
    {
        var imageBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAYAAABw4pVUAAAAnElEQVR42u3RAQ0AAAgDIE1u9FvDOahAVzLFGS1ECEKEIEQIQoQgRIgQIQgRghAhCBGCECEIQYgQhAhBiBCECEEIQoQgRAhChCBECEIQIgQhQhAiBCFCEIIQIQgRghAhCBGCEIQIQYgQhAhBiBCEIEQIQoQgRAhChCAEIUIQIgQhQhAiBCEIEYIQIQgRghAhCBEiRAhChCBECEK+W3uw+TnWoJc/AAAAAElFTkSuQmCC");
        var imageStream = new MemoryStream(imageBytes);
        return imageStream;
    }

    private static MemoryStream GetExampleSVG()
    {
        var imageBytes = Convert.FromBase64String("PD94bWwgdmVyc2lvbj0iMS4wIiA/PjxzdmcgaWQ9IkxheWVyXzFfMV8iIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDE2IDE2OyIgdmVyc2lvbj0iMS4xIiB2aWV3Qm94PSIwIDAgMTYgMTYiIHhtbDpzcGFjZT0icHJlc2VydmUiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiPjxwYXRoIGQ9Ik04LjYxMiwyLjM0N0w4LDIuOTk3bC0wLjYxMi0wLjY1Yy0xLjY5LTEuNzk1LTQuNDMtMS43OTUtNi4xMiwwYy0xLjY5LDEuNzk1LTEuNjksNC43MDYsMCw2LjUwMmwwLjYxMiwwLjY1TDgsMTYgIGw2LjEyLTYuNTAybDAuNjEyLTAuNjVjMS42OS0xLjc5NSwxLjY5LTQuNzA2LDAtNi41MDJDMTMuMDQyLDAuNTUxLDEwLjMwMiwwLjU1MSw4LjYxMiwyLjM0N3oiLz48L3N2Zz4=");
        var imageStream = new MemoryStream(imageBytes);
        return imageStream;
    }

    [Test]
    public async Task SaveImage()
    {
        var imagePath = Guid.NewGuid().ToString() + ".png";

        try
        {
            Assert.That(File.Exists(imagePath), Is.False);

            var imageSaver = new ImageSaver();
            using var image = GetExampleImage();

            await imageSaver.SaveAsync(imagePath, image, CancellationToken.None);

            Assert.That(File.Exists(imagePath), Is.True);
        }
        finally
        {
            File.Delete(imagePath);
        }
    }

    [Test]
    public async Task SaveSVG()
    {
        var imagePath = Guid.NewGuid().ToString() + ".svg";

        try
        {
            Assert.That(File.Exists(imagePath), Is.False);

            var imageSaver = new ImageSaver();
            using var image = GetExampleSVG();

            await imageSaver.SaveAsync(imagePath, image, CancellationToken.None);

            Assert.That(File.Exists(imagePath), Is.True);
        }
        finally
        {
            File.Delete(imagePath);
        }
    }

    [Test]
    public async Task SaveImageDefaultConfig()
    {
        var imagePath = Guid.NewGuid().ToString() + ".png";

        try
        {
            Assert.That(File.Exists(imagePath), Is.False);

            var imageSaver = new ImageSaver();
            using var image = GetExampleImage();

            await imageSaver.SaveAsync(imagePath, image, new ImageConfiguration()
            {
                Quality = default
            }, CancellationToken.None);

            Assert.That(File.Exists(imagePath), Is.True);

            using var savedImage = await Image.LoadAsync(imagePath);
            Assert.That(savedImage.Width, Is.EqualTo(100));
            Assert.That(savedImage.Height, Is.EqualTo(100));
        }
        finally
        {
            File.Delete(imagePath);
        }
    }

    [Test]
    public async Task SaveImageConfig()
    {
        var imagePath = Guid.NewGuid().ToString() + ".png";

        try
        {
            Assert.That(File.Exists(imagePath), Is.False);

            var imageSaver = new ImageSaver();
            using var image = GetExampleImage();

            await imageSaver.SaveAsync(imagePath, image, new ImageConfiguration()
            {
                MaxWidth = 50,
                Quality = 80
            }, CancellationToken.None);

            Assert.That(File.Exists(imagePath), Is.True);

            using var savedImage = await Image.LoadAsync(imagePath);
            Assert.That(savedImage.Width, Is.EqualTo(50));
            Assert.That(savedImage.Height, Is.EqualTo(50));
        }
        finally
        {
            File.Delete(imagePath);
        }
    }

    [Test]
    public async Task SaveMultiConfig()
    {
        var imagePath1 = Guid.NewGuid().ToString() + ".png";
        var imagePath2 = Guid.NewGuid().ToString() + ".png";
        var imagePath3 = Guid.NewGuid().ToString() + ".png";

        try
        {
            Assert.That(File.Exists(imagePath1), Is.False);
            Assert.That(File.Exists(imagePath2), Is.False);
            Assert.That(File.Exists(imagePath3), Is.False);

            var imageSaver = new ImageSaver();
            using var image = GetExampleImage();

            ImageTarget[] targets = [
                new ImageTarget(imagePath1, new ImageConfiguration()
                {
                    MaxWidth = default,
                    Quality = 80
                }),
                new ImageTarget(imagePath2, new ImageConfiguration()
                {
                    MaxWidth = 75,
                    Quality = 70
                }),
                new ImageTarget(imagePath3, new ImageConfiguration()
                {
                    MaxWidth = 50,
                    Quality = 60
                })
            ];

            await imageSaver.SaveAsync(image, targets, CancellationToken.None);

            Assert.That(File.Exists(imagePath1), Is.True);
            Assert.That(File.Exists(imagePath2), Is.True);
            Assert.That(File.Exists(imagePath3), Is.True);

            using var savedImage1 = await Image.LoadAsync(imagePath1);
            Assert.That(savedImage1.Width, Is.EqualTo(100));
            Assert.That(savedImage1.Height, Is.EqualTo(100));

            using var savedImage2 = await Image.LoadAsync(imagePath2);
            Assert.That(savedImage2.Width, Is.EqualTo(75));
            Assert.That(savedImage2.Height, Is.EqualTo(75));

            using var savedImage3 = await Image.LoadAsync(imagePath3);
            Assert.That(savedImage3.Width, Is.EqualTo(50));
            Assert.That(savedImage3.Height, Is.EqualTo(50));
        }
        finally
        {
            File.Delete(imagePath1);
            File.Delete(imagePath2);
            File.Delete(imagePath3);
        }
    }
}
