using Services.Dtos;
using Services.Models._Users;
using Services.ViewModels._UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Applications
{
    public interface IUserService
    {
        public Task<IResultData<AppUser>> GetUserByIdAsync(Guid userId);
        public Task<IResultData<Guid>> CreateUser(CreateUser createUser);
        public Task<IResultData<UserList>> GetUserList(GetUserList query);
        public Task<IResultData<UserItem>> GetUserViewByIdAsync(Guid userId);
    }
}
