using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeParticle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem particles;

    // Start is called before the first frame update
    public void StartParticles(Color? particleColor = null, float radious = 1)
    {

        if (particleColor != null)
            particles.startColor = (Color)particleColor;

        if (radious != 1)
        {
            ParticleSystem.ShapeModule shape = particles.shape;
            shape.radius = radious;
        }

        particles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (particles.isStopped)
            Destroy(this.gameObject);
    }
}
