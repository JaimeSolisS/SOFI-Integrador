using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_ProductionGoalsDetail
    {
        public int RowNumber { get; set; }
        public int GoalDetailID { get; set; }
        public int HourValue { get; set; }
        public decimal GoalValue { get; set; }
    }
}
