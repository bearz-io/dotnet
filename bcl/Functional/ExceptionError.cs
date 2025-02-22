using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Bearz;

public class ExceptionError : Error
{
    private readonly Exception? ex;

#pragma warning disable S2933 // false positive. can't be read only since we set it in the constructor.
    private string code = "Error";

    public ExceptionError(string message, IInnerError? inner = null)
        : base(message, inner)
    {
        this.ex = null;
    }

    public ExceptionError(Exception ex)
        : this(ex, true)
    {
        this.ex = ex;
    }

    protected ExceptionError(Exception ex, bool setCode)
        : base(ex.Message, ex.InnerException is null ? null : new ExceptionError(ex.InnerException))
    {
        this.ex = null;

        if (!setCode)
            return;

        var e = ex.GetType().Name;

        // Remove the "Exception" suffix
        this.code = e.Replace("Exception", "Error");
    }

    public override string Code => this.code;

    [JsonIgnore]
    public override string? StackTrace
    {
        get
        {
            if (base.StackTrace is not null)
                return base.StackTrace;

            if (this.ex is null)
                return null;

            base.StackTrace = this.ex.StackTrace;
            return this.ex.StackTrace;
        }

        protected set => base.StackTrace = value;
    }

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
}