using System;
using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Responses
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public double ExpectedAllocationInPercentage { get; set; }
        
        public IEnumerable<SubCategoryResponse> SubCategories { get; set; }

        public IEnumerable<CategoryAssetResponse> GetAssets()
        {
            var result = new List<CategoryAssetResponse>();
            
            foreach (var subCategory in SubCategories) 
                result.AddRange(subCategory.GetAssets());

            return result;
        }
    }
}