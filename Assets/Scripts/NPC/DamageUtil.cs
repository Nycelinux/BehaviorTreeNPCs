using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageUtil
{
    public static Health GetHealth(GameObject obj)
    {
        if (obj == null) return null;
        return obj.transform.root.GetComponent<Health>();
    }
}
