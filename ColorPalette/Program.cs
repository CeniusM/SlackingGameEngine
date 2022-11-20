using SlackingGameEngine;
using SlackingGameEngine.Render;

const int ScreenWidth = 160;
const int ScreenHeight = 80;

// Init engine
SlackingGameEngine.SlackingGameEngine engine = new SlackingGameEngine.SlackingGameEngine(ScreenWidth, ScreenHeight);

engine.Start();
engine.ShowFPS = true;

IntPtr buffer = engine.GetActiveBuffer();

while (true)
{
    for (ushort i = 0; i < 16; i++)
    {
        for (ushort j = 0; j < 16; j++)
        {
            Renderer.RenderRect(buffer, (ushort)(i * 10), (ushort)(j * 10), 16, 10, new Pixel(Renderer.HALF, (byte)i, (byte)j));
        }
    }


    engine.RenderBuffer();
    Thread.Sleep(100);
}
