using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePRefabActivator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public ParticleSystem _particleSystem;
    [SerializeField] public Transform player;
    public Vector3 pos = Vector3.zero;
    public float timer = 0.4f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _particleSystem = GetComponent<ParticleSystem>();
        pos = player.position + new Vector3(0, -1, 0);
        this.transform.position = pos; 
       _particleSystem.Play();  

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
