using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.Dashboard
{
    public class IndexViewModel
    {
        public List<DashboardMachine> MachinesList;
        public HeaderViewModel HeaderModel { get; set; }
        public List<OperationTask> TaskList { set; get; }
        public string CultureID { get; set; }

        public IndexViewModel()
        {
            HeaderModel = new HeaderViewModel();
            MachinesList = new List<DashboardMachine>();
            TaskList = new List<OperationTask>();
        }
    }
}