namespace FinancialPortfolio.APIGateway.Web.Models.Settings
{
    public class ServicesSettings
    {
        public ServiceSettings EquityService { get; set; }
        
        public ServiceSettings AccountsService { get; set; }
        
        public ServiceSettings AssetsService { get; set; }
        
        public ServiceSettings OrdersService { get; set; }
        
        public ServiceSettings CategoriesService { get; set; }
    }
}