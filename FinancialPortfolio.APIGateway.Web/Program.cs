using System.Threading.Tasks;
using FinancialPortfolio.WebHosting;

namespace FinancialPortfolio.APIGateway.Web
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Application.RunAsync(args, typeof(Startup));
        }
    }
}
