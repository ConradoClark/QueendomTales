using UnityEngine;
using System.Collections;

public class FadeParticle : MonoBehaviour
{

    public ParticleSystem ps;
    float opacity=1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material.SetFloat("_Opacity", opacity);
        opacity -= Time.deltaTime*2;
    }
}
