using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Enemies/Basic Enemy Behaviour")]
public class BasicEnemy : FrostyPoolableObject
{
    public CharacterStats stats;
    private CharacterStats currentStats;

    public FrostyMovementPredicateCustom hurt;
    public Transform mainTransform;
    public Animator animator;
    private bool damaged;
    private bool dying;
    public float deathDelay;
    private bool dead;

    public FrostyPoolableObject expCollectable;
    public FrostyPoolableObject coinsCollectable;
    public FrostyPoolableObject dyingParticle;
    public TargetableObject targetableObject;
    public TimeLayers timeLayer;

    public int expAmount;
    public int minCoinAmount;
    public int maxCoinAmount;

    public string randomPool;
    public int generatorSeed;

    void Start()
    {
        this.currentStats = new CharacterStats()
        {
            Attack = stats.Attack,
            Defense = stats.Defense,
            HP = stats.HP,
            FP = stats.FP,
            Level = stats.Level
        };
        Toolbox.Instance.random.AddGenerator(randomPool, generatorSeed);
    }

    void Update()
    {
        animator.SetBool("hurt", hurt.Value);
        if (damaged)
        {
            damaged = false;
            return;
        }
        if (dead)
        {
            var cursor = this.GetComponentInChildren<TargetCursor>();
            if (cursor != null)
            {
                cursor.transform.SetParent(null);
            }
            Toolbox.Instance.pool.Release(this, this.gameObject);
        }
        if (dying)
        {
            animator.SetBool("dying", true);
            deathDelay -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
            if (deathDelay <= 0)
            {
                this.dying = false;
                this.dead = true;
            }
        }
        hurt.SetValue(false);
    }

    public virtual bool TryDamage(uint amount)
    {
        this.hurt.SetValue(true);
        damaged = true;
        this.currentStats.HP -= (int)amount;
        if (this.currentStats.HP <= 0)
        {
            this.Die();
        }
        return true;
    }

    protected virtual void Die()
    {
        if (dyingParticle == null) return;
        GameObject particle = Toolbox.Instance.pool.Retrieve(dyingParticle);
        particle.transform.position = this.transform.position + new Vector3(0, 0, dyingParticle.transform.localPosition.z);

        float expPerc = (float)expAmount / QTToolbox.Instance.characterManager.character.OriginalStats.Experience * 100;
        int expPercInt = (int)expPerc;

        for (int i = 0; i < expPercInt; i++)
        {
            GameObject exp = Toolbox.Instance.pool.Retrieve(expCollectable);
            exp.transform.position = this.transform.position;
        }

        int coins = Toolbox.Instance.random.GetRange(randomPool, minCoinAmount, maxCoinAmount + 1);
        //Debug.Log(coins.ToString() + " coins");
        for (int i = 0; i < coins; i++)
        {
            GameObject exp = Toolbox.Instance.pool.Retrieve(coinsCollectable);
            exp.transform.position = this.transform.position;
        }

        dying = true;
    }

    public override void ResetState()
    {
        base.ResetState();

        this.currentStats = new CharacterStats()
        {
            Attack = stats.Attack,
            Defense = stats.Defense,
            HP = stats.HP,
            FP = stats.FP,
            Level = stats.Level
        };

        this.damaged = this.dead = this.dying = false;
        animator.StopPlayback();
        animator.transform.localScale = Vector3.one;
        animator.transform.localRotation = Quaternion.identity;
        animator.Rebind();
    }

    public bool IsDead()
    {
        return this.dead;
    }
}
