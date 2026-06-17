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
        public Task<bool> UpdateDocumentBranch(DocumentBranch documentBranch);
        public Task<DocumentBranch> GetDocumentBranchAsync(Guid documentId, Guid branchId);
        public Task<bool> UpdateDocumentBranch(Guid documentId, Guid updateBranchId, Guid submitUserId);
        public Task<bool> DeleteDocumentBranch(Guid documentId, Guid submitUserId);
        public Task <bool> UpdateAsync(Document document);
        public Task<bool> DeleteAsync(Guid documentId, Guid submitUserId);
        public Task<Document> GetByIdAsync(Guid documentId);

        #region Client
        public Task<PagedResult<DocumentItem>> GetListDocumentItem(GetDocumentItemCriteria criteria);
        #endregion
    }
}
