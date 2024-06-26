using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class latajaceCiala : MonoBehaviour
{

    private Transform transform;
    private SpriteRenderer spriteRenderer;
    private float speed;
    private float accelerationY = -9.81f;
    private Vector3 velocity;
    [SerializeField]private float floorLvl;
    [SerializeField] Vector2 rangeX;
    [SerializeField] Vector2 rangeY;

    // Początkowa wartość alpha
    public float initialAlpha = 1.0f;

    // Wartość alpha, do której chcemy zmienić
    public float targetAlpha = 0.0f;

    // Czas trwania zmiany alpha
    public float duration = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        
        float x = Random.Range(rangeX.x, rangeX.y);
        float y = Random.Range(rangeY.x, rangeY.y);
        velocity = new Vector3(x,y,0);
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ustawienie początkowej wartości alpha
        Color color = spriteRenderer.color;
        color.a = initialAlpha;
        spriteRenderer.color = color;

        // Rozpoczęcie zmiany alpha
        

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += velocity * Time.deltaTime - (new Vector3(0.0f,accelerationY,0.0f) * Time.deltaTime *Time.deltaTime * 0.5f);
        velocity += new Vector3(0.0f,accelerationY,0.0f)*Time.deltaTime;
        
        
        if (transform.position.y < floorLvl){Vector3 newPosition = new Vector3(transform.position.x, floorLvl, transform.position.z);
            transform.position = newPosition; velocity.y=0.0f;
            velocity.x = 0.0f; velocity.y = 0.0f; StartCoroutine(ChangeAlphaOverTime(targetAlpha, duration));
        }

        if (spriteRenderer.color.a <= 0.0f) Destroy(gameObject);
    }

    private IEnumerator ChangeAlphaOverTime(float targetAlpha, float duration)
    {
        float startAlpha = spriteRenderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            Color color = spriteRenderer.color;
            color.a = newAlpha;
            spriteRenderer.color = color;

            yield return null;
        }

        // Upewnienie się, że końcowa wartość alpha jest dokładnie taka, jak chcemy
        Color finalColor = spriteRenderer.color;
        finalColor.a = targetAlpha;
        spriteRenderer.color = finalColor;
    }
}
