using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public interface ICalculateSpawnPositionData
{
    public   Transform RightDownPoint { get; }
    public   Transform LeftUpPoint { get; }
    public float MinDistanceForEnemyAndPlayer { get; }
    public float MinDistanceForEnemyAndEnemy { get; }
    
    
}


public class EnemyManager : MonoBehaviour, ICalculateSpawnPositionData
{
    [SerializeField] private MovingEnemyItem movingEnemyPrefab;
    [SerializeField] private BombItem bombPrefab;
    [SerializeField] private Transform player;
    
    [SerializeField] private Transform rightDownPoint;
    [SerializeField] private Transform leftUpPoint;

    public Transform RightDownPoint => rightDownPoint;
    public Transform LeftUpPoint => leftUpPoint;
    
    private float coefDistanceForEnemyAndPlayer=3;
    private float minDistanceForEnemyAndPlayer;
    private float coefDistanceForEnemyAndEnemy=1.5f;
    private float minDistanceForEnemyAndEnemy;
    public float MinDistanceForEnemyAndPlayer => minDistanceForEnemyAndPlayer;
    public float MinDistanceForEnemyAndEnemy => minDistanceForEnemyAndEnemy;

    private List<IMovingEnemyItem> listMovingEnemies = new List<IMovingEnemyItem>();
    private List<IBombItem> listBombs = new List<IBombItem>();
    
    private float spawnerTime ;
    private bool isActive;
    private float currentTime;
    private float currentSpeed;
    
    private ISpawnPositionCalculator spawnPositionCalculator;

   
    private void Start()
    {
        isActive = true;
        CalculationMinExtremeSpawnDistance();
       
        spawnPositionCalculator = new SpawnPositionCalculator(this);
        SpawnBombs();
        SpawnEnemy();
    }

   

    private void Update()
    {
        if(isActive)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= spawnerTime)
            {
                currentTime = 0;
                SpawnEnemy();
            }
        }
    }


    private void CalculationMinExtremeSpawnDistance()
    {
        MeshRenderer enemyMeshRenderer = movingEnemyPrefab.GetComponent<MeshRenderer>();
        if (enemyMeshRenderer == null)
        {
            Debug.LogError("Enemy item doesn`t have MeshRenderer");
            return;
        }

        float sizeEnemy = Mathf.Max(enemyMeshRenderer.bounds.size.x, enemyMeshRenderer.bounds.size.z);
        minDistanceForEnemyAndPlayer = sizeEnemy * coefDistanceForEnemyAndPlayer;
        minDistanceForEnemyAndEnemy = sizeEnemy * coefDistanceForEnemyAndEnemy;
    }



    private void SpawnBombs()
    {
        for (int j = 0; j < Constants.BombAmount; j++)
        {
            List<Vector3> allObjectsPositions = listBombs.Select(b => b.Position).ToList();
            bool isSpawn =  spawnPositionCalculator.TrySpawnPositionCalculate(player.position, bombPrefab.transform.position.y,allObjectsPositions, out Vector3 spawnPosition);
            if (isSpawn == null)
            {
                Debug.Log("There is no new place to create a bomb.");
                return;
            }
            BombItem bomb = Instantiate(bombPrefab);
            bomb.transform.position = spawnPosition;
            listBombs.Add(bomb);
        }
    }

    private void SpawnEnemy()
    {
        List<Vector3> allObjectsPositions = listMovingEnemies.Select(e => e.Position).ToList();
        bool isSpawn = spawnPositionCalculator.TrySpawnPositionCalculate(player.position, movingEnemyPrefab.transform.position.y,allObjectsPositions, out Vector3 spawnPosition);
        if (!isSpawn )
        {
            Debug.Log("There is no new place to create an enemy. Try again later");
            return;
        }
        MovingEnemyItem movingEnemy = Instantiate(movingEnemyPrefab, transform, false);
        movingEnemy.transform.position = spawnPosition;
        movingEnemy.SetTarget(player);
        movingEnemy.SetSpeed(currentSpeed);
        listMovingEnemies.Add(movingEnemy);
    }

   

     public void SwitchEnemySpawn(bool b)
     {
         isActive = false;
     }

     public void StopMoveEnemies()
     {
        listMovingEnemies.ForEach(e=>e.StopMove());
     }

    

     public float ChangeSpawnDelayByLerp(float lerp)
     {
         spawnerTime =  Constants.EnemyMaxSpawnDelay -
                                     (Constants.EnemyMaxSpawnDelay - Constants.EnemyMinSpawnDelay) * lerp;
         return spawnerTime;
     }
     
     public float ChangeSpeedByLerp(float lerpSeed)
     {
         currentSpeed = (Constants.EnemyMaxSpeed - Constants.EnemyMinSpeed) * lerpSeed + Constants.EnemyMinSpeed;
         listMovingEnemies.ForEach(e=>e.SetSpeed(currentSpeed));
         return currentSpeed;
     }

     public void Restart()
     {
         RestartBombs();
         RestartEnemies();
         isActive = true;
     }

     private void RestartEnemies()
     {
         listMovingEnemies.ForEach(e => e.Destroy());
         listMovingEnemies.Clear();
         SpawnEnemy();
     }

     private void RestartBombs()
     {
         listBombs.ForEach(b => b.Destroy());
         listBombs.Clear();
         SpawnBombs();
     }
}

