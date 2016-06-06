using UnityEngine;
using System.Collections;

public class FollowMouseLightTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
	}
}
