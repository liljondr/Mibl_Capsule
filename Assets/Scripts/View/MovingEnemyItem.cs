using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovingEnemyItem
{
    void StopMove();
    void SetSpeed(float speed);
    void Destroy();
    Vector3 Position { get; }
}

public class MovingEnemyItem : MonoBehaviour, IMovingEnemyItem
{
    public Vector3 Position => transform.position;
    private Transform target;
    private bool isActive = true;
    private float speed;

    private void Update()
    {
        if (isActive)
        {
            Vector3 vectorNormalized = (target.position - transform.position).normalized;
            Vector3 correctVectorNormalized = new Vector3(vectorNormalized.x, 0, vectorNormalized.z);
            transform.position += correctVectorNormalized * speed;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void StopMove()
    {
        isActive = false;
    }


    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}