using Services.Dtos;
using Services.Dtos.ApplicationDtos._Branch;
using Services.ViewModels._BranchViewModels;

namespace Services.Applications
{
    public interface IBranchService
    {
        public Task<IResult> CreateBranchAsync(CreateBranch model, Guid createUserId);
        public Task<IResult> UpdateBranchAsync(UpdateBranch branch, Guid updateUserId);
        public Task<IResult> DeleteBranchAsync(Guid branchId, Guid updateUserId);
        public Task<IResultData<BranchItem>> GetBranchItemByIdAsync(Guid branchId);
        public Task<IResultData<BranchList>> GetListBranchAsync(GetListBranch query = null);
    }
}
