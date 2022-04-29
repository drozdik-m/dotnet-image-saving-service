using MartinDrozdik.Services.ImageSaving;
using MartinDrozdik.Services.ImageSaving.Configuration;
using NUnit.Framework;

namespace MartinDrozdik.Tests.Services.ImageProcessing
{
    public class ImageSizeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetSizeByHeight()
        {
            Assert.AreEqual(25, ImageSaver.GetSizeByHeight(100, 50, 25).Height);
            Assert.AreEqual(50, ImageSaver.GetSizeByHeight(100, 50, 25).Width);

            Assert.AreEqual(9, ImageSaver.GetSizeByHeight(32, 18, 9).Height);
            Assert.AreEqual(16, ImageSaver.GetSizeByHeight(32, 18, 9).Width);

            Assert.AreEqual(50, ImageSaver.GetSizeByHeight(100, 100, 50).Height);
            Assert.AreEqual(50, ImageSaver.GetSizeByHeight(100, 100, 50).Width);
        }

        [Test]
        public void GetSizeByWidth()
        {
            Assert.AreEqual(50, ImageSaver.GetSizeByWidth(50, 100, 25).Height);
            Assert.AreEqual(25, ImageSaver.GetSizeByWidth(50, 100, 25).Width);

            Assert.AreEqual(9, ImageSaver.GetSizeByWidth(32, 18, 16).Height);
            Assert.AreEqual(16, ImageSaver.GetSizeByWidth(32, 18, 16).Width);

            Assert.AreEqual(50, ImageSaver.GetSizeByWidth(100, 100, 50).Height);
            Assert.AreEqual(50, ImageSaver.GetSizeByWidth(100, 100, 50).Width);
        }

        [Test]
        public void GetImageSize_Empty()
        {
            Assert.AreEqual(160, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()).Width);
            Assert.AreEqual(90, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()).Height);
        }

        [Test]
        public void GetImageSize_FixedDimensions()
        {
            Assert.AreEqual(60, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Height = 50,
                Width = 60
            }).Width);
            Assert.AreEqual(50, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Height = 50,
                Width = 60
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Height = 9
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Height = 9
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 16
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 16
            }).Height);
        }

        [Test]
        public void GetImageSize_MaxDimensions_Shrink()
        {
            Assert.AreEqual(60, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxHeight = 50,
                MaxWidth = 60
            }).Width);
            Assert.AreEqual(50, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxHeight = 50,
                MaxWidth = 60
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxHeight = 9
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxHeight = 9
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxWidth = 16
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxWidth = 16
            }).Height);
        }

        [Test]
        public void GetImageSize_MaxDimensions_Idle()
        {
            Assert.AreEqual(160, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxHeight = 500,
                MaxWidth = 600
            }).Width);
            Assert.AreEqual(90, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxHeight = 500,
                MaxWidth = 600
            }).Height);

            Assert.AreEqual(160, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxHeight = 90
            }).Width);
            Assert.AreEqual(90, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxHeight = 90
            }).Height);

            Assert.AreEqual(160, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxWidth = 160
            }).Width);
            Assert.AreEqual(90, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                MaxWidth = 160
            }).Height);
        }

        [Test]
        public void GetImageSize_MixedDimensions()
        {
            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 16,
                Height = 9,
                MaxWidth = 160,
                MaxHeight = 90
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 16,
                Height = 9,
                MaxWidth = 160,
                MaxHeight = 90
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 160,
                Height = 90,
                MaxWidth = 16,
                MaxHeight = 9
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 16,
                Height = 9,
                MaxWidth = 16,
                MaxHeight = 9
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Height = 90,
                MaxHeight = 9
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Height = 90,
                MaxHeight = 9
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Height = 9,
                MaxHeight = 90
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Height = 9,
                MaxHeight = 90
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 160,
                MaxWidth = 16
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 160,
                MaxWidth = 16
            }).Height);

            Assert.AreEqual(16, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 16,
                MaxWidth = 160
            }).Width);
            Assert.AreEqual(9, ImageSaver.GetImageSize(160, 90, new ImageConfiguration()
            {
                Width = 16,
                MaxWidth = 160
            }).Height);
        }


    }
}
