using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Excel;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Equity.Enums;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Requests;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Services;
using FinancialPortfolio.APIGateway.Contracts.Orders.Commands;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;
using FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets.Models;

namespace FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets
{
    public class IntegrationService : IIntegrationService
    {
        private const string DealsSection = "Deals";
        private const string OrdersSection = "Orders";
        private const string TransferDeal = "balance";
        private const string BuyDeal = "buy";
        private const string SellDeal = "sell";
        private const string FilledOrderState = "filled";

        public async Task<IEnumerable<IntegrateOrderCommand>> ParseOrdersAsync(IntegrateRequest request)
        {
            await using var stream = request.File.OpenReadStream();
            using var parser = new ExcelParser(stream, CultureInfo.InvariantCulture);
            using var csvReader = new CsvReader(parser);
            
            var orders = await ReadOrdersAsync(csvReader);
            
            return orders.Select(MapOrder);
        }

        public async Task<IEnumerable<IntegrateTransferCommand>> ParseTransfersAsync(IntegrateRequest request)
        {
            await using var stream = request.File.OpenReadStream();
            using var parser = new ExcelParser(stream, CultureInfo.InvariantCulture);
            using var csvReader = new CsvReader(parser);
            
            var deals = await ReadTransferDealsAsync(csvReader);

            return deals.Select(MapTransfer);
        }

        private static IntegrateTransferCommand MapTransfer(TransferDeal deal)
        {
            var transferType = deal.Profit > 0 ? TransferType.Deposit : TransferType.Withdrawal;
            return new IntegrateTransferCommand(Math.Abs(deal.Profit), transferType, deal.DateTime);
        }
        
        private static IntegrateOrderCommand MapOrder(Order order)
        {
            var type = order.Type == BuyDeal ? OrderType.Buy : OrderType.Sell;
            var amount = order.Deals.Sum(d => Math.Abs(d.Volume));
            var totalPrice = order.Deals.Sum(d => Math.Abs(d.Price) * Math.Abs(d.Volume));
            var commission = order.Deals.Sum(d => Math.Abs(d.Fee) + Math.Abs(d.Commission));
                
            return new IntegrateOrderCommand(type, amount, totalPrice / amount, order.DateTime, commission, order.Symbol);
        }
        
        private static async Task<IEnumerable<Order>> ReadOrdersAsync(CsvReader csvReader)
        {
            var orders = new List<Order>();

            while (await csvReader.ReadAsync())
            {
                var section = csvReader.GetField<string>(0);
                if (section != OrdersSection)
                    continue;

                await csvReader.ReadAsync();
                
                while (csvReader.TryGetRecord<Order>(out var order))
                { 
                    if (order.State == FilledOrderState)
                        orders.Add(order);

                    await csvReader.ReadAsync();
                }
                
                break;
            }
            
            var deals = await ReadOrderDealsAsync(csvReader);
            foreach (var order in orders)
            {
                order.Deals = deals.Where(d => d.OrderId == order.OrderId).ToList();
            }

            return orders;
        }
        
        private static async Task<IEnumerable<Deal>> ReadOrderDealsAsync(CsvReader csvReader)
        {
            var deals = new List<Deal>();

            do
            {
                var section = csvReader.GetField<string>(0);
                if (section != DealsSection)
                    continue;

                await csvReader.ReadAsync();
                
                await csvReader.ReadAsync();

                while (csvReader.TryGetRecord<Deal>(out var deal))
                {
                    if (deal.Type is BuyDeal or SellDeal)
                        deals.Add(deal);

                    await csvReader.ReadAsync();
                }

                break;
            } while (await csvReader.ReadAsync());

            return deals;
        }

        private static async Task<IEnumerable<TransferDeal>> ReadTransferDealsAsync(CsvReader csvReader)
        {
            var deals = new List<TransferDeal>();
            
            while (await csvReader.ReadAsync())
            {
                var section = csvReader.GetField<string>(0);
                if (section != DealsSection)
                    continue;

                await csvReader.ReadAsync();
                
                while (csvReader.TryGetRecord<TransferDeal>(out var deal))
                {
                    if (deal.Type == TransferDeal)
                        deals.Add(deal);

                    await csvReader.ReadAsync();
                }

                break;
            }

            return deals;
        }
    }
}