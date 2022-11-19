using System.Runtime.InteropServices;

namespace SlackingGameEngine.Render;

[StructLayout(LayoutKind.Sequential)]
public ref struct Rect
{
    public ushort x;
    public ushort y;
    public ushort w;
    public ushort h;
}