using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Models
{
    public record SubCategory
    {
        public string Name { get; private set; }
        
        public string Description { get; private set; }

        public double ExpectedAllocationInPercentage { get; private set; }
        
        public IEnumerable<SubCategory> SubCategories { get; private set; }
        
        public IEnumerable<CategoryStock> Stocks { get; private set; }

        public SubCategory(string name, string description, double expectedAllocationInPercentage, IEnumerable<SubCategory> subCategories = null, IEnumerable<CategoryStock> stocks = null)
        {
            Name = name;
            Description = description;
            ExpectedAllocationInPercentage = expectedAllocationInPercentage;
            SubCategories = subCategories;
            Stocks = stocks;
        }
    }
}