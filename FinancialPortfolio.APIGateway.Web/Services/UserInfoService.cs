﻿using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Web.Extensions;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinancialPortfolio.APIGateway.Web.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public Task<T> GetClaimAsync<T>(string claimName) where T : class
        {
            var claim = GetFromToken<T>(claimName);
            return Task.FromResult(claim);
        }

        private T GetFromToken<T>(string claimName) where T : class
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.GetClaim<T>(claimName);
        }
    }
}