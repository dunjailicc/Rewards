using Rewards.Application.DTO;

namespace Rewards.Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<CustomerDto> GetCustomerAsync(string customerId);
    }
}
