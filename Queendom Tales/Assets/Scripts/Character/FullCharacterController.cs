using UnityEngine;
using System.Collections;

public class FullCharacterController : MonoBehaviour
{
    public Animator characterAnimator;
    public FrostyKinematics kinematics;
    public FrostyMovementPredicate grounded;
    public FrostyMovementPredicate landing;
    public FrostyPatternMovement jump;
    private bool updatingAnimator;
    private bool createdAttackPrefab;
    private Vector2 facingDirection;
    public TargetCursor targetCursor;
    public FrostyMovementPredicate rightSlope;
    public FrostyMovementPredicate leftSlope;
    public FrostySlopeMovement slopeMovementR;
    public FrostySlopeMovement slopeMovementL;

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
        float inputAxisX = Input.GetAxisRaw("Horizontal");
        bool isMoving = inputAxisX != 0;
        if (isMoving && !targetCursor.lockedOn)
        {
            characterAnimator.SetFloat("x", inputAxisX);
            facingDirection = new Vector2(inputAxisX,0);
        }else if (targetCursor.lockedOn)
        {
            characterAnimator.SetFloat("x", targetCursor.GetLockOnFacingDirection().x);
        }
        characterAnimator.SetBool("isMoving", isMoving);
        characterAnimator.SetBool("jumped", jump.IsActivating);
        characterAnimator.SetBool("grounded", grounded.Value);
        characterAnimator.SetBool("landing", landing.Value);
        characterAnimator.SetBool("slope", rightSlope.Value || leftSlope.Value);
    }

    void CheckAttacks()
    {
        //test
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!targetCursor.lockedOn)
            {
                targetCursor.LockOn();
            }else
            {
                targetCursor.LockOff();
            }            
        }
    }

    void Update()
    {
        CheckGlobalStates();
        CheckAttacks();
    }

    void LateUpdate()
    {        
        float inputAxisX = Input.GetAxisRaw("Horizontal");
        bool lockOnBackwards = targetCursor.lockedOn && targetCursor.GetLockOnFacingDirection().x != inputAxisX;
        characterAnimator.SetFloat("xSpeed", (kinematics.GetSpeed(Vector2.right) + 0.2f*inputAxisX) * inputAxisX * (lockOnBackwards ? -1 : 1) * slopeMovementR.animSpeed * slopeMovementL.animSpeed);
        characterAnimator.SetFloat("ySpeed", (kinematics.GetSpeed(Vector2.up)));
    }

    public Vector2 GetFacingDirection()
    {
        return facingDirection;
    }
}
