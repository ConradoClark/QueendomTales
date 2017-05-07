using UnityEngine;
using System.Collections;

public class GhostAI : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator animator;
    public Vector2 targetOffset = new Vector2(5, -10);
    public BasicEnemy enemy;
    public float minDistanceToReact;
    public float minWanderTime;
    public float wanderSpeed;
    public float wanderSpeedDampening;
    public FrostyKinematics kinematics;
    public TimeLayers timeLayer;
    private Transform target;

    private float xVelocity, yVelocity;

    IEnumerator Start()
    {
        if (!QTToolbox.Instance.characterManager.IsInitialized())
        {
            yield return QTToolbox.Instance.characterManager.WaitForInitialization();
        }
        StartCoroutine(SelectMode());
        target = QTToolbox.Instance.characterManager.character.transform;
    }

    void Update()
    {
    }

    IEnumerator SelectMode()
    {
        yield return 1;
        int rng = Random.Range(0, 3);

        float distance = Vector2.Distance((Vector2)target.transform.position + targetOffset, enemy.transform.position);
        if (distance > minDistanceToReact)
        {
            rng = 0;
        }

        switch (rng)
        {
            case 0:
                StartCoroutine(Wander());
                yield break;
            case 1:
                StartCoroutine(Teleport());
                yield break;
            case 2:
                StartCoroutine(Attack());
                yield break;
        }
    }

    IEnumerator Wander()
    {
        float time = minWanderTime;
        int rng = Random.Range(0, 2);
        while (minWanderTime > 0 || rng == 0)
        {
            minWanderTime -= Toolbox.Instance.time.GetDeltaTime(timeLayer);

            float x = Mathf.SmoothDamp(kinematics.transform.position.x, target.transform.position.x + targetOffset.x, ref xVelocity, wanderSpeedDampening, wanderSpeed);
            float y = Mathf.SmoothDamp(kinematics.transform.position.y, target.transform.position.y + targetOffset.y, ref yVelocity, wanderSpeedDampening, wanderSpeed);

            x = kinematics.transform.position.x - x;
            y = -kinematics.transform.position.y + y;

            x *= Toolbox.Instance.time.GetLayerMultiplier(timeLayer);
            y *= Toolbox.Instance.time.GetLayerMultiplier(timeLayer);

            kinematics.ApplyMovement(new Vector2(-1, 0), x);
            kinematics.ApplyMovement(new Vector2(0, Mathf.Sign(y)), y * Mathf.Sign(y));

            yield return 1;
        }

        StartCoroutine(SelectMode());
        yield break;
    }

    IEnumerator Teleport()
    {
        yield break;
    }

    IEnumerator Attack()
    {
        yield break;
    }
}
