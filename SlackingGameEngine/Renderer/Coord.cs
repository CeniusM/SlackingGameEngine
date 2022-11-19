using System.Runtime.InteropServices;

namespace SlackingGameEngine.Render;

[StructLayout(LayoutKind.Sequential)]
public struct Coord
{
    public ushort X;
    public ushort Y;

    public Coord(ushort X, ushort Y)
    {
        this.X = X;
        this.Y = Y;
    }
};
