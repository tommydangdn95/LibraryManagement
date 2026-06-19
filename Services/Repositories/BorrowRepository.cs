using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly AppDbContext _dbContext;
        public BorrowRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(BorrowRequest borrowRequest)
        {
            await _dbContext.BorrowRequest.AddAsync(borrowRequest);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid borrowRequestId, Guid submitUserId)
        {
            var borrowRequest = await GetById(borrowRequestId);
            if (borrowRequest == null) 
            {
                return false;
            }

            borrowRequest.DeletedDate = DateTime.Now;
            borrowRequest.DeleteBy = submitUserId;
            borrowRequest.IsDeleted = true;
            return await UpdateAsync(borrowRequest);
        }

        public Task<BorrowRequest> GetById(Guid borrowRequestId)
        {
            return _dbContext.BorrowRequest.FirstOrDefaultAsync(x => x.Id == borrowRequestId);
        }

        public async Task<bool> UpdateAsync(BorrowRequest borrowRequest)
        {
            _dbContext.BorrowRequest.Update(borrowRequest);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
