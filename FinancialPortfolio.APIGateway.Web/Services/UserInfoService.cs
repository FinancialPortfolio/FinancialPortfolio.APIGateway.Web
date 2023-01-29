using System;
using System.Linq;
using System.Threading.Tasks;
using AccountApi;
using AutoMapper;
using FinancialPortfolio.APIGateway.Web.Extensions;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using FinancialPortfolio.APIGateway.Web.Models.Exceptions;
using Microsoft.AspNetCore.Http;

namespace FinancialPortfolio.APIGateway.Web.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Account.AccountClient _accountClient;
        private readonly IMapper _mapper;
        
        public UserInfoService(IHttpContextAccessor httpContextAccessor, Account.AccountClient accountClient, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountClient = accountClient;
            _mapper = mapper;
        }
        
        public Task<T> GetClaimAsync<T>(string claimName) where T : class
        {
            var claim = GetFromToken<T>(claimName);
            return Task.FromResult(claim);
        }

        public async Task ValidateUserAccountAsync(Guid userId, Guid accountId)
        {
            // TODO: add cache for this request
            var query = _mapper.Map<GetAccountsQuery>(userId);
            var response = await _accountClient.GetAllAsync(query);
            
            if (response.Accounts.All(account => account.Id != accountId.ToString()))
                throw new ForbiddenException($"Account with id: {accountId} is not allowed to user: {userId}.");
        }

        private T GetFromToken<T>(string claimName) where T : class
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.GetClaim<T>(claimName);
        }
    }
}