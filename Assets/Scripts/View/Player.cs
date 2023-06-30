using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
   [SerializeField] private Rigidbody myRB;

   public event Action<Vector3> IsCollisionWithEnemy;
   public event Action<Vector3> IsCollisionWithBomb;
    private float speedIndex=1.5f;
    private bool isCollision;
    private Vector3 startPosition;

    public void Start()
    {
        startPosition = transform.position;
    }

    public void SetVelocity(Vector2 delta)
    {
        myRB.velocity = new Vector3(delta.x*speedIndex,0,delta.y*speedIndex);
    }


    private void OnCollisionEnter(Collision other)
    {
        if(!isCollision)
        {
            if (other.transform.tag == "Enemy")
            {
                isCollision = true;
                SetVelocity(Vector2.zero);
                IsCollisionWithEnemy?.Invoke(other.contacts[0].point);
                myRB.isKinematic = true;
            }

            if (other.transform.tag == "Bomb")
            {
                isCollision = true;
                Vector3 normalized = ( transform.position-other.contacts[0].point).normalized;
                myRB.constraints = RigidbodyConstraints.None;
                myRB.AddTorque(new Vector3(100,100,100));
                myRB.AddForce(normalized*30,ForceMode.Impulse);
                IsCollisionWithBomb?.Invoke(other.contacts[0].point);
                BombItem bombItem = other.collider.GetComponent<BombItem>();
                if (bombItem == null)
                {
                    Debug.Log("It isn`t a bomb");
                    return;
                }

                bombItem.ChangeDarkMaterial();
            }
        }
    }

    public void Reset()
    {
        myRB.isKinematic = false;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        myRB.constraints = RigidbodyConstraints.FreezeRotation;
        SetVelocity(Vector2.zero);
        isCollision = false;
    }
}
