using UnityEngine;
using TMPro;
using System.Collections;

public class FadeInText : MonoBehaviour
{
    public float fadeInTime = 1.0f; 
    
    private TMP_Text textComponent; 
    private Color originalColor; 

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        originalColor = textComponent.color;
        originalColor.a = 0;
        textComponent.color = originalColor;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0.0f;
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / fadeInTime);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }
}