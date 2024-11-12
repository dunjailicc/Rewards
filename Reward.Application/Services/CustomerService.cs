using Rewards.Application.DTO;
using Rewards.Application.Interfaces;

namespace Rewards.Business.Services
{
    internal class CustomerService : ICustomerService
    {
        public Task<CustomerDto> GetCustomerAsync(string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
