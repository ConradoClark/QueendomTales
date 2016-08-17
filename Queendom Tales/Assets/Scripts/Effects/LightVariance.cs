using UnityEngine;
using System.Collections;

public class LightVariance : MonoBehaviour
{
    public Light lightSource;
    public float varianceIntensity;
    public float varianceDelay;
    public float flickerFrequency;
    public float flickerFrequencyRandomness;
    public float flickerAmplitude;
    public float flickerRange;
    private float elapsedTime;
    private float intensity;
    private float newIntensity;
    public float waitToStart;
    private bool started=false;
    public float colorVarianceIntensity;
    public float colorVarianceDelay;
    public Gradient colorVariance;
    private Color lightColor;
    private Color newColor;

    public void OverrideInitialIntensity(float intensity)
    {
        this.intensity = intensity;
    }

    void Start()
    {
        if (this.lightSource == null) return;
        this.intensity = lightSource.intensity;
        this.lightColor = lightSource.color;
        StartCoroutine(VaryIntensity());
        StartCoroutine(Flicker());
    }

    void Update()
    {
        if (!started) return;
        newIntensity = 0f;
        newColor = this.lightColor;
        elapsedTime += Time.deltaTime;
    }

    IEnumerator VaryIntensity()
    {
        if (waitToStart>0f)
        {
            yield return new WaitForSeconds(waitToStart);
        }
        started = true;
        while (this.enabled)
        {
            newIntensity += intensity + Mathf.Sin(varianceDelay * elapsedTime) * varianceIntensity;

            var value = Mathf.Sin(elapsedTime * colorVarianceDelay) * colorVarianceIntensity;
            newColor = Color.Lerp((this.lightColor + colorVariance.Evaluate(0))/2, colorVariance.Evaluate(value), value);
            newColor.a = 1;
            yield return 1;
        }
    }

    IEnumerator Flicker()
    {
        while (this.enabled)
        {
            newIntensity += flickerAmplitude * Random.Range(-flickerRange, flickerRange);
            yield return new WaitForSeconds(flickerFrequency + Random.Range(-flickerFrequencyRandomness, flickerFrequencyRandomness));
        }
    }

    void LateUpdate()
    {
        if (started)
        {
            lightSource.intensity = newIntensity;
            lightSource.color = newColor;
        }
    }
}
