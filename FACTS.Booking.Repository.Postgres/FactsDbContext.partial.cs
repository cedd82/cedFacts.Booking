using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using NLog;

namespace FACTS.GenericBooking.Repository.Postgres
{
    public partial class FactsDbContext
    {
        private void RejectChanges()
        {
            foreach (EntityEntry entry in ChangeTracker.Entries().Where(e => e.Entity != null).ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            try
            {
                int result = base.SaveChanges();
            #if DEBUG
                DisplayStates(ChangeTracker.Entries());
            #endif
                return result;
            }
            catch (ValidationException exception)
            {
                string members = string.Join(",", exception.ValidationResult.MemberNames);
                Logger.Log(LogLevel.Error, exception, $"{nameof(SaveChanges)} validation exception: {exception.Message}; members:{members} inner exp {exception.InnerException}");
                RejectChanges();
                throw;
            }
            catch (DbUpdateException exception)
            {
                IDictionary data = exception.InnerException?.Data;
                string detail = string.Empty;
                string schema = string.Empty;
                string table = string.Empty;
                if (data != null)
                {
                    detail = data["Detail"]?.ToString();
                    schema = data["SchemaName"]?.ToString();
                    table  = data["TableName"]?.ToString();
                }

                Logger.Log(LogLevel.Error, exception, $"{nameof(SaveChanges)} db update error table {schema}.{table} details: {detail} innerExp: {exception.InnerException}");
                RejectChanges();
                throw;
            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.Error, exception, $"{nameof(SaveChanges)} db exception: {exception.InnerException}");
                RejectChanges();
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                int result = await base.SaveChangesAsync(cancellationToken);
            #if DEBUG
                DisplayStates(ChangeTracker.Entries());
            #endif
                return result;
            }
            catch (ValidationException exception)
            {
                string members = string.Join(",", exception.ValidationResult.MemberNames);
                Logger.Log(LogLevel.Error, exception, $"{nameof(SaveChanges)} validation exception: {exception.Message}; members:{members} inner exp {exception.InnerException}");
                RejectChanges();
                throw;
            }
            catch (DbUpdateException exception)
            {
                IDictionary data = exception.InnerException?.Data;
                string detail = string.Empty;
                string schema = string.Empty;
                string table = string.Empty;
                if (data != null)
                {
                    detail = data["Detail"]?.ToString();
                    schema = data["SchemaName"]?.ToString();
                    table  = data["TableName"]?.ToString();
                }

                Logger.Log(LogLevel.Error, exception, $"{nameof(SaveChanges)} db update error table {schema}.{table} details: {detail} innerExp: {exception.InnerException}");
                RejectChanges();
                throw;
            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.Error, exception, $"{nameof(SaveChanges)} db exception: {exception.InnerException}");
                RejectChanges();
                throw;
            }
        }

        private static void DisplayStates(IEnumerable<EntityEntry> entries)
        {
            IEnumerable<EntityEntry> entityEntries = entries.ToList();
            Console.WriteLine(entityEntries.Count());
            Debug.WriteLine(entityEntries.Count());
            foreach (EntityEntry entry in entityEntries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State.ToString()} ");
                Debug.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State.ToString()} ");
            }

            Console.WriteLine();
            Debug.WriteLine("");
        }
    }
}
