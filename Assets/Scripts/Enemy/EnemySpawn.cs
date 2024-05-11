using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] Transform player;
    private GameController gameController;
    private CinemachineBrain cinemachineBrain;
    private float timer = 0;
    private float basicRespawnTime = 5.0f;
    private int how_much_enemy_to_respawn = 3;
    [SerializeField] public GameObject myPrefab;
    private void Start()
    {
        // Pobierz komponent CinemachineBrain z kamery
        cinemachineBrain = GetComponent<CinemachineBrain>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            for(int i = 0; i < how_much_enemy_to_respawn + gameController.playerSpeedMultiplier * 0.25f; i++)
            {
                Vector3  pos = generatePos();
                Instantiate(myPrefab, pos, Quaternion.identity);
            }
            timer = basicRespawnTime - (basicRespawnTime*gameController.playerSpeedMultiplier*0.1f);
        }


    }

    private Vector3 generatePos()
    {
        Vector3 randomPoint = new(Random.Range(0f, 1f), Random.Range(0f, 1f));
        randomPoint.z = 10f; // set this to whatever you want the distance of the point from the camera to be. Default for a 2D game would be 10.
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(randomPoint);
        if (worldPoint.y < 2.0f) worldPoint.y = 2.0f;
        if(player.position.x < worldPoint.x)
        {
            worldPoint.x += 22;
        }
        else
        {
            worldPoint.x -= 22;
        }

        return worldPoint;
    }
}
