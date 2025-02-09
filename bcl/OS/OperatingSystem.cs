using System.Runtime.InteropServices;

namespace Bearz.OS;

public static class OperatingSystem
{
    public static Architecture Arch => RuntimeInformation.OSArchitecture;

#if NET8_0_OR_GREATER

#else
    public static bool Is64Bit => RuntimeInformation.OSArchitecture == Architecture.X64 || RuntimeInformation.OSArchitecture == Architecture.Arm64;

    public static bool IsWindows()
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    public static bool IsDarwin() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
}