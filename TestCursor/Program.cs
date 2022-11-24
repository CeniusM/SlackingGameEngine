using SlackingGameEngine;
using SlackingGameEngine.Rendering;
using SlackingGameEngine.Utility;
using System.Diagnostics;

var engine = new SlackingGameEngine.SlackingGameEngine(600, 200);

engine.Start();

KeyBoard keyBoard = engine.GetKeyBoardController();
Cursor cursor = engine.GetCursorController();


float x = 0;
float y = 50;
float w = 60;
float h = 20;
//float xVec = 0;
//float yVec = 0;

Stopwatch sw = new Stopwatch();
sw.Start();

//cursor.LockCursor = true;
cursor.ShowCursor_NotImplemented = false;
engine.ShowFPS = true;


// runds for 10 seconds
while (sw.Elapsed.TotalSeconds < 15)
{
    string OutPut = "";

    engine.Update();


    while (cursor.TryGetNextUpdate(out var info))
    {
        OutPut += "Box x: " + (int)x + ", ";
        OutPut += "Box y: " + (int)y + ", ";
        OutPut += "x: " + info.x + ", ";
        OutPut += "y: " + info.y + ", ";
        OutPut += "xOffset: " + info.xOffset + ", ";
        OutPut += "yOffset: " + info.yOffset;

        x += (float)info.xOffset / 2;
        y += (float)info.yOffset / 2;
    }

    Renderer.Clear(engine.GetActiveBuffer());
    Renderer.RenderRect(engine.GetActiveBuffer(), (ushort)x, (ushort)y, (ushort)w, (ushort)h, new Pixel(Unicode.FULL, Color.FG_White));
    Renderer.RenderText(engine.GetActiveBuffer(), 0, 1, OutPut, Color.FG_White | Color.BG_Black);
    engine.RenderBuffer();
    //Thread.Sleep(50);
}

cursor.Reset();
engine.Update();