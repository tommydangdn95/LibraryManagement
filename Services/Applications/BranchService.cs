using Services.Dtos;
using Services.Dtos.ApplicationDtos._Branch;
using Services.Models;
using Services.Repositories;
using Services.ViewModels._BranchViewModels;

namespace Services.Applications
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        public BranchService(IBranchRepository branchRepository)
        {
            this._branchRepository = branchRepository;
        }
        public async Task<IResult> CreateBranchAsync(CreateBranch model, Guid createUserId)
        {
            var branch = new Branch
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                Description = model.Description,
                CreatedBy = createUserId,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            var result = await _branchRepository.CreateAsync(branch);
            if (!result)
            {
                return Result.Failed($"Create new branch failed");
            }

            return Result.Success("Create new branch successfully");
        }

        public async Task<IResultData<List<BranchItemDto>>> GetAllBranchAsync()
        {
            var pageResults = await _branchRepository.GetAllAsync();

            var branchDtos = pageResults.Items.Select(b => new BranchItemDto
            {
                BranchId = b.Id,
                Name = b.Name,
                Address = b.Address,
                Phone = b.Phone,
                Email = b.Email,
                Description = b.Description,
                IsActive = b.IsActive
            }).ToList();

            return ResultData<List<BranchItemDto>>.SuccessData("Get all list branch successfully", branchDtos);
        }
    }
}
