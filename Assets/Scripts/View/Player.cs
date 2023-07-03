using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody myRB;

    public event Action<Vector3> CollidedWithEnemy;
    public event Action<Vector3> CollidedWithBomb;
    private float speedMultiplayer = 1.5f;
    private bool alreadyCollider;
    private Vector3 startPosition;

    public void Start()
    {
        startPosition = transform.position;
    }

    public void SetVelocity(Vector2 delta)
    {
        myRB.velocity = new Vector3(delta.x * speedMultiplayer, 0, delta.y * speedMultiplayer);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (!alreadyCollider)
        {
            if (other.transform.tag == "Enemy")
            {
                CollisionWithEnemy(other);
            }

            if (other.transform.tag == "Bomb")
            {
                CollisionWithBomb(other);
            }
        }
    }

    private void CollisionWithEnemy(Collision other)
    {
        alreadyCollider = true;
        SetVelocity(Vector2.zero);
        CollidedWithEnemy?.Invoke(other.contacts[0].point);
        myRB.isKinematic = true;
    }

    private void CollisionWithBomb(Collision other)
    {
        alreadyCollider = true;
        Vector3 normalized = (transform.position - other.contacts[0].point).normalized;
        myRB.constraints = RigidbodyConstraints.None;
        myRB.AddTorque(new Vector3(100, 100, 100));
        myRB.AddForce(normalized * 30, ForceMode.Impulse);
        CollidedWithBomb?.Invoke(other.contacts[0].point);
        BombItem bombItem = other.collider.GetComponent<BombItem>();
        if (bombItem == null)
        {
            Debug.Log("It isn`t a bomb");
            return;
        }

        bombItem.ChangeDarkMaterial();
    }

    public void Reset()
    {
        myRB.isKinematic = false;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        myRB.constraints = RigidbodyConstraints.FreezeRotation;
        SetVelocity(Vector2.zero);
        alreadyCollider = false;
    }
}