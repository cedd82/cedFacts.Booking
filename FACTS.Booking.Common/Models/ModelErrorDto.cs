using System;
using System.Collections.Generic;

namespace FACTS.GenericBooking.Common.Models
{
    public class ModelErrorDto
    {
        //public ModelErrorDto(string message, string field = null)
        public ModelErrorDto(string message)
        {
            //Field = field;
            Message = message;
        }

        //public string Field { get; set; }
        public string Message { get; set; }
    }

    public class BadRequestResponse
    {
        public List<ModelErrorDto> Errors { get; set; }
        public int Code => 400;
        public string Name => "BadRequest";
        public DateTime TimeStamp => DateTime.Now;
    }
}