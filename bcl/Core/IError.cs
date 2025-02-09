namespace Bearz;

public interface IError : IInnerError
{
}

public interface IInnerError
{
    public string Message { get; }

    public string Code { get; }

    public IInnerError? InnerError { get; }
}

public interface IArgumentError : IError
{
    public string ArgumentName { get; }
}

public interface IArgumentValueError : IArgumentError
{
    public object? Value { get; }
}