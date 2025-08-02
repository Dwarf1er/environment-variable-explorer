using EnvironmentVariableExplorer.Shared;
using System;

namespace EnvironmentVariableExplorer.Extensions
{
    public static class ResultExtensions
    {
        public static Result<T, string> Try<T>(Func<T> func, string? errorMessagePrefix = null)
        {
            try
            {
                return Result<T, string>.Ok(func());
            }
            catch (Exception ex)
            {
                string message = errorMessagePrefix is not null
                    ? $"{errorMessagePrefix}: {ex.Message}"
                    : ex.Message;
                return Result<T, string>.Err(message);
            }
        }

        public static Result<Unit, string> Try(Action action, string? errorMessagePrefix = null)
        {
            try
            {
                action();
                return Result<Unit, string>.Ok(Unit.Value);
            }
            catch (Exception ex)
            {
                string message = errorMessagePrefix is not null
                    ? $"{errorMessagePrefix}: {ex.Message}"
                    : ex.Message;
                return Result<Unit, string>.Err(message);
            }
        }
    }
}
