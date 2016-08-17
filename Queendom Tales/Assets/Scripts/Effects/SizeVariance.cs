using UnityEngine;
using System.Collections;

public class SizeVariance : MonoBehaviour
{
    public float varianceIntensity;
    public float varianceDelay;
    private float elapsedTime;
    public float waitToStart;
    private bool started = false;
    private Vector2 size;
    private Vector2 newSize;

    void Start()
    {
        this.size = transform.localScale;
        StartCoroutine(VarySize());

    }

    void Update()
    {
        if (!started) return;
        newSize = Vector2.zero;
        elapsedTime += Time.deltaTime;
    }

    IEnumerator VarySize()
    {
        if (waitToStart > 0f)
        {
            yield return new WaitForSeconds(waitToStart);
        }
        started = true;
        while (this.enabled)
        {
            float val = Mathf.Sin(varianceDelay * elapsedTime) * varianceIntensity;
            newSize += size + new Vector2(val, val);
            yield return 1;
        }
    }

    void LateUpdate()
    {
        if (started)
        {
            transform.localScale = newSize;
        }
    }
}
