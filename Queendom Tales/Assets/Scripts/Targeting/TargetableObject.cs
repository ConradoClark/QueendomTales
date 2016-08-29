using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Queendom-Tales/Targeting/Targetable Object")]
public class TargetableObject : MonoBehaviour
{
    public Vector3 cursorOffset;

    void Start()
    {        
    }

    void Update()
    {

    }

    void OnEnable()
    {
        QTToolbox.Instance.targetableObjectManager.AddTarget(this);
    }

    void OnDisable()
    {
        if (!QTToolbox.IsApplicationQuitting())
        {
            QTToolbox.Instance.targetableObjectManager.RemoveTarget(this);
        }
    }
}
