using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ApplicationDtos._Branch
{
    public class GetListBranchCriteria
    {
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 25;
    }
}
