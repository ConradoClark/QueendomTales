using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Character/Character Container")]
public class Character : MonoBehaviour
{
    [Header("Controller")]
    public FullCharacterController controller;

    [Header("Stats")]
    public CharacterStats CurrentStats;
    public CharacterStats OriginalStats;

    [Header("Equipment")]
    public CharacterWeapon CurrentWeapon;
    public CharacterUI UI { get; protected set; }

    [Header("Currency")]
    public int Coins;

    void Start()
    {
        UI.HPCounter.currentValue = CurrentStats.HP;
        UI.HPCounter.maxValue = OriginalStats.HP;
        UI.FPCounter.currentValue = CurrentStats.FP;
        UI.FPCounter.maxValue = OriginalStats.FP;
        UI.ExpCounter.maxValue = 100;
        UI.ExpCounter.currentValue = CurrentStats.Experience;
    }

    void Update()
    {
        if (this.CurrentStats.Experience == 100 && UI.ExpCounter.finished)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        AddExp(-100, false);
    }

    public void ConnectUI(CharacterUI ui)
    {
        this.UI = ui;
    }

    public void AddHP(int amount, bool flash = true)
    {
        this.CurrentStats.HP = Mathf.Clamp(this.CurrentStats.HP + amount, 0, OriginalStats.HP);
        if (UI.HPCounter == null) return;
        if (amount > 0)
        {
            UI.HPCounter.Increase(amount, flash);
        }
        else if (amount < 0)
        {
            UI.HPCounter.Decrease(amount, flash);
        }
    }

    public void AddFP(int amount, bool flash = true)
    {
        this.CurrentStats.FP = Mathf.Clamp(this.CurrentStats.FP + amount, 0, OriginalStats.FP);
        if (UI.FPCounter == null) return;
        if (amount > 0)
        {
            UI.FPCounter.Increase(amount, flash);
        }
        else if (amount < 0)
        {
            UI.FPCounter.Decrease(-amount, flash);
        }
    }

    public void AddExp(int amount, bool flash = true)
    {
        this.CurrentStats.Experience = Mathf.Clamp(this.CurrentStats.Experience + amount, 0, OriginalStats.Experience);
        if (UI.ExpCounter == null) return;
        if (amount > 0)
        {
            UI.ExpCounter.Increase(amount, flash);
        }
        else if (amount < 0)
        {
            UI.ExpCounter.Decrease(-amount, flash);
        }
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public void Collect(Collectable.CollectableType type, float multiplier)
    {
        switch (type)
        {
            case Collectable.CollectableType.Coin:
                AddCoins((int)multiplier);
                break;
            case Collectable.CollectableType.Exp:
                AddExp((int)multiplier);
                break;
        }
    }
}
