using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using FACTS.GenericBooking.Common.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FACTS.GenericBooking.Api.Filters
{
    public class FluentValidateAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if (context.ModelState.IsValid)
            {
                return;
            }

            ILookup<string, KeyValuePair<string, ModelStateEntry>> modelErrorsLookup = context.ModelState.ToLookup(e => e.Key);
            List<ModelErrorDto> errors = new List<ModelErrorDto>();
            foreach (IGrouping<string, KeyValuePair<string, ModelStateEntry>> entry in modelErrorsLookup)
            {
                //string fieldName = entry.Key;
                List<ModelStateEntry> fieldErrors = entry.Select(x => x.Value).ToList();
                IEnumerable<ModelErrorDto> fieldErrorsMapped = fieldErrors.SelectMany(x => x.Errors)
                    .Select(error => new ModelErrorDto(error.ErrorMessage));
                    //.Select(error => new ModelErrorDto(error.ErrorMessage, fieldName));
                errors.AddRange(fieldErrorsMapped);
            }

            context.Result = new BadRequestObjectResult(new
            {
                Errors = errors,
                Status = new
                {
                    Code = (int) HttpStatusCode.BadRequest,
                    Name = HttpStatusCode.BadRequest.ToString(),
                    Timestamp = DateTime.UtcNow.ToString("s")
                }
            });
        }
    }
}