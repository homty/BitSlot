using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class BitSlotLogoAnimator : MonoBehaviour
{
    public Transform[] letters;
    public float fadeDuration = 0.5f;
    public float pulseScale = 1.5f;
    public float pulseDuration = 0.2f;
    public float fallDistance = 2f;
    public float fallDuration = 0.4f;
    public float delayBetweenFalls = 0.1f;

    private Vector3[] originalPositions;
    private SpriteRenderer[] spriteRenderers;

    void Start()
    {
        originalPositions = new Vector3[letters.Length];
        spriteRenderers = new SpriteRenderer[letters.Length];

        for (int i = 0; i < letters.Length; i++)
        {
            originalPositions[i] = letters[i].position;
            spriteRenderers[i] = letters[i].GetComponent<SpriteRenderer>();

            // Устанавливаем альфу в 0 (прозрачность)
            if (spriteRenderers[i] != null)
            {
                Color color = spriteRenderers[i].color;
                color.a = 0f;
                spriteRenderers[i].color = color;
            }
        }
        
        StartCoroutine(AnimateSequence());
    }

    IEnumerator AnimateSequence()
    {
        yield return StartCoroutine(FadeInAll());
        yield return StartCoroutine(PulseAll());

        // Затем каждая буква падает по очереди
        foreach (Transform letter in letters)
        {
            yield return StartCoroutine(Fall(letter));
            yield return new WaitForSeconds(delayBetweenFalls);
        }
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("SlotScene");
    }

    IEnumerator FadeInAll()
    {
        AudioManager.Instance.PlayDownloadSound();
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] != null)
                {
                    Color c = spriteRenderers[i].color;
                    c.a = Mathf.Lerp(0f, 1f, t);
                    spriteRenderers[i].color = c;
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Установим альфу точно на 1
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                Color c = spriteRenderers[i].color;
                c.a = 1f;
                spriteRenderers[i].color = c;
            }
        }
    }

    IEnumerator PulseAll()
    {
        Vector3[] originalScales = new Vector3[letters.Length];
        Vector3[] targetScales = new Vector3[letters.Length];

        for (int i = 0; i < letters.Length; i++)
        {
            originalScales[i] = letters[i].localScale;
            targetScales[i] = originalScales[i] * pulseScale;
        }

        float elapsed = 0f;

        // Увеличение
        while (elapsed < pulseDuration)
        {
            float t = elapsed / pulseDuration;
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i].localScale = Vector3.Lerp(originalScales[i], targetScales[i], t);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Обратно
        while (elapsed < pulseDuration)
        {
            float t = elapsed / pulseDuration;
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i].localScale = Vector3.Lerp(targetScales[i], originalScales[i], t);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Финальный масштаб
        for (int i = 0; i < letters.Length; i++)
        {
            letters[i].localScale = originalScales[i];
        }
    }

    IEnumerator Fall(Transform letter)
    {
        Vector3 startPos = letter.position;
        Vector3 endPos = startPos + Vector3.down * fallDistance;
        float elapsed = 0f;

        while (elapsed < fallDuration)
        {
            float t = elapsed / fallDuration;
            letter.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        letter.position = endPos;
    }
}
