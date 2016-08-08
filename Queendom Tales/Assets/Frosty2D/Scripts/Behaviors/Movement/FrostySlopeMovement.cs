using UnityEngine;
using System.Collections;

public class FrostySlopeMovement : MonoBehaviour
{
    public FrostyKinematics kinematics;
    public Vector2 slopeDirection;
    public FrostyMovementPredicate isColliding;
    public FrostyEventOnCollision onCollisionEvent;
    public FrostyPatternMovement jumpMovement;
    public FrostyMovementPredicate onGround;
    private float speed;
    private int direction;
    private Vector2 normal;
    private float timeElapsed;
    public float animSpeed { get; private set; }

    void Update()
    {
        if (isColliding.Value && onCollisionEvent.impactOnPoints.Count > 0 && !jumpMovement.IsActive())
        {
            speed = kinematics.GetSpeed(new Vector2(slopeDirection.x, 0));
            direction = speed > 0 ? 1 : -1;
            var slope = onCollisionEvent.impactOnPoints[0].collider.GetComponent<FrostySlope>();
            normal = slope != null ? slope.direction / 2 : Vector2.zero;
            timeElapsed = 0;
        }

        timeElapsed += Time.smoothDeltaTime;
        if (!jumpMovement.IsActive())
        {
            bool sameDir = slopeDirection.x * direction > 0;
            kinematics.ApplyMovement(new Vector2(slopeDirection.x / 2, slopeDirection.y * 5), (normal * speed).magnitude * direction * (direction == -1 ? 4 : 1));
            animSpeed = direction == -1 ? 1.1f : 0.65f;
        }
        else
        {
            speed = 0;
            animSpeed = 1f;
        }
        speed = direction == -1 && timeElapsed < 0.3f ? speed : 0;
    }
}
