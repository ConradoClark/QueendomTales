using UnityEngine;
using System.Collections;

public class InvertedLightTest : MonoBehaviour {

    public Light light;
	// Use this for initialization
	void Start () {
        light.color = new Color(-1f, -1f, -1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
