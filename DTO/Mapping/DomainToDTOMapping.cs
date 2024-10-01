using AutoMapper;
using DTO.Auth;
using MeuCubico.DTO;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DTO.Mapping
{
    public class DomainToDTOMapping : Profile
    {
        public DomainToDTOMapping()
        {
            CreateMap<Expense, ExpenseDTO>().ReverseMap();
            CreateMap<Expense, ExpenseUpdateDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserForRegistrationDTO, User>()
           .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
