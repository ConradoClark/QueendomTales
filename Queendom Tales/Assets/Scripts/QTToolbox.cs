using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Architecture/QTToolbox Behaviour")]
[DisallowMultipleComponent]
public class QTToolbox : Singleton<QTToolbox>
{
    public TargetableObjectManager targetableObjectManager;
    public CharacterManager characterManager;   

    protected QTToolbox() { } // guarantee this will be always a singleton only - can't use the constructor!

    void Awake()
    {
        targetableObjectManager = RegisterComponent<TargetableObjectManager>();
        characterManager = RegisterComponent<CharacterManager>();
    }

    // (optional) allow runtime registration of global objects
    static public T RegisterComponent<T>() where T : Component
    {
        return Instance.GetOrAddComponent<T>();
    }
}