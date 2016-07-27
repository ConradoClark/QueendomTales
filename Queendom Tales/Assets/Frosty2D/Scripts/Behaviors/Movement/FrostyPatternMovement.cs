using UnityEngine;
using System.Linq;
using System.Collections;
using Assets.Frosty2D.Scripts.Core.Movement;

[AddComponentMenu("Frosty-Movement/Pattern Movement")]
public class FrostyPatternMovement : MonoBehaviour
{
    [Header("Kinematics Reference")]
    public FrostyKinematics kinematics;

    [Header("Movement Pattern")]
    public FrostySingleMovementPattern[] patterns;

    [Header("Conditions")]
    public FrostyMovementPredicate[] resetOnPredicate;
    public FrostyMovementPredicate[] reactivateOnPredicate;
    public FrostyMovementPredicate[] deactivateOnPredicate;
    public FrostyMovementPredicate[] abortOnPredicate;

    private Vector2 currentDirection;
    private float currentSpeed;
    private Vector2 rawMovement;

    void Update()
    {
        currentSpeed = 0f;
        rawMovement = Vector2.zero;
        currentDirection = Vector2.zero;

        if (resetOnPredicate.Any(pred => pred.Value))
        {
            Reactivate(false, true);
        }

        if (reactivateOnPredicate.Any(pred => pred.Value))
        {
            Reactivate(false, false);
        }

        if (deactivateOnPredicate.Any(pred => pred.Value))
        {
            this.Deactivate();
        }

        if (abortOnPredicate.Any(pred => pred.Value) && this.IsActive())
        {
            this.Abort();
        }

        if (patterns == null) return;

        for (int i = 0; i < patterns.Length; i++)
        {
            FrostySingleMovementPattern pattern = patterns[i];
            float speed;
            Vector2 dir = pattern.Evaluate(Time.fixedDeltaTime, out speed);
            rawMovement += (dir.normalized * speed);
        }

        currentDirection = rawMovement.normalized;
        currentSpeed = rawMovement.magnitude;

        if (kinematics != null)
        {
            kinematics.ApplyMovement(currentDirection, currentSpeed);
        }
    }

    public float GetCurrentTime()
    {
        return patterns.Select(p=>p.GetCurrentTime()).DefaultIfEmpty(0).Sum();
    }

    public bool IsActive()
    {
        return patterns.Any(p => p.active);
    }

    public bool IsActivating
    {
        get
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].currentState == FrostySingleMovementPattern.STATE_ACTIVATION)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool IsOnLoop
    {
        get
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].currentState != FrostySingleMovementPattern.STATE_LOOP)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool IsDeactivating
    {
        get
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].currentState == FrostySingleMovementPattern.STATE_DEACTIVATION)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool HasFinished
    {
        get
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].active)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public void Reactivate(bool keepSpeed = true, bool evenIfActive=true)
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].Reactivate(keepSpeed,evenIfActive);
        }
    }

    public void Deactivate()
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].Deactivate();
        }
    }

    public void Abort()
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].Deactivate();
            patterns[i].active = false;
        }
    }
}
