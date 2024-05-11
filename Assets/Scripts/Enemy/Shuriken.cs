using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shuriken : MonoBehaviour
{
    float rotationSpeed = 1;
    Vector2 targetPos = Vector2.zero;
    Vector2 direction = Vector2.zero;
    GameObject player;
    public float speed = 7.0f;
    float liveTime = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 1200.0f;
        player = GameObject.FindGameObjectWithTag("Player");
        targetPos = new Vector2(player.transform.position.x, player.transform.position.y);
        direction = new Vector2(targetPos.x-this.transform.position.x,targetPos.y-this.transform.position.y);
        direction.Normalize();
        speed = 7.0f;
        liveTime = 4.0f;
    }
    private void FixedUpdate()
    {
        Quaternion currentRotation = transform.rotation;

        // Oblicz now¹ rotacjê, dodaj¹c do obecnej rotacji odpowiedni¹ zmianê
        Quaternion newRotation = Quaternion.Euler(
            currentRotation.eulerAngles.x,
            currentRotation.eulerAngles.y,
            currentRotation.eulerAngles.z + (rotationSpeed * Time.deltaTime)
        );

        // Ustaw nowy kwaternion rotacji
        transform.rotation = newRotation;


        this.transform.position += new Vector3(direction.x,direction.y,0)* speed * Time.deltaTime;
        liveTime -= Time.deltaTime;
        if( liveTime < 0 ) {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            //tu damagem;
            Destroy(this.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
