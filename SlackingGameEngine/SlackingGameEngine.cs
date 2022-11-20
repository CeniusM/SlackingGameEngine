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

    public SlackingGameEngine(ushort width, ushort height)
    {
        cmdHandle = new CommandPromptHandle();
        keyboardHandle = new KeyboardHandle();
        
        activeBuffer = PixelBuffer.GetNewPixelBuffer(width, height);
        cmdHandle.SetWindowSizeToBuffer(activeBuffer);
    }

    /// <summary>
    /// True = pressed.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKeyState(char key) => keyboardHandle.GetKeyState(key);

    public IntPtr GetActiveBuffer() => new IntPtr(activeBuffer);
    public void SetActiveBuffer(IntPtr* buffer) => activeBuffer = (PixelBuffer*)buffer;

    public ushort GetWidthOfBuffer() => activeBuffer->width;
    public ushort GetHeightOfBuffer() => activeBuffer->height;

    public void Start()
    {

    }

    public void Update()
    {
        keyboardHandle.Update();
    }

    public bool ShowFPS = false;
    public float DeltaF = 0;
    public double Delta = 0;
    private Stopwatch sw = new Stopwatch();
    public void RenderBuffer()
    {
        if (ShowFPS)
            Console.Title = sw.ElapsedMilliseconds.ToString();
        cmdHandle.RenderBuffer(activeBuffer);
        Delta = sw.Elapsed.TotalMilliseconds;
        DeltaF = (float)Delta;
        sw.Restart();
    }

    ~SlackingGameEngine()
    {
        PixelBuffer.DeletePixelBuffer(activeBuffer);
    }
}