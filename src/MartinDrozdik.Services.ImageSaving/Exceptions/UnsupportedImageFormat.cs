using System;
using System.Collections.Generic;
using System.Text;

namespace MartinDrozdik.Services.ImageSaving.Exceptions
{
    public class UnsupportedImageFormat : ImageProcessingException
    {
        public UnsupportedImageFormat(string message) : base(message)
        {
        }

        public UnsupportedImageFormat(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnsupportedImageFormat()
        {
        }
    }
}
