﻿using System.Runtime.InteropServices;

namespace SlackingGameEngine.Render;

/// <summary>
/// Used as a pixel on the command prompt. Has the size of an uint
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct Pixel
{
    /// <summary>
    /// The character that will disblayed represented in unicode 
    /// </summary>
    [FieldOffset(0)] public short Char;

    /// <summary>
    /// The color of the character disblayed, the 0b00001111 bits are forground color, and 0b11110000 is background color. All using the ConsoleColor enum.
    /// </summary>
    [FieldOffset(2)] public short Color;


    public Pixel(short c, byte foreground, byte background)
    {
        Char = c;
        Color = (short)((int)foreground | ((int)background << 4));
    }

    public Pixel(short c, ConsoleColor foreground, ConsoleColor background)
    {
        Char = c;
        Color = (short)((int)foreground | ((int)background << 4));
    }

    public Pixel(short c, short color)
    {
        Char = c;
        Color = color;
    }
}
