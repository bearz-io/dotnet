using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Bearz;

public class Error : IError
{
    public Error(string? message = null, IInnerError? innerError = null)
    {
        this.Message = message ?? "Unknown error";
        this.InnerError = innerError;
    }

    [JsonPropertyName("message")]
    public string Message { get; protected set; }

    [JsonPropertyName("code")]
    public virtual string Code => "Error";

    public virtual string Target { get; protected set; } = string.Empty;

    [JsonIgnore]
    public virtual int LineNumber { get; protected set; }

    [JsonIgnore]
    public virtual string FilePath { get; protected set; } = string.Empty;

    [JsonIgnore]
    public virtual string? StackTrace { get; protected set; }

    [JsonPropertyName("innerError")]
    public IInnerError? InnerError { get; set; }

    protected Exception? Exception { get; set; }

    public static implicit operator Error(Exception ex)
        => ErrorsConverter.Convert(ex);

    public static Error New(string message)
        => new(message);

    public static Error New(string message, IInnerError inner)
        => new(message, inner);

    public static Error Format(string message, params object[] args)
        => new(string.Format(message, args));

    public Error TrackCallerInfo(
        [CallerLineNumber] int line = 0,
        [CallerFilePath] string file = "",
        [CallerMemberName] string target = "")
    {
        this.Target = target;
        this.LineNumber = line;
        this.FilePath = file;
        return this;
    }

    public bool Is(string code)
    {
        return this.Code == code;
    }

    public bool Is(Type type)
    {
        return type.IsInstanceOfType(this);
    }

    public bool Is<T>()
    {
        return this.Is(typeof(T));
    }

    public virtual Exception ToException()
    {
        this.Exception ??= new InvalidOperationException(this.Message);

        return this.Exception;
    }

    public override string ToString()
    {
        return this.Exception?.ToString() ?? this.Message;
    }
}