using System.Diagnostics;
using SlackingGameEngine.Win32Handles;
using SlackingGameEngine.Render;

namespace SlackingGameEngine;

/// <summary>
/// A Windows specific, commandpromt based game engine
/// </summary>
public unsafe class SlackingGameEngine
{
    internal CommandPromptHandle cmdHandle;
    internal KeyboardHandle keyboardHandle;
    internal PixelBuffer* activeBuffer;

    public SlackingGameEngine(short height, short width)
    {
        cmdHandle = new CommandPromptHandle();
        keyboardHandle = new KeyboardHandle();
        
        activeBuffer = PixelBuffer.GetPixelBuffer(height, width);
        cmdHandle.SetWindowSizeToBuffer(activeBuffer);
    }

    /// <summary>
    /// True = pressed.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKeyState(KeyCode key) => keyboardHandle.GetKeyState(key);

    public IntPtr GetActiveBuffer() => new IntPtr(activeBuffer);
    public void SetActiveBuffer(IntPtr* buffer) => activeBuffer = (PixelBuffer*)buffer;

    public short GetWidthOfBuffer() => activeBuffer->width;
    public short GetHeightOfBuffer() => activeBuffer->height;

    public void Start()
    {

    }

    public void Update()
    {
        keyboardHandle.Update();
    }

    public bool ShowFPS = false;
    private Stopwatch FPSWatch = new Stopwatch();
    public void RenderBuffer()
    {
        if (ShowFPS)
            Console.Title = FPSWatch.ElapsedMilliseconds.ToString();
        cmdHandle.RenderBuffer(activeBuffer);
        FPSWatch.Restart();
    }

    ~SlackingGameEngine()
    {
        PixelBuffer.DeletePixelBuffer(activeBuffer);
    }
}