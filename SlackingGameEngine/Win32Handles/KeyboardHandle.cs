using System.Runtime.InteropServices;

namespace SlackingGameEngine.Win32Handles;

internal class KeyboardHandle
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetKeyboardState(byte[] lpKeyState);

    internal byte[] keyStates;

    internal KeyboardHandle()
    {
        keyStates = new byte[256];
        GetKeyboardState(keyStates);
    }

    public void Update()
    {
        GetKeyboardState(keyStates);
    }

    public bool GetKeyState(KeyCode key)
    {
        //return (keyStates[(int)key] & 0x80) != 0;
        return keyStates[(int)key] != 0;
    }
}
