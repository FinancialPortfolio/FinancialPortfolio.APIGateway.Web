using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Responses
{
    public class SubCategoryResponse
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public double ExpectedAllocationInPercentage { get; set; }
        
        public IEnumerable<SubCategoryResponse> SubCategories { get; set; }
        
        public IEnumerable<CategoryAssetResponse> Assets { get; set; }
        
        public IEnumerable<CategoryAssetResponse> GetAssets()
        {
            var result = new List<CategoryAssetResponse>(Assets);
            
            foreach (var subCategory in SubCategories) 
                result.AddRange(subCategory.GetAssets());

            return result;
        }
    }
}