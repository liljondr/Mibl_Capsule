using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public interface ICalculatorSpawnPosition
{
   Vector3? CalculateSpawnPosition(Vector3 playerPosition, float positionY,List<Vector3> allObjectsPositions);
  
}
public class CalculatorSpawnPosition : ICalculatorSpawnPosition
{
   private ICalculateSpawnPositionData data;
   
   private float minZ;
   private float maxZ;
   private float minX;
   private float maxX;
   private Random rand;

   private float sqrMinDistanceForEnemyAndPlayer;
   private float sqrMinDistanceForEnemyAndEnemy;
   public CalculatorSpawnPosition(ICalculateSpawnPositionData data)
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

   public Vector3? CalculateSpawnPosition(Vector3 playerPosition, float positionY,List<Vector3> allObjectsPositions)
   {
      bool isCorrectPosition = true;
      Vector3 spawnPosition;
      int whileIndex = 0;
      do
      {
         whileIndex++;
         if (whileIndex > 150)
         {
            return null;
         }
         isCorrectPosition = true;
         spawnPosition = CalculateSpawnPositionToPlayer(playerPosition,positionY);
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
      return spawnPosition;
   }

  

  

   private Vector3 CalculateSpawnPositionToPlayer(Vector3 playerPosition, float positionY)
   {
      float distance=0;
      Vector3 resultPosition;
      do
      {
         int randomIndex= rand.Next(0, 100);
         float randomX = minX + (maxX-minX) * randomIndex / 100;
         
         randomIndex= rand.Next(0, 100);
         float randomZ = minZ + (maxZ-minZ) * randomIndex / 100;
         resultPosition = new Vector3(randomX, positionY, randomZ);
         distance = Vector3.Magnitude(playerPosition - resultPosition);
      } while (distance<data.MinDistanceForEnemyAndPlayer);
      

      return resultPosition;
   }

  
}


