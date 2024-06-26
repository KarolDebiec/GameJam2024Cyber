using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float destroyTime = 1.0f;
  
    private SpriteRenderer spriteRenderer;
    // Początkowa wartość alpha
    public float initialAlpha = 1.0f;

    // Wartość alpha, do której chcemy zmienić
    public float targetAlpha = 0.0f;

    // Czas trwania zmiany alpha
    public float duration = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = initialAlpha;
        spriteRenderer.color = color;
        StartCoroutine(ChangeAlphaOverTime(targetAlpha, duration));
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if(destroyTime<0.0f) Destroy(gameObject);
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
