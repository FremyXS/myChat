using Mapster;
using Pract.Dto;
using Pract.Models;
using Pract.Requests;
using System.Runtime.CompilerServices;

namespace Pract.Mappers
{
    public static class AccountMapping
    {
        static AccountMapping()
        {
            TypeAdapterConfig<Account, AccountDto>
                .NewConfig()
                .Map(x => x.UserName, src => src.User.Name)
                .Map(y => y.UserId, src => src.User.Id);
        }
        public static Account ToModel(this AccountCreateRequest accountCreateRequest)
        {
            return accountCreateRequest.Adapt<Account>();
        }

        public static AccountDto ToDto(this Account account)
        {
            return account.Adapt<AccountDto>();
        }
    }
}
