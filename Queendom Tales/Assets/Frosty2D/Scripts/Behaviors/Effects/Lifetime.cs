using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour
{
    public float lifeTime;
    public TimeLayers timeLayer;

    void Start()
    {
        StartCoroutine(WaitAndDie());
    }

    IEnumerator WaitAndDie()
    {
        yield return Toolbox.Instance.frostyTime.WaitForSeconds(timeLayer, lifeTime);
        GameObject.Destroy(this.gameObject);
    }
}
