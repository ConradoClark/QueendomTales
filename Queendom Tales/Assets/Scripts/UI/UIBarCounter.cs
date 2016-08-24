using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/UI/UI - Bar Counter")]
public class UIBarCounter : MonoBehaviour
{
    public int minValue = 0;
    public int maxValue = 10;
    public int currentValue = 10;
    public SpriteRenderer bar;
    public SpriteRenderer decreaseTrail;
    public SpriteRenderer increaseTrail;
    public int baseEaseSpeed;
    public int trailEaseSpeed;
    bool changing;
    bool endChange;

    public void Set(bool useTrail = false)
    {

    }

    public void Start()
    {
        if (bar != null)
        {
            bar.material.SetFloat("_CutoffFactor", ((float)currentValue) / maxValue);
        }
        if (decreaseTrail != null)
        {
            decreaseTrail.material.SetFloat("_CutoffFactor", ((float)currentValue) / maxValue);
        }
        if (increaseTrail != null)
        {
            increaseTrail.material.SetFloat("_CutoffFactor", ((float)currentValue) / maxValue);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.Increase(10);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.Increase(50);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            this.Decrease(10);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.Decrease(50);
        }
    }

    public void Increase(int amount, bool ease = true)
    {
        int oldValue = currentValue;
        currentValue = (int)Mathf.Clamp(currentValue + amount, minValue, maxValue);

        if (bar == null) return;

        StartCoroutine(EaseCounter(bar, oldValue, currentValue, baseEaseSpeed));
        if (decreaseTrail != null)
        {
            StartCoroutine(EaseCounter(decreaseTrail, oldValue, currentValue, baseEaseSpeed));
        }
        if (increaseTrail == null)
        {
            if (!ease)
            {
                bar.material.SetFloat("_CutoffFactor", ((float)currentValue) / maxValue);
                return;
            }
            return;
        }
        StartCoroutine(EaseCounter(increaseTrail, oldValue, currentValue, trailEaseSpeed));
    }

    public void Decrease(int amount, bool ease = true)
    {
        int oldValue = currentValue;
        currentValue = (int)Mathf.Clamp(currentValue - amount, minValue, maxValue);

        if (bar == null) return;

        StartCoroutine(EaseCounter(bar, oldValue, currentValue, trailEaseSpeed));
        if (increaseTrail != null)
        {
            StartCoroutine(EaseCounter(increaseTrail, oldValue, currentValue, trailEaseSpeed));
        }
        if (decreaseTrail == null)
        {
            if (!ease)
            {
                bar.material.SetFloat("_CutoffFactor", ((float)currentValue) / maxValue);
                return;
            }
            return;
        }
        StartCoroutine(EaseCounter(decreaseTrail, oldValue, currentValue, baseEaseSpeed));
    }

    IEnumerator EaseCounter(SpriteRenderer renderer, int oldValue, int newValue, int speed)
    {
        float step = 0f;
        while (step < 1f)
        {
            step += Time.smoothDeltaTime * speed;
            float changedValue = Mathf.SmoothStep(oldValue, newValue, step);
            renderer.material.SetFloat("_CutoffFactor", changedValue / maxValue);
            yield return 1;
        }
        renderer.material.SetFloat("_CutoffFactor", (float)currentValue / maxValue);
    }
}
