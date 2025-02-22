using Bearz.OS;
using Bearz.Text.DotEnv.Documents;
using Bearz.Text.DotEnv.Serialization;

namespace Bearz.Text.DotEnv;

public static class DotEnvLoader
{
    public static DotEnvDocument Parse(DotEnvLoadOptions options)
    {
        if (options.Files.Count == 1 && options.Content is null)
            return Serializer.DeserializeDocument(options.Files[0], options);
        else if (options.Files.Count == 0 && options.Content is not null)
            return Serializer.DeserializeDocument(options.Content, options);
        else if (options.Files.Count == 0 && options.Content is null)
            return new DotEnvDocument();

        DotEnvDocument doc = new();
        if (options.Files.Count > 0)
        {
            foreach (var file in options.Files)
            {
                var clone = (DotEnvLoadOptions)options.Clone();
                clone.ExpandVariables = doc;
                var d = (IDictionary<string, string>)DotEnvSerializer.DeserializeDocument(file, options);
                foreach (var pair in d)
                {
                    doc[pair.Key] = pair.Value;
                }
            }
        }

        if (options.Content is not null)
        {
            var clone = (DotEnvLoadOptions)options.Clone();
            clone.ExpandVariables = doc;
            var d = (IDictionary<string, string>)DotEnvSerializer.DeserializeDocument(options.Content, options);
            foreach (var pair in d)
            {
                doc[pair.Key] = pair.Value;
            }
        }

        return doc;
    }

    public static void Load(DotEnvLoadOptions options)
    {
        var doc = Parse(options);
        foreach (var entry in doc)
        {
            if (entry is DotEnvEntry var && (options.OverrideEnvironment || !Env.Has(var.Name)))
            {
                Env.Set(var.Name, var.Value);
            }
        }
    }
}