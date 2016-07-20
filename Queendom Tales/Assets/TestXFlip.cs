using UnityEngine;
using System.Collections;

public class TestXFlip : MonoBehaviour {

    public SpriteRenderer[] renderers;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey(KeyCode.RightArrow))
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].flipX = false;
            }
            return;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].flipX = true;
            }
        }
    }
}
