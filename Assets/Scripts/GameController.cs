using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float enemySpeedMultiplicator = 1.0f;
    public float playerSpeedMultiplicator = 1.0f;
    public float minPlayerSpeedMultiplicator = 1.0f;
    public float speedGainFactor = 0.01f;
    void Start()
    {
        
    }

    void Update()
    {
        playerSpeedMultiplicator += speedGainFactor * Time.deltaTime;
        minPlayerSpeedMultiplicator += (speedGainFactor * Time.deltaTime)/2f;
    }

    public void SpeedUpPlayer(float value)
    {
        playerSpeedMultiplicator += value;
        minPlayerSpeedMultiplicator += value / 2f;
    }

    public void SlowDownPlayer(float value)
    {
        if(playerSpeedMultiplicator - value >= minPlayerSpeedMultiplicator)
        {
            playerSpeedMultiplicator -= value;
            minPlayerSpeedMultiplicator -= value / 2f;
        }
    }
}
