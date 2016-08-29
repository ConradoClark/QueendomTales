using UnityEngine;
using System.Collections;

public class ParallaxBackground : MonoBehaviour
{
    private float origX;
    public float cameraMultiplier;
    void Start()
    {
        this.origX = this.transform.position.x;
    }

    void Update()
    {
        this.transform.position = new Vector3(this.origX + Camera.main.transform.position.x * cameraMultiplier, this.transform.position.y, this.transform.position.z);
    }
}
