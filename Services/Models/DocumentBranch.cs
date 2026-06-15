using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class DocumentBranch : BaseModel
    {
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }

        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
