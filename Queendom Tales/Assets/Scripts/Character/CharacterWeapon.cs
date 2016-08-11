using UnityEngine;
using System.Collections;

public class CharacterWeapon : MonoBehaviour
{
    public int currentComboSlot;
    public float comboTimeout;
    public Character character;
    public int chargeEventStep = 1;
    public delegate void weaponEvent(int currentComboSlot);
    public delegate void chargeEvent(int currentCharge);
    public event weaponEvent onSwing;
    //public event weaponEvent 

    public virtual IEnumerator Standby()
    {
        while (!Input.GetKeyDown(KeyCode.Z))
        {
            yield return 1;
        }
        StartCoroutine(Combo());
    }

    public virtual IEnumerator Combo()
    {
        IEnumerator result = null;
        bool pressedComboButton = false;

        currentComboSlot = 1;
        while (currentComboSlot < character.CurrentStats.ComboLength)
        {
            result = WaitForInput();
            pressedComboButton = false;
            while(!result.MoveNext())
            {
                pressedComboButton = (bool) result.Current;
                yield return 1;
            }

            if (!pressedComboButton)
            {
                StartCoroutine(Standby());
                yield break;
            }

            yield return 1;
            currentComboSlot++;
        }

        result = WaitForInput();
        pressedComboButton = false;
        while (!result.MoveNext())
        {
            pressedComboButton = (bool)result.Current;
            yield return 1;
        }

        if (!pressedComboButton)
        {
            StartCoroutine(Standby());
            yield break;
        }

        StartCoroutine(Finisher());
    }

    public virtual IEnumerator Charge()
    {
        yield break;
    }

    public virtual IEnumerator Finisher()
    {
        StartCoroutine(Standby());
        yield break;
    }

    private IEnumerator WaitForInput()
    {
        bool pressedComboButton = false;
        float currentComboTimeout = comboTimeout;
        while (currentComboTimeout > 0 && !pressedComboButton)
        {            
            pressedComboButton |= Input.GetKeyDown(KeyCode.Z);
            currentComboTimeout -= Time.deltaTime;
            if (pressedComboButton)
            {
                yield return true;
                yield break;
            }else
            {
                yield return false;
            }
        }
        yield return pressedComboButton;
    }
}
