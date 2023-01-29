using System;
using System.Threading.Tasks;

namespace FinancialPortfolio.APIGateway.Web.Interfaces
{
    public interface IUserInfoService
    {
        Task<T> GetClaimAsync<T>(string claimName) where T : class;

        Task ValidateUserAccountAsync(Guid userId, Guid accountId);
    }
}