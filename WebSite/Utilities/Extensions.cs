using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static WebSite.Models.StaticModels;

namespace WebSite.Utilities
{
    public static class Extensions
    {
        public static string CleanInvalidChar(this string Message)
        {
            if (Message.Length > 255) { Message = Message.Substring(0, 255); }
            Message = Message.Replace("'", "").Replace("\"", "");
            Message = Regex.Replace(Message, @"\\r\\n", " - ");
            Message = Message.Replace(System.Environment.NewLine, " - ");

            return Message;
        }
        public static AttachmentFile GetAttachmentFile(string filepath)
        {
            switch (Path.GetExtension(filepath).ToUpper())
            {
                case ".PNG":
                case ".JPG":
                case ".GIF":
                case ".JPEG":
                case ".ICO":
                    return AttachmentFile.Image;
                case ".XLS":
                case ".XLSX":
                case ".DOC":
                case ".DOCX":
                case ".PPT":
                case ".PPTX":
                case ".CSV":
                case ".RAR":
                case ".ZIP":
                    return AttachmentFile.File;
                default:
                    return AttachmentFile.Other;
            }
        }
        public static string GetVirtualPath4File(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {

                path = path.Substring((HttpContext.Current.Server.MapPath("\\")).Length);
                path = "/" + path.Replace("\\", "/");//se cambio ../ a / 
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