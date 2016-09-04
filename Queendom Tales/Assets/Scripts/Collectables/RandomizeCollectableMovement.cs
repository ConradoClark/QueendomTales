using UnityEngine;
using System.Collections;

public class RandomizeCollectableMovement : MonoBehaviour
{
    public FrostyPatternMovement movement;
    public float xMagnitude;
    void Start()
    {
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        float x = Random.Range(-1f, 1f) * xMagnitude;
        movement.SetDirection(new Vector2(x, movement.GetDirection().y));
    }
}
