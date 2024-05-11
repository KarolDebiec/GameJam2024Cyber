using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public float score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public Slider speedSlider;
    public TrailRenderer playerTrail;
    public float enemySpeedMultiplier = 1.0f;
    public float playerSpeedMultiplier = 1.0f;
    public float minPlayerSpeedMultiplier = 1.0f;
    public float speedGainFactor = 0.01f;
    public float maxPlayerSpeedMultiplier = 2.8f;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    public Cinemachine.CinemachineConfiner2D cinemachineConfiner;
    private bool isPlayerDead;
    void Update()
    {
        if(playerSpeedMultiplier < maxPlayerSpeedMultiplier)
        {
            enemySpeedMultiplier += speedGainFactor * Time.deltaTime;
            playerSpeedMultiplier += speedGainFactor * Time.deltaTime;
            minPlayerSpeedMultiplier += (speedGainFactor * Time.deltaTime) / 2f;
            playerTrail.time = playerSpeedMultiplier;
            speedSlider.value = playerSpeedMultiplier;
        }
        else
        {
            PlayerDeath();
        }
        if(scoreText.gameObject.activeSelf)
        {
            scoreText.text = score.ToString("F0");
        }
        if(isPlayerDead)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = 2f;
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
        score += 100;
    }

    public void calculateSpeedup()
    {
        float randomNumber = Random.Range(0.05f, 0.10f);
        if (playerSpeedMultiplier + playerSpeedMultiplier * randomNumber < maxPlayerSpeedMultiplier)
        {
            playerSpeedMultiplier = playerSpeedMultiplier + playerSpeedMultiplier * randomNumber;
            minPlayerSpeedMultiplier = minPlayerSpeedMultiplier + minPlayerSpeedMultiplier * randomNumber; 
            if(score >= 10)
            {
                score -= 10;
            }
        }
        else
        {
            PlayerDeath();
        }
    }
    public void calculateSpeeddown()
    {
        score += 100;
        float randomNumber = Random.Range(0.3f, 0.8f);
        if (playerSpeedMultiplier - playerSpeedMultiplier * randomNumber > minPlayerSpeedMultiplier)
            playerSpeedMultiplier = playerSpeedMultiplier - playerSpeedMultiplier * randomNumber;
        else playerSpeedMultiplier = minPlayerSpeedMultiplier;


    }

    public void PlayerDeath()
    {
        Debug.Log("player is dead");
        cinemachineConfiner.enabled = false;
        isPlayerDead = true;
        finalScoreText.text = score.ToString("F0");
        scoreText.gameObject.SetActive(false);
        speedSlider.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().isPlayerDead = true;
    }
}
