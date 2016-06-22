using UnityEngine;
using System.Collections;

public class OneTimeAnimGroup : AnimGroup
{
    public SpriteRenderer[] oneTimeRenderers;
    public float animLength = 0.2f;
    protected bool facingRight = true;
    protected bool active;
    private bool hasFinished = false;
    private Coroutine waitForLengthCoroutine;

    public override void FaceLeft()
    {
        for (int i = 0; i < oneTimeRenderers.Length; i++)
        {
            oneTimeRenderers[i].flipX = true;
        }
        
        facingRight = false;
    }

    public override void FaceRight()
    {
        for (int i = 0; i < oneTimeRenderers.Length; i++)
        {
            oneTimeRenderers[i].flipX = false;
        }
        facingRight = true;
    }

    public override void StartAnim()
    {
        hasFinished = false;
        active = true;
        waitForLengthCoroutine = StartCoroutine(WaitForLength());        
        Set();
    }

    public override void StopAnim()
    {
        active = false;
        Set();
    }

    public override bool HasFinished()
    {
        return hasFinished;
    }

    protected void Set()
    {
        for (int i = 0; i < oneTimeRenderers.Length; i++)
        {
            oneTimeRenderers[i].gameObject.SetActive(active);
        }
    }

    IEnumerator WaitForLength()
    {
        float timeElapsed = 0f;
        while (timeElapsed < animLength && active)
        {
            timeElapsed += Time.smoothDeltaTime;
            yield return 1;
        }
        hasFinished = active;
    }
}
