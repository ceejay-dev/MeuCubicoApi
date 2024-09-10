using AutoMapper;
using DTO;
using MeuCubicoApi.Pagination;
using Model;
using Pagination;
using Shared.IRepositories;
using Shared.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            repository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserDTO> GetUserById(string id)
        {
            var expense = await repository.GetUserById(id);

            if (expense != null)
            {
                var dto = mapper.Map<UserDTO>(expense);
                return dto;
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<PagedList<UserDTO>> GetAllUsers(UserParameters usersParameters)
        {
            var users = await repository.GetAllUsers(usersParameters);

            if (users != null)
            {
                var dtoList = mapper.Map<IEnumerable<UserDTO>>(users);

                // Cria um novo PagedList de DTOs, mantendo as informações de paginação
                var dtoPagedList = new PagedList<UserDTO>(
                    dtoList.ToList(),
                    users.TotalCount,
                    users.CurrentPage,
                    users.PageSize);

                return dtoPagedList;
            }
            else
            {
                throw new Exception("No users found.");
            }
        }
    }
}
