using SlackingGameEngine.Win32Handles;
using System.Diagnostics;
using SlackingGameEngine.Render;


var game = new SlackingGameEngine.SlackingGameEngine(100, 200);

game.ShowFPS = true;
game.Start();

IntPtr buffer = game.GetActiveBuffer();
Random rand = new Random();
short x = 0;
short y = 0;

while (true)
{
    game.Update();

    Renderer.Clear(buffer, new Pixel());

    if (game.GetKeyState(KeyCode.W))
        Renderer.RenderRect(buffer, x, y, 50, 25, new Pixel(Renderer.HIGH, ConsoleColor.Green, ConsoleColor.Black));
    else
        Renderer.RenderRect(buffer, x, y, 50, 25, new Pixel(Renderer.HIGH, ConsoleColor.Red, ConsoleColor.Black));

    x++;
    if (x >= 200)
        x = -50;


    game.RenderBuffer();

    //Thread.Sleep(100);
}