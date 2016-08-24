using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Character/Character Container")]
public class Character : MonoBehaviour {

    [Header("Controller")]
    public FullCharacterController controller;

    [Header("Stats")]
    public CharacterStats CurrentStats;
    public CharacterStats OriginalStats;

    [Header("Equipment")]
    public CharacterWeapon CurrentWeapon;
}
