using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.Dashboard
{
    public class ShowViewModel
    {
        public List<DashboardOperationParamater> ParametersList { set; get; }
        public List<OperationTask> TaskList { set; get; }
        public Machine DashboardMachine { set; get; }
        public HeaderViewModel HeaderModel { get; set; }
        public List<Catalog> SectionsList { get; set; }

        public string MaterialName { get; set; }
        public string SetupName { get; set; }
        public ShowViewModel()
        {
            HeaderModel = new HeaderViewModel();
            DashboardMachine = new Machine();
            ParametersList = new List<DashboardOperationParamater>();
            TaskList = new List<OperationTask>();
            SectionsList = new List<Catalog>();
        }
    }
}