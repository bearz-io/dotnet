namespace Bearz.OS.Win32;

[CLSCompliant(false)]
public enum WinCredPersistence : uint
{
    Session = 1,
    LocalMachine = 2,
    Enterprise = 3,
}