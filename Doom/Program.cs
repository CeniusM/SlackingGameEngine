using SlackingGameEngine;
using SlackingGameEngine.Render;

namespace Game;

class Doom
{
    static void Main()
    {
        // Init engine
        SlackingGameEngine.SlackingGameEngine engine = new SlackingGameEngine.SlackingGameEngine(100, 50);

        engine.Start();
        engine.ShowFPS = true;

        IntPtr buffer = engine.GetActiveBuffer();


        // Init game data
        string[] MapConstruct =
        {
            "##########",
            "#      # #",
            "# ##   # #",
            "#      # #",
            "#        #",
            "#  ####  #",
            "#        #",
            "####     #",
            "#       ##",
            "##########",
        };

        int MapWidth = MapConstruct[0].Length;
        int MapHeight = MapConstruct.Length;

        List<string> DisblayMap = new List<string>();
        int[,] Map = new int[MapHeight, MapWidth];
        for (int i = 0; i < MapHeight; i++)
        {
            DisblayMap.Add("");
            for (int j = 0; j < MapWidth; j++)
            {
                if (MapConstruct[i][j] == '#')
                {
                    DisblayMap[i] += "[]";
                    Map[i, j] = 1;
                }
                else
                    DisblayMap[i] += "..";

            }
        }

        const float WalkSpeed = 0.01f;
        float PlayerX = 0;
        float PlayerY = 0;
        float PlayerAngle = 0;

        void MoveTo()
        {

        }

        while (true)
        {
            engine.Update();

            // Game Logic
            if (engine.GetKeyState('W'))
                PlayerY -= WalkSpeed * engine.DeltaF;
            if (engine.GetKeyState('S'))
                PlayerY += WalkSpeed * engine.DeltaF;
            if (engine.GetKeyState('A'))
                PlayerX -= WalkSpeed * engine.DeltaF;
            if (engine.GetKeyState('D'))
                PlayerX += WalkSpeed * engine.DeltaF;

            // Render map
            for (int i = 0; i < DisblayMap.Count; i++)
            {
                Renderer.RenderText(buffer, 0, (ushort)i, DisblayMap[i], Color.GetColor(ConsoleColor.White, ConsoleColor.Black));
            }
            // Player
            int x = (int)PlayerX;
            int y = (int)PlayerY;
            if (x < MapWidth && x > -1 && y < MapHeight && y > -1)
                Renderer.RenderText(buffer, (ushort)(x * 2), (ushort)y, "!!", Color.GetColor(ConsoleColor.Red, ConsoleColor.Black));

            // Render world


            Thread.Sleep(10);

            engine.RenderBuffer();
        }

    }
}