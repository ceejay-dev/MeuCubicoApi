using MeuCubicoApi.Pagination;
using Model;
using Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(string id);
        Task<PagedList<User>> GetAllUsers(UserParameters users);
    }
}
