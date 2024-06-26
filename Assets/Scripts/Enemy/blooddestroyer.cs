using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blooddestroyer : MonoBehaviour
{
    [SerializeField] private float destroyTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
       if(destroyTime<0.0f) Destroy(gameObject);
    }
}
