# Image saving service for quick use

A service to save your images to disk with little effort. It solves the problem with resizing and quality handling.

(!!!) The code uses [ImageSharp](https://sixlabors.com/products/imagesharp/) – check your license and stuff.

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
    Task SaveAsync(string path, Stream imageData);
    Task SaveAsync(string path, Stream imageData, IImageConfiguration config);
    Task SaveAsync(Stream imageData, params (string path, IImageConfiguration config)[] targets);
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

If all config values are default or the file is of type .svg, the stream is straight up dumped into the file without any modifications.



## Under the hood

It uses [ImageSharp](https://sixlabors.com/products/imagesharp/) for all image operations, and that's it. 

Checkout the [project with tests](./src/MartinDrozdik.Services.ImageSaving.Tests).

