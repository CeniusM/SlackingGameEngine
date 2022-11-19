using System.Runtime.InteropServices;

namespace SlackingGameEngine.Render;

[StructLayout(LayoutKind.Sequential)]
public ref struct CoordRect
{
    public ushort Left;
    public ushort Top;
    public ushort Right;
    public ushort Bottom;
}