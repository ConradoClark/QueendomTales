using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour
{
    public CharacterStats stats;
    private CharacterStats currentStats;

    public FrostyMovementPredicateCustom hurt;
    public Animator animator;
    private bool damaged;

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
    }

    void Update()
    {
        animator.SetBool("hurt", hurt.Value);
        if (damaged)
        {
            damaged = false;
            return;
        }
        hurt.SetValue(false);
    }

    public virtual bool TryDamage(uint amount)
    {
        this.hurt.SetValue(true);
        damaged = true;
        //this.currentStats.HP -= (int)amount;
        //if (this.currentStats.HP <= 0)
        //{
        //    this.Die();
        //}
        return true;
    }

    protected virtual void Die()
    {

    }
}
