using MeuCubicoApi.Pagination;
using Microsoft.EntityFrameworkCore;
using Model;
using Pagination;
using Shared.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MeuCubicoDbContext _dbContext;

        public UserRepository(MeuCubicoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Id == id);

            return user;
        }
        public async Task<PagedList<User>> GetAllUsers(UserParameters usersParameters)
        {
            var users = await _dbContext.Users
                .OrderBy(e => e.Id)
            .ToListAsync();

            var usersOrdered = PagedList<User>.ToPagedList(users.AsQueryable(), usersParameters.PageNumber, usersParameters.PageSize);
            // converting the data collection to an IQueryable because PagedList wait this datatype
            return usersOrdered;
        }
    }
}
