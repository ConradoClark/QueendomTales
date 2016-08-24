using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Character/Walking Poof Effect")]
public class WalkingPoof : MonoBehaviour
{
    public FrostyKinematics kinematics;
    public FrostyPatternMovement movement;
    public FrostyMovementPredicate condition;
    public GameObject poofObject;
    public float frequency;
    public Vector3 offset;
    public float minSpeed;

    void Start()
    {
        StartCoroutine(WaitForActivation());
    }

    IEnumerator WaitForActivation()
    {
        while (!movement.IsActive())
        {
            yield return 1;
        }
        StartCoroutine(CreatePoof());
    }

    IEnumerator CreatePoof()
    {
        yield return new WaitForSeconds(frequency);
        while (movement.IsActive())
        {
            if (condition.Value && kinematics.GetSpeed(movement.GetDirection()) > minSpeed)
            {
                var poof = GameObject.Instantiate(poofObject);
                poof.transform.position = this.transform.position + offset;
            }
            yield return new WaitForSeconds(frequency);
        }
        StartCoroutine(WaitForActivation());
    }
}
