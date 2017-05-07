using UnityEngine;
using System.Collections;

public class CollectableHoming :FrostyPoolableObject
{
    public float delay;
    public TimeLayers timeLayer;
    public FrostyKinematics kinematics;
    public float speed;
    public float damp = 0.15f;
    public float minDistance;
    public Vector2 offset;
    public FrostyPatternMovement[] movements;

    private Transform target;
    private float xVelocity, yVelocity;
    private bool pulling;
    private float delayElapsed;

    IEnumerator Start()
    {
        if (!QTToolbox.Instance.characterManager.IsInitialized())
        {
            yield return QTToolbox.Instance.characterManager.WaitForInitialization();
        }
        this.target = QTToolbox.Instance.characterManager.character.transform;
        delayElapsed = delay;
    }

    void Update()
    {
        delayElapsed -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
        if (delayElapsed > 0f) return;

        if (!QTToolbox.Instance.characterManager.IsInitialized()) return;

        float distance = Vector2.Distance((Vector2)target.transform.position + offset, kinematics.transform.position);

        if (distance > minDistance && !pulling) return;

        pulling = true;
        for(int i = 0; i < movements.Length; i++)
        {
            movements[i].enabled = false;
        }

        float x = Mathf.SmoothDamp(kinematics.transform.position.x, target.transform.position.x + offset.x, ref xVelocity, damp, speed);
        float y = Mathf.SmoothDamp(kinematics.transform.position.y, target.transform.position.y + offset.y, ref yVelocity, damp, speed);

        x = kinematics.transform.position.x - x;
        y = -kinematics.transform.position.y + y;

        x *= Toolbox.Instance.time.GetLayerMultiplier(timeLayer);
        y *= Toolbox.Instance.time.GetLayerMultiplier(timeLayer);

        kinematics.ApplyMovement(new Vector2(-1, 0), x);
        kinematics.ApplyMovement(new Vector2(0, Mathf.Sign(y)), y * Mathf.Sign(y));
    }

    public override void ResetState()
    {
        base.ResetState();
        for (int i = 0; i < movements.Length; i++)
        {
            movements[i].enabled = true;
            movements[i].Reactivate(false, true);
        }
        pulling = false;
        delayElapsed = delay;
        xVelocity = yVelocity = 0f;
    }
}
