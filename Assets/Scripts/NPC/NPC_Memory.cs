using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Memory : MonoBehaviour
{
    public List<string> memories = new();
    public void AddMemory(string memory)
    {
        memories.Add(memory);
        if (memories.Count > 10)
            memories.RemoveAt(0);
    }

    public string GetMemoryText()
    {
        return string.Join(", ", memories);
    }

    public string GetConvaiMemory()
    {
        return string.Join("\n ", memories);
    }
}
