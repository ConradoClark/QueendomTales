using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Queendom-Tales/Character/Character Weapon")]
public class CharacterWeapon : MonoBehaviour
{
    public int currentComboSlot { get; private set; }
    public Character character;
    public int chargeEventStep = 1;
    public delegate void weaponEvent(int currentComboSlot);
    public delegate void chargeEvent(int currentCharge);
    public FrostyInputSequence combo;

    [Header("Animation")]
    public FrostyMovementPredicateCustom IsAttackingPredicate;
    public Animator animator;
    public FrostyMovementPredicate isOnAir;

    [Header("Effects")]
    public WeaponEffect[] weaponEffects;
    public HomingMovement homing;

    [Header("Cooldown")]
    public float comboCooldown;
    public float attackCooldown;
    public bool IsAttacking { get; set; }
    public bool IsFinisher { get; set; }

    private bool rotating = false;
    private float currentAngle;
    private float angleVelocity;
    private float currentSide = 1;

    public TimeLayers timeLayer;
    public TimeLayers globalTimeLayer;

    void Start()
    {
        StartCoroutine(Combo());
    }
    //void OnGUI()
    //{
    //    GUI.contentColor = new Color(0, 0.55f, 1);
    //    GUI.Label(new Rect(150, 200, 1000, 100), combo.currentMove.ToString());
    //}

    protected virtual void Attack()
    {
        if (character.CurrentWeapon.weaponEffects.Length <= 0) return;

        var angle = Random.Range(15f, 25f);
        if (isOnAir.Value)
        {
            homing.enableMovement = true;
            rotating = true;
            if (currentComboSlot == 0)
            {
                currentSide = character.controller.targetCursor.currentTarget != null && this.transform.position.y > character.controller.targetCursor.currentTarget.transform.position.y ? -1 : 1;
            }
            currentAngle += angle * currentSide * character.controller.GetFacingDirection().x;            
            currentSide = -currentSide;
        }
        else
        {
            homing.enableMovement = false;
        }

        var nextAnim = isOnAir.Value && rotating && currentComboSlot == 0 ? 2 : Random.Range(0, character.CurrentWeapon.weaponEffects.Length);

        var effect = character.CurrentWeapon.weaponEffects[nextAnim];

        animator.SetFloat("attackSpeed", 1f - 0.5f * character.CurrentWeapon.attackCooldown);
        animator.CrossFade(effect.Animation, 0f, 0, 0f);        

        var obj = GameObject.Instantiate(character.controller.GetFacingDirection().x == -1 ? effect.Prefab_Left : effect.Prefab_Right);
        Vector3 pos = obj.transform.localPosition;
        obj.transform.SetParent(animator.transform, false);
        obj.transform.localPosition = pos + new Vector3(effect.Offset.x * character.controller.GetFacingDirection().x, effect.Offset.y);

        if (effect.Movement != null)
        {
            effect.Movement.SetDirection(character.controller.GetFacingDirection());
            effect.Movement.Reactivate(false, true);
        }

        obj.transform.Rotate(new Vector3(0, 0, angle));

        var pss = obj.GetComponentsInChildren<ParticleSystem>();

        if (IsFinisher)
        {
            obj.transform.localScale = new Vector3(1.5f, 1.5f, 2);
            foreach(var ps in pss)
            {
                ps.startColor = Color.yellow;
            }
            this.IsFinisher = false;
        }

        effect.Hit.transform.localPosition = new Vector3(-10, 0, 0);
        effect.Hit.direction = (Quaternion.AngleAxis(angle, Vector2.right) * character.controller.GetFacingDirection()).normalized;
        StartCoroutine(HitTest(effect));
    }

    void Update()
    {
        float newRot = Mathf.SmoothDampAngle(character.transform.localRotation.eulerAngles.z, currentAngle, ref angleVelocity, 0.15f);
        character.transform.localRotation = Quaternion.Euler(0, 0, newRot);

        if (!rotating)
        {
            currentAngle = 0f;
        }
    }

    public virtual IEnumerator HitTest(WeaponEffect effect)
    {
        float delayElapsed = 0f;
        while (delayElapsed < effect.HitDelay)
        {
            delayElapsed += Toolbox.Instance.time.GetDeltaTime(timeLayer);
            yield return 1;
        }

        effect.Hit.enabled = true;
        yield return 1;

        Dictionary<int, BasicEnemy> colliders = new Dictionary<int, BasicEnemy>();
        List<int> exclusion = new List<int>();

        float lockTime = 0f;

        delayElapsed = 0f;
        while (delayElapsed < effect.HitDuration)
        {
            foreach (var hit in effect.Hit.AllHits)
            {
                if (!colliders.ContainsKey(hit.collider.GetInstanceID()))
                {
                    colliders.Add(hit.collider.GetInstanceID(), hit.collider.GetComponent<BasicEnemy>());
                }
            }

            foreach (var enemy in colliders)
            {
                if (exclusion.Contains(enemy.Key)) continue;
                var atk = (character.CurrentStats.Attack * effect.AttackMultiplier);
                if (atk >= 0)
                {
                    enemy.Value.TryDamage((uint)atk);
                    Toolbox.Instance.time.SetLayerMultiplier(timeLayer, 0.1f);
                    lockTime = 0.2f;
                    exclusion.Add(enemy.Key);
                }
            }

            foreach (var enemy in exclusion)
            {
                if (colliders.ContainsKey(enemy))
                {
                    colliders.Remove(enemy);
                }
            }

            lockTime = Mathf.Clamp(lockTime - Toolbox.Instance.time.GetDeltaTime(globalTimeLayer), 0, lockTime);
            if (lockTime <= 0)
            {
                Toolbox.Instance.time.SetLayerMultiplier(timeLayer, 1.0f);
            }
            delayElapsed += Toolbox.Instance.time.GetDeltaTime(globalTimeLayer);
            yield return 1;
        }
        Toolbox.Instance.time.SetLayerMultiplier(timeLayer, 1.0f);
        effect.Hit.enabled = false;
    }

    public virtual IEnumerator Combo()
    {
        while (this.enabled)
        {
            IsAttackingPredicate.SetValue(false);
            this.combo.EvaluateInput().MoveNext();

            if (currentComboSlot == 0f)
            {
                currentAngle = 0f;
                homing.enableMovement = false;
            }

            if (currentComboSlot < combo.currentMove)
            {
                if (combo.currentMove == character.CurrentStats.ComboLength)
                {
                    IsFinisher = true;
                }
                Attack();
                IsAttackingPredicate.SetValue(true);
                yield return Toolbox.Instance.time.WaitForSeconds(timeLayer, attackCooldown);
            }

            this.currentComboSlot = combo.currentMove;

            if (this.currentComboSlot == character.CurrentStats.ComboLength)
            {
                StartCoroutine(Finisher());
                yield break;
            }

            yield return 1;
        }
    }

    public virtual IEnumerator Charge()
    {
        yield break;
    }

    public virtual IEnumerator Finisher()
    {
        Debug.Log("FINISH! ");
        IsAttackingPredicate.SetValue(false);
        currentAngle = 0f;
        homing.enableMovement = false;
        yield return Toolbox.Instance.time.WaitForSeconds(timeLayer, comboCooldown);

        currentComboSlot = 0;
        StartCoroutine(Combo());
        yield break;
    }
}
