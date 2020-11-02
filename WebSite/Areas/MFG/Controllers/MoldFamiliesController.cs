using Core.Service;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.MoldFamilies;
using WebSite.Models;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace WebSite.Areas.MFG.Controllers
{
    public class MoldFamiliesController : BaseController
    {
        public ActionResult Index(string Message)
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                var MoldFamiliesNamesList = vw_CatalogService.List("LensMoldFamilies", BaseGenericRequest);
                MoldFamiliesNamesList.Insert(0, new Catalog() { CatalogDetailID = 0, ValueID = Resources.Common.TagAll });
                model.MoldFamiliesNamesList = new SelectList(MoldFamiliesNamesList, "CatalogDetailID", "ValueID");

                var LensTypesList = LensTypesService.List(BaseGenericRequest);
                LensTypesList.Insert(0, new LenType() { LensTypeID = 0, LensTypeName = Resources.Common.TagAll });
                model.LensTypesList = new SelectList(LensTypesList, "LensTypeID", "LensTypeName");

                model.MoldFamiliesRecordsList = MoldFamilyLensTypesService.List(BaseGenericRequest);

                if (!String.IsNullOrEmpty(Message))
                    model.Message =Message;

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                return View(model);
            }

            return View(model);
        }

        public ActionResult New()
        {
            EditViewModel model = new EditViewModel();
            string viewPath = "~/Areas/MFG/Views/MoldFamilies/_Mo_NewEdit.cshtml";

            try
            {
                model.Title = Resources.MFG.MoldFamilies.title_NewMoldFamily;
                model.LensTypesList = new SelectList(LensTypesService.List(BaseGenericRequest), "LensTypeID", "LensTypeName");
                model.MoldFamilyObj.Enabled = true;
                model.AuxiliarClass = "neweditRecord";
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                View = RenderRazorViewToString(viewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int MoldFamilyID)
        {
            EditViewModel model = new EditViewModel();
            string viewPath = "~/Areas/MFG/Views/MoldFamilies/_Mo_NewEdit.cshtml";

            try
            {
                int[] MoldFamilies = new int[] { MoldFamilyID };

                var MoldFamilyData = MoldFamilyLensTypesService.List(MoldFamilies, null, BaseGenericRequest).FirstOrDefault();
                model.IsEdit = true;
                model.AuxiliarClass = "neweditRecord";

                model.Title = Resources.MFG.MoldFamilies.title_NewMoldFamily;
                model.LensTypesList = new SelectList(LensTypesService.List(BaseGenericRequest), "LensTypeID", "LensTypeName");

                model.LensOfFamily = MoldFamilyLensTypesService.ListOfLens(MoldFamilyID, BaseGenericRequest);

                model.MoldFamilyObj.FamilyName = MoldFamilyData.FamilyName;
                model.MoldFamilyObj.Enabled = MoldFamilyData.Enabled;

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                View = RenderRazorViewToString(viewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(int[] MoldFamiliesID, int[] MoldLensTypesID)
        {
            List<MoldFamily> list = new List<MoldFamily>();
            string ViewPath = "~/Areas/MFG/Views/MoldFamilies/_Tbl_MoldFamiliesRecords.cshtml";

            try
            {
                list = MoldFamilyLensTypesService.List(MoldFamiliesID, MoldLensTypesID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                View = RenderRazorViewToString(ViewPath, list)
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadLensOfFamily(int MoldFamilyID, int?[] MoldLensTypeIDs)
        {
            EditViewModel model = new EditViewModel();
            string ViewPath = "~/Areas/MFG/Views/MoldFamilies/_Tbl_LenTypes.cshtml";

            try
            {
                model.LensOfFamily = MoldFamilyLensTypesService.ListOfLens(MoldFamilyID, MoldLensTypeIDs, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(int? MoldFamilyID, string MoldFamilyName, string LensTypeIDs, bool Enabled)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                if (MoldFamilyID == null)
                    result = MoldFamilyLensTypesService.Save(MoldFamilyName, LensTypeIDs, Enabled, BaseGenericRequest);
                else
                    result = MoldFamilyLensTypesService.Update(MoldFamilyID, MoldFamilyName, LensTypeIDs, Enabled, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult DeleteMoldFamily(int MoldFamilyID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = CatalogDetailService.Delete(MoldFamilyID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLenTypeFromFamily(int MoldFamilyLenTypeID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MoldFamilyLensTypesService.Delete(MoldFamilyLenTypeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMoldFamiliesList()
        {
            IEnumerable<SelectListItem> list = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                var MoldFamiliesNamesList = vw_CatalogService.List("LensMoldFamilies", BaseGenericRequest);
                MoldFamiliesNamesList.Insert(0, new Catalog() { CatalogDetailID = 0, ValueID = Resources.Common.TagAll });
                list = new SelectList(MoldFamiliesNamesList, "CatalogDetailID", "ValueID");


                if (list != null)
                {
                    if (list.Count() == 1)
                    {
                        var AuxList = new List<Catalog>();
                        AuxList.Insert(0, new Catalog() { CatalogDetailID = 0, ValueID = Resources.Common.TagAll });
                        list = new SelectList(AuxList, "CatalogDetailID", "ValueID");
                    }

                    result.ErrorCode = 0;
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                list
            }, JsonRequestBehavior.AllowGet);
        }


        #region Excel
        public ActionResult UploadExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {           
            IndexViewModel model = new IndexViewModel();
            GenericReturn result = new GenericReturn();

            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            var AuxiliarName = (int)t.TotalSeconds;

            DataTable dt = new DataTable();
            //Checking file content length and Extension must be .xlsx  
            if (file != null && file.ContentLength > 0 && Path.GetExtension(file.FileName).ToLower() == ".xlsx")
            {
                string path = Path.Combine(Server.MapPath("~/Files/Temp/MoldFamiliesExcelUpdate"), Path.GetFileName(file.FileName.ToLower().Replace(".xlsx", "_" + AuxiliarName + ".xlsx")));
                try
                {
                    //Saving the file  
                    file.SaveAs(path);
                    //Started reading the Excel file.  
                    using (XLWorkbook workbook = new XLWorkbook(path))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        bool FirstRow = true;
                        //Range for reading the cells based on the last cell used.  
                        string readRange = "1:1";
                        foreach (IXLRow row in worksheet.RowsUsed())
                        {
                            string DataValues = "";
                            if (FirstRow)
                            {
                                //El SP que se postea requiere estos campos porque es una variable tipo tabla
                                dt.Columns.Add("RowNumber");
                                dt.Columns.Add("FieldKey");
                                dt.Columns.Add("FieldValue");
                                FirstRow = false;
                            }
                            else
                            {
                                dt.Rows.Add();
                                int cellIndex = 0;
                                readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);

                                foreach (IXLCell cell in row.Cells(readRange))
                                {
                                    DataValues = DataValues + "," + cell.Value.ToString().Replace(",", "");
                                    cellIndex++;
                                }

                                //La variable tipo tabla no acepta valroes nulos, por eso se le pasa un numero cualquiera, para evitar errores
                                dt.Rows[dt.Rows.Count - 1][0] = 1;
                                dt.Rows[dt.Rows.Count - 1][1] = 1;
                                dt.Rows[dt.Rows.Count - 1][2] = DataValues.Remove(0, 1).Trim();

                            }
                        }


                        //If no data in Excel file  
                        if (FirstRow)
                        {
                            result.ErrorMessage = "Empty Excel File!";
                        }
                    }

                    System.IO.File.Delete(path);
                    result = MoldFamilyLensTypesService.SaveExcelImport(dt, BaseGenericRequest);
                }
                catch (Exception ex)
                {
                    result.ErrorCode = 99;
                    result.ErrorMessage = ex.ToString();
                }
            }
            else
            {
                //If file extension of the uploaded file is different then .xlsx  
                result.ErrorMessage = "Please select file with .xlsx extension!";
            }

            return RedirectToAction("Index", "MoldFamilies", new { Message = result.ErrorMessage });
        }

        #endregion Excel

    }
}