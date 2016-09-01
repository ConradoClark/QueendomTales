using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemySpawnZone : MonoBehaviour
{
    public Rect bounds;
    public int rngSeed;
    public EnemySpawnSet[] enemySets;
    public float spawnDelay;
    public float spawnCooldown;

    public SpawnType spawnType;
    public BattleManager battleManager;
    public TimeLayers spawnTimeLayer;
    public string customTimeCounterName;

    private Transform target;
    private float spawnDelayElapsed;
    private System.Random rng;
    private bool spawned;

    public enum SpawnType
    {
        SpawnAlways,
        OncePerRoom
    }

    IEnumerator Start()
    {
        yield return QTToolbox.Instance.characterManager.WaitForInitialization();

        this.target = QTToolbox.Instance.characterManager.character.transform;
        this.spawnDelayElapsed = spawnDelay;

        Toolbox.Instance.time.AddCustomTimeCounter(this.customTimeCounterName, spawnTimeLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(bounds.min, new Vector2(bounds.xMax, bounds.yMin));
        Gizmos.DrawLine(bounds.min, new Vector2(bounds.xMin, bounds.yMax));
        Gizmos.DrawLine(new Vector2(bounds.xMax, bounds.yMin), new Vector2(bounds.xMax, bounds.yMax));
        Gizmos.DrawLine(new Vector2(bounds.xMin, bounds.yMax), new Vector2(bounds.xMax, bounds.yMax));
    }

    void Update()
    {
        if (!QTToolbox.Instance.characterManager.IsInitialized()) return;

        if (spawnDelayElapsed > 0f)
        {
            spawnDelayElapsed -= Toolbox.Instance.time.GetDeltaTime(spawnTimeLayer);
            return;
        }

        bool isInZone = bounds.Contains(target.position);
        if (Toolbox.Instance.time.GetTotalElapsedTime(customTimeCounterName) > spawnCooldown && isInZone && (!spawned || this.spawnType == SpawnType.SpawnAlways))
        {
            spawned = true;
            StartCoroutine(StartBattle());
            Toolbox.Instance.time.ResetCustomTimeCounter(customTimeCounterName);
        }
    }

    IEnumerator StartBattle()
    {        
        if (battleManager == null) yield break;

        Toolbox.Instance.time.PauseCustomTimeCounter(customTimeCounterName);

        EnemySpawnSet set = enemySets[0];
        BasicEnemy[] enemies = new BasicEnemy[set.enemies.Length];

        for (int i = 0; i < set.enemies.Length; i++)
        {
            GameObject enemy = Toolbox.Instance.pool.Retrieve(set.enemies[i].poolObject);
            enemy.transform.position = set.enemies[i].spawnPosition;
            BasicEnemy basicEnemy = enemy.GetComponent<BasicEnemy>(); // not called every frame so it's ok            
            enemies[i] = basicEnemy;
        }

        battleManager.StartBattle(enemies);

        while (battleManager.IsInBattle())
        {
            yield return 1;
        }

        Toolbox.Instance.time.UnpauseCustomTimeCounter(customTimeCounterName);
    }
}