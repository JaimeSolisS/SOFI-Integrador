using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.Production.Models.Goals
{
    public class TableDetailViewModel
    {
        public List<Core.Entities.ProductionGoalsDetail> ProductionGoalsDetails;
        public string TableID;
        public int GoalID;

        public TableDetailViewModel()
        {
            ProductionGoalsDetails = new List<Core.Entities.ProductionGoalsDetail>();
            TableID = string.Empty;
            GoalID = 0;
        }
    }
}