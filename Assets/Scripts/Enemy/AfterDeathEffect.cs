using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterDeathEffect : MonoBehaviour
{
    public float Speed;
    public float timeToDeath;

    void Update()
    {
        timeToDeath -= Time.deltaTime;
        if(timeToDeath < 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.up * Speed * Time.deltaTime);
    }
}
