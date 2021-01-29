using System;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Helpers;
using FACTS.GenericBooking.Domain.Services.Interfaces;
using FACTS.GenericBooking.Repository.Postgres;
using FACTS.GenericBooking.Repository.Postgres.Entities;

using Newtonsoft.Json;

using NLog;

namespace FACTS.GenericBooking.Domain.Services
{
    public class ProcessEventService : IProcessEventService
    {
        private readonly FactsDbContext _dbContext;
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public ProcessEventService(FactsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProcessEventData> AddProcessEventDataAsync<T>(T document, string userCode, string eventName, string url)
        {
            ProcessEventData processEventData = new ProcessEventData
            {
                AppName     = "Facts",
                CreateDate  = DateTime.Now,
                EventName   = eventName,
                ProcessData = JsonConvert.SerializeObject(document, JsonHelpers.SerializerSettings),
                ProcessUrl  = url,
                UserCode    = userCode,
                StatusCode  = "INP"
            };
            await _dbContext.AddAsync(processEventData);
            await _dbContext.SaveChangesAsync();
            return processEventData;
        }

        public Task UpdateProcessEventDataCompleteAsync<T>(ProcessEventData processEventData, T outputDocument)
        {
            processEventData.StatusCode        = "COM";
            processEventData.ProcessDataOutput = JsonConvert.SerializeObject(outputDocument, JsonHelpers.SerializerSettings);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateProcessEventDataToErrorAndLogAsync<T>(ProcessEventData processEventData, T outputDocument)
        {
            string error = outputDocument as string;
            Logger.Log(LogLevel.Error, error);
            processEventData.StatusCode        = "ERR";
            processEventData.ProcessDataOutput = JsonConvert.SerializeObject(outputDocument, JsonHelpers.SerializerSettings);
            return _dbContext.SaveChangesAsync();
        }
    }
}