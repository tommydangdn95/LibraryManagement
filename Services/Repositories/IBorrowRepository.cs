using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
using Services.Models;
using Services.Models.Criterias;
using Services.ViewModels.Clients._DocumentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public interface IBorrowRepository
    {
        public Task<bool> CreateAsync(BorrowRequest borrowRequest);
        public Task<bool> UpdateAsync(BorrowRequest borrowRequest);
        public Task<bool> DeleteAsync(Guid borrowRequestId, Guid submitUserId);
        public Task<BorrowRequest> GetById(Guid borrowRequestId);

        #region Client
        public Task<PagedResult<DocumentItem>> GetBorrowDocumentByUser(Guid userId, GetDocumentListBorrowItemCriteria criteria);
        #endregion
    }
}
