using SlackingGameEngine;
using SlackingGameEngine.Rendering;
using SlackingGameEngine.Utility;

var engine = new SlackingGameEngine.SlackingGameEngine(200, 100);

engine.Start();

KeyBoard keyBoard = engine.KeyBoard;

float x = 0;
float y = 50;
float w = 20;
float h = 10;
float xVec = 0;
float yVec = 0;

while (true)
{
    if (keyBoard.IsKeyPressed(' '))
        yVec += 1f;


}