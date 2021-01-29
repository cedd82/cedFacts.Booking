using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Domain.Models.Customer;

namespace FACTS.GenericBooking.Domain.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<CustomerDetailsDto>> GetAccountDetailsAsync(string username, string accountNumber);
    }
}
