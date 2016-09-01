using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Effects/Particles/Fade Particle Effect")]
public class FadeParticle : FrostyPoolableObject
{
    public ParticleSystem ps;
    float opacity=1;
    public float speed = 1;

    private ParticleSystemRenderer psr;
    private ParticleSystemRenderer GetPsr()
    {
        if (psr == null)
        {
            psr = ps.GetComponent<ParticleSystemRenderer>();
        }
        return psr;
    }

    void Update()
    {
        var psr = GetPsr();
        psr.material.SetFloat("_Opacity", opacity);
        opacity -= Time.deltaTime*2 * speed;
    }

    public override void ResetState()
    {        
        this.opacity = 1;
        var psr = GetPsr();
        psr.material.SetFloat("_Opacity", opacity);
    }
}
