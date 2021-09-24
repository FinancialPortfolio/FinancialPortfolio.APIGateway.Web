using System.Threading.Tasks;

namespace FinancialPortfolio.APIGateway.Web.Services.Abstraction
{
    public interface IUserInfoService
    {
        Task<T> GetClaimAsync<T>(string claimName) where T : class;
    }
}