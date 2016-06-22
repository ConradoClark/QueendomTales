using UnityEngine;
using System.Collections;

public class FullCharacterController : MonoBehaviour
{
    [Header("Animations")]
    public AnimGroup StandAnimation;
    public AnimGroup WalkAnimation;
    public AnimGroup StartRunningAnimation;
    public AnimGroup RunningAnimation;
    public AnimGroup StopRunningAnimation;
    public AnimGroup StartJumpingAnimation;

    [Header("Movement")]
    public FrostyPatternMovement MoveRight;
    public FrostyPatternMovement MoveLeft;
    public FrostyPatternMovement Jump;
    public FrostyMovementPredicate OnGround;
    public FrostyMovementPredicate IsJumping;
    private bool goingRight;

    void Start()
    {
        StartCoroutine(HandleStanding());
    }

    void DeactivateAllAnims()
    {
        StandAnimation.StopAnim();
        WalkAnimation.StopAnim();
        StartRunningAnimation.StopAnim();
        RunningAnimation.StopAnim();
        StopRunningAnimation.StopAnim();
        StartJumpingAnimation.StopAnim();
    }

    void FaceLeft()
    {
        StandAnimation.FaceLeft();
        WalkAnimation.FaceLeft();
        StartRunningAnimation.FaceLeft();
        RunningAnimation.FaceLeft();
        StopRunningAnimation.FaceLeft();
        StartJumpingAnimation.FaceLeft();
    }

    void FaceRight()
    {
        StandAnimation.FaceRight();
        WalkAnimation.FaceRight();
        StartRunningAnimation.FaceRight();
        RunningAnimation.FaceRight();
        StopRunningAnimation.FaceRight();
        StartJumpingAnimation.FaceRight();
    }

    bool IsGoingRight()
    {
        bool moveLeft = MoveLeft.IsActive();
        bool moveRight = MoveRight.IsActive();
        return moveLeft && !moveRight ? false : true;
    }

    bool IsTryingToMove()
    {
        bool moveLeft = MoveLeft.IsActive();
        bool moveRight = MoveRight.IsActive();
        return moveLeft || moveRight;
    }

    bool TryingToMoveBothWays()
    {
        return MoveLeft.IsActive() && MoveRight.IsActive();
    }


    IEnumerator HandleStanding()
    {
        DeactivateAllAnims();
        StandAnimation.StartAnim();
        while (true)
        {
            if (Jump.IsActive() && !IsJumping.Value)
            {
                StartCoroutine(HandleStartJumping());
                yield break;
            }

            if (IsTryingToMove() && !TryingToMoveBothWays() && OnGround.Value)
            {
                StartCoroutine(HandleStartRunning());
                yield break;
            }

            yield return 1;
        }
    }

    IEnumerator HandleStartJumping()
    {
        if (IsGoingRight())
        {
            goingRight = true;
            FaceRight();
        }
        else
        {
            goingRight = false;
            FaceLeft();
        }

        DeactivateAllAnims();
        StartJumpingAnimation.StartAnim();
        while (true)
        {
            yield return 1;
            if (StartJumpingAnimation.HasFinished() || IsJumping.Value)
            {
                StartCoroutine(HandleFalling());
                yield break;
            }            
        }
    }

    IEnumerator HandleFalling()
    {
        while (!OnGround.Value)
        {
            yield return 1;
        }
        StartCoroutine(HandleStanding());
        yield break;
    }

    IEnumerator HandleStartRunning()
    {
        if (IsGoingRight())
        {
            goingRight = true;
            FaceRight();
        }else
        {
            goingRight = false;
            FaceLeft();
        }

        DeactivateAllAnims();
        StartRunningAnimation.StartAnim();
        while (true)
        {
            if (Jump.IsActive() && !IsJumping.Value)
            {
                StartCoroutine(HandleStartJumping());
                yield break;
            }

            if (!IsTryingToMove())
            {
                StartCoroutine(HandleStanding());
                yield break;
            }

            if (StartRunningAnimation.HasFinished())
            {
                StartCoroutine(HandleRunning());
                yield break;
            }
            yield return 1;
        }
    }

    IEnumerator HandleRunning()
    {
        DeactivateAllAnims();
        RunningAnimation.StartAnim();

        while (true)
        {
            if (Jump.IsActive() && !IsJumping.Value)
            {
                StartCoroutine(HandleStartJumping());
                yield break;
            }

            if (!IsTryingToMove())
            {
                StartCoroutine(HandleStopRunning());
                yield break;
            }

            if (MoveLeft.IsActive() == goingRight || TryingToMoveBothWays())
            {
                StartCoroutine(HandleStopRunning());
                yield break;
            }

            yield return 1;
        }
    }

    IEnumerator HandleStopRunning()
    {
        DeactivateAllAnims();
        StopRunningAnimation.StartAnim();
        while (true)
        {
            if (Jump.IsActive() && !IsJumping.Value)
            {
                StartCoroutine(HandleStartJumping());
                yield break;
            }

            if (IsTryingToMove() && !TryingToMoveBothWays())
            {
                StartCoroutine(HandleStartRunning());
                yield break;
            }

            if (StopRunningAnimation.HasFinished())
            {
                StartCoroutine(HandleStanding());
                yield break;
            }

            yield return 1;
        }
    }
}
