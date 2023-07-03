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
        CreateParticle(collisionParticle, collisionPoint);
    }

   

    public void PlayExplosionParticle(Vector3 collisionPoint)
    {
        CreateParticle(explosionParticle, collisionPoint);
    }
    
    private void CreateParticle(GameObject spawnParticle, Vector3 collisionPoint)
    {
        if (!isParticle)
        {
            isParticle = true;
            particle = Instantiate(spawnParticle);
            particle.transform.position = collisionPoint;
        }
    }

    public void Reset()
    {
        Destroy(particle);
        isParticle = false;
    }
}