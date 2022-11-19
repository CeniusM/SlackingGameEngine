using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using SlackingGameEngine.Render;

namespace SlackingGameEngine;

internal unsafe class WindowsAPI
{
    [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern SafeFileHandle CreateFile(
      string fileName,
      [MarshalAs(UnmanagedType.U4)] uint fileAccess,
      [MarshalAs(UnmanagedType.U4)] uint fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
      [MarshalAs(UnmanagedType.U4)] int flags,
      IntPtr template);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool WriteConsoleOutputW(
      SafeFileHandle hConsoleOutput,
      Pixel* lpBuffer,
      Coord dwBufferSize,
      Coord dwBufferCoord,
      ref CoordRect lpWriteRegion);
}
