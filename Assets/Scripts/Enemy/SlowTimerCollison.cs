using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowTimerCollison : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdza, czy tag obiektu kolidującego to "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterController2D controller = other.gameObject.GetComponent<CharacterController2D>();
            if (controller != null)
            {
                controller.pickUpSlow();
                Destroy(gameObject);
            }
        }
    }
}
