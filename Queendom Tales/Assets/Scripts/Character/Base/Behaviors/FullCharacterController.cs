using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Character/Full Character Controller")]
public class FullCharacterController : MonoBehaviour
{
    public Animator characterAnimator;
    public FrostyKinematics kinematics;
    public FrostyMovementPredicate grounded;
    public FrostyMovementPredicate landing;
    public FrostyPatternMovement jump;
    private Vector2 facingDirection;
    public TargetCursor targetCursor;
    public FrostyMovementPredicate rightSlope;
    public FrostyMovementPredicate leftSlope;
    public FrostySlopeMovement slopeMovementR;
    public FrostySlopeMovement slopeMovementL;
    public FrostyInputActionFragment lockOnAction;
    public FrostyInputActionFragment rightAction;
    public FrostyInputActionFragment leftAction;
    public float turnDelay = 0.1f;
    public TimeLayers timeLayer;

    private bool pressingLeft, pressingRight;
    private float inputAxisX;

    float currentTurnDelay = 0f;
    bool turning = false;

    void Start()
    {
        InitializeDirection();
    }

    void InitializeDirection()
    {
        characterAnimator.SetFloat("x", 1);
        facingDirection = Vector2.right;
    }

    void CheckGlobalStates()
    {
        float currentFacingX = characterAnimator.GetFloat("x");
        float newX = currentFacingX;

        var rightInput = rightAction.EvaluateInput();
        rightInput.MoveNext();

        var leftInput = leftAction.EvaluateInput();
        leftInput.MoveNext();

        pressingRight = rightInput.Current;
        pressingLeft = leftInput.Current;

        inputAxisX = pressingRight ? 1 : (pressingLeft ? -1 : 0);
        bool isMoving = inputAxisX != 0;

        if (isMoving && !targetCursor.lockedOn)
        {
            newX = inputAxisX;
            facingDirection = new Vector2(inputAxisX, 0);
        } else if (targetCursor.lockedOn)
        {
            newX = targetCursor.GetLockOnFacingDirection().x;
        }

        if (currentFacingX != newX && !turning)
        {
            currentTurnDelay = turnDelay;
            turning = true;
        }

        if (currentTurnDelay<=0)
        {
            turning = false;
        }

        characterAnimator.SetFloat("x", turning ? currentFacingX : newX);
        characterAnimator.SetBool("isMoving", isMoving);
        characterAnimator.SetBool("jumped", jump.IsActivating);
        characterAnimator.SetBool("grounded", grounded.Value);
        characterAnimator.SetBool("landing", landing.Value);
        characterAnimator.SetBool("slope", rightSlope.Value || leftSlope.Value);

        if (currentTurnDelay > 0)
        {
            currentTurnDelay -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
        }
    }

    void CheckAttacks()
    {
        var input = lockOnAction.EvaluateInput();
        if (input.MoveNext() && input.Current)
        {
            if (!targetCursor.lockedOn)
            {
                targetCursor.LockOn();
            }
            else
            {
                targetCursor.LockOff();
            }
        }
    }

    void Update()
    {
        CheckGlobalStates();
        CheckAttacks();

        characterAnimator.speed = Toolbox.Instance.time.GetLayerMultiplier(timeLayer);
    }

    void LateUpdate()
    {
        bool lockOnBackwards = currentTurnDelay > 0 || (targetCursor.lockedOn && targetCursor.GetLockOnFacingDirection().x != inputAxisX);
        characterAnimator.SetFloat("xSpeed", 0.5f*(kinematics.GetSpeed(Vector2.right) + 0.2f*inputAxisX) * inputAxisX * (lockOnBackwards ? -1 : 1) * slopeMovementR.animSpeed * slopeMovementL.animSpeed);
        characterAnimator.SetFloat("ySpeed", (kinematics.GetSpeed(Vector2.up)));
    }

    public Vector2 GetFacingDirection()
    {
        return new Vector2(characterAnimator.GetFloat("x"), 0);
    }
}
