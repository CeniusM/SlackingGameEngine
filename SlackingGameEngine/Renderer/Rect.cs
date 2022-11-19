using System.Runtime.InteropServices;

namespace SlackingGameEngine.Render;

[StructLayout(LayoutKind.Sequential)]
public ref struct Rect
{
    public short x;
    public short y;
    public short w;
    public short h;
}