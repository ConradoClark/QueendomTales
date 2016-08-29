using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Instance/Create Object on Predicate")]
public class CreateObjectOnPredicate : MonoBehaviour
{
    public GameObject prefabToCreate;
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
                yield return Toolbox.Instance.frostyTime.WaitForSeconds(timeLayer, loopingInterval);
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
        var obj = Instantiate(prefabToCreate);
        obj.transform.position = transform.position + offset;
        if (attachToTransform != null)
        {
            obj.transform.SetParent(attachToTransform, true);
        }
    }
}
