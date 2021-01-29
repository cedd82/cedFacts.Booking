using System.Threading.Tasks;

using FACTS.GenericBooking.Repository.Postgres.Entities;

namespace FACTS.GenericBooking.Domain.Services.Interfaces
{
    public interface IProcessEventService
    {
        Task<ProcessEventData> AddProcessEventDataAsync<T>(T document, string userCode, string eventName, string url);
        Task UpdateProcessEventDataCompleteAsync<T>(ProcessEventData processEventData, T outputDocument);
        Task UpdateProcessEventDataToErrorAndLogAsync<T>(ProcessEventData processEventData, T outputDocument);
    }
}
