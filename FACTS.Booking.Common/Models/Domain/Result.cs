using FACTS.GenericBooking.Common.Constants;

namespace FACTS.GenericBooking.Common.Models.Domain
{
    public class Result
    {
        public Result() { }

        public Result(ApiMessage apiMessage)
        {
            ApiMessage = apiMessage;
        }

        public ApiMessage ApiMessage { get; private set; }
        public static Result Fail(ApiMessage message) => new Result(message);
        public static Result Fail(Result result) => result;
        public static Result<T> Fail<T>(ApiMessage message) => new Result<T>(message);
        public static Result<T> Fail<T>(T value) => new Result<T>(value);
        public bool Failure() => ApiMessage != null;
        public bool Info() => ApiMessage?.Level == MessageLevel.Information;
        public static Result<T> Ok<T>(T value) => new Result<T>(value);
        public static Result Ok() => new Result();
        public bool Success() => ApiMessage == null;
        public override string ToString() => ApiMessage.ToString();
        public bool Warning() => ApiMessage?.Level == MessageLevel.Warn;
    }

    public class Result<T> : Result
    {
        public Result(ApiMessage apiMessage) : base(apiMessage) { }
        public Result(string message, int level = MessageLevel.Error) : base(new ApiMessage(message, level)) { }

        public Result(T value)
        {
            Value = value;
        }

        public Result()
        {
            Value = default;
        }

        public T Value { get; set; }
    }
}