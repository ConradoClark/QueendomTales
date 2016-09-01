using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Character/Walking Poof Effect")]
public class WalkingPoof : MonoBehaviour
{
    public FrostyKinematics kinematics;
    public FrostyPatternMovement movement;
    public FrostyMovementPredicate condition;
    public FrostyPoolInstance poofObject;
    public float frequency;
    public Vector3 offset;
    public float minSpeed;
    public TimeLayers timeLayer;

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
        yield return Toolbox.Instance.time.WaitForSeconds(timeLayer, frequency);
        while (movement.IsActive())
        {
            if (condition.Value && kinematics.GetSpeed(movement.GetDirection()) > minSpeed)
            {                
                var poof = Toolbox.Instance.pool.Retrieve(poofObject);
                poof.transform.position = this.transform.position + offset;
            }
            yield return Toolbox.Instance.time.WaitForSeconds(timeLayer, frequency);
        }
        StartCoroutine(WaitForActivation());
    }
}
