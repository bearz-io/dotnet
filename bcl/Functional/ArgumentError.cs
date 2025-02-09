using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Bearz;

public class ArgumentError : ExceptionError, IArgumentError
{
    public ArgumentError()
        : base("Argument is invalid.")
    {
        this.ArgumentName = string.Empty;
    }

    public ArgumentError(string argumentName, string? message = null, IInnerError? inner = null)
        : base(message ?? $"Argument {argumentName} is invalid", inner)
    {
        this.ArgumentName = argumentName;
    }

    public ArgumentError(ArgumentException ex)
        : base(ex, false)
    {
        this.ArgumentName = ex.ParamName ?? string.Empty;
    }

    public override string Code => "ArgumentError";

    [JsonPropertyName("argument")]
    public string ArgumentName { get; set; }

    public new ExceptionError TrackCallerInfo(
        [CallerLineNumber] int line = 0,
        [CallerFilePath] string file = "",
        [CallerMemberName] string target = "")
    {
        this.Target = target;
        this.LineNumber = line;
        this.FilePath = file;
        return this;
    }

    public override Exception ToException()
    {
        this.Exception ??= new ArgumentException(this.Message, this.ArgumentName);

        return this.Exception;
    }
}

public class ArgumentNullError : ArgumentError
{
    public ArgumentNullError()
        : base("Argument is null.")
    {
        this.ArgumentName = string.Empty;
    }

    public ArgumentNullError(string argumentName, string? message = null, IInnerError? inner = null)
        : base(argumentName, message ?? $"Argument {argumentName} is null.", inner)
    {
        this.ArgumentName = argumentName;
    }

    public ArgumentNullError(ArgumentNullException ex)
        : base(ex)
    {
    }

    public override string Code => "ArgumentNullError";

    public new ExceptionError TrackCallerInfo(
        [CallerLineNumber] int line = 0,
        [CallerFilePath] string file = "",
        [CallerMemberName] string target = "")
    {
        this.Target = target;
        this.LineNumber = line;
        this.FilePath = file;
        return this;
    }

    public override Exception ToException()
    {
        this.Exception ??= new ArgumentNullException(this.ArgumentName, this.Message);

        return this.Exception;
    }
}

public class ArgumentOutOfRangeError : ArgumentError
{
    public ArgumentOutOfRangeError()
        : base("Argument is out of range.")
    {
        this.ArgumentName = string.Empty;
    }

    public ArgumentOutOfRangeError(string argumentName)
        : this(argumentName, null, null, null)
    {
        this.ArgumentName = argumentName;
    }

    public ArgumentOutOfRangeError(string argumentName, object? value)
        : this(argumentName, value, null, null)
    {
        this.ArgumentName = argumentName;
    }

    public ArgumentOutOfRangeError(string argumentName, string? message)
        : this(argumentName, null, message, null)
    {
        this.ArgumentName = argumentName;
    }

    public ArgumentOutOfRangeError(string argumentName, object? value, string? message)
        : this(argumentName, value, message, null)
    {
        this.ArgumentName = argumentName;
    }

    public ArgumentOutOfRangeError(string argumentName, object? value, string? message, IInnerError? inner)
        : base(argumentName, message ?? $"Argument {argumentName} with is out of range.", inner)
    {
        this.ArgumentName = argumentName;
        this.Value = value;
    }

    public ArgumentOutOfRangeError(ArgumentOutOfRangeException ex)
        : base(ex)
    {
        this.ArgumentName = ex.ParamName ?? string.Empty;
    }

    public override string Code => "ArgumentRangeError";

    public object? Value { get; set; }

    public new ArgumentOutOfRangeError TrackCallerInfo(
        [CallerLineNumber] int line = 0,
        [CallerFilePath] string file = "",
        [CallerMemberName] string target = "")
    {
        this.Target = target;
        this.LineNumber = line;
        this.FilePath = file;
        return this;
    }
}