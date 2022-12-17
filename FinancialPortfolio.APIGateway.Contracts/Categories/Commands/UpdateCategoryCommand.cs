using System;
using System.Collections.Generic;
using FinancialPortfolio.APIGateway.Contracts.Categories.Models;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Commands
{
    [Message("Categories", "Category")]
    public record UpdateCategoryCommand : ICommand
    {
        public string Name { get; }
        
        public string Description { get; }
        
        public double ExpectedAllocationInPercentage { get; }

        public Guid UserId { get; }
        
        public IEnumerable<SubCategory> SubCategories { get; }
        
        public IEnumerable<CategoryAsset> Assets { get; }

        public UpdateCategoryCommand(string name, string description, double expectedAllocationInPercentage, 
            Guid userId, IEnumerable<SubCategory> subCategories, IEnumerable<CategoryAsset> assets)
        {
            Name = name;
            Description = description;
            ExpectedAllocationInPercentage = expectedAllocationInPercentage;
            UserId = userId;
            SubCategories = subCategories;
            Assets = assets;
        }
    }
}