using System;
using System.Collections.Generic;
using System.Text;

namespace MartinDrozdik.Services.ImageSaving.Exceptions
{
    public class ImageSaverException : ImageProcessingException
    {
        public ImageSaverException(string message) : base(message)
        {
        }

        public ImageSaverException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ImageSaverException()
        {
        }
    }
}
