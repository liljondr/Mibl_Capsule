using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] private Player player;
   [SerializeField] private DragPanel dragPanel;
   [SerializeField] private EnemyManager enemyManager;
   [SerializeField] private UIManager uiManager;
   [SerializeField] private ParticleManager particleManager;
   [SerializeField] private CameraMover camera;

   private ISaveManager saveManager = new SaveManager();

   private DateTime startTime;

   private void Awake()
   {
      SetStartData();
      dragPanel.OnSetDragDelta += OnSetDragPosition;
      dragPanel.OnEndDragOnPanel += OnEndDragOnPanel;
      player.IsCollisionWithEnemy += OnIsCollisionWithEnemy;
      player.IsCollisionWithBomb += OnIsCollisionWithBomb;
      uiManager.OnChangeSpeed +=OnChangeSpeedSlider;
      uiManager.OnChangeSpawnDelay +=OnChangeSpawnDelaySlider;
      uiManager.OnPressReplayButton +=OnPressReplayButton;
      
      startTime = DateTime.Now;
   }

  


   private void SetStartData()
   {
      SetStartDataForSpeed();
      SetStartDataForSpawnDelay();
   }

   private void SetStartDataForSpeed()
   {
      float lerpSeed = saveManager.GetLerpEnemySpeed();
      float speed = enemyManager.ChangeSpeedByLerp(lerpSeed);
      uiManager.SetSpeedText(speed);
      uiManager.SetSpeedValue(lerpSeed);
   }

   private void SetStartDataForSpawnDelay()
   {
      float lerpSpawnDelay = saveManager.GetLerpEnemySpawnDelay();
      float spawnDelay = enemyManager.ChangeSpawnDelayByLerp(lerpSpawnDelay);
      uiManager.SetSpawnDelayText(spawnDelay);
      uiManager.SetSpawnDelayValue(lerpSpawnDelay);
   }


   private void OnSetDragPosition(Vector2 delta)
   {
      player.SetVelocity(delta);
   }
   
   private void OnEndDragOnPanel()
   {
      player.SetVelocity(Vector2.zero);
   }
   private void OnIsCollisionWithEnemy(Vector3 collisionPoint)
   {
      enemyManager.SwitchEnemySpawn(false);
      enemyManager.StopMoveEnemies();
      dragPanel.gameObject.SetActive(false);

      TimeSpan gameTime = DateTime.Now.Subtract( startTime);
      uiManager.OpenLossPopup(gameTime);
      particleManager.PlayCollisionParticle(collisionPoint);
   }
   
   private void OnIsCollisionWithBomb(Vector3 collisionPoint)
   {
      enemyManager.SwitchEnemySpawn(false);
      enemyManager.StopMoveEnemies();
      dragPanel.gameObject.SetActive(false);

      TimeSpan gameTime = DateTime.Now.Subtract( startTime);
      uiManager.OpenLossPopup(gameTime);
      particleManager.PlayExplosionParticle(collisionPoint);
      camera.SwithLookAtPlayer(false);
   }
   
   private void OnChangeSpeedSlider(float lerp)
   {
      saveManager.SetLerpEnemySpeed(lerp);
      float speed = enemyManager.ChangeSpeedByLerp(lerp);
      uiManager.SetSpeedText(speed);
   }
   
   private void OnChangeSpawnDelaySlider(float lerp)
   {
      saveManager.SetLerpEnemySpawnDelay(lerp);
     float spawnDelay = enemyManager.ChangeSpawnDelayByLerp(lerp);
     uiManager.SetSpawnDelayText(spawnDelay);
     
   }
   
   private void OnPressReplayButton()
   {
      startTime = DateTime.Now;
      dragPanel.gameObject.SetActive(true);
      player.Reset();
      enemyManager.Reset();
      particleManager.Reset();
      camera.SwithLookAtPlayer(true);

   }

   
}
