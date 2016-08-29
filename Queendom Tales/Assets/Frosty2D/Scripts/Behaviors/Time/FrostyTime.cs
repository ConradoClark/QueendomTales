using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrostyTime : MonoBehaviour
{
    protected Dictionary<TimeLayers, float> timeMultipliers;

    void Awake()
    {
        timeMultipliers = new Dictionary<TimeLayers, float>();
    }

    public float GetDeltaTime(TimeLayers layer)
    {
        if (!timeMultipliers.ContainsKey(layer))
        {
            timeMultipliers[layer] = 1.0f;
        }

        return Time.deltaTime * timeMultipliers[layer];
    }

    public float GetSmoothDeltaTime(TimeLayers layer)
    {
        if (!timeMultipliers.ContainsKey(layer))
        {
            timeMultipliers[layer] = 1.0f;
        }

        return Time.smoothDeltaTime * timeMultipliers[layer];
    }

    public float GetFixedDeltaTime(TimeLayers layer)
    {
        if (!timeMultipliers.ContainsKey(layer))
        {
            timeMultipliers[layer] = 1.0f;
        }

        return Time.fixedDeltaTime * timeMultipliers[layer];
    }

    public void SetLayerMultiplier(FrostyTimeLayerGroup group, float multiplier)
    {
        foreach (TimeLayers layer in group.children)
        {
            SetLayerMultiplier(layer, multiplier);
        }
    }

    public IEnumerator WaitForSeconds(TimeLayers layer, float seconds)
    {
        if (seconds <= 0f) yield break;
        while (seconds > 0f)
        {
            seconds -= this.GetFixedDeltaTime(layer);
            yield return 1;
        }
    }

    public void SetLayerMultiplier(TimeLayers layer, float multiplier)
    {
        timeMultipliers[layer] = multiplier;
    }

    public float GetLayerMultiplier(TimeLayers layer)
    {
        if (timeMultipliers.ContainsKey(layer))
        {
            return timeMultipliers[layer];
        }
        return 1.0f;
    }
}
