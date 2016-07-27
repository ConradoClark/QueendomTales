using UnityEngine;
using System.Linq;
using System.Collections;

public class FrostyEventOnCollision : FrostyOnCollision
{
    public FrostyKinematics kinematics;
    public FrostyCollision[] colliders;
    public bool clamp = true;

    void Update()
    {
        if (kinematics == null) return;

        bool value = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            FrostyCollision collision = colliders[i];
            for (int j = 0; j < collision.AllHits.Length; j++)
            {
                RaycastHit2D hit = collision.AllHits[j];
                if (hit.collider != null)
                {
                    if (hit.distance < 0.5f)
                    {
                        value = true;
                    }
                    if (clamp)
                    {
                        kinematics.ClampPosition(collision.direction, -collision.direction.y * (transform.position.y + (collision.direction * hit.distance).y) + collision.direction.x * (transform.position.x + (collision.direction * hit.distance).x));
                    }
                }
            }
        }

        if (PredicateOnCollision != null)
        {
            PredicateOnCollision.SetValue(value);
        }
    }
}
