using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Vector3 startDistance;
    private float smoothSpeed = 0.1f;
    private Vector3 velocity = Vector3.zero;

    public float firstAngle;
    public float endAngle;
    [SerializeField] private Transform frontWall;
    [SerializeField] private Transform backWall;
    private float distanceBetweenWalls;
    private bool isActive = true;
    
    void Start()
    {
        startDistance = target.transform.position - transform.position;
        distanceBetweenWalls = backWall.position.z - frontWall.position.z;
    }

    
    void LateUpdate()
    {
      if(isActive)
      {
          transform.position = target.transform.position - startDistance;
          float lerp = (target.transform.position.z - frontWall.position.z) / distanceBetweenWalls;
          float angle = Mathf.Lerp(firstAngle, endAngle, lerp);
          transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);
      }
    }

    public void SwithLookAtPlayer(bool b)
    {
        isActive = b;
    }
}