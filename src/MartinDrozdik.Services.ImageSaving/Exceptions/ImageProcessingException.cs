using System;
using System.Collections.Generic;
using System.Text;

namespace MartinDrozdik.Services.ImageSaving.Exceptions
{
    public class ImageProcessingException : Exception
    {
        public ImageProcessingException()
        {
        }

        public ImageProcessingException(string message) : base(message)
        {
        }

        public ImageProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}
