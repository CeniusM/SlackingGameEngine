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
            "#    P   #",
            "#  ####  #",
            "#        #",
            "####     #",
            "#       ##",
            "##########",
        };

        int MapWidth = MapConstruct[0].Length;
        int MapHeight = MapConstruct.Length;

        const float WalkSpeed = 0.001f;
        const float TurnSpeed = 0.01f;
        float PlayerX = 0;
        float PlayerY = 0;
        // Radiants, 0 - ~6.283
        float PlayerAngle = 0;

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
                else if (MapConstruct[i][j] == 'P')
                {
                    PlayerX = j;
                    PlayerY = i;
                    DisblayMap[i] += "..";
                }
                else
                    DisblayMap[i] += "..";

            }
        }

        void Move(float offSet)
        {
            float x = MathF.Cos(PlayerAngle + offSet) * engine.DeltaF * WalkSpeed;
            float y = MathF.Sin(PlayerAngle + offSet) * engine.DeltaF * WalkSpeed;

            PlayerX += x;
            PlayerY += y;

            if (PlayerX >= MapWidth)
                PlayerX = MapWidth - 1;
            else if (PlayerX < 0)
                PlayerX = 0;
            if (PlayerY >= MapHeight)
                PlayerY = MapHeight - 1;
            else if (PlayerY < 0)
                PlayerY = 0;
        }

        while (true)
        {
            engine.Update();

            // Game Logic
            // Turn
            if (engine.GetKeyState((char)37)) // Left
            {
                PlayerAngle -= TurnSpeed * engine.DeltaF;
                if (PlayerAngle < 0)
                    PlayerAngle += 6.9f;
            }
            if (engine.GetKeyState((char)39)) // right
            {
                PlayerAngle += TurnSpeed * engine.DeltaF;
                if (PlayerAngle > 6.9f)
                    PlayerAngle -= 6.9f;
            }

            // Move
            if (engine.GetKeyState('W'))
                Move(0f);
            if (engine.GetKeyState('S'))
                Move(3.14f);
            if (engine.GetKeyState('A'))
                Move(4.7f);
            if (engine.GetKeyState('D'))
                Move(1.57f);





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