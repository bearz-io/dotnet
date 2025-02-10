using Bearz.Text.DotEnv.Tokens;

namespace Bearz.Text.DotEnv.Documents;

public class DotEnvComment : DotEnvNode
{
    public DotEnvComment(ReadOnlySpan<char> value)
    {
        this.RawValue = value.ToArray();
    }

    public DotEnvComment(char[] value)
    {
        this.RawValue = value;
    }

    public DotEnvComment(EnvCommentToken token)
    {
        this.RawValue = token.RawValue;
    }
}