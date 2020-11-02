//using CrystalDecisions.CrystalReports.Engine;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using QRCoder;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Microsoft.Azure.NotificationHubs;
using Core.Entities;
using Core.Services;
using System.Collections.Generic;
using System.Configuration;
using Core.Services;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Core.Service;
using Microsoft.Ajax.Utilities;
//using Core.Utilities;

namespace WebSite.Utilities
{
    public class Tools
    {
        public struct Catalogs
        {
            public struct FieldsTypes
            {
                public const char Image = 'I';
                public const char Details = 'D';
                public const char Header = 'S';
            }
        }

        public static BaseFont GetFont(string FontName)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            FontName = FontName.ToUpper();

            if (FontName == "ARIAL_BOLD" || FontName == "COURIER_BOLD")
                return BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "ARIAL_OBLIQUE" || FontName == "COURIER_OBLIQUE")
                return BaseFont.CreateFont(BaseFont.COURIER_OBLIQUE, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "ARIAL_BOLDOBLIQUE" || FontName == "COURIER_BOLDOBLIQUE")
                return BaseFont.CreateFont(BaseFont.COURIER_BOLDOBLIQUE, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "HELVETICA")
                return BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "HELVETICA_BOLD")
                return BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "HELVETICA_OBLIQUE")
                return BaseFont.CreateFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "HELVETICA_BOLDOBLIQUE")
                return BaseFont.CreateFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "SYMBOL")
                return BaseFont.CreateFont(BaseFont.SYMBOL, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "TIMES_ROMAN")
                return BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "TIMES_BOLD")
                return BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "TIMES_ITALIC")
                return BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "TIMES_BOLDITALIC")
                return BaseFont.CreateFont(BaseFont.TIMES_BOLDITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            else if (FontName == "ZAPFDINGBATS")
                return BaseFont.CreateFont(BaseFont.ZAPFDINGBATS, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            return bf;
        }

        public static BaseColor GetBaseColor(string ColorName)
        {
            BaseColor color = BaseColor.BLACK;

            switch (ColorName.ToUpper())
            {
                case "BLUE":
                    return BaseColor.BLUE;
                case "CYAN":
                    return BaseColor.CYAN;
                case "DARK_GRAY":
                    return BaseColor.DARK_GRAY;
                case "GRAY":
                    return BaseColor.GRAY;
                case "GREEN":
                    return BaseColor.GREEN;
                case "LIGHT_GRAY":
                    return BaseColor.LIGHT_GRAY;
                case "MAGENTA":
                    return BaseColor.MAGENTA;
                case "ORANGE":
                    return BaseColor.ORANGE;
                case "PINK":
                    return BaseColor.PINK;
                case "RED":
                    return BaseColor.RED;
                case "WHITE":
                    return BaseColor.WHITE;
                case "YELLOW":
                    return BaseColor.YELLOW;
            }

            return color;
        }

        public static string GeneratePDFPath(string TemplateName)
        {
            string pdfFilePath = HttpContext.Current.Server.MapPath("~/");
            return pdfFilePath + TemplateName.Replace("..","");
        }

        public static string GeneratePDFReport(int ReportID, string TemplateName, string SQLStoredProcedure, string ReportFileID, string OutputPath,
                                                            string ReportValue, int? ReferenceID, string Parameters, string CultureID, int FacilityID, int UserID,
                                                            string LogoURL, out bool Result)
        {
            string pdfFilePath = HttpContext.Current.Server.MapPath("~/Content/FilesTemplates/");
            string pdfFilePathResult = HttpContext.Current.Server.MapPath(OutputPath + ReportValue + ".pdf");
            string rptFilePath = HttpContext.Current.Server.MapPath("~/Reports");
            string ServerOutputPath = HttpContext.Current.Server.MapPath(OutputPath);

            //Path.Combine(rptFilePath, TemplateName)

            //distinguir de PDF y RPT
            if (TemplateName.Contains("pdf"))
            {
                try
                {
                    if (!Directory.Exists(ServerOutputPath))
                        Directory.CreateDirectory(ServerOutputPath);



                    //llamar funcion, que llama sp para llenar los datos                 
                    var dt = MiscellaneousService.eReq_GetReportData(SQLStoredProcedure, ReferenceID, ReportFileID, FacilityID, UserID, CultureID, Parameters);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (File.Exists(pdfFilePathResult))
                            File.Delete(pdfFilePathResult);

                        using (Stream inputPdfStream = new FileStream(pdfFilePath + TemplateName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (Stream outputPdfStream = new FileStream(pdfFilePathResult, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            var reader = new PdfReader(inputPdfStream);
                            var stamper = new PdfStamper(reader, outputPdfStream);
                            var pdfContentByte = stamper.GetOverContent(1);

                            // Texto Para el archivo
                            foreach (DataRow row in dt.Rows)
                            {
                                int PositionX, PositionY;
                                float FontSize;
                                string FontName;
                                string TextValue;

                                TextValue = row["FieldValue"].ToString();
                                FontName = row["FontName"].ToString();
                                PositionX = Convert.ToInt32(row["PositionX"].ToString());
                                PositionY = Convert.ToInt32(row["PositionY"].ToString());
                                float.TryParse(row["FontSize"].ToString(), out FontSize);

                                // we tell the ContentByte we're ready to draw text
                                pdfContentByte.BeginText();

                                // we draw some text on a certain position
                                BaseFont bf = GetFont(FontName); //BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                pdfContentByte.SetFontAndSize(bf, FontSize);
                                pdfContentByte.SetTextMatrix(PositionX, PositionY);
                                pdfContentByte.SetColorFill(GetBaseColor(row["FontColor"].ToString()));

                                pdfContentByte.ShowText(TextValue);

                                // we tell the contentByte, we've finished drawing text
                                pdfContentByte.EndText();

                            } // For Each row from DataTable


                            stamper.Close();
                        }
                    }
                    Result = true;
                    return pdfFilePathResult;
                }
                catch (Exception ex)
                {
                    Result = false;
                    return ex.Message;
                }
            }
            else if (TemplateName.Contains("rpt"))
            {
             
            }
            Result = false;
            return Resources.Common.ntf_UnknowTemplate;
        }

        public static string ReportPasedeSalida(int ReportID, string TemplateName, string SQLStoredProcedure, string OutputPath,
                                                            string ReportValue, int? ReferenceID, string Parameters, string CultureID, int FacilityID, int UserID,
                                                            string LogoURL)
        {
            DataSet ds = new DataSet();
            int numDoctos = 0;

            string[] files;
            string fileNameAppends = "";
            string pdfFilePath = HttpContext.Current.Server.MapPath("~/Content/FilesTemplates/");
            string pdfFilePathResult = "";
            float FontSize;
            PDFFilesDetail entity;



            var da =  MiscellaneousService.eReq_GetReportDataDetail(SQLStoredProcedure, ReferenceID, FacilityID, UserID, CultureID, Parameters);
            try
            {
              

                //ds = da.GetTables();

                if (ds != null && ds.Tables.Count > 0)
                {
                    numDoctos = ds.Tables.Count;

                    // Headers
                    using (DataTable dt = ds.Tables[0])
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 1; i < numDoctos; i++)
                            {
                                pdfFilePathResult = HttpContext.Current.Server.MapPath(String.Format("~/Files/Temp/PasedeSalida_{0}_{1}.pdf", i, Guid.NewGuid()));
                                fileNameAppends += pdfFilePathResult + ",";

                                using (Stream inputPdfStream = new FileStream(pdfFilePath + TemplateName, FileMode.Open, FileAccess.Read, FileShare.Read))
                                using (Stream outputPdfStream = new FileStream(pdfFilePathResult, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    var reader = new PdfReader(inputPdfStream);
                                    var stamper = new PdfStamper(reader, outputPdfStream);
                                    var pdfContentByte = stamper.GetOverContent(1);

                                    foreach (DataRow row in dt.Rows)
                                    {
                                        FontSize = 0;
                                        float.TryParse(row["FontSize"].ToString(), out FontSize);
                                        entity = new PDFFilesDetail();

                                        entity.FieldValue = row["FieldValue"].ToString();
                                        entity.FontName = row["FontName"].ToString();
                                        entity.PositionX = Convert.ToInt32(row["PositionX"].ToString());
                                        entity.PositionY = Convert.ToInt32(row["PositionY"].ToString());
                                        entity.FontSize = FontSize;
                                        entity.FontColor = row["FontColor"].ToString();

                                        // we tell the ContentByte we're ready to draw text
                                        pdfContentByte.BeginText();

                                        // we draw some text on a certain position
                                        BaseFont bf = GetFont(entity.FontName); //BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                        pdfContentByte.SetFontAndSize(bf, entity.FontSize);
                                        pdfContentByte.SetTextMatrix(entity.PositionX, entity.PositionY);
                                        pdfContentByte.SetColorFill(GetBaseColor(entity.FontColor));
                                        pdfContentByte.ShowText(entity.FieldValue);

                                        // we tell the contentByte, we've finished drawing text
                                        pdfContentByte.EndText();
                                    } // Fin foreach

                                    using (DataTable dt2 = ds.Tables[i])
                                    {
                                        if (dt2 != null && dt2.Rows.Count > 0)
                                        {
                                            foreach (DataRow row in dt2.Rows)
                                            {
                                                FontSize = 0;
                                                float.TryParse(row["FontSize"].ToString(), out FontSize);
                                                entity = new PDFFilesDetail();

                                                entity.FieldValue = row["FieldValue"].ToString();
                                                entity.FontName = row["FontName"].ToString();
                                                entity.PositionX = Convert.ToInt32(row["PositionX"].ToString());
                                                entity.PositionY = Convert.ToInt32(row["PositionY"].ToString());
                                                entity.FontSize = FontSize;
                                                entity.FontColor = row["FontColor"].ToString();
                                                entity.FieldName = row["FieldName"].ToString();

                                                // we tell the ContentByte we're ready to draw text
                                                pdfContentByte.BeginText();


                                                // we draw some text on a certain position
                                                BaseFont bf = GetFont(entity.FontName); //BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                                pdfContentByte.SetFontAndSize(bf, entity.FontSize);
                                                pdfContentByte.SetColorFill(GetBaseColor(entity.FontColor));


                                                pdfContentByte.SetTextMatrix(entity.PositionX, entity.PositionY);
                                                pdfContentByte.ShowText(entity.FieldValue);
                                                // we tell the contentByte, we've finished drawing text
                                                pdfContentByte.EndText();
                                            } // Fin foreach
                                        }
                                    }
                                    stamper.Close();
                                }

                            } // fin for (Documentos)

                            if (!string.IsNullOrEmpty(fileNameAppends))
                            {
                                fileNameAppends = fileNameAppends.TrimEnd(',');
                                files = fileNameAppends.Split(',');
                                pdfFilePathResult = HttpContext.Current.Server.MapPath(String.Format("~/Files/temp/PasedeSalida_{0:yyyyMMdd}.pdf",DateTime.Now, Guid.NewGuid()));
                                Tools.MergeFiles(pdfFilePathResult, files);
                            }
                        } // fin dt 
                    } // fin using
                }
            }
            finally
            {
                ds = null;
                da = null;
            }

            return pdfFilePathResult;
        }
        private static byte[] GetByteImage(string path)
        {
            string serverpath = HttpContext.Current.Server.MapPath(path);
            if (File.Exists(serverpath))
            {
                using (FileStream FilStr = new FileStream(serverpath, FileMode.Open))
                {
                    BinaryReader BinRed = new BinaryReader(FilStr);
                    return BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                }
            }
            return new byte[0];
        }
        private static string GetQRUrl(string xmlpath)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(xmlpath);
            string UUID = "", fe = "", QRurl = "";
            DataRow e = ds.Tables["Emisor"].Rows[0];
            DataRow r = ds.Tables["Receptor"].Rows[0];
            DataRow c = ds.Tables["Comprobante"].Rows[0];
            DataRow tfd = ds.Tables["TimbreFiscalDigital"].Rows[0];
            if (tfd["UUID"].ToString() != "")
            {
                tfd = ds.Tables["TimbreFiscalDigital"].Rows[0];
                UUID = tfd["UUID"].ToString();
                fe = c["Sello"].ToString().Substring(c["Sello"].ToString().Length - 8); //ultimos 8 caracteres
                QRurl = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?&id=" + UUID + "&re=" + e["Rfc"].ToString() + "&rr=" + r["Rfc"].ToString() + "&tt=" + c["Total"].ToString() + "&fe=" + fe;
            }
            return QRurl;
        }

        public static string ConvertTotaltoWords(object NUMERO, string Moneda, string MonedaAbrv)
        {
            var TEXTO = "";
            var MILLONES = "";
            var MILES = "";
            var CIENTOS = "";
            var DECIMALES = "";
            var CADENA = "";
            var CADMILLONES = "";
            var CADMILES = "";
            var CADCIENTOS = "";
            TEXTO = String.Format("{0,14:0,0.00}", NUMERO);
            MILLONES = TEXTO.Substring(0, 3);
            MILES = TEXTO.Substring(4, 3);
            CIENTOS = TEXTO.Substring(8, 3);
            DECIMALES = TEXTO.Substring(12, 2);

            CADMILLONES = ConvertNumber(MILLONES, true);
            CADMILES = ConvertNumber(MILES, true);
            CADCIENTOS = ConvertNumber(CIENTOS, false);


            if (CADMILLONES == "  ")
            {
                CADMILLONES = CADMILLONES.Replace(" ", "");
            }
            if (CADMILES == "  ")
            {
                CADMILES = CADMILES.Replace(" ", "");
            }
            if (CADCIENTOS == "  ")
            {
                CADCIENTOS = CADCIENTOS.Replace(" ", "");
            }

            if (CADMILLONES != "")
            {
                if (CADMILLONES.Trim() == "UN")
                    CADENA = CADMILLONES + " MILLON";
                else
                    CADENA = CADMILLONES + " MILLONES";
            }


            if (CADMILES != "")
                CADENA = CADENA + " " + CADMILES + " MIL";


            if ((CADMILES + CADCIENTOS) == "UN")
                CADENA = CADENA + "UNO " + Moneda + " " + DECIMALES + "/100" + " M.N. ";
            else if (MILES + CIENTOS == "000000")
                CADENA = CADENA + " " + CADCIENTOS + " " + Moneda + " " + DECIMALES + "/100" + " " + MonedaAbrv + " ";
            else
                CADENA = CADENA + " " + CADCIENTOS + " " + Moneda + " " + DECIMALES + "/100" + " " + MonedaAbrv + " ";

            return CADENA;
        }

        public static string ConvertNumber(string TEXTO, bool SW)
        {
            var CENTENA = "";
            var DECENA = "";
            var UNIDAD = "";
            var TXTCENTENA = "";
            var TXTDECENA = "";
            var TXTUNIDAD = "";
            CENTENA = TEXTO.Substring(0, 1);
            DECENA = TEXTO.Substring(1, 1);
            UNIDAD = TEXTO.Substring(2, 1);
            switch (CENTENA)
            {
                case "1":
                    {
                        TXTCENTENA = "CIEN";
                        if (DECENA + UNIDAD != "00")
                            TXTCENTENA = "CIENTO";
                        break;
                    }

                case "2":
                    {
                        TXTCENTENA = "DOSCIENTOS";
                        break;
                    }

                case "3":
                    {
                        TXTCENTENA = "TRESCIENTOS";
                        break;
                    }

                case "4":
                    {
                        TXTCENTENA = "CUATROCIENTOS";
                        break;
                    }

                case "5":
                    {
                        TXTCENTENA = "QUINIENTOS";
                        break;
                    }

                case "6":
                    {
                        TXTCENTENA = "SEISCIENTOS";
                        break;
                    }

                case "7":
                    {
                        TXTCENTENA = "SETECIENTOS";
                        break;
                    }

                case "8":
                    {
                        TXTCENTENA = "OCHOCIENTOS";
                        break;
                    }

                case "9":
                    {
                        TXTCENTENA = "NOVECIENTOS";
                        break;
                    }
            }

            switch (DECENA)
            {
                case "1":
                    {
                        TXTDECENA = "DIEZ";
                        switch (UNIDAD)
                        {
                            case "1":
                                {
                                    TXTDECENA = "ONCE";
                                    break;
                                }

                            case "2":
                                {
                                    TXTDECENA = "DOCE";
                                    break;
                                }

                            case "3":
                                {
                                    TXTDECENA = "TRECE";
                                    break;
                                }

                            case "4":
                                {
                                    TXTDECENA = "CATORCE";
                                    break;
                                }

                            case "5":
                                {
                                    TXTDECENA = "QUINCE";
                                    break;
                                }

                            case "6":
                                {
                                    TXTDECENA = "DIECISEIS";
                                    break;
                                }

                            case "7":
                                {
                                    TXTDECENA = "DIECISIETE";
                                    break;
                                }

                            case "8":
                                {
                                    TXTDECENA = "DIECIOCHO";
                                    break;
                                }

                            case "9":
                                {
                                    TXTDECENA = "DIECINUEVE";
                                    break;
                                }
                        }

                        break;
                    }

                case "2":
                    {
                        TXTDECENA = "VEINTE";
                        if (UNIDAD != "0")
                            TXTDECENA = "VEINTI";
                        break;
                    }

                case "3":
                    {
                        TXTDECENA = "TREINTA";
                        if (UNIDAD != "0")
                            TXTDECENA = "TREINTA Y ";
                        break;
                    }

                case "4":
                    {
                        TXTDECENA = "CUARENTA";
                        if (UNIDAD != "0")
                            TXTDECENA = "CUARENTA Y ";
                        break;
                    }

                case "5":
                    {
                        TXTDECENA = "CINCUENTA";
                        if (UNIDAD != "0")
                            TXTDECENA = "CINCUENTA Y ";
                        break;
                    }

                case "6":
                    {
                        TXTDECENA = "SESENTA";

                        if (UNIDAD != "0")
                            TXTDECENA = "SESENTA Y ";
                        break;
                    }

                case "7":
                    {
                        TXTDECENA = "SETENTA";
                        if (UNIDAD != "0")
                            TXTDECENA = "SETENTA Y ";
                        break;
                    }

                case "8":
                    {
                        TXTDECENA = "OCHENTA";
                        if (UNIDAD != "0")
                            TXTDECENA = "OCHENTA Y ";
                        break;
                    }

                case "9":
                    {
                        TXTDECENA = "NOVENTA";
                        if (UNIDAD != "0")
                            TXTDECENA = "NOVENTA Y ";
                        break;
                    }
            }

            if (DECENA != "1")
            {
                switch (UNIDAD)
                {
                    case "0":
                        {
                            //TXTUNIDAD = "CERO";
                            if (SW)
                                TXTUNIDAD = "CERO";
                            else
                                TXTUNIDAD = "";
                            break;
                        }
                    case "1":
                        {
                            if (SW)
                                TXTUNIDAD = "UN";
                            else
                                TXTUNIDAD = "UNO";
                            break;
                        }

                    case "2":
                        {
                            TXTUNIDAD = "DOS";
                            break;
                        }

                    case "3":
                        {
                            TXTUNIDAD = "TRES";
                            break;
                        }

                    case "4":
                        {
                            TXTUNIDAD = "CUATRO";
                            break;
                        }

                    case "5":
                        {
                            TXTUNIDAD = "CINCO";
                            break;
                        }

                    case "6":
                        {
                            TXTUNIDAD = "SEIS";
                            break;
                        }

                    case "7":
                        {
                            TXTUNIDAD = "SIETE";
                            break;
                        }

                    case "8":
                        {
                            TXTUNIDAD = "OCHO";
                            break;
                        }

                    case "9":
                        {
                            TXTUNIDAD = "NUEVE";
                            break;
                        }
                }
            }

            return TXTCENTENA + " " + TXTDECENA + " " + TXTUNIDAD;
        }

        public static string GetUUID(string XMLString)
        {
            System.Xml.Linq.XNamespace tfd = @"http://www.sat.gob.mx/TimbreFiscalDigital";
            var xdoc = System.Xml.Linq.XDocument.Parse(XMLString);
            var elt = xdoc.Descendants(tfd + "TimbreFiscalDigital").First();

            return (string)elt.Attribute("UUID");
        }

        public static string SaveStampedInvoice(string ServerOutputPath, string XMLFilePath, string XMLString, out bool Result)
        {
            try
            {
                var XDocumentnewXML = System.Xml.Linq.XDocument.Parse(XMLString);
                XDocumentnewXML.Declaration = new System.Xml.Linq.XDeclaration("1.0", "utf-8", "yes");

                if (!Directory.Exists(ServerOutputPath))
                    Directory.CreateDirectory(ServerOutputPath);

                XDocumentnewXML.Save(ServerOutputPath + XMLFilePath);
                Result = true;
                return ServerOutputPath + XMLFilePath;
            }
            catch (Exception ex)
            {
                Result = false;
                return ex.Message;
            }
        }

        public static byte[] ConvertFile(string originalFileName, int NewWidth)
        {
            FileInfo fiFile = new FileInfo(originalFileName);

            if ((fiFile.Extension.ToLower() == ".jpg") ||
                (fiFile.Extension.ToLower() == ".jpeg") ||
                (fiFile.Extension.ToLower() == ".png") ||
                (fiFile.Extension.ToLower() == ".gif") ||
                (fiFile.Extension.ToLower() == ".bmp"))
            {
                ResizeAndSave(originalFileName, "", new FileStream(originalFileName, FileMode.Open), 1, false);
                return null;
                //return ResizeImage(originalFileName, 1200);
            }
            else
            {
                FileStream fs = default(FileStream);
                fs = new FileStream(originalFileName, FileMode.Open);
                BinaryReader brReader = new BinaryReader(fs);
                long byteLength = new FileInfo(originalFileName).Length;
                byte[] byteFile = brReader.ReadBytes((Int32)byteLength);
                fs.Close();
                fs.Dispose();
                brReader.Close();
                return byteFile;
            }
        }

        private static byte[] ResizeImage(string originalFileName, int NewWidth)
        {
            string newFileName = null;
            Bitmap tmpImage = default(Bitmap);
            Bitmap newImage = default(Bitmap);
            Graphics g = default(Graphics);
            int newHeight = 0;
            FileStream fs = default(FileStream);
            if (File.Exists(originalFileName))
            {
                try
                {
                    fs = new FileStream(originalFileName, FileMode.Open);
                    tmpImage = (Bitmap)Bitmap.FromStream(fs);
                    fs.Close();

                    newHeight = (NewWidth * tmpImage.Height) / tmpImage.Width;
                    newImage = new Bitmap(NewWidth, newHeight);

                    g = Graphics.FromImage(newImage);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    g.DrawImage(tmpImage, 0, 0, NewWidth, newHeight);
                    g.Dispose();

                    newFileName = "tmp" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".jpg";
                    newImage.Save(HttpContext.Current.Server.MapPath(newFileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                    newImage.Dispose();
                    tmpImage.Dispose();
                    tmpImage = null;
                    newImage = null;
                    g = null;

                    fs = new FileStream(HttpContext.Current.Server.MapPath(newFileName), FileMode.Open);
                    BinaryReader brReader = new BinaryReader(fs);
                    long byteLength = new FileInfo(HttpContext.Current.Server.MapPath(newFileName)).Length;
                    byte[] byteFile = brReader.ReadBytes((Int32)byteLength);
                    fs.Close();
                    fs.Dispose();
                    brReader.Close();
                    File.Delete(HttpContext.Current.Server.MapPath(newFileName));
                    return byteFile;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else { return null; }
        }

        public static void ResizeAndSave(string savePath, string fileName, Stream imageBuffer, int maxSideSize, bool makeItSquare)
        {
            int newWidth;
            int newHeight;
            System.Drawing.Image image = System.Drawing.Image.FromStream(imageBuffer);
            int oldWidth = image.Width;
            int oldHeight = image.Height;
            Bitmap newImage;
            if (makeItSquare)
            {
                int smallerSide = oldWidth >= oldHeight ? oldHeight : oldWidth;
                double coeficient = maxSideSize / (double)smallerSide;
                newWidth = Convert.ToInt32(coeficient * oldWidth);
                newHeight = Convert.ToInt32(coeficient * oldHeight);
                Bitmap tempImage = new Bitmap(image, newWidth, newHeight);
                int cropX = (newWidth - maxSideSize) / 2;
                int cropY = (newHeight - maxSideSize) / 2;
                newImage = new Bitmap(maxSideSize, maxSideSize);
                Graphics tempGraphic = Graphics.FromImage(newImage);
                tempGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                tempGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                tempGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                tempGraphic.DrawImage(tempImage, new System.Drawing.Rectangle(0, 0, maxSideSize, maxSideSize), cropX, cropY, maxSideSize, maxSideSize, GraphicsUnit.Pixel);
            }
            else
            {
                int maxSide = oldWidth >= oldHeight ? oldWidth : oldHeight;

                if (maxSide > maxSideSize)
                {
                    double coeficient = maxSideSize / (double)maxSide;
                    newWidth = Convert.ToInt32(coeficient * oldWidth);
                    newHeight = Convert.ToInt32(coeficient * oldHeight);
                }
                else
                {
                    newWidth = oldWidth;
                    newHeight = oldHeight;
                }
                newImage = new Bitmap(image, newWidth, newHeight);
            }
            newImage.Save(savePath + fileName + ".jpg", ImageFormat.Jpeg);
            image.Dispose();
            newImage.Dispose();
        }

        public static void MergeFiles(string destinationFile, string[] sourceFiles)
        {
            try
            {
                int f = 0;
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(sourceFiles[f]);
                // we retrieve the total number of pages
                int n = reader.NumberOfPages;
                //Console.WriteLine("There are " + n + " pages in the original file.");
                // step 1: creation of a document-object
                using (Document document = new Document(reader.GetPageSizeWithRotation(1)))
                {
                    // step 2: we create a writer that listens to the document
                    using (PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create)))
                    {
                        // step 3: we open the document
                        document.Open();
                        PdfContentByte cb = writer.DirectContent;
                        PdfImportedPage page;
                        int rotation;
                        // step 4: we add content
                        while (f < sourceFiles.Length)
                        {
                            int i = 0;
                            while (i < n)
                            {
                                i++;
                                document.SetPageSize(reader.GetPageSizeWithRotation(i));
                                document.NewPage();
                                page = writer.GetImportedPage(reader, i);
                                rotation = reader.GetPageRotation(i);
                                if (rotation == 90 || rotation == 270)
                                {
                                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                                }
                                else
                                {
                                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                                }
                                //Console.WriteLine("Processed page " + i);
                            }
                            f++;
                            if (f < sourceFiles.Length)
                            {
                                reader = new PdfReader(sourceFiles[f]);
                                // we retrieve the total number of pages
                                n = reader.NumberOfPages;
                                //Console.WriteLine("There are " + n + " pages in the original file.");
                            }
                        }
                    }
                    // step 5: we close the document
                    document.Close();
                }
            }
            catch (Exception e)
            {
                string strOb = e.Message;
            }
        }

    }
}