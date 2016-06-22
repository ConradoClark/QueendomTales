using UnityEngine;
using System.Collections;

public class ChangeFaceAnimGroup : OneTimeAnimGroup
{
    public SpriteRenderer[] rightAnims;
    public SpriteRenderer[] leftAnims;

    public override void FaceRight()
    {
        base.FaceRight();
        foreach (SpriteRenderer rightAnim in rightAnims)
        {
            rightAnim.gameObject.SetActive(this.active);
            rightAnim.flipX = false;
        }

        foreach(SpriteRenderer leftAnim in leftAnims)
        {
            leftAnim.gameObject.SetActive(false);
        }
    }

    public override void FaceLeft()
    {
        base.FaceLeft();
        foreach (SpriteRenderer leftAnim in leftAnims)
        {
            leftAnim.gameObject.SetActive(this.active);
            leftAnim.flipX = true;
        }
        foreach (SpriteRenderer rightAnim in rightAnims)
        {
            rightAnim.gameObject.SetActive(false);
        }
    }

    public override void StartAnim()
    {
        base.StartAnim();
        foreach (SpriteRenderer rightAnim in rightAnims)
        {
            rightAnim.gameObject.SetActive(this.active && facingRight);
        }
        foreach (SpriteRenderer leftAnim in leftAnims)
        {
            leftAnim.gameObject.SetActive(this.active && !facingRight);
        }
    }

    public override void StopAnim()
    {
        base.StopAnim();
        foreach (SpriteRenderer rightAnim in rightAnims)
        {
            rightAnim.gameObject.SetActive(false);
        }
        foreach (SpriteRenderer leftAnim in leftAnims)
        {
            leftAnim.gameObject.SetActive(false);
        }
    }
}
