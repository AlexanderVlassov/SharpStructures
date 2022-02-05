using System;

namespace DataStructures;

public class Result<T>
{
    public static Result<T> Success(T obj)
    {
        return new Result<T>(obj, true, null);
    }

    public static Result<T> Failure(Exception exception)
    {
        return new Result<T>(default(T), false, exception);
    }

    private Result(T value, bool isSuccess, Exception exception)
    {
        IsSuccess = isSuccess;
        Exception = exception;
        Value = value;
    }

    public T Value { get; }
    public bool IsSuccess { get; }
    public Exception Exception { get; }
}