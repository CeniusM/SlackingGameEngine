using SlackingGameEngine;
using SlackingGameEngine.Rendering;
using SlackingGameEngine.Utility;

namespace Game;

class Doom
{
    const int ScreenWidth = 300;
    const int ScreenHeight = 100;

    const float XSizeOfMap = 0.002f * ScreenWidth;
    const float YSizeOfMap = 0.002f * ScreenHeight;
    const float SizeOfPlayer = 0.0001f;

    static void Main()
    {
        // Init engine
        SlackingGameEngine.SlackingGameEngine engine = new SlackingGameEngine.SlackingGameEngine(ScreenWidth, ScreenHeight);

        KeyBoard keyBoard = engine.KeyBoard;

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

        bool ShowMap = true;

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
            if (keyBoard.IsKeyPressed(37)) // Left
            {
                PlayerAngle -= TurnSpeed * engine.DeltaF;
                if (PlayerAngle < 0)
                    PlayerAngle += 6.28f;
            }
            if (keyBoard.IsKeyPressed(39)) // right
            {
                PlayerAngle += TurnSpeed * engine.DeltaF;
                if (PlayerAngle > 6.28f)
                    PlayerAngle -= 6.28f;
            }

            // Move
            if (keyBoard.IsKeyPressed('W'))
                Move(0f);
            if (keyBoard.IsKeyPressed('S'))
                Move(3.14f);
            if (keyBoard.IsKeyPressed('A'))
                Move(4.7f);
            if (keyBoard.IsKeyPressed('D'))
                Move(1.57f);

            if (keyBoard.IsKeyJustPressed(9))
                ShowMap = true;
            if (keyBoard.IsKeyJustReleased(9))
                ShowMap = false;



            // -- Render --
            Renderer.Clear(buffer, new Pixel());

            // Render world
            // Floor
            Renderer.RenderRect(buffer, 0, (ushort)(ScreenHeight - ScreenHeight / 10 * 3), ScreenWidth, (ushort)(ScreenHeight / 10), new Pixel(Unicode.LOW, Color.DarkGreen, Color.Black));
            Renderer.RenderRect(buffer, 0, (ushort)(ScreenHeight - ScreenHeight / 10 * 2), ScreenWidth, (ushort)(ScreenHeight / 10), new Pixel(Unicode.LOW, Color.Green, Color.Black));
            Renderer.RenderRect(buffer, 0, (ushort)(ScreenHeight - ScreenHeight / 10 * 1), ScreenWidth, (ushort)(ScreenHeight / 10), new Pixel(Unicode.HALF, Color.DarkGreen, Color.Black));

            // Generate VeiwLine
            for (ushort i = 0; i < ScreenWidth; i++)
            {
                // Get vector for ray
                const float StepSize = 0.001f;
                float xPos = PlayerX;
                float yPos = PlayerY;
                float xVec = MathF.Cos(PlayerAngle + FOV / ScreenWidth * (i - ScreenWidth / 2)) * StepSize;
                float yVec = MathF.Sin(PlayerAngle + FOV / ScreenWidth * (i - ScreenWidth / 2)) * StepSize;

                if (i == 0)
                    DebugMessage += xVec.ToString();

                // Cast ray and set distace value
                const int MaxSteps = 10000;
                VeiwLine[i] = MaxSteps;
                for (int step = 0; step < MaxSteps; step++)
                {
                    int tempX = (int)xPos;
                    int tempY = (int)yPos;

                    if (tempX < 0 || tempY < 0 || tempX >= MapWidth || tempY >= MapHeight)
                        break;

                    if (Map[tempX, tempY] != 0)
                    {
                        VeiwLine[i] = step / 100;
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
                //ushort line = (ushort)(100 - VeiwLine[i]);
                //Renderer.RenderRect(buffer, i, (ushort)(line), 1, (ushort)(line), new Pixel(Unicode.HIGH, Color.White, Color.DarkGray));
                if (VeiwLine[i] < ScreenHeight / 5 * 1)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel(Unicode.HIGH, Color.White, Color.DarkGray));
                else if (VeiwLine[i] < ScreenHeight / 5 * 2)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel(Unicode.HIGH, Color.White, Color.Black));
                else if (VeiwLine[i] < ScreenHeight / 5 * 3)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel(Unicode.HALF, Color.White, Color.DarkGray));
                else if (VeiwLine[i] < ScreenHeight / 5 * 4)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel(Unicode.LOW, Color.White, Color.Black));
                else if (VeiwLine[i] < ScreenHeight / 5 * 5)
                    Renderer.RenderRect(buffer, i, (ushort)(VeiwLine[i] >> 1), 1, (ushort)(ScreenHeight - VeiwLine[i]), new Pixel((short)'-', Color.White, Color.Black));


            }




            // Render map
            //if (ShowMap)
            //for (ushort i = 0; i < DisblayMap.Count; i++)
            //{
            //    Renderer.RenderText(buffer, 0, i, DisblayMap[i], Color.GetColor(Color.White, Color.Black));
            //}
            //// Player
            //int x = (int)PlayerX;
            //int y = (int)PlayerY;
            //if (ShowMap)
            //if (x < MapWidth && x > -1 && y < MapHeight && y > -1)
            //    Renderer.RenderText(buffer, (ushort)(x * 2), (ushort)y, "!!", Color.GetColor(Color.Red, Color.Black));
            if (ShowMap)
            {
                ushort tileSizeX = (ushort)(MapWidth * XSizeOfMap);
                ushort tileSizeY = (ushort)(MapHeight * YSizeOfMap);

                for (int i = 0; i < MapWidth; i++)
                {
                    for (int j = 0; j < MapHeight; j++)
                    {
                        if (Map[j, i] == 0)
                            Renderer.RenderRect(buffer, (ushort)(tileSizeX * i), (ushort)(tileSizeY * j), tileSizeX, tileSizeY,
                                new Pixel(Unicode.FULL, Color.GetColor(Color.Green, 0)));
                        else if (Map[j, i] == 1)
                            Renderer.RenderRect(buffer, (ushort)(tileSizeX * i), (ushort)(tileSizeY * j), tileSizeX, tileSizeY,
                                new Pixel(Unicode.FULL, Color.DarkGreen, 0));

                    }
                }

                // Render player
                ushort xSize = (ushort)(MapWidth * XSizeOfMap);
                ushort ySize = (ushort)(MapHeight * YSizeOfMap);
                ushort x = (ushort)((float)PlayerX * MapWidth * XSizeOfMap);
                ushort y = (ushort)((float)PlayerY * MapHeight * YSizeOfMap);
                Renderer.RenderRect(buffer, x, y, xSize, ySize, new Pixel(Unicode.FULL, Color.Red, 0));

            }



            Renderer.RenderText(buffer, 0, 20, DebugMessage, Color.GetColor(Color.White, Color.Black));

            Thread.Sleep(10);

            engine.RenderBuffer();
        }

    }
}