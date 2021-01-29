using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Domain.Models.Quote;

namespace FACTS.GenericBooking.Domain.Services.Interfaces
{
    public interface IQuoteService
    {
        Task<Result<CreateQuoteResultDto>> CreateQuoteAsync(CreateQuoteDto model);
    }
}