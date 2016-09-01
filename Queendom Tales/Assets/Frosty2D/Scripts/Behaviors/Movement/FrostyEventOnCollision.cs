using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class FrostyEventOnCollision : FrostyOnCollision
{
    public FrostyKinematics kinematics;
    public FrostyCollision[] colliders;
    public List<RaycastHit2D> impactOnPoints { get; private set; }
    public bool clamp = true;
    public bool useTriggerDistance = true;
    public float triggerDistance = 0.5f;

    void Start()
    {
        impactOnPoints = new List<RaycastHit2D>();
    }

    void Update()
    {
        if (impactOnPoints == null) return;

        impactOnPoints.Clear();
        if (kinematics == null) return;

        bool value = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            FrostyCollision collision = colliders[i];
            if (collision.AllHits == null) continue;
            for (int j = 0; j < collision.AllHits.Length; j++)
            {
                RaycastHit2D hit = collision.AllHits[j];
                if (hit.collider != null)
                {
                    if (!useTriggerDistance || hit.distance < triggerDistance)
                    {
                        value = true;
                        impactOnPoints.Add(hit);
                    }
                    if (clamp)
                    {
                        Vector2 validDirection = collision.GetClampDirection();
                        kinematics.ClampPosition(validDirection, -validDirection.y * (transform.position.y + (validDirection * hit.distance).y) + validDirection.x * (transform.position.x + (validDirection * hit.distance).x));
                    }
                }
            }
        }

        if (PredicateOnCollision != null)
        {
            PredicateOnCollision.SetValue(value);
        }

        if (ExtraPredicatesOnCollision != null)
        {
            for (int i = 0; i < ExtraPredicatesOnCollision.Length; i++)
            {
                ExtraPredicatesOnCollision[i].SetValue(ExtraPredicatesOnCollision[i].Value || value);
            }
        }
    }
}
