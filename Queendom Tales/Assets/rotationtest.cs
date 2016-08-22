using UnityEngine;
using System.Collections;

public class rotationtest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}

    float f;
	// Update is called once per frame
	void Update () {
        f += Time.deltaTime;
        this.transform.Rotate(new Vector3(0, 0, 2* f));	
	}
}
