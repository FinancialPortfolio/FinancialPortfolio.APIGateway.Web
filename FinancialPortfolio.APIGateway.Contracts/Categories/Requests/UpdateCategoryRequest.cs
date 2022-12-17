using System;
using System.Collections.Generic;
using FinancialPortfolio.APIGateway.Contracts.Categories.Models;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Requests
{
    public record UpdateCategoryRequest
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public double ExpectedAllocationInPercentage { get; set; }

        public Guid UserId { get; set; }
        
        public IEnumerable<SubCategory> SubCategories { get; set; }
        
        public IEnumerable<CategoryStock> Stocks { get; set; }
    }
}