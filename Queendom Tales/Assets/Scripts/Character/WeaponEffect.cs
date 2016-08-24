using System;
using UnityEngine;

[Serializable]
public class WeaponEffect
{
    public GameObject Prefab_Right;
    public GameObject Prefab_Left;
    public Vector3 Offset;
    public string Animation;
    public FrostyPatternMovement Movement;
    public FrostyCollision Hit;
    public float HitDelay;
    public float HitDuration;
    public float AttackMultiplier = 1f;
}

