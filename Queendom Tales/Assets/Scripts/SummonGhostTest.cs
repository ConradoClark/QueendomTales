using UnityEngine;
using System.Collections;

public class SummonGhostTest : MonoBehaviour
{
    public FrostyPoolInstance poolInstance;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            var g = Toolbox.Instance.pool.Retrieve(poolInstance);
            g.transform.position = new Vector3(-195 + Random.Range(-5,15), -160 + Random.Range(-10,20));
        }
    }
}
