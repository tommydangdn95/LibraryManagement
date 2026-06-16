using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
using Services.Models;
using Services.Models.Criterias;
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

        #region Client
        public Task<PagedResult<DocumentItem>> GetListDocumentItem(GetDocumentItemCriteria criteria);
        #endregion
    }
}
