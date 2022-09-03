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

        Stream GetExampleSVG()
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
                Assert.IsFalse(File.Exists(imagePath));

                var imageSaver = new ImageSaver();
                using var image = GetExampleImage();

                await imageSaver.SaveAsync(imagePath, image);

                Assert.IsTrue(File.Exists(imagePath));
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
                Assert.IsFalse(File.Exists(imagePath));

                var imageSaver = new ImageSaver();
                using var image = GetExampleSVG();

                await imageSaver.SaveAsync(imagePath, image);

                Assert.IsTrue(File.Exists(imagePath));
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
                Assert.IsFalse(File.Exists(imagePath));

                var imageSaver = new ImageSaver();
                using var image = GetExampleImage();

                await imageSaver.SaveAsync(imagePath, image, new ImageConfiguration()
                {
                    Quality = default
                });

                Assert.IsTrue(File.Exists(imagePath));

                using var savedImage = Image.Load(imagePath);
                Assert.AreEqual(100, savedImage.Width);
                Assert.AreEqual(100, savedImage.Height);
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

                await imageSaver.SaveAsync(imagePath, image, new ImageConfiguration()
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

        [Test]
        public async Task SaveMultiConfig()
        {
            var imagePath1 = Guid.NewGuid().ToString() + ".png";
            var imagePath2 = Guid.NewGuid().ToString() + ".png";
            var imagePath3 = Guid.NewGuid().ToString() + ".png";

            try
            {
                Assert.IsFalse(File.Exists(imagePath1));
                Assert.IsFalse(File.Exists(imagePath2));
                Assert.IsFalse(File.Exists(imagePath3));

                var imageSaver = new ImageSaver();
                using var image = GetExampleImage();

                await imageSaver.SaveAsync(image, (imagePath1, new ImageConfiguration()
                {
                    MaxWidth = default,
                    Quality = 80
                }), (imagePath2, new ImageConfiguration()
                {
                    MaxWidth = 75,
                    Quality = 70
                }), (imagePath3, new ImageConfiguration()
                {
                    MaxWidth = 50,
                    Quality = 60
                }));

                Assert.IsTrue(File.Exists(imagePath1));
                Assert.IsTrue(File.Exists(imagePath2));
                Assert.IsTrue(File.Exists(imagePath3));

                using var savedImage1 = Image.Load(imagePath1);
                Assert.AreEqual(100, savedImage1.Width);
                Assert.AreEqual(100, savedImage1.Height);

                using var savedImage2 = Image.Load(imagePath2);
                Assert.AreEqual(75, savedImage2.Width);
                Assert.AreEqual(75, savedImage2.Height);

                using var savedImage3 = Image.Load(imagePath3);
                Assert.AreEqual(50, savedImage3.Width);
                Assert.AreEqual(50, savedImage3.Height);
            }
            finally
            {
                File.Delete(imagePath1);
                File.Delete(imagePath2);
                File.Delete(imagePath3);
            }
        }
    }
}
