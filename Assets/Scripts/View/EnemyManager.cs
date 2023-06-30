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
    [SerializeField] private EnemyItem enemyPrefab;
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

    private List<IEnemyItem> listEnemies = new List<IEnemyItem>();
    private List<IBombItem> listBombs = new List<IBombItem>();
    
    private float spawnerTime ;
    private bool isActive;
    private float currentTime;
    private float currentSpeed;
    
    private ICalculatorSpawnPosition calculatorSpawnPosition;

   
    private void Start()
    {
        isActive = true;
        CalculationMinExtremeSpawnDistance();
       
        calculatorSpawnPosition = new CalculatorSpawnPosition(this);
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
        MeshRenderer enemyMeshRenderer = enemyPrefab.GetComponent<MeshRenderer>();
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
            Vector3? position = calculatorSpawnPosition.CalculateSpawnPosition(player.position, bombPrefab.transform.position.y,allObjectsPositions);
            if (position == null)
            {
                Debug.Log("There is no new place to create a bomb.");
                return;
            }
            BombItem bomb = Instantiate(bombPrefab);
            bomb.transform.position = (Vector3)position;
            listBombs.Add(bomb);
        }
    }

    private void SpawnEnemy()
    {
        List<Vector3> allObjectsPositions = listEnemies.Select(e => e.Position).ToList();
        Vector3? position = calculatorSpawnPosition.CalculateSpawnPosition(player.position, enemyPrefab.transform.position.y,allObjectsPositions);
        if (position == null)
        {
            Debug.Log("There is no new place to create an enemy. Try again later");
            return;
        }
        EnemyItem enemy = Instantiate(enemyPrefab, transform, false);
        enemy.transform.position = (Vector3)position;
        enemy.SetTarget(player);
        enemy.SetSpeed(currentSpeed);
        listEnemies.Add(enemy);
    }

   

     public void SwitchEnemySpawn(bool b)
     {
         isActive = false;
     }

     public void StopMoveEnemies()
     {
        listEnemies.ForEach(e=>e.StopMove());
     }

    

     public float ChangeSpawnDelayByLerp(float lerp)
     {
         spawnerTime = spawnerTime = Constants.EnemyMaxSpawnDelay -
                                     (Constants.EnemyMaxSpawnDelay - Constants.EnemyMinSpawnDelay) * lerp;
         return spawnerTime;
     }
     
     public float ChangeSpeedByLerp(float lerpSeed)
     {
         currentSpeed = (Constants.EnemyMaxSpeed - Constants.EnemyMinSpeed) * lerpSeed + Constants.EnemyMinSpeed;
         listEnemies.ForEach(e=>e.SetSpeed(currentSpeed));
         return currentSpeed;
     }

     public void Reset()
     {
         ResetBombs();
         ResetEnemies();
         isActive = true;
     }

     private void ResetEnemies()
     {
         listEnemies.ForEach(e => e.Destroy());
         listEnemies.Clear();
         SpawnEnemy();
     }

     private void ResetBombs()
     {
         listBombs.ForEach(b => b.Destroy());
         listBombs.Clear();
         SpawnBombs();
     }
}

