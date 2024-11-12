using Rewards.Application.DTO;
using Rewards.Application.Interfaces;

namespace Rewards.Implementation
{
    public class CustomerService : ICustomerService
    {
        public Task<CustomerDto> GetCustomer(string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
