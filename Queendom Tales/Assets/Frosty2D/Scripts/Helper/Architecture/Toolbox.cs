using UnityEngine;
using System.Collections;

[AddComponentMenu("Frosty2D/Architecture/Toolbox Behaviour")]
[DisallowMultipleComponent]
public class Toolbox : Singleton<Toolbox>
{
    public FrostyTime frostyTime;
    protected Toolbox() { } // guarantee this will be always a singleton only - can't use the constructor!
    
    void Awake()
    {
        frostyTime = RegisterComponent<FrostyTime>();
    }

    // (optional) allow runtime registration of global objects
    static public T RegisterComponent<T>() where T : Component
    {
        return Instance.GetOrAddComponent<T>();
    }
}