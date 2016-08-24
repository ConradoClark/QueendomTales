using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Queendom-Tales/Targeting/Targetable Object Manager")]
public class TargetableObjectManager : MonoBehaviour
{
    public List<TargetableObject> objects;

    void Awake()
    {
        objects = new List<TargetableObject>();
    }

    void Start()
    {

    }

    void Update()
    {
    }

    public void AddTarget(TargetableObject target)
    {
        this.objects.Add(target);
    }

    public void RemoveTarget(TargetableObject target)
    {
        this.objects.Remove(target);
    }
}
