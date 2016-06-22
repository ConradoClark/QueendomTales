using UnityEngine;
using System.Collections;

public class StandAnimGroup : AnimGroup
{
    public Transform FacingRight;
    public Transform FacingLeft;
    private bool facingRight = true;
    private bool active;

    public override void FaceLeft()
    {
        facingRight = false;
        Set();
    }

    public override void FaceRight()
    {
        facingRight = true;
        Set();
    }

    public override void StartAnim()
    {
        active = true;
        Set();
    }

    public override void StopAnim()
    {
        active = false;
        Set();
    }

    void Set()
    {
        FacingRight.gameObject.SetActive(active && facingRight);
        FacingLeft.gameObject.SetActive(active && !facingRight);
    }
}
