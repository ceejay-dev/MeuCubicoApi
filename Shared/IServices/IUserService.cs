using DTO;
using MeuCubicoApi.Pagination;
using Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IServices
{
    public interface IUserService
    {
        Task<UserDTO> GetUserById(string id);
        Task<PagedList<UserDTO>> GetAllUsers(UserParameters userParameters);
    }
}
