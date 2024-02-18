#nullable disable warnings

/*
  Copyright 2017 Captive Reality Ltd

  Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is 
  hereby granted, provided that the above copyright notice and this permission notice appear in all copies.
  THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOFTWARE 
  INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE
  FOR ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM 
  LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, 
  ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
  
  Source: https://gist.github.com/markcastle/3cc99c8e5756c7e27532900a5f8a2a93

  Description: 
  Simple Class to detect / infer an image in pure C# type by looking at the first three or four bytes of the image.
  
  Either load a file up directly (ensuring that you have enough memory to read the whole file) and get the file type with:

          CaptiveReality.IO.Filesystem.ImageDetection.GetImageFormat(string fileName).ToString()

  ..or Pass a byte array (of at least 4 bytes) to..
 
          CaptiveReality.IO.Filesystem.ImageDetection.GetImageFormat(byte[] byteArray).ToString()
  
*/

using System.Linq;

namespace CaptiveReality.IO.Filesystem
{
    public class ImageDetection
    {
        public enum ImageFormat
        {
            BMP,
            JPEG,
            GIF,
            TIFF,
            PNG,
            UNKNOWN
        }

        /// <summary>
        /// If a file exists, Read all bytes from it 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] LoadFileRawBytes(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                return System.IO.File.ReadAllBytes(filePath);
            }
            else
                return null;
        }

        /// <summary>
        /// Load a file and determine what image type the file is
        /// This method needs to have enough free memory in order to load the whole file.
        /// Use other methods if you need more efficiency (eg load only the first four bytes of the file). - MC
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>ImageFormat</returns>
        public static ImageFormat GetImageFormat(string filePath)
        {
            byte[] fileBytes = LoadFileRawBytes(filePath);

            if (fileBytes == null)
            {
                return ImageFormat.UNKNOWN;
            }
            else
            {
                return GetImageFormat(fileBytes);
            }
        }

        /// <summary>
        /// Infer an Image type by looking at the first four bytes of a raw byte array
        /// Based on: https://stackoverflow.com/questions/210650/validate-image-from-file-in-c-sharp
        /// except that I already had the byte Array and didn't need the extra conversion to a stream
        /// I've checked bmp, gif, png, jpeg and tif but not tiff or jpeg2 (as I've no need for those) - MC
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns>ImageFormat</returns>
        public static ImageFormat GetImageFormat(byte[] byteArray)
        {
            const int INT_SIZE = 4; // We only need to check the first four bytes of the file / byte array.

            var bmp = System.Text.Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = System.Text.Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };                // PNG
            var tiff = new byte[] { 73, 73, 42 };                    // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };                   // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 };            // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 };           // jpeg2 (canon)

            // Copy the first 4 bytes into our buffer 
            var buffer = new byte[INT_SIZE];
            System.Buffer.BlockCopy(byteArray, 0, buffer, 0, INT_SIZE);

            if (bmp.SequenceEqual(buffer.Take(bmp.Length)))
                return ImageFormat.BMP;

            if (gif.SequenceEqual(buffer.Take(gif.Length)))
                return ImageFormat.GIF;

            if (png.SequenceEqual(buffer.Take(png.Length)))
                return ImageFormat.PNG;

            if (tiff.SequenceEqual(buffer.Take(tiff.Length)))
                return ImageFormat.TIFF;

            if (tiff2.SequenceEqual(buffer.Take(tiff2.Length)))
                return ImageFormat.TIFF;

            if (jpeg.SequenceEqual(buffer.Take(jpeg.Length)))
                return ImageFormat.JPEG;

            if (jpeg2.SequenceEqual(buffer.Take(jpeg2.Length)))
                return ImageFormat.JPEG;

            return ImageFormat.UNKNOWN;
        }
    }
}