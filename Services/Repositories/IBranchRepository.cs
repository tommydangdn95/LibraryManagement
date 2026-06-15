using Services.Dtos;
using Services.Models;

namespace Services.Repositories
{
    public interface IBranchRepository
    {
        public Task<bool> CreateAsync(Branch branch);
        public Task<PagedResult<Branch>> GetAllAsync();
    }
}
