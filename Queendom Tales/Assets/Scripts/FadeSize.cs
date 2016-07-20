using UnityEngine;
using System.Collections;

public class FadeSize : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale -= (Vector3) Vector2.one * Time.deltaTime;
    }
}
