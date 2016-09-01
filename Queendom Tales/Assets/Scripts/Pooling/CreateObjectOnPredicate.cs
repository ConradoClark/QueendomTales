using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Instance/Create Object on Predicate")]
public class CreateObjectOnPredicate : FrostyPoolableObject
{
    public enum ObjectMode
    {
        PoolInstance,
        PoolableObject
    }

    public ObjectMode Mode = ObjectMode.PoolInstance;
    public FrostyPoolInstance prefabPoolInstance;
    public FrostyPoolableObject prefabPoolableObject;
    public FrostyMovementPredicate predicate;
    public bool looping;
    public float loopingInterval;
    public bool createAtStart;
    public Vector3 offset;
    public Transform attachToTransform;
    public TimeLayers timeLayer;

    void Start()
    {
        StartCoroutine(WaitForNextActivation());
    }

    IEnumerator WaitForNextActivation()
    {
        if (predicate.Value)
        {
            if (createAtStart)
            {
                StartCoroutine(Activate());
                yield break;
            }
            else
            {
                while (predicate.Value)
                {
                    yield return 1;
                }
            }
        }
        while (!predicate.Value)
        {
            yield return 0;
        }
        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        CreatePrefab();

        if (looping)
        {
            while (predicate.Value)
            {
                yield return Toolbox.Instance.time.WaitForSeconds(timeLayer, loopingInterval);
                CreatePrefab();
            }
        }else
        {
            while (predicate.Value)
            {
                yield return 1;
            }
        }

        StartCoroutine(WaitForNextActivation());
    }

    void CreatePrefab()
    {
        GameObject obj = null;
        if (this.Mode == ObjectMode.PoolInstance)
        {
            obj = Toolbox.Instance.pool.Retrieve(prefabPoolInstance);
        }else
        {
            obj = Toolbox.Instance.pool.Retrieve(prefabPoolableObject);
        }

        obj.transform.position = transform.position + offset;
        if (attachToTransform != null)
        {
            obj.transform.SetParent(attachToTransform, true);
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        StopAllCoroutines();
        StartCoroutine(WaitForNextActivation());
    }
}
