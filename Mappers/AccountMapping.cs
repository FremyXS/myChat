using Mapster;
using Pract.Dto;
using Pract.Models;
using Pract.Requests;
using System.Runtime.CompilerServices;

namespace Pract.Mappers
{
    public static class AccountMapping
    {
        public static Account ToModel(this AccountCreateRequest accountCreateRequest)
        {
            return accountCreateRequest.Adapt<Account>();
        }
    }
}
