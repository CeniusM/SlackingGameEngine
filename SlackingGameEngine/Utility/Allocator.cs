﻿using System.Runtime.InteropServices;

namespace SlackingGameEngine.Utility;

/// <summary>
/// Will hold the pointers to ussafe memory, 
/// </summary>
internal class Allocator
{
    Dictionary<IntPtr, int> AllocatedMemory = new Dictionary<IntPtr, int>();

    internal long BytesAllocated { get; private set; }

    internal IntPtr GetUnsafeMemory(int bytes)
    {
        IntPtr ptr = Marshal.AllocHGlobal(bytes);

        AllocatedMemory.Add(ptr, bytes);
        BytesAllocated += bytes;

        return ptr;
    }

    internal void FreePointer(IntPtr ptr)
    {
        if (!AllocatedMemory.ContainsKey(ptr))
            return;

        AllocatedMemory.Remove(ptr, out int value);
        BytesAllocated -= value;

        Marshal.FreeHGlobal(ptr);
    }

    internal int GetSizeOfPtr(IntPtr ptr)
    {
        return AllocatedMemory.TryGetValue(ptr, out int size) ? size : -1;
    }

    internal void Clear()
    {
        var Values = AllocatedMemory.ToList();
        for (int i = 0; i < Values.Count(); i++)
        {
            AllocatedMemory.Remove(Values[i].Key, out int value);
            BytesAllocated -= value;
        }
    }
}