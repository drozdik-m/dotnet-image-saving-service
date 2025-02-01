using MartinDrozdik.Services.ImageSaving.Configuration;
using MartinDrozdik.Services.ImageSaving.Dimension;
using NUnit.Framework;

namespace MartinDrozdik.Services.ImageSaving.Tests;

public class ImageSizeTests
{
    [Test]
    public void GetSizeByHeight()
    {
        Assert.That(DimensionsCalculator.GetSizeByHeight(100, 50, 25).Height, Is.EqualTo(25));
        Assert.That(DimensionsCalculator.GetSizeByHeight(100, 50, 25).Width, Is.EqualTo(50));

        Assert.That(DimensionsCalculator.GetSizeByHeight(32, 18, 9).Height, Is.EqualTo(9));
        Assert.That(DimensionsCalculator.GetSizeByHeight(32, 18, 9).Width, Is.EqualTo(16));

        Assert.That(DimensionsCalculator.GetSizeByHeight(100, 100, 50).Height, Is.EqualTo(50));
        Assert.That(DimensionsCalculator.GetSizeByHeight(100, 100, 50).Width, Is.EqualTo(50));
    }

    [Test]
    public void GetSizeByWidth()
    {
        Assert.That(DimensionsCalculator.GetSizeByWidth(50, 100, 25).Height, Is.EqualTo(50));
        Assert.That(DimensionsCalculator.GetSizeByWidth(50, 100, 25).Width, Is.EqualTo(25));

        Assert.That(DimensionsCalculator.GetSizeByWidth(32, 18, 16).Height, Is.EqualTo(9));
        Assert.That(DimensionsCalculator.GetSizeByWidth(32, 18, 16).Width, Is.EqualTo(16));

        Assert.That(DimensionsCalculator.GetSizeByWidth(100, 100, 50).Height, Is.EqualTo(50));
        Assert.That(DimensionsCalculator.GetSizeByWidth(100, 100, 50).Width, Is.EqualTo(50));
    }

    [Test]
    public void GetImageSize_Empty()
    {
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()).Width, Is.EqualTo(160));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()).Height, Is.EqualTo(90));
    }

    [Test]
    public void GetImageSize_FixedDimensions()
    {
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Height = 50,
            Width = 60
        }).Width, Is.EqualTo(60));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Height = 50,
            Width = 60
        }).Height, Is.EqualTo(50));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Height = 9
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Height = 9
        }).Height, Is.EqualTo(9));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 16
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 16
        }).Height, Is.EqualTo(9));
    }

    [Test]
    public void GetImageSize_MaxDimensions_Shrink()
    {
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxHeight = 50,
            MaxWidth = 60
        }).Width, Is.EqualTo(60));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxHeight = 50,
            MaxWidth = 60
        }).Height, Is.EqualTo(50));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxHeight = 9
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxHeight = 9
        }).Height, Is.EqualTo(9));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxWidth = 16
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxWidth = 16
        }).Height, Is.EqualTo(9));
    }

    [Test]
    public void GetImageSize_MaxDimensions_Idle()
    {
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxHeight = 500,
            MaxWidth = 600
        }).Width, Is.EqualTo(160));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxHeight = 500,
            MaxWidth = 600
        }).Height, Is.EqualTo(90));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxHeight = 90
        }).Width, Is.EqualTo(160));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxHeight = 90
        }).Height, Is.EqualTo(90));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxWidth = 160
        }).Width, Is.EqualTo(160));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            MaxWidth = 160
        }).Height, Is.EqualTo(90));
    }

    [Test]
    public void GetImageSize_MixedDimensions()
    {
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 16,
            Height = 9,
            MaxWidth = 160,
            MaxHeight = 90
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 16,
            Height = 9,
            MaxWidth = 160,
            MaxHeight = 90
        }).Height, Is.EqualTo(9));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 160,
            Height = 90,
            MaxWidth = 16,
            MaxHeight = 9
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 16,
            Height = 9,
            MaxWidth = 16,
            MaxHeight = 9
        }).Height, Is.EqualTo(9));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Height = 90,
            MaxHeight = 9
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Height = 90,
            MaxHeight = 9
        }).Height, Is.EqualTo(9));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Height = 9,
            MaxHeight = 90
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Height = 9,
            MaxHeight = 90
        }).Height, Is.EqualTo(9));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 160,
            MaxWidth = 16
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 160,
            MaxWidth = 16
        }).Height, Is.EqualTo(9));

        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 16,
            MaxWidth = 160
        }).Width, Is.EqualTo(16));
        Assert.That(DimensionsCalculator.GetImageSize(160, 90, new ImageConfiguration()
        {
            Width = 16,
            MaxWidth = 160
        }).Height, Is.EqualTo(9));
    }
}
