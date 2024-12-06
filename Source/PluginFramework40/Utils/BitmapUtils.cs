//=============================================================================
//D The BitmapUtils class.
//
// ----------------------------------------------------------------------------
// Copyright 2011 Delcam plc., Birmingham, UK
// ----------------------------------------------------------------------------
//
// History.
// DICC  Who When     What
// ----- --- -------- ---------------------------------------------------------
// 93978 PSL 10/11/11 Written.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Delcam.Plugins.Utils
{
    //=============================================================================
    /// <summary>
    /// A utility class to aid with converting bitmap resources to a pixel array.
    /// </summary>
    //
    // History.
    // DICC  Who When     What
    // ----- --- -------- ---------------------------------------------------------
    // 93978 PSL 10/11/11 Written.
    //-----------------------------------------------------------------------------
    public static class BitmapUtils
    {
        //=============================================================================
        /// <summary>
        /// A static function to take an assembly name and bitmap resource path, load the bitmap resource, and copy the underlying data into a pixel array.
        /// </summary>
        /// <param name="assemblyName">The name of the executing assembly.</param>
        /// <param name="bitmap_resource_path">The resource path of the image.</param>
        /// <param name="pFormat">The pixel format, i.e. order of RGB pixels and possibly alpha channel.</param>
        /// <param name="pixels">The byte array.</param>
        /// <param name="width">The width of the icon, which should be 32 pixels.</param>
        /// <param name="height">The height of the icon, which should be 32 pixels.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static void get_bitmap(string assemblyName, string bitmap_resource_path, out PowerMILL.PluginBitmapFormat pFormat, out byte[] pixels, out int width, out int height)
        {
            // Assume things won't work out
            pFormat = PowerMILL.PluginBitmapFormat.PMILL_BITMAP_FORMAT_FAIL;
            pixels = null;
            width = 0;
            height = 0;

            // Check the resource path
            if (bitmap_resource_path.Length == 0)
            {
                return;
            }

            try
            {
                BitmapImage bm = new BitmapImage();
                bm.BeginInit();
                string pack = "pack://application:,,,/" + assemblyName + ";component/" + bitmap_resource_path;
                bm.UriSource = new Uri(pack);
                bm.EndInit();
        
                PixelFormat pf = bm.Format;
                if (pf == PixelFormats.Bgr24)
                {
                    pFormat = PowerMILL.PluginBitmapFormat.PMILL_BITMAP_FORMAT_BGR24;
                }
                else if (pf == PixelFormats.Bgra32)
                {
                    pFormat = PowerMILL.PluginBitmapFormat.PMILL_BITMAP_FORMAT_BGRA32;
                }
                else if (pf == PixelFormats.Rgb24)
                {
                    pFormat = PowerMILL.PluginBitmapFormat.PMILL_BITMAP_FORMAT_RGB24;
                }
                else
                {
                    //MessageBox.Show("Failed due to format: " + pf.ToString());
                    return;
                }

                int bits_per_pixel = pf.BitsPerPixel;
                int stride = bm.PixelWidth * (bits_per_pixel / 8);

                if ((bits_per_pixel != (3 * 8) && bits_per_pixel != (4 * 8)))
                {
                    //MessageBox.Show("Failed due to bits per pixel: " + bits_per_pixel);
                    return;
                }

                // Set the width and height
                width = bm.PixelWidth;
                height = bm.PixelHeight;

                // Create the pixel array
                pixels = new byte[width * height * (bits_per_pixel / 8)];

                //MessageBox.Show("Bitmap OK");
                bm.CopyPixels(pixels, stride, 0);
            }
            catch (Exception)
            {
                pFormat = PowerMILL.PluginBitmapFormat.PMILL_BITMAP_FORMAT_FAIL;
                width = 0;
                height = 0;
                pixels = null;
            }
        }  
    }
}
