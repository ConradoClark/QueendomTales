using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/UI/UI - Bar Counter")]
public class UIBarCounter : MonoBehaviour
{
    public int minValue = 0;
    public int maxValue = 10;
    public int currentValue = 10;
    private float snapshotValue;
    public SpriteRenderer border;
    public SpriteRenderer bar;
    public SpriteRenderer decreaseTrail;
    public SpriteRenderer increaseTrail;
    public int baseEaseSpeed;
    public int trailEaseSpeed;

    private float currentBaseSpeed;
    private float currentDecreaseTrailSpeed;
    private float currentIncreaseTrailSpeed;

    public SpriteRenderer extraCutoff;
    public bool enableBarFlashing;
    public float rgbBarFlashingMax;
    private float rgbBarFlashingInitial;
    private float rgbFloat;
    private float rgbFloatDamp;

    public bool enableDecreaseBarFlashing;
    public float decreaseBarFlashingMax;
    private float redBarFlashingInitial;
    private float redFloat;
    private float redFloatDamp;

    public TimeLayers timeLayer;

    private float f;
    public bool debug;

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
        this.snapshotValue = currentValue;
        this.rgbBarFlashingInitial = border.sharedMaterial.GetFloat("_LevelsMaxInput");
        this.redBarFlashingInitial = border.sharedMaterial.GetFloat("_LevelsRMaxInput");
    }

    void OnGUI()
    {
        if (debug)
        {
            GUI.contentColor = Color.green;
            GUI.Label(new Rect(150, 350, 1000, 100), (f / maxValue).ToString());
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

        float baseCutoff = Mathf.SmoothDamp(snapshotValue, currentValue, ref currentBaseSpeed, 1f / baseEaseSpeed);
        snapshotValue = baseCutoff;

        f = baseCutoff;

        if (currentValue > snapshotValue)
        {
            float increaseCutoff = Mathf.SmoothDamp(snapshotValue, currentValue, ref currentIncreaseTrailSpeed, 1f / trailEaseSpeed);
            increaseTrail.material.SetFloat("_CutoffFactor", increaseCutoff / maxValue);
            decreaseTrail.material.SetFloat("_CutoffFactor", baseCutoff / maxValue);
            bar.material.SetFloat("_CutoffFactor", baseCutoff / maxValue);
        }
        else if (currentValue < snapshotValue)
        {
            float decreaseCutoff = Mathf.SmoothDamp(snapshotValue, currentValue, ref currentDecreaseTrailSpeed, 1f / trailEaseSpeed);
            increaseTrail.material.SetFloat("_CutoffFactor", decreaseCutoff / maxValue);
            bar.material.SetFloat("_CutoffFactor", decreaseCutoff / maxValue);
            decreaseTrail.material.SetFloat("_CutoffFactor", baseCutoff / maxValue);
        }
        else if (Mathf.Abs(currentValue - snapshotValue) < 0.5f)
        {
            increaseTrail.material.SetFloat("_CutoffFactor", baseCutoff / maxValue);
            bar.material.SetFloat("_CutoffFactor", baseCutoff / maxValue);
            decreaseTrail.material.SetFloat("_CutoffFactor", baseCutoff / maxValue);
        }

        if (extraCutoff != null)
        {
            extraCutoff.material.SetFloat("_CutoffFactor", snapshotValue / maxValue);
        }

        float currentLevels = bar.material.GetFloat("_LevelsMaxInput");
        float currentLevelsRed = bar.material.GetFloat("_LevelsRMaxInput");
        
        if (rgbFloat > 0f)
        {
            border.material.SetFloat("_LevelsMaxInput", Mathf.SmoothDamp(currentLevels, rgbBarFlashingInitial - rgbFloat * rgbBarFlashingMax, ref rgbFloatDamp, 0.001f));
            rgbFloat -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
        }
        else if (Mathf.Abs(currentLevels - rgbBarFlashingInitial) > 0.5f)
        {
            border.material.SetFloat("_LevelsMaxInput", Mathf.SmoothDamp(currentLevels, rgbBarFlashingInitial, ref rgbFloatDamp, 0.001f));
        }

        if (redFloat > 0f)
        {
            border.material.SetFloat("_LevelsRMaxInput", Mathf.SmoothDamp(currentLevelsRed, redBarFlashingInitial - redFloat * decreaseBarFlashingMax, ref rgbFloatDamp, 0.001f));
            bar.material.SetFloat("_LevelsRMaxInput", Mathf.SmoothDamp(currentLevelsRed, redBarFlashingInitial - redFloat * decreaseBarFlashingMax, ref rgbFloatDamp, 0.001f));
            redFloat -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
        }
        else if (Mathf.Abs(currentLevelsRed - redBarFlashingInitial) > 0.5f)
        {
            border.material.SetFloat("_LevelsRMaxInput", Mathf.SmoothDamp(currentLevelsRed, redBarFlashingInitial, ref redFloatDamp, 0.001f));
            bar.material.SetFloat("_LevelsRMaxInput", Mathf.SmoothDamp(currentLevelsRed, redBarFlashingInitial, ref redFloatDamp, 0.001f));
        }
    }

    public void Increase(int amount, bool flash=true)
    {
        currentValue = Mathf.Clamp(currentValue + amount, minValue, maxValue);
        currentBaseSpeed = 0f;
        if (enableBarFlashing && flash && Mathf.Abs(snapshotValue - currentValue) > 0.5f)
        {
            rgbFloat = 1.0f;
        }
    }

    public void Decrease(int amount, bool flash = true)
    {
        currentValue = Mathf.Clamp(currentValue - amount, minValue, maxValue);
        currentBaseSpeed = 0f;

        if (enableDecreaseBarFlashing && Mathf.Abs(snapshotValue - currentValue) > 0.5f)
        {
            redFloat = 1.0f;
        }
    }
}
