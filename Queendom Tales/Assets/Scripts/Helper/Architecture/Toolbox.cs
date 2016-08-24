using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Architecture/Toolbox Behaviour")]
[DisallowMultipleComponent]
public class Toolbox : Singleton<Toolbox>
{
    protected Toolbox() { } // guarantee this will be always a singleton only - can't use the constructor!

    public TargetableObjectManager targetableObjectManager;

    void Awake()
    {
        targetableObjectManager = RegisterComponent<TargetableObjectManager>();
    }

    // (optional) allow runtime registration of global objects
    static public T RegisterComponent<T>() where T : Component
    {
        return Instance.GetOrAddComponent<T>();
    }
}