using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public interface ISpawnPositionCalculator
{
    bool TrySpawnPositionCalculate(Vector3 playerPosition, float positionY, List<Vector3> allObjectsPositions,
        out Vector3 spawnPosition);
}

public class SpawnPositionCalculator : ISpawnPositionCalculator
{
    private ICalculateSpawnPositionData data;

    private float minZ;
    private float maxZ;
    private float minX;
    private float maxX;
    private Random rand;

    private float sqrMinDistanceForEnemyAndPlayer;
    private float sqrMinDistanceForEnemyAndEnemy;

    public SpawnPositionCalculator(ICalculateSpawnPositionData data)
    {
        this.data = data;
        rand = new Random();
        DeterminationExtremePoints();
    }

    private void DeterminationExtremePoints()
    {
        minX = data.LeftUpPoint.position.x;
        maxX = data.RightDownPoint.position.x;
        maxZ = data.LeftUpPoint.position.z;
        minZ = data.RightDownPoint.position.z;
        sqrMinDistanceForEnemyAndPlayer = data.MinDistanceForEnemyAndPlayer;
        sqrMinDistanceForEnemyAndEnemy = data.MinDistanceForEnemyAndEnemy;
    }

    public bool TrySpawnPositionCalculate(Vector3 playerPosition, float positionY, List<Vector3> allObjectsPositions,
        out Vector3 spawnPosition)
    {
        bool isCorrectPosition = true;
        spawnPosition = Vector3.zero;
        int whileIndex = 0;
        do
        {
            whileIndex++;
            if (whileIndex > 150)
            {
                return false;
            }

            isCorrectPosition = true;
            spawnPosition =  CalculateSpawnPositionByPlayer(playerPosition, positionY);
            foreach (Vector3 pos in allObjectsPositions)
            {
                float distance = Vector3.Distance(pos, spawnPosition);
                if (distance < data.MinDistanceForEnemyAndEnemy)
                {
                    isCorrectPosition = false;
                    break;
                }
            }
        } while (!isCorrectPosition);
        
        return true;
    }

    private Vector3 CalculateSpawnPositionByPlayer(Vector3 playerPosition, float positionY)
    {
        float distance = 0;
        Vector3 resultPosition;
        do
        {
            int randomIndex = rand.Next(0, 100);
            float randomX = minX + (maxX - minX) * randomIndex / 100;

            randomIndex = rand.Next(0, 100);
            float randomZ = minZ + (maxZ - minZ) * randomIndex / 100;
            resultPosition = new Vector3(randomX, positionY, randomZ);
            distance = Vector3.Magnitude(playerPosition - resultPosition);
        } while (distance < data.MinDistanceForEnemyAndPlayer);
        
        return resultPosition;
    }
}