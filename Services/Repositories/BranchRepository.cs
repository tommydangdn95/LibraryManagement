using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly AppDbContext _appDbContext;
        public BranchRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public async Task<bool> CreateAsync(Branch branch)
        {
            await _appDbContext.Branchs.AddAsync(branch);
            var result = await _appDbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid branchId, Guid submitUserId)
        {
            var branch = await GetById(branchId);
            if (branch == null)
            {
                return false;
            }

            branch.DeleteBy = submitUserId;
            branch.DeletedDate = DateTime.Now;
            branch.IsDeleted = true;

            var result = await UpdateAsync(branch);
            return result;
        }

        public async Task<PagedResult<Branch>> GetAllAsync()
        {
            var branches = await _appDbContext.Branchs.Where(x => !x.IsDeleted).ToListAsync();
            if (!branches.Any())
            {
                return PagedResult<Branch>.Empty();
            }

            return new PagedResult<Branch>(branches, branches.Count(), 1, 100000);
        }

        public async Task<Branch> GetById(Guid branchId)
        {
            return await _appDbContext.Branchs.FirstOrDefaultAsync(x => x.Id == branchId);
        }

        public async Task<bool> UpdateAsync(Branch branch)
        {
            _appDbContext.Branchs.Update(branch);
            var result = await _appDbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
