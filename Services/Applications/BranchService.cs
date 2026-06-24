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

        public async Task<IResult> DeleteBranchAsync(Guid branchId, Guid updateUserId)
        {
            var result = await _branchRepository.DeleteAsync(branchId, updateUserId);
            if (!result)
            {
                return Result.Failed($"Delete branch failed");
            }

            return Result.Success("Delete branch successfully");
        }

        public async Task<IResultData<BranchList>> GetListBranchAsync(GetListBranch query = null)
        {
            query = query == null ? new GetListBranch() : query;
            var criteria = new GetListBranchCriteria()
            {
                Page = query.Page,
                RowsPerPage = query.RowsPerPage
            };

            var pageResults = await _branchRepository.GetListBranchAsync(criteria);
            var branchList = new BranchList()
            {
                Items = pageResults.Items.Select(b => new BranchItem
                {
                    BranchId = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Phone = b.Phone,
                    Email = b.Email,
                    Description = b.Description,
                    IsActive = b.IsActive
                }).ToList(),
                Paging = Paging.GetPaging(query.Page, query.RowsPerPage, pageResults.TotalCount)
            };

            return ResultData<BranchList>.SuccessData("Get all list branch successfully", branchList);
        }

        public async Task<IResultData<BranchItem>> GetBranchItemByIdAsync(Guid branchId)
        {
            var branch = await _branchRepository.GetById(branchId);
            if (branch == null)
            {
                return ResultData<BranchItem>.Failed("Could not found branch");
            }

            var item = new BranchItem
            {
                BranchId = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                Phone = branch.Phone,
                Email = branch.Email,
                Description = branch.Description,
                IsActive = branch.IsActive
            };

            return ResultData<BranchItem>.SuccessData("Get branch successfully", item);

        }

        public async Task<IResult> UpdateBranchAsync(UpdateBranch branch, Guid updateUserId)
        {
            var branchUpdate = await _branchRepository.GetById(branch.BranchId);
            if (branchUpdate == null)
            {
                return Result.Failed("Could not found branch");
            }

            branchUpdate.Name = branch.Name;
            branchUpdate.Address = branch.Address;
            branchUpdate.Phone = branch.Phone;
            branchUpdate.Email = branch.Email;
            branchUpdate.Description = branch.Description;
            branchUpdate.UpdatedDate = DateTime.Now;
            branchUpdate.UpdatedBy = updateUserId;

            var result = await _branchRepository.UpdateAsync(branchUpdate);
            if (!result)
            {
                return Result.Failed("Could not found branch");
            }

            return Result.Success("Update branch successfully");
        }
    }
}
