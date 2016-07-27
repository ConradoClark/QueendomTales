using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    [Header("Controller")]
    public FullCharacterController controller;
    [Header("Effects")]
    public GameObject Slash1Prefab_Right;
    public GameObject Slash1Prefab_Left;
    public Vector3 Slash1Offset;

    public GameObject Slash2Prefab_Right;
    public GameObject Slash2Prefab_Left;
    public Vector3 Slash2Offset;

    [Header("Stats")]
    public CharacterStats CurrentStats;
    public CharacterStats OriginalStats;
}
