using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
   public Vector3 GetRandomPoint()
    {
        Bounds bounds = GetComponent<Collider>().bounds;
        return new Vector3(
            Random.Range(bounds.min.x,bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z)
            );
    }
}
