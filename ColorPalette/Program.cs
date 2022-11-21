//#define ColorPalette
#define Shades

using SlackingGameEngine;
using SlackingGameEngine.Render;



#if ColorPalette
const int ScreenWidth = 160;
const int ScreenHeight = 80;

// Init engine
SlackingGameEngine.SlackingGameEngine engine = new SlackingGameEngine.SlackingGameEngine(ScreenWidth, ScreenHeight);

#elif Shades

const int ScreenWidth = 4 * 4 * 4 * 2;
const int ScreenHeight = 20;

// Init engine
SlackingGameEngine.SlackingGameEngine engine = new SlackingGameEngine.SlackingGameEngine(ScreenWidth, ScreenHeight);


#endif


engine.Start();
engine.ShowFPS = true;

IntPtr buffer = engine.GetActiveBuffer();


#if ColorPalette

#elif Shades

// Shades
Pixel[] shades = new Pixel[4 * 4 * 4];
ConsoleColor[] colorShades = new ConsoleColor[4];
colorShades[0] = ConsoleColor.White;
colorShades[1] = ConsoleColor.Gray;
colorShades[2] = ConsoleColor.DarkGray;
colorShades[3] = ConsoleColor.Black;

for (int i = 0; i < 4; i++)
{
    for (int j = 0; j < 4; j++)
    {
        for (int k = 0; k < 4; k++)
        {
            if (i == 0)
                shades[(k * 4 * 4) + (j * 4) + i] = new Pixel(Renderer.FULL, colorShades[k], colorShades[j]);
            else if (i == 1)
                shades[(k * 4 * 4) + (j * 4) + i] = new Pixel(Renderer.HIGH, colorShades[k], colorShades[j]);
            else if (i == 2)
                shades[(k * 4 * 4) + (j * 4) + i] = new Pixel(Renderer.HALF, colorShades[k], colorShades[j]);
            else if (i == 3)
                shades[(k * 4 * 4) + (j * 4) + i] = new Pixel(Renderer.LOW, colorShades[k], colorShades[j]);
        }
    }
}

#endif



while (true)
{
#if ColorPalette
    for (ushort i = 0; i < 16; i++)
    {
        for (ushort j = 0; j < 16; j++)
        {
            Renderer.RenderRect(buffer, (ushort)(i * 10), (ushort)(j * 10), 16, 10, new Pixel(Renderer.HALF, (byte)i, (byte)j));
        }
    }
#elif Shades

    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            for (int k = 0; k < 4; k++)
            {

                //for (int k = 0; k < 4; k++)
                //{
                //    Renderer.RenderRect(buffer, (ushort)(k * 2), 0, 2, 10, new Pixel(Renderer.FULL, colorShades[k], ConsoleColor.Black));
                //}

                // Not implemented, but will contain a range of (FULL, HIGH, HALF, LOW) * ForeGround(White, Gray, DarkGray, Dark) * BackGround(White, Gray, DarkGray, Dark)
                Renderer.RenderRect(buffer, (ushort)(((k * 4 * 4) + (j * 4) + i) * 2), 0, 2, 10, shades[(k * 4 * 4) + (j * 4) + i]);
            }
        }
    }


#endif


    engine.RenderBuffer();
    Thread.Sleep(100);
}
