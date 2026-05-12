using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetRules
{
    public static bool CanBeTargeted(Transform attacker, Transform target)
    {
        if (target == null) return false;
        Health hp = target.root.GetComponent<Health>();
        if (hp == null || hp.isDead)
            return false;

        bool attackerIsGuard = attacker.root.CompareTag("Guard");
        if (attackerIsGuard && target.root.CompareTag("Player"))
        {
            if (GameManager.instance.reputation > 10f)
                return false;
        }
        return true;
    }
}
