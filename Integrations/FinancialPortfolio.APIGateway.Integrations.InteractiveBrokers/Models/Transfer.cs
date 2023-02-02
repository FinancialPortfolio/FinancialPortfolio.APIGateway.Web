using System;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using FinancialPortfolio.APIGateway.Contracts.Equity.Enums;

namespace FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers.Models
{
    public class Transfer
    {
        [Index(3)]
        public DateTime DateTime { get; set; }
        
        [Index(5)]
        public string AmountString { get; set; }
        
        public decimal Amount => decimal.Parse(AmountString.Replace(',', '.'), CultureInfo.InvariantCulture);
        
        public TransferType Type => Amount >= 0 ? TransferType.Deposit : TransferType.Withdrawal; 
    }
}