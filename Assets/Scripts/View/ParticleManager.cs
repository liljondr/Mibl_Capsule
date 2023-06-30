using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject collisionParticle;
    [SerializeField] private GameObject explosionParticle;
    private bool isParticle;
    private GameObject particle;

    public void PlayCollisionParticle(Vector3 collisionPoint)
    {
        if(!isParticle)
        {
            isParticle = true;
             particle = Instantiate(collisionParticle);
            particle.transform.position = collisionPoint;
        }
    }
    
    public void PlayExplosionParticle(Vector3 collisionPoint)
    {
        if(!isParticle)
        {
            isParticle = true;
            particle = Instantiate(explosionParticle);
            particle.transform.position = collisionPoint;
        }
    }

    public void Reset()
    {
        Destroy(particle);
        isParticle = false;
    }

   
}

