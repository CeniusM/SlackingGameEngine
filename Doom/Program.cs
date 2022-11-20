using SlackingGameEngine;
using SlackingGameEngine.Render;

namespace Game;

class Doom
{
    const int ScreenWidth = 150;
    const int ScreenHeight = 50;

    static void Main()
    {
        // Init engine
        SlackingGameEngine.SlackingGameEngine engine = new SlackingGameEngine.SlackingGameEngine(ScreenWidth, ScreenHeight);

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
        const float TurnSpeed = 0.002f;
        const float FOV = 1.57f; // 90 degress
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

        // Will be a one d line that tells us how far away a wall is, 0 very close, 100 very far
        // This tells us how tall to make the lines
        int[] VeiwLine = new int[engine.GetWidthOfBuffer()];

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
            string DebugMessage = "";
            engine.Update();

            // Game Logic
            // Turn
            if (engine.GetKeyState((char)37)) // Left
            {
                PlayerAngle -= TurnSpeed * engine.DeltaF;
                if (PlayerAngle < 0)
                    PlayerAngle += 6.28f;
            }
            if (engine.GetKeyState((char)39)) // right
            {
                PlayerAngle += TurnSpeed * engine.DeltaF;
                if (PlayerAngle > 6.28f)
                    PlayerAngle -= 6.28f;
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



            // -- Render --
            Renderer.Clear(buffer, new Pixel());

            // Render world
            // Floor
            Renderer.RenderRect(buffer, 0, 30, ScreenWidth, 10, new Pixel(Renderer.LOW, ConsoleColor.Green, ConsoleColor.Black));
            Renderer.RenderRect(buffer, 0, 40, ScreenWidth, 10, new Pixel(Renderer.HALF, ConsoleColor.Green, ConsoleColor.Black));

            // Generate VeiwLine
            for (ushort i = 0; i < ScreenWidth; i++)
            {
                // Get vector for ray
                const float StepSize = 0.1f;
                float xPos = PlayerX;
                float yPos = PlayerY;
                float xVec = MathF.Cos(PlayerAngle + FOV / 2 / ScreenWidth * (i - ScreenWidth / 2)) * StepSize;
                float yVec = MathF.Sin(PlayerAngle + FOV / 2 / ScreenWidth * (i - ScreenWidth / 2)) * StepSize;

                if (i == 0)
                    DebugMessage += xVec.ToString();

                // Cast ray and set distace value
                const int MaxSteps = 100;
                for (int step = 0; step < MaxSteps; step++)
                {
                    int tempX = (int)xPos;
                    int tempY = (int)yPos;

                    if (tempX < 0 || tempY < 0 || tempX >= MapWidth || tempY >= MapHeight)
                        break;

                    if (Map[tempX, tempY] != 0)
                    {
                        VeiwLine[i] = step;
                        break;
                    }
                    else
                    {
                        xPos += xVec;
                        yPos += yVec;
                    }
                }
                if (i == 0)
                    DebugMessage += " " + VeiwLine[i].ToString();
            }

            // 10 = 40 h
            // 10 = 5 y

            // 20 = 30 h
            // 20= 10 y

            // Disblay VeiwLine
            for (ushort i = 0; i < ScreenWidth; i++)
            {
                if (VeiwLine[i] < ScreenHeight / 5 * 1)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel(Renderer.FULL, ConsoleColor.White, ConsoleColor.Black));
                else if (VeiwLine[i] < ScreenHeight / 5 * 2)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel(Renderer.HIGH, ConsoleColor.White, ConsoleColor.Black));
                else if (VeiwLine[i] < ScreenHeight / 5 * 3)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel(Renderer.HALF, ConsoleColor.White, ConsoleColor.Black));
                else if (VeiwLine[i] < ScreenHeight / 5 * 4)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel(Renderer.LOW, ConsoleColor.White, ConsoleColor.Black));
                else if (VeiwLine[i] < ScreenHeight / 5 * 5)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel((short)'-', ConsoleColor.White, ConsoleColor.Black));
                

            }




            // Render map
            for (ushort i = 0; i < DisblayMap.Count; i++)
            {
                Renderer.RenderText(buffer, 0, i, DisblayMap[i], Color.GetColor(ConsoleColor.White, ConsoleColor.Black));
            }

            //for (ushort i = 0; i < 10; i++)
            //{
            //    for (ushort j = 0; j < 10; j++)
            //    {
            //        Renderer.RenderText(buffer, j, (ushort)(i + 10), Map[i, j] + "", Color.GetColor(ConsoleColor.White, ConsoleColor.Black));

            //    }
            //}


            // Player
            int x = (int)PlayerX;
            int y = (int)PlayerY;
            if (x < MapWidth && x > -1 && y < MapHeight && y > -1)
                Renderer.RenderText(buffer, (ushort)(x * 2), (ushort)y, "!!", Color.GetColor(ConsoleColor.Red, ConsoleColor.Black));




            Renderer.RenderText(buffer, 0, 20, DebugMessage, Color.GetColor(ConsoleColor.White, ConsoleColor.Black));

            Thread.Sleep(10);

            engine.RenderBuffer();
        }

    }
}