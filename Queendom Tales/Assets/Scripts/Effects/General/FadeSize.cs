using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Effects/General/Fade Size Effect")]
public class FadeSize : FrostyPoolableObject
{
    public float speed = 1f;
    private Vector3 initialScale;

    void Awake()
    {
        this.initialScale = this.transform.localScale;
    }

    void Update()
    {
        this.transform.localScale -= (Vector3) Vector2.one * Time.deltaTime * speed;
    }

    public override void ResetState()
    {
        this.transform.localScale = initialScale;
    }
}
