using Services.Dtos;
using Services.Dtos.ApplicationDtos._Branch;
using Services.Models;

namespace Services.Repositories
{
    public interface IBranchRepository
    {
        public Task<bool> CreateAsync(Branch branch);
        public Task<bool> UpdateAsync(Branch branch);
        public Task<bool> DeleteAsync(Guid branchId, Guid submitUserId);
        public Task<Branch> GetById(Guid branchId);
        public Task<PagedResult<Branch>> GetListBranchAsync(GetListBranchCriteria criteria);
    }
}
