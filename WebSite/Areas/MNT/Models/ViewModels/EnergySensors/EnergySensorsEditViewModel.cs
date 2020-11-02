using Core.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.MNT.Models.ViewModels.EnergySensors
{
    public class EnergySensorsEditViewModel
    {

        public string Header { get; set; }
        public bool IsEdit { get; set; }
        public List<Core.Entities.IAttachment> AttachmentsList { get; set; }
        public IEnumerable<SelectListItem> SensorConfigForCopyList { get; set; }
        public List<EnergySensorValue> SensorsConfigList { get; set; }
        public string AttachmentType { get; set; }
        public Core.Entities.EnergySensors ES { get; set; }

        public int TempAttachmentID
        {
            get
            {
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                return (int)t.TotalSeconds;
            }
        }

        public EnergySensorsEditViewModel()
        {
            ES = new Core.Entities.EnergySensors();
            Header = "";
            IsEdit = true;
            AttachmentsList = new List<Core.Entities.IAttachment>();
            AttachmentType = "TEMPID";
            SensorConfigForCopyList = new SelectList(new List<SelectListItem>());
            SensorsConfigList = new List<EnergySensorValue>();

        }
    }
}