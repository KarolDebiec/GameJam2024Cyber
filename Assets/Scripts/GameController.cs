using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float enemySpeedMultiplicator = 1.0f;
    public float playerSpeedMultiplicator = 1.0f;
    public float speedGainFactor = 0.01f;
    void Start()
    {
        
    }

    void Update()
    {
        playerSpeedMultiplicator += speedGainFactor * Time.deltaTime;
    }
}
