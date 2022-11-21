using System.Runtime.InteropServices;

namespace SlackingGameEngine.Render;

[StructLayout(LayoutKind.Sequential)]
public struct Color
{
    #region Colors

    // Not implementet

    public const uint FGBlack = (uint)ConsoleColor.Black;

    #endregion

    public short Value;

    public Color(short c, byte foreground, byte background)
    {
        Value = (short)((int)foreground | ((int)background << 4));
    }

    public Color(short c, ConsoleColor foreground, ConsoleColor background)
    {
        Value = (short)((int)foreground | ((int)background << 4));
    }

    public Color(short c, short color)
    {
        Value = color;
    }

    public static implicit operator short(Color color) => color.Value;

    #region Static
    public static short GetColor(ConsoleColor foreGround, ConsoleColor backGround)
    {
        return (short)((int)foreGround | ((int)backGround << 4));
    }

    public static short GetColor(byte foreGround, byte backGround)
    {
        return (short)((int)foreGround | ((int)backGround << 4));
    }

    public static short GetColor(short foreGround, short backGround)
    {
        return (short)((int)foreGround | ((int)backGround << 4));
    }
    #endregion
}
