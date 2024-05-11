using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TrailRenderer playerTrail;
    public float enemySpeedMultiplier = 1.0f;
    public float playerSpeedMultiplier = 1.0f;
    public float minPlayerSpeedMultiplier = 1.0f;
    public float speedGainFactor = 0.01f;
    public float maxPlayerSpeedMultiplier = 2.8f;
    void Start()
    {
        
    }

    void Update()
    {
        if(playerSpeedMultiplier < maxPlayerSpeedMultiplier)
        {
            playerSpeedMultiplier += speedGainFactor * Time.deltaTime;
            minPlayerSpeedMultiplier += (speedGainFactor * Time.deltaTime) / 2f;
            playerTrail.time = playerSpeedMultiplier;
        }
        else
        {
           //ded
        }
    }

    public void SpeedUpPlayer(float value)
    {
        playerSpeedMultiplier += value;
        minPlayerSpeedMultiplier += value / 2f;
    }

    public void SlowDownPlayer(float value)
    {
        if(playerSpeedMultiplier - value >= minPlayerSpeedMultiplier)
        {
            playerSpeedMultiplier -= value;
            minPlayerSpeedMultiplier -= value / 2f;
        }
    }

    public void calculateSpeedup()
    {
        float randomNumber = Random.Range(0.05f, 0.10f);
        playerSpeedMultiplier = playerSpeedMultiplier + playerSpeedMultiplier *randomNumber;
        minPlayerSpeedMultiplier = minPlayerSpeedMultiplier + minPlayerSpeedMultiplier * randomNumber;
    }
    public void calculateSpeeddown()
    {
        float randomNumber = Random.Range(0.05f, 0.10f);
        playerSpeedMultiplier = playerSpeedMultiplier - playerSpeedMultiplier * randomNumber;
        minPlayerSpeedMultiplier = minPlayerSpeedMultiplier - minPlayerSpeedMultiplier * randomNumber;
    }
}
