using Core.Entities;
using Core.Entities.QA;
using Core.Entities.Utilities;
using Core.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.QA.Models.ViewModels.Filmetric;

namespace WebSite.Areas.QA.Controllers
{
    public class FilmetricController : BaseController
    {
        // GET: QA/Filmetric
         public ActionResult New()
        { 
            NewViewModel model = new NewViewModel();
            try
            {                
                model.ProductList = new SelectList(vw_CatalogService.List4Select("QA_ProductFilmetrics", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.SubstractList = new SelectList(vw_CatalogService.List4Select("QA_SubstractsFilmetrics", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.DateFormat = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                model.BaseList = new SelectList(vw_CatalogService.List4Select("QA_BasesFilmetrics", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.MaterialList = new SelectList(vw_CatalogService.List4Select("QA_MaterialFilmetrics", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            }
            catch (Exception)
            {
                throw;
            }
            return View(model);
        }


        public ActionResult Index(int? ProductID)
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                model.BaseList = new SelectList(vw_CatalogService.List4Select("QA_BasesFilmetrics", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.SubstractList = new SelectList(vw_CatalogService.List4Select("QA_SubstractsFilmetrics", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.ProductList = new SelectList(vw_CatalogService.List4Select("QA_ProductFilmetrics", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.MaterialList = new SelectList(vw_CatalogService.List4Select("QA_MaterialFilmetrics", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.InspectionList = QA_FilmetricInspectionService.List(null, ProductID, null, null, null, null, null, null, BaseGenericRequest);
            }
            catch (Exception)
            {
                throw;
            }
            return View(model);
        }

        public ActionResult DetailInfo_List(int? detailID)
        {
            var model = QA_FilmetricInspectionService.DetailsList(detailID,null, BaseGenericRequest);
            return PartialView("_DetailInspectionTable",model);
        }

    

        //public JsonResult Search(int? ddl_F_Product, int? ddl_F_Base, int? ddl_F_Substract, DateTime txt_F_StartDay, DateTime txt_F_EndDay)
        //{
        //    List<FilmetricInspection> model = new List<FilmetricInspection>();
        //    GenericReturn result = new GenericReturn();
        //    string ViewPath = "~/Areas/QA/Views/Filmetric/_Tbl_Inspections.cshtml";
        //    try
        //    {
        //        model = QA_FilmetricInspectionService.List(null, ddl_F_Product, ddl_F_Substract, ddl_F_Base, null, null, txt_F_StartDay, txt_F_EndDay, BaseGenericRequest);
        //        result.ErrorCode = 0;
        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorCode = 99;
        //        result.ErrorMessage = e.Message;
        //    }
        //    return Json(new
        //    {
        //        result.ErrorCode,
        //        result.ErrorMessage,
        //        View = RenderRazorViewToString(ViewPath, model)
        //    }, JsonRequestBehavior.AllowGet);

        //}
        public ActionResult Create(int? ProductID, int? MaterialID, int? SubstractID, int? BaseID, int? AdditionID, int? LineID, int? UserID, decimal? HcValue, decimal? BcValue, decimal? PuValue, decimal? EuValue)
        {
            EditViewModel model = new EditViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/QA/Views/Filmetric/Edit.cshtml";
            try
            {
                result = QA_FilmetricInspectionService.Insert(ProductID,MaterialID ,SubstractID ,BaseID ,AdditionID ,LineID ,UserID,HcValue, BcValue, PuValue, EuValue, BaseGenericRequest);
                if (result.ErrorCode == 0)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(ViewPath, model);
        }

        public ActionResult Dashboard(int? ProductID) {

            IndexViewModel model = new IndexViewModel();
            try
            {
                model.BaseList = new SelectList(vw_CatalogService.List4Select("QA_BasesFilmetrics", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.SubstractList = new SelectList(vw_CatalogService.List4Select("QA_SubstractsFilmetrics", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.ProductList = new SelectList(vw_CatalogService.List4Select("QA_ProductFilmetrics", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.MaterialList = new SelectList(vw_CatalogService.List4Select("QA_MaterialFilmetrics", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
            }
            catch (Exception)
            {
                throw;
            }




            List<DataPoint> Hc_Max = new List<DataPoint>();
            List<DataPoint> Eu_Max = new List<DataPoint>();
            List<DataPoint> Pu_Max = new List<DataPoint>();
            List<DataPoint> Bc_Max = new List<DataPoint>();

            List<DataPoint> Hc_Min = new List<DataPoint>();
            List<DataPoint> Eu_Min = new List<DataPoint>();
            List<DataPoint> Pu_Min = new List<DataPoint>();
            List<DataPoint> Bc_Min = new List<DataPoint>();

            List<DataPoint> Hc_Record = new List<DataPoint>();
            List<DataPoint> Eu_Record = new List<DataPoint>();
            List<DataPoint> Pu_Record = new List<DataPoint>();
            List<DataPoint> Bc_Record = new List<DataPoint>();

            var registro = QA_FilmetricInspectionService.DetailsList(null,ProductID, BaseGenericRequest);

            foreach (var item in registro)
            {

                if (item.FilmName == "HC")
                {

                    Hc_Record.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)item.Value));
                }

                if (item.FilmName == "EU")
                {

                    Eu_Record.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)item.Value));
                }
                if (item.FilmName == "PU")
                {

                    Pu_Record.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)item.Value));
                }
                if (item.FilmName == "BC")
                {

                    Bc_Record.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)item.Value));
                }


            }




            if (ProductID != null && ProductID != 0)
            {
                var Limits = QA_FilmetricFilmLimitsService.FilmLimitsList(null, ProductID, null, null, null, BaseGenericRequest).FirstOrDefault();
                foreach (var item in registro)
                {

                    if (item.FilmName == "HC")
                    {

                        Hc_Max.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)Limits.MaxValue));
                        Hc_Min.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)Limits.MinValue));

                    }

                    if (item.FilmName == "EU")
                    {


                        Eu_Max.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)Limits.MaxValue));
                        Eu_Min.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)Limits.MinValue));
                    }
                    if (item.FilmName == "PU")
                    {


                        Pu_Max.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)Limits.MaxValue));
                        Pu_Min.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)Limits.MinValue));
                    }
                    if (item.FilmName == "BC")
                    {


                        Bc_Max.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)Limits.MaxValue));
                        Bc_Min.Add(new DataPoint(item.DateAdded.ToShortDateString(), (double)Limits.MinValue));
                    }


                }
            }
          
           
            ViewBag.Hc = JsonConvert.SerializeObject(Hc_Record.OrderBy(m => m.Label));
            ViewBag.Bc = JsonConvert.SerializeObject(Bc_Record.OrderBy(m => m.Label));
            ViewBag.Pu = JsonConvert.SerializeObject(Pu_Record.OrderBy(m => m.Label));
            ViewBag.Eu = JsonConvert.SerializeObject(Eu_Record.OrderBy(m => m.Label));

            ViewBag.HcMax = JsonConvert.SerializeObject(Hc_Max);
            ViewBag.BcMax = JsonConvert.SerializeObject(Bc_Max);
            ViewBag.PuMax = JsonConvert.SerializeObject(Pu_Max);
            ViewBag.EuMax = JsonConvert.SerializeObject(Eu_Max);

            ViewBag.HcMin = JsonConvert.SerializeObject(Hc_Min);
            ViewBag.BcMin = JsonConvert.SerializeObject(Bc_Min);
            ViewBag.PuMin = JsonConvert.SerializeObject(Pu_Min);
            ViewBag.EuMin = JsonConvert.SerializeObject(Eu_Min);

            return View(model);
        }

        //public ActionResult Edit(int OperationRecordID)
        //{
        //    EditViewModel model = new EditViewModel();
        //    int DFT_ReferenceTypeID;
        //    try
        //    {
        //        model.OperatorNumber = BaseGenericRequest.OperatorNumber;
        //        model.Operation = OperationRecordService.Get(OperationRecordID, BaseGenericRequest);
        //        model.Setup = OperationSetupService.Get(OperationRecordID, BaseGenericRequest);
        //        model.CavitiesNumber = model.Setup.MachineSetupCavities;
        //        var production = OperationProductionService.Get(OperationRecordID, BaseGenericRequest);
        //        if (production != null)
        //        {
        //            model.Production = production;
        //        }

        //        DFT_ReferenceTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemReferenceTypes", "PS", out string ErrorMessage);
        //        model.DownTimesList = DownTimeService.List(OperationRecordID, DFT_ReferenceTypeID, BaseGenericRequest);
        //        model.DepartmentsList = new SelectList(DepartmentService.List4Select(BaseGenericRequest), "DepartmentID", "DepartmentName");
        //        model.ReasonsList = new SelectList(vw_CatalogService.List4Select("DowntimeReasons", BaseGenericRequest), "CatalogDetailID", "DisplayText");
        //        model.DefectsList = new SelectList(vw_CatalogService.List4Select("ProductionGasketDefects", BaseGenericRequest), "CatalogDetailID", "DisplayText");
        //        model.RejectsList = new SelectList(vw_CatalogService.List4Select("ProductionGasketDefects", BaseGenericRequest), "CatalogDetailID", "DisplayText");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return View(model);
        //}

    }
}