using System.Runtime.CompilerServices;

namespace Bearz;

public class AggregateError : ExceptionError
{
    public AggregateError()
        : base("One or more errors occurred.")
    {
    }

    public AggregateError(string message, params Exception[] exceptions)
        : base(message)
    {
        this.Errors = exceptions.Select(ErrorsConverter.Convert).ToArray();
    }

    public AggregateError(string message, params Error[] errors)
        : base(message)
    {
        this.Errors = errors;
    }

    public AggregateError(string message, IEnumerable<Error> errors)
        : base(message)
    {
        this.Errors = errors.ToArray();
    }

    public AggregateError(string message, IEnumerable<Exception> exceptions)
        : base(message)
    {
        this.Errors = exceptions.Select(ErrorsConverter.Convert).ToArray();
    }

    public AggregateError(AggregateException ex)
        : base(ex, false)
    {
        this.Errors = ex.InnerExceptions.Select(ErrorsConverter.Convert).ToArray();
    }

    public override string Code => "AggregateError";

    public Error[] Errors { get; set; } = [];

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
        this.Exception ??= new AggregateException(this.Message, this.Errors.Select(e => e.ToException()));

        return this.Exception;
    }
}