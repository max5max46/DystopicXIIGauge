using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem particles;

    // Start is called before the first frame update
    public void StartParticles(Color particleColor)
    {
        particles.startColor = particleColor;
        particles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (particles.isStopped)
            Destroy(this.gameObject);
    }
}
