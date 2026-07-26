using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Common
{
   public record Result(bool Success, string? Error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result Ok() => new Result(true);
        public static Result NotFound(string message = "Not found") => new (false, message, ResultKind.NotFound);
        public static Result fail(string message, ResultKind kind = ResultKind.Conflict) => new (false, message, kind);
        public static Result ValidationFailed(string message = "Validation failed") => new (false, message, ResultKind.ValidationFailed);
    }

    public record Result<T> (bool Success, T? Value, string? Error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new(true, value);
        public static Result<T> NotFound(string message = "Not found") => new(false, default, message, ResultKind.NotFound);
        public static Result<T> fail(string message, ResultKind kind = ResultKind.Conflict) => new(false, default, message, kind);
        public static Result<T> ValidationFailed(string message = "Validation failed") => new(false, default, message, ResultKind.ValidationFailed);
    }
}
