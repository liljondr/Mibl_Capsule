using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBombItem
{
    public Vector3 Position { get; }
    void Destroy();
}

public class BombItem : MonoBehaviour, IBombItem
{
    [SerializeField] private Material blackMaterial;
    [SerializeField] private MeshRenderer myRender;
    public Vector3 Position => transform.position;


    public void Destroy()
    {
        Destroy(gameObject);
    }


    public void ChangeDarkMaterial()
    {
        myRender.material = blackMaterial;
    }
}