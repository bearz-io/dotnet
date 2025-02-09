using System.Runtime.InteropServices;

namespace Bearz.OS.Linux;

[StructLayout(LayoutKind.Sequential)]
internal struct GError
{
    public uint Domain;

    public int Code;

    public IntPtr Message;
}