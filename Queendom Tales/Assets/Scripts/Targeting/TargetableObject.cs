using UnityEngine;
using System.Collections;
using System;

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
        Toolbox.Instance.targetableObjectManager.AddTarget(this);
    }

    void OnDisable()
    {
        if (!Toolbox.IsApplicationQuitting())
        {
            Toolbox.Instance.targetableObjectManager.RemoveTarget(this);
        }
    }
}
