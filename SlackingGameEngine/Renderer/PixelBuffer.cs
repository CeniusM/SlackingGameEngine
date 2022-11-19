using System.Runtime.InteropServices;

namespace SlackingGameEngine.Render;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PixelBuffer
{
    internal Pixel* buffer;
    internal uint bufferSize;
    internal short height;
    internal short width;

    public PixelBuffer(short height, short width, Pixel* buffer)
    {
        this.height = height;
        this.width = width;
        this.buffer = buffer;
        bufferSize = (uint)(height * width);
    }

    public static PixelBuffer* GetPixelBuffer(short Height, short Width)
    {
        // Argument check
        if (Height < 1 || Width < 1)
            throw new ArgumentException("Neither height nor width can be blow 1");

        IntPtr pixelBuffer = Marshal.AllocHGlobal(sizeof(PixelBuffer));
        IntPtr pixelArray = Marshal.AllocHGlobal(Height * Width);
        if (pixelArray == pixelBuffer)
        Console.WriteLine("HI");

        // Set buffer varibles
        PixelBuffer* buffer = (PixelBuffer*)Marshal.AllocHGlobal(sizeof(PixelBuffer));
        buffer->height = Height;
        buffer->width = Width;
        buffer->bufferSize = (uint)(Height * Width);
        buffer->buffer = (Pixel*)Marshal.AllocHGlobal((int)buffer->bufferSize * 4);
        ClearBuffer(buffer);

        return buffer;
    }

    public static void SetPixelBuffer(PixelBuffer* buffer, short Height, short Width)
    {
        // Argument check
        if (Height < 1 || Width < 1)
            throw new ArgumentException("Neither height nor width can be blow 1");

        // Delete buffer if it is allready allocated
        DeletePixelBuffer(buffer);

        // Set buffer varibles
        buffer->height = Height;
        buffer->width = Width;
        buffer->bufferSize = (uint)(Height * Width);
        buffer->buffer = (Pixel*)Marshal.AllocHGlobal((int)buffer->bufferSize * sizeof(Pixel));
        ClearBuffer(buffer);
    }

    public static void DeletePixelBuffer(PixelBuffer* buffer)
    {
        if ((uint)buffer->buffer != 0)
            Marshal.FreeHGlobal((IntPtr)buffer->buffer);
    }

    public static void ClearBuffer(PixelBuffer* buffer)
    {
        if ((uint)buffer->buffer == 0)
            throw new NullReferenceException("Buffer have not been initialized");

        uint* tempBufferPtr = (uint*)buffer->buffer;
        for (int i = 0; i < buffer->bufferSize; i++)
            tempBufferPtr[i] = 0;
    }
}
