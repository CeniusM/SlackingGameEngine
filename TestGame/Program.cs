using SlackingGameEngine.Win32Handles;
using System.Diagnostics;
using SlackingGameEngine.Render;

namespace TestGame;

class Program
{
    static void Main()
    {

        var game = new SlackingGameEngine.SlackingGameEngine(100, 50);

        game.ShowFPS = true;
        game.Start();

        IntPtr buffer = game.GetActiveBuffer();
        Random rand = new Random();
        ushort x = 0;
        ushort y = 0;

        while (true)
        {
            game.Update();

            Renderer.Clear(buffer, new Pixel());

            if (game.GetKeyState('W'))
                Renderer.RenderRect(buffer, x, y, 50, 25, new Pixel(Renderer.HIGH, ConsoleColor.Green, ConsoleColor.Black));
            else
                Renderer.RenderRect(buffer, x, y, 50, 25, new Pixel(Renderer.HIGH, ConsoleColor.Red, ConsoleColor.Black));



            string str = "";
            for (int i = 0; i < 256; i++)
                str += (game.GetKeyState((char)i) ? i + "True. " : "");

            //Renderer.RenderText(buffer, 0, 10, "Hello: " + x.ToString(), new Pixel(0, ConsoleColor.White, ConsoleColor.Black));
            Renderer.RenderText(buffer, 0, 10, str, Color.GetColor(ConsoleColor.White, ConsoleColor.Black));

            x++;
            if (x >= 100)
                x = 0;


            game.RenderBuffer();

            //Thread.Sleep(100);
        }
    }
}