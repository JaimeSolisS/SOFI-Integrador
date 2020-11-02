using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.MachineSetup
{
    public class ModalNewMachineViewModel
    {
        public IEnumerable<SelectListItem> ProductionLineList { get; set; }
        public IEnumerable<SelectListItem> MachineCategoriesList { get; set; }
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public string MachineDescrption { get; set; }
        public int ProductionLineID { get; set; }
        public bool Enabled { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }

        public int TempAttachmentID
        {
            get
            {
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                return (int)t.TotalSeconds;
            }
        }


        public ModalNewMachineViewModel()
        {
            ProductionLineList = new SelectList(new List<SelectListItem>());
            MachineCategoriesList = new SelectList(new List<SelectListItem>());
            MachineID = 0;
            MachineName = "";
            MachineDescrption = "";
            ProductionLineID = 0;
            Enabled = true;
            Type = "";
            Title = "";
            ImagePath = "";
        }
    }
}