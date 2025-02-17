﻿using System;
using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using FinancialPortfolio.APIGateway.Web.Models.Constants;
using FinancialPortfolio.APIGateway.Web.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;
        
        protected ApiControllerBase(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }
        
        protected async Task<Guid> GetUserIdAsync()
        {
            var claim = await _userInfoService.GetClaimAsync<string>(ClaimConstants.UserId);
            if (claim == null)
                throw new ForbiddenException($"You do not have {ClaimConstants.UserId} claim.");

            if (Guid.TryParse(claim, out var result))
                return result;

            throw new ForbiddenException($"{ClaimConstants.UserId} claim '{claim}' is incorrect Guid value.");
        }

        protected async Task ValidateUserAccountAsync(Guid accountId)
        {
            var userId = await GetUserIdAsync();
            await _userInfoService.ValidateUserAccountAsync(userId, accountId);
        }
        
        protected async Task ValidateUserAccountAsync(Guid userId, Guid accountId)
        {
            await _userInfoService.ValidateUserAccountAsync(userId, accountId);
        }
    }
}