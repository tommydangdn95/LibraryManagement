using Services.Dtos;
using Services.Dtos.ApplicationDtos._Branch;
using Services.ViewModels._BranchViewModels;

namespace Services.Applications
{
    public interface IBranchService
    {
        public Task<IResult> CreateBranchAsync(CreateBranch model, Guid createUserId);
        public Task<IResultData<List<BranchItemDto>>> GetAllBranchAsync();
    }
}
