using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using MartinDrozdik.Services.ImageSaving;
using MartinDrozdik.Services.ImageSaving.Configuration;
using NUnit.Framework;
using SixLabors.ImageSharp;

namespace MartinDrozdik.Tests.Services.ImageProcessing
{
    public class ImageSaveTests
    {

        [SetUp]
        public void Setup()
        {

        }

        Stream GetExampleImage()
        {
            var imageBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAYAAABw4pVUAAAAnElEQVR42u3RAQ0AAAgDIE1u9FvDOahAVzLFGS1ECEKEIEQIQoQgRIgQIQgRghAhCBGCECEIQYgQhAhBiBCECEEIQoQgRAhChCBECEIQIgQhQhAiBCFCEIIQIQgRghAhCBGCEIQIQYgQhAhBiBCEIEQIQoQgRAhChCAEIUIQIgQhQhAiBCEIEYIQIQgRghAhCBEiRAhChCBECEK+W3uw+TnWoJc/AAAAAElFTkSuQmCC");
            var imageStream = new MemoryStream(imageBytes);
            return imageStream;
        }

        [Test]
        public async Task SaveImage()
        {
            var imagePath = Guid.NewGuid().ToString() + ".png";

            try
            {
                Assert.IsFalse(File.Exists(imagePath));

                var imageSaver = new ImageSaver();
                using var image = GetExampleImage();

                await imageSaver.Save(imagePath, image);

                Assert.IsTrue(File.Exists(imagePath));
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
                Assert.IsFalse(File.Exists(imagePath));

                var imageSaver = new ImageSaver();
                using var image = GetExampleImage();

                await imageSaver.Save(imagePath, image, new ImageConfiguration()
                {
                    MaxWidth = 50,
                    Quality = 80
                });

                Assert.IsTrue(File.Exists(imagePath));

                using var savedImage = Image.Load(imagePath);
                Assert.AreEqual(50, savedImage.Width);
                Assert.AreEqual(50, savedImage.Height);
            }
            finally
            {
                File.Delete(imagePath);
            }
        }
    }
}
