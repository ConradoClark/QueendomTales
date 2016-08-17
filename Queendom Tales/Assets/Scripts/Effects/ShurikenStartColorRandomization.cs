using UnityEngine;
using System.Collections;

public class ShurikenStartColorRandomization : MonoBehaviour {

    public ParticleSystem ps;
    public Color[] colors;

	void Start () {
	
	}
	
	void Update () {
        if (colors.Length <= 0) return;
        ps.startColor = colors[Random.Range(0, colors.Length)];
	}
}
