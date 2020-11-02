using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Utilities
{
    public class Wrappers
    {
        public static string GetVirtualPath4File(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                path = path.Substring((HttpContext.Current.Server.MapPath("\\")).Length);
                path = "../../" + path.Replace("\\", "/");
            }

            return path;
        }

        public static string GetOrientationImage(string fileName)
        {
            //reference https://www.daveperrett.com/articles/2012/07/28/exif-orientation-handling-is-a-ghetto/
            int exifOrientationID = 0x112;
            string EXIFOrientationCss = "";
            //string type = "1";
            try
            {
                using (var image = System.Drawing.Image.FromFile(fileName))
                {
                    var prop = image.GetPropertyItem(exifOrientationID);
                    if (prop != null)
                    {
                        int val = BitConverter.ToUInt16(prop.Value, 0);
                        //if (val != 1)
                        EXIFOrientationCss = "EXIFOrientation" + val.ToString();
                        //else if (image.Width < image.Height)
                        //    EXIFOrientationCss = "EXIFOrientation1";
                        //else if (image.Width > image.Height)
                        //    EXIFOrientationCss = "EXIFOrientation6"; // "landscape";
                    }
                    else
                    {
                        if (image.Width > image.Height)
                            EXIFOrientationCss = "EXIFOrientation6"; // "landscape";
                        else if (image.Width < image.Height)
                            EXIFOrientationCss = "EXIFOrientation1"; // "portrait";
                    }
                }

            }
            catch (Exception)
            {

            }

            return EXIFOrientationCss;
        }
    }
}