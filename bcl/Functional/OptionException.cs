using System.Diagnostics;

// ReSharper disable VirtualMemberCallInConstructor
namespace Bearz;

#if NETLEGACY
[System.Serializable]
#endif
public class OptionException : System.Exception
{
    public OptionException()
    {
    }

    public OptionException(string? message)
        : base(message)
    {
    }

    public OptionException(string? message, System.Exception? inner)
        : base(message, inner)
    {
    }

#if NETLEGACY
    protected OptionException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }
#endif

    public virtual string Target { get; protected set; } = string.Empty;

    public virtual int LineNumber { get; protected set; }

    public virtual string FilePath { get; protected set; } = string.Empty;

    public OptionException TrackCallerInfo(
        [System.Runtime.CompilerServices.CallerLineNumber] int line = 0,
        [System.Runtime.CompilerServices.CallerFilePath] string file = "",
        [System.Runtime.CompilerServices.CallerMemberName] string target = "")
    {
        this.Target = target;
        this.LineNumber = line;
        this.FilePath = file;
        return this;
    }
}