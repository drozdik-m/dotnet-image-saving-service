# Image saving service for quick use

A service to save your images to disk with little effort. It solves the problem with resizing and quality handling.

## Setup

Add the saver to your service collection or straight up instantiate it like a bad boy.

```csharp
builder.Services.AddImageSaver();

var saver = new ImageSaver();
```


## Usage

Use the following interface:

```csharp
public interface IImageSaver
{
    Task Save(string path, Stream imageData);
    Task Save(string path, Stream imageData, IImageConfiguration config);
}
```

It will save a stream to a path like `/you/have/ligma.png`.

The awesome part is the `IImageConfiguration`. Checkout what properties this baby can fit in:

```csharp
public interface IImageConfiguration
{
    int Height { get; set; }
    int Width { get; set; }
    int MaxHeight { get; set; }
    int MaxWidth { get; set; }
    int Quality { get; set; }
}
```

The saver will try to match your set width/height properties. If you set only one dimension (width/height), the **aspect ratio will be preserved**!

The quality should be in the range 1-100 and is applied for formats that support it (jpeg, webm). 

```csharp
var saver = new ImageSaver();
await imageSaver.Save(imagePath, image, new ImageConfiguration()
{
    MaxWidth = 50,
    Quality = 80
});
```

## Under the hood

It uses [ImageSharp](https://sixlabors.com/products/imagesharp/) for all image operations, and that's it. 

Checkout the [project with tests](./src/MartinDrozdik.Services.ImageSaving.Tests).

