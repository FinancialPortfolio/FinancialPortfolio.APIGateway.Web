using System;
using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Responses
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public double ExpectedAllocation { get; set; }
        
        public IEnumerable<SubCategoryResponse> SubCategories { get; set; }
    }
}