using UnityEngine;
using System.Collections;
using System.Linq;

[AddComponentMenu("Queendom-Tales/Targeting/Target Cursor")]
public class TargetCursor : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    private const string LockOn_Animator = "LockOn";
    private const string NoTarget_Animator = "NoTarget";
    public FullCharacterController player;
    [Header("Configuration")]
    public float minimumDistance;
    public bool targetingEnabled;
    public FrostyMovementPredicateCustom lockOnRight;
    public FrostyMovementPredicateCustom lockOnLeft;
    [Header("Lock-On (Leave empty)")]
    public bool lockedOn;
    public TargetableObject lockOnTarget;
    public TargetableObject currentTarget;

    void Start()
    {
        StartCoroutine(WaitForTarget());
    }

    void Update()
    {
        Vector2 facingDir = GetLockOnFacingDirection();
        lockOnRight.SetValue(facingDir == Vector2.right);
        lockOnLeft.SetValue(facingDir == Vector2.left);
    }

    IEnumerator WaitForTarget()
    {
        bool changedTarget = true;
        LockOff();
        while (targetingEnabled)
        {
            lockedOn = false;
            TargetableObject target;
            if (CheckDistances(out target))
            {
                if (changedTarget)
                {
                    animator.CrossFade("Cursor_FadeIn", 0f);
                }
                StartCoroutine(NormalState(target));
                yield break;
            }
            changedTarget = false;
            yield return 1;
        }
        StartCoroutine(WaitForActivation());
    }

    IEnumerator NormalState(TargetableObject target)
    {
        LockOff();
        animator.SetBool(LockOn_Animator, false);
        animator.SetBool(NoTarget_Animator, false);
        this.transform.SetParent(target.transform, false);
        this.transform.localPosition = target.cursorOffset;

        TargetableObject closestTarget = target;
        while (targetingEnabled)
        {
            if (this.lockedOn)
            {
                this.lockOnTarget = closestTarget;
            }

            if (closestTarget != target && !lockedOn)
            {
                StartCoroutine(ChangeTarget(closestTarget));
                yield break;
            }

            if (!CheckDistances(out closestTarget))
            {
                animator.SetBool(NoTarget_Animator, true);
                StartCoroutine(WaitForTarget());
                yield break;
            }

            yield return 1;
        }
        StartCoroutine(WaitForActivation());
    }

    IEnumerator ChangeTarget(TargetableObject target)
    {
        yield return 1;
        StartCoroutine(NormalState(target));
    }

    IEnumerator WaitForActivation()
    {
        while (!targetingEnabled)
        {
            yield return 1;
        }
        StartCoroutine(WaitForTarget());
    }

    public void LockOn()
    {
        this.lockedOn = true;
        animator.SetBool(LockOn_Animator, true);
    }

    public void LockOff()
    {
        this.lockedOn = false;
        lockOnTarget = null;
        animator.SetBool(LockOn_Animator, false);
    }

    private bool CheckDistances(out TargetableObject target)
    {
        float foundDistance = float.MaxValue;
        TargetableObject foundObject = null;

        if (lockedOn && lockOnTarget!=null)
        {
            foundObject = lockOnTarget;
            foundDistance = Vector2.Distance(foundObject.transform.position, player.transform.position);
        }
        else
        {
            for (int i = 0; i < Toolbox.Instance.targetableObjectManager.objects.Count; i++)
            {
                TargetableObject obj = Toolbox.Instance.targetableObjectManager.objects[i];
                float distance = Vector2.Distance(obj.transform.position, player.transform.position);
                bool isFacing = (obj.transform.position.x * player.GetFacingDirection().x > player.transform.position.x * player.GetFacingDirection().x);
                if (distance < foundDistance && isFacing)
                {
                    foundObject = obj;
                    foundDistance = distance;
                }
            }
        }

        if (foundDistance > minimumDistance * (lockedOn ? 2 : 1))
        {
            target = currentTarget = null;

            return false;
        }
        else
        {
            target = currentTarget = foundObject;
            return true;
        }
    }

    public Vector2 GetLockOnFacingDirection()
    {
        if (!lockedOn || lockOnTarget==null) return Vector2.zero;

        if (lockOnTarget.transform.position.x < player.transform.position.x) return Vector2.left;

        return Vector2.right;
    }
}
