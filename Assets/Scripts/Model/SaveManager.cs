using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    float GetLerpEnemySpawnDelay();
    void SetLerpEnemySpawnDelay(float lerp);
    float GetLerpEnemySpeed();
    void SetLerpEnemySpeed(float lerp);
}

public class SaveManager : ISaveManager
{
    public float GetLerpEnemySpawnDelay()
    {
        float lerp = PlayerPrefs.GetFloat("LerpEnemySpawnDelay", Constants.DefaultLerpEnemySpawnDelay);
        return lerp;
    }

    public void SetLerpEnemySpawnDelay(float lerp)
    {
        PlayerPrefs.SetFloat("LerpEnemySpawnDelay", lerp);
    }

    public float GetLerpEnemySpeed()
    {
        float lerp = PlayerPrefs.GetFloat("LerpEnemySpeed", Constants.DefaultLerpEnemySpeed);
        return lerp;
    }

    public void SetLerpEnemySpeed(float lerp)
    {
        PlayerPrefs.SetFloat("LerpEnemySpeed", lerp);
    }
}