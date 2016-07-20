using UnityEngine;
using System.Collections;

public class FullCharacterController : MonoBehaviour
{
    public Animator characterAnimator;
    public FrostyKinematics kinematics;
    public FrostyMovementPredicate grounded;
    public FrostyMovementPredicate landing;
    public FrostyPatternMovement jump;

    void Start()
    {
        characterAnimator.SetFloat("x", 1);
    }

    void Update()
    {
        float inputAxisX = Input.GetAxisRaw("Horizontal");
        bool isMoving = inputAxisX != 0;
        if (isMoving)
        {
            characterAnimator.SetFloat("x", inputAxisX);
        }
        characterAnimator.SetBool("isMoving", isMoving);
        characterAnimator.SetBool("jumped", jump.IsActivating);
        characterAnimator.SetBool("grounded", grounded.Value);
        characterAnimator.SetBool("landing", landing.Value);

    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        characterAnimator.SetFloat("xSpeed", (kinematics.GetSpeed(Vector2.right) + 0.2f) * horizontal);
    }
}
