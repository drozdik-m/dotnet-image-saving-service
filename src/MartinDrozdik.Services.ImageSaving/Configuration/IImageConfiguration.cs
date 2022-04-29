using System;
using System.Collections.Generic;
using System.Text;

namespace MartinDrozdik.Services.ImageSaving.Configuration
{
    public interface IImageConfiguration
    {
        /// <summary>
        /// Fixed height of the image [px]
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Fix width of the image [px]
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Maximum height of the image [px]
        /// </summary>
        int MaxHeight { get; set; }

        /// <summary>
        /// Maximum width of the image [px]
        /// </summary>
        int MaxWidth { get; set; }

        /// <summary>
        /// Quality of the image [%]
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        int Quality { get; set; }

    }
}
