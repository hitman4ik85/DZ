using AutoMapper;
using PeopleBudgetTracker.Core.DTOs;
using PeopleBudgetTracker.Entities.Models;

namespace PeopleBudgetTracker.Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Account, AccountDTO>().ReverseMap();
        CreateMap<TransactionDTO, IncomeOperation>().ReverseMap();
        CreateMap<TransactionDTO, ExpenseOperation>().ReverseMap();
    }
}
