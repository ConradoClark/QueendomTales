using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FrostyPooledObject
{
    public Transform poolObject;
    public bool available;
    public FrostyPoolableObject[] poolBehaviours;
}
