using System.Runtime.InteropServices;

namespace SlackingGameEngine.Render;

[StructLayout(LayoutKind.Sequential)]
public ref struct CoordRect
{
    public short Left;
    public short Top;
    public short Right;
    public short Bottom;
}