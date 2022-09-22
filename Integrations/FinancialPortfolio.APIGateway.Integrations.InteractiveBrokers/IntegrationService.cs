using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Requests;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Services;
using FinancialPortfolio.APIGateway.Contracts.Orders.Commands;
using FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers.Models;

namespace FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers
{
    public class IntegrationService : IIntegrationService
    {
        private const string TradesSection = "Trades";
        private const string TransfersSection = "Deposits & Withdrawals";
        
        private const string DataRow = "Data";
        private const string OrderRow = "Order";
        private const string StocksRow = "Stocks";
        
        private const string ElectronicFundTransferRow = "Electronic Fund Transfer";
        
        private readonly CsvConfiguration _csvConfiguration = new (CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            TrimOptions = TrimOptions.Trim,
            Encoding = Encoding.UTF8
        };
            
        private readonly TypeConverterOptions _csvOptions = new ()
        {   
            Formats = new [] { "yyyy-MM-dd; HH:mm:ss", "dd.MM.yyyy" }
        };
        
        public async Task<IEnumerable<IntegrateOrderCommand>> ParseOrdersAsync(IntegrateRequest request)
        {
            await using var stream = request.File.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csvReader = new CsvReader(reader, _csvConfiguration);

            csvReader.Context.TypeConverterOptionsCache.AddOptions<DateTime>(_csvOptions);
            
            var result = new List<IntegrateOrderCommand>();
            
            while (await csvReader.ReadAsync())
            {
                var section = csvReader.GetField<string>(0);
                if (section != TradesSection)
                    continue;
                
                var tradeTypeRow = csvReader.GetField<string>(2);
                if (tradeTypeRow != OrderRow)
                    continue;
                
                var assetCategoryRow = csvReader.GetField<string>(3);
                if (assetCategoryRow != StocksRow)
                    continue;
                
                var order = csvReader.GetRecord<Order>();
                result.Add(new IntegrateOrderCommand(order.Type, order.Quantity, order.Price, 
                    order.DateTime, Math.Abs(order.Commission), order.Symbol, order.Exchange, order.Currency));
            }

            return result;
        }
        
        public async Task<IEnumerable<IntegrateTransferCommand>> ParseTransfersAsync(IntegrateRequest request)
        {
            await using var stream = request.File.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csvReader = new CsvReader(reader, _csvConfiguration);

            csvReader.Context.TypeConverterOptionsCache.AddOptions<DateTime>(_csvOptions);

            var result = new List<IntegrateTransferCommand>();
            
            while (await csvReader.ReadAsync())
            {
                var section = csvReader.GetField<string>(0);
                if (section != TransfersSection)
                    continue;
                
                var rowType = csvReader.GetField<string>(1);
                if (rowType != DataRow)
                    continue;
                
                var descriptionRow = csvReader.GetField<string>(4);
                if (!descriptionRow.Contains(ElectronicFundTransferRow))
                    continue;

                var transfer = csvReader.GetRecord<Transfer>();
                result.Add(new IntegrateTransferCommand(Math.Abs(transfer.Amount), transfer.Type, transfer.DateTime));
            }

            return result;
        }
    }
}