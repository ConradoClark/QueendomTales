using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrostySingleInputFragment : FrostyInputActionFragment
{
    public float minTime;
    public float maxTime;
    public InputFragmentType fragmentType;
    private float elapsedTime = 0f;
    public string usedAction;
    private IEnumerator<bool> evaluation;
    public TimeLayers timeLayer;

    public override IEnumerator<bool> EvaluateInput()
    {
        if (evaluation == null || !evaluation.MoveNext())
        {
            Evaluate();
        }
        else
        {
            yield return evaluation.Current;
        }

        while (evaluation.MoveNext())
        {
            yield return evaluation.Current;
        }
        evaluation = null;
    }

    private void Evaluate()
    {
        switch (this.fragmentType)
        {
            case InputFragmentType.NoInput:
                evaluation = EvaluateNoInput();
                break;
            case InputFragmentType.Click:
                evaluation = EvaluateClick();
                break;
            case InputFragmentType.Hold:
                evaluation = EvaluateHold();
                break;
            case InputFragmentType.Release:
                evaluation = EvaluateRelease();
                break;
            default:
                return;
        }
    }

    protected override IEnumerator<bool> EvaluateNoInput()
    {
        elapsedTime = 0f;
        while (elapsedTime < minTime)
        {
            elapsedTime += Toolbox.Instance.frostyTime.GetDeltaTime(timeLayer);
            yield return false;
        }

        while (elapsedTime < maxTime)
        {
            if (inputManager.GetActionHeld(this.usedAction))
            {
                yield return false;
                yield break;
            }
            elapsedTime += Toolbox.Instance.frostyTime.GetDeltaTime(timeLayer);
            yield return false;
        }
        yield return true;
    }

    protected override IEnumerator<bool> EvaluateClick()
    {
        elapsedTime = 0f;
        while (elapsedTime < minTime)
        {
            elapsedTime += Toolbox.Instance.frostyTime.GetDeltaTime(timeLayer);
            yield return false;
        }

        while (elapsedTime < maxTime)
        {
            if (inputManager.GetActionClicked(this.usedAction))
            {
                yield return true;
                yield break;
            }

            elapsedTime += Toolbox.Instance.frostyTime.GetDeltaTime(timeLayer);
            yield return false;
        }

        if (inputManager.GetActionClicked(this.usedAction))
        {
            yield return true;
            yield break;
        }

        yield return false;
    }

    protected override IEnumerator<bool> EvaluateHold()
    {
        elapsedTime = 0f;
        while (elapsedTime < minTime)
        {
            if (!inputManager.GetActionHeld(this.usedAction))
            {
                yield return false;
                yield break;
            }
            elapsedTime += Toolbox.Instance.frostyTime.GetDeltaTime(timeLayer);
            yield return false;
        }

        if (minTime == 0f)
        {
            if (!inputManager.GetActionHeld(this.usedAction))
            {
                yield return false;
                yield break;
            }
        }

        elapsedTime = 0f;
        while (elapsedTime < maxTime && inputManager.GetActionHeld(this.usedAction))
        {
            elapsedTime += Toolbox.Instance.frostyTime.GetDeltaTime(timeLayer);
            yield return false;
        }

        yield return true;
    }

    protected override IEnumerator<bool> EvaluateRelease()
    {
        elapsedTime = 0f;
        while (elapsedTime < minTime)
        {
            elapsedTime += Toolbox.Instance.frostyTime.GetDeltaTime(timeLayer);
            yield return false;
        }

        while (elapsedTime < maxTime)
        {
            elapsedTime += Toolbox.Instance.frostyTime.GetDeltaTime(timeLayer);
            if (inputManager.GetActionClicked(this.usedAction))
            {
                yield return false;
                yield break;
            }

            if (inputManager.GetActionReleased(this.usedAction))
            {
                yield return true;
                yield break;
            }
            yield return false;
        }

        if (inputManager.GetActionReleased(this.usedAction))
        {
            yield return true;
            yield break;
        }
        yield return false;
        yield break;
    }
}
