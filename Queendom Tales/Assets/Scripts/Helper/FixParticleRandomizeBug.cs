using UnityEngine;
using System.Collections;

public class FixParticleRandomizeBug : MonoBehaviour
{
    public ParticleSystem ps;

    void Start()
    {
        ps.Stop();
        ps.randomSeed = (uint)Random.Range(0, uint.MaxValue);
        ps.Play();
    }
}
