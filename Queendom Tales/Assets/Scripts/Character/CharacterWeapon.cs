using UnityEngine;
using System.Collections;

public class CharacterWeapon : MonoBehaviour
{
    public int currentComboSlot { get; private set; }
    public Character character;
    public int chargeEventStep = 1;
    public delegate void weaponEvent(int currentComboSlot);
    public delegate void chargeEvent(int currentCharge);
    public event weaponEvent onSwing;
    public FrostyInputSequence combo;

    [Header("Effects")]
    public GameObject Slash1Prefab_Right;
    public GameObject Slash1Prefab_Left;
    public Vector3 Slash1Offset;

    public GameObject Slash2Prefab_Right;
    public GameObject Slash2Prefab_Left;
    public Vector3 Slash2Offset;

    public GameObject Slash3Prefab_Right;
    public GameObject Slash3Prefab_Left;
    public Vector3 Slash3Offset;

    void Start()
    {
        StartCoroutine(Combo());
    }
    //void OnGUI()
    //{
    //    GUI.contentColor = new Color(0, 0.55f, 1);
    //    GUI.Label(new Rect(150, 200, 1000, 100), combo.currentMove.ToString());
    //}
    

    public virtual IEnumerator Combo()
    {
        while (this.enabled)
        {
            this.combo.EvaluateInput().MoveNext();
            this.currentComboSlot = combo.currentMove;
            if (this.currentComboSlot == character.CurrentStats.ComboLength)
            {
                StartCoroutine(Finisher());
                yield break;
            }
            yield return 1;
        }
        //IEnumerator result = null;
        //bool pressedComboButton = false;

        //currentComboSlot = 1;
        //while (currentComboSlot < character.CurrentStats.ComboLength)
        //{
        //    result = combo.EvaluateInput();
        //    while(result.MoveNext() || pressedComboButton)
        //    {
        //        pressedComboButton |= currentComboSlot < combo.currentMove;
        //        yield return 1;
        //    }

        //    if (!pressedComboButton)
        //    {
        //        StartCoroutine(Standby());
        //        yield break;
        //    }

        //    yield return 1;
        //    currentComboSlot++;
        //}

        //result = combo.EvaluateInput();
        //pressedComboButton = false;
        //while (!result.MoveNext())
        //{
        //    pressedComboButton = (bool)result.Current;
        //    yield return 1;
        //}

        //if (!pressedComboButton)
        //{
        //    StartCoroutine(Standby());
        //    yield break;
        //}

        //StartCoroutine(Finisher());
    }

    public virtual IEnumerator Charge()
    {
        yield break;
    }

    public virtual IEnumerator Finisher()
    {
        Debug.Log("FINISH! ");
        StartCoroutine(Combo());
        yield break;
    }
}
