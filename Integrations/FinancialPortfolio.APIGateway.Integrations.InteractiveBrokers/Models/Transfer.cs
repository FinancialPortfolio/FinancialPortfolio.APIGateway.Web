using System;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using FinancialPortfolio.APIGateway.Contracts.Equity.Enums;
using FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers.Helpers;

namespace FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers.Models
{
    public class Transfer
    {
        [Index(3)]
        public DateTime DateTime { get; set; }
        
        [Index(5)]
        public string AmountString { get; set; }
        
        public decimal Amount => ParseHelper.Parse(AmountString);
        
        public TransferType Type => Amount >= 0 ? TransferType.Deposit : TransferType.Withdrawal; 
    }
}