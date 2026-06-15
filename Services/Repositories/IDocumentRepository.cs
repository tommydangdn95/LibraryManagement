using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public interface IDocumentRepository
    {
        public Task<bool> CreateAsync(Document document);
        public Task<bool> CreateDocumentBranch(DocumentBranch documentBranch);
    }
}
