using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatSystem : MonoBehaviour
{
    private static Dictionary<Transform, Dictionary<Transform, float>> threatTable
         = new Dictionary<Transform, Dictionary<Transform, float>>();

    public static void AddThreat(Transform target, Transform source, float amount)
    {
        if (!threatTable.ContainsKey(target))
            threatTable[target] = new Dictionary<Transform, float>();
        if (!threatTable[target].ContainsKey(source))
            threatTable[target][source] = 0;
        threatTable[target][source] += amount;
    }

    public static Transform GetHighestThreat(Transform target)
    {
        if (!threatTable.ContainsKey(target)) return null;
        float max = 0;
        Transform best = null;

        foreach(var pair in threatTable[target])
        {
            if(pair.Value > max)
            {
                max = pair.Value;
                best = pair.Key;
            }
        }
        return best;
    }

}
