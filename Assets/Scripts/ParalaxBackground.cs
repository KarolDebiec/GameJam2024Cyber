using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{

    public Transform cameraTransform;
    public float parallaxEffectMultiplier;
    private Vector3 lastCameraPosition;
    public float textureUnitSizeX;
    public float startpos;

    void Start()
    {
        startpos = transform.position.x;
        textureUnitSizeX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cameraTransform.position.x * (1 - parallaxEffectMultiplier));
        float dist = (cameraTransform.position.x * parallaxEffectMultiplier);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if(temp > startpos+ textureUnitSizeX)
        {
            startpos += 2 * textureUnitSizeX;
        }
        else if(temp < startpos - textureUnitSizeX)
        {
            startpos -= 2*textureUnitSizeX;
        }
    }
}