using SlackingGameEngine.Win32Handles;
using System.Runtime.InteropServices;
using Unicode = System.Int16;

namespace SlackingGameEngine.Render;

public unsafe class Renderer
{
    #region Consts
    //  gradiants
    /*
    █ full			= UTF-16 (hex) 0x2588 = alt 219
    ▓ sort of full	= UTF-16 (hex) 0x2593 = alt 178
    ▒ half			= UTF-16 (hex) 0x2592 = alt 177
    ░ low			= UTF-16 (hex) 0x2591 = alt 176
   ' ' empty		= UTF-16 (hex) 0x20   = spacebar
    */
    public const Unicode FULL = 0x2588;
    public const Unicode HIGH = 0x2593;
    public const Unicode HALF = 0x2592;
    public const Unicode LOW = 0x2591;
    public const Unicode EMPTY = 0x20;

    /// <summary>
    /// Range 0 - 4
    /// </summary>
    public readonly int[] GRADIANTS = {
        0x20,
        0x2591,
        0x2592,
        0x2593,
        0x2588,
    };

    // Colors in byte
    public const byte BLACK = (byte)ConsoleColor.Black;
    public const byte DARKBLUE = (byte)ConsoleColor.DarkBlue;
    public const byte DARKGREEN = (byte)ConsoleColor.DarkGreen;
    public const byte DARKCYAN = (byte)ConsoleColor.DarkCyan;
    public const byte DARKRED = (byte)ConsoleColor.DarkRed;
    public const byte DARKMAGENTA = (byte)ConsoleColor.DarkMagenta;
    public const byte DARKYELLOW = (byte)ConsoleColor.DarkYellow;
    public const byte GRAY = (byte)ConsoleColor.Gray;
    public const byte DARKGRAY = (byte)ConsoleColor.DarkGray;
    public const byte BLUE = (byte)ConsoleColor.Blue;
    public const byte GREEN = (byte)ConsoleColor.Green;
    public const byte CYAN = (byte)ConsoleColor.Cyan;
    public const byte RED = (byte)ConsoleColor.Red;
    public const byte MAGENTA = (byte)ConsoleColor.Magenta;
    public const byte YELLOW = (byte)ConsoleColor.Yellow;
    public const byte WHITE = (byte)ConsoleColor.White;

    // Color masks
    public const int FOREGROUND_MASK = 0b00001111;
    public const int BACKGROUND_MASK = 0b11110000;
    #endregion

    public static void Clear(IntPtr buffer, Pixel pixel) =>
                       Clear((PixelBuffer*)buffer, pixel);
    public static void Clear(PixelBuffer* buffer, Pixel pixel)
    {
        uint bufferSize = buffer->bufferSize;
        uint* bufferPtr = (uint*)buffer->buffer;
        for (int i = 0; i < bufferSize; i++)
            bufferPtr[i] = 0;
    }

    #region RectRender
    public static void RenderRect(IntPtr buffer, ushort x, ushort y, ushort width, ushort height, Pixel pixel) =>
                       RenderRect((PixelBuffer*)buffer, x, y, width, height, pixel);
    public static void RenderRect(PixelBuffer* buffer, ushort x, ushort y, ushort width, ushort height, Pixel pixel)
    {
        int Left = x < buffer->width ? x : buffer->width;
        int Top = y < buffer->height ? y : buffer->height;
        width = x + width > buffer->width ? (ushort)(buffer->width - Left) : width;
        int Bottom = y + height > buffer->height ? buffer->height : height + Top;

        for (int i = Top; i < Bottom; i++)
        {
            Pixel* ptr = &buffer->buffer[i * buffer->width + Left];
            for (int j = 0; j < width; j++)
            {
                ptr[j] = pixel;
            }
        }
    }
    #endregion

    #region TextRender

    public static void RenderText(IntPtr buffer, ushort Left, ushort Top, string text, short color)
    {
        ushort lenght = (ushort)text.Length;
        Unicode[] newText = new Unicode[lenght];

        for (ushort i = 0; i < lenght; i++)
            newText[i] = (Unicode)text[i];

        RenderText((PixelBuffer*)buffer, Left, Top, newText, color);
    }

    public static void RenderText(PixelBuffer* buffer, ushort Left, ushort Top, Unicode[] text, short color)
    {
        ushort Lenght = (ushort)text.Length;

        if (buffer->height <= Top)
            return;
        if (buffer->width < Left + Lenght)
            Lenght = (ushort)(buffer->width - Left);

        Pixel* StartOfBuffer = &buffer->buffer[Top * buffer->width + Left];
        Pixel pixel = new Pixel((Unicode)' ', color);
        for (int i = 0; i < Lenght; i++)
        {
            pixel.Char = text[i];
            StartOfBuffer[i] = pixel;
        }


    }
    #endregion

}
