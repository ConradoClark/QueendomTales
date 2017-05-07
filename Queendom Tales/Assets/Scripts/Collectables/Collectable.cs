using UnityEngine;
using System.Collections;

public class Collectable : FrostyPoolableObject
{
    public enum CollectableType
    {
        Coin,
        Exp
    }

    public float delay;    
    public TimeLayers timeLayer;
    public CollectableType type;
    public float collectableMultiplier;
    public Vector2 targetOffset;
    public float minCollectDistance = 5f;
    private float delayElapsed;
    private Transform target;

    IEnumerator Start()
    {
        if (!QTToolbox.Instance.characterManager.IsInitialized())
        {
            yield return QTToolbox.Instance.characterManager.WaitForInitialization();
        }
        this.target = QTToolbox.Instance.characterManager.character.transform;
        this.delayElapsed = delay;
    }

    void Update()
    {
        if (!QTToolbox.Instance.characterManager.IsInitialized()) return;

        delayElapsed -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
        if (delayElapsed > 0) return;

        float distance = Vector2.Distance((Vector2)target.transform.position + targetOffset, this.transform.position);

        if (distance < minCollectDistance)
        {
            Toolbox.Instance.pool.Release(this, this.gameObject);
            QTToolbox.Instance.characterManager.character.Collect(type, collectableMultiplier);
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        delayElapsed = delay;
    }

}
