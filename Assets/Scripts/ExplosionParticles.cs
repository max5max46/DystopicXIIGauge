using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private ParticleSystem particles;

    // Start is called before the first frame update
    public void StartParticles(float radious)
    {
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.radius = radious;
        particles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (particles.isStopped)
            Destroy(this.gameObject);
    }
}
