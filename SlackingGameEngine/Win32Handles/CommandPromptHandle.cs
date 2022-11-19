using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using SlackingGameEngine.Render;

namespace SlackingGameEngine.Win32Handles;


internal unsafe class CommandPromptHandle
{
    SafeFileHandle handle;

    internal CommandPromptHandle()
    {
        // Get cmd buffer
        handle = WindowsAPI.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        if (handle.IsInvalid)
            throw new Exception("Were not able to create cmd SafeFileHandle");
    }

    internal void SetWindowSizeToBuffer(PixelBuffer* buffer)
    {
        if ((uint)buffer == 0)
            throw new NullReferenceException("Buffer have not been initialized");

#pragma warning disable CA1416 // Validate platform compatibility
        Console.SetWindowSize(buffer->width, buffer->height);
        Console.SetBufferSize(buffer->width, buffer->height);
#pragma warning restore CA1416 // Validate platform compatibility
    }

    internal bool RenderBuffer(PixelBuffer* buffer)
    {
        if ((uint)buffer == 0)
            throw new NullReferenceException("Buffer have not been initialized");

        Console.CursorVisible = false;

        // Stack allocated struct
        var s = new CoordRect() { Left = 0, Top = 0, Right = buffer->width, Bottom = buffer->height };
        return WindowsAPI.WriteConsoleOutputW(handle, buffer->buffer, new Coord(buffer->width, buffer->height), new Coord(0, 0), ref s);
    }



    //~CommandPromptHandle()
    //{
    //    if (buffer is not null)
    //        Marshal.FreeHGlobal((IntPtr)buffer);
    //}
}