using System.Threading.Tasks;
using MJonesMmtTest.Api.Models.Dto;

namespace MJonesMmtTest.Api.Services
{
    public interface ICustomerService
    {
        public Task<ApiCustomer> GetCustomerAsync(string emailAddress);
    }
}
