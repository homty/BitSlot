using System.Collections;
using TMPro;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public static UIAnimator Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public IEnumerator AnimateMultiplier(TextMeshProUGUI text, int value)
    {
        Vector3 originalScale = text.rectTransform.localScale;
        Vector3 largeScale = originalScale * 2f;

        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            text.rectTransform.localScale = Vector3.Lerp(originalScale, largeScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float shakeTime = 0.3f;
        float shakeMagnitude = 5f;
        elapsedTime = 0f;
        Vector2 originalPosition = text.rectTransform.anchoredPosition;

        while (elapsedTime < shakeTime)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;
            text.rectTransform.anchoredPosition = originalPosition + randomOffset;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.text = $"x{value}";
        text.rectTransform.anchoredPosition = originalPosition;

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            text.rectTransform.localScale = Vector3.Lerp(largeScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.rectTransform.localScale = originalScale;
    }

    public IEnumerator AnimateFreeSpin(TextMeshProUGUI text, int spins)
    {
        Vector3 originalScale = text.rectTransform.localScale;
        Vector3 largeScale = originalScale * 1.5f;

        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            text.rectTransform.localScale = Vector3.Lerp(originalScale, largeScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float shakeTime = 0.3f;
        float shakeMagnitude = 5f;
        elapsedTime = 0f;
        Vector2 originalPosition = text.rectTransform.anchoredPosition;

        while (elapsedTime < shakeTime)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;
            text.rectTransform.anchoredPosition = originalPosition + randomOffset;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.text = spins > 0 ? $"{spins}" : "";
        text.rectTransform.anchoredPosition = originalPosition;

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            text.rectTransform.localScale = Vector3.Lerp(largeScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.rectTransform.localScale = originalScale;
    }

    public IEnumerator AnimateWinnings(TextMeshProUGUI text, float from, float to, Vector3 originalScale)
    {
        Vector3 largeScale = originalScale * 1.5f;

        float elapsed = 0f;
        float duration = 0.6f;
        while (elapsed < duration)
        {
            float current = Mathf.Lerp(from, to, elapsed / duration);
            text.text = $"{current:F2}";
            text.rectTransform.localScale = Vector3.Lerp(originalScale, largeScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        text.text = $"{to:F2}";
        text.rectTransform.localScale = largeScale;

        float shakeTime = 0.3f;
        float shakeMagnitude = 5f;
        elapsed = 0f;
        Vector2 originalPosition = text.rectTransform.anchoredPosition;

        while (elapsed < shakeTime)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;
            text.rectTransform.anchoredPosition = originalPosition + randomOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        text.rectTransform.anchoredPosition = originalPosition;
        text.rectTransform.localScale = originalScale;
    }

    public IEnumerator AnimateWinningsToZero(TextMeshProUGUI text, float from, Vector3 originalScale)
    {
        float elapsed = 0f;
        float to = 0f;
        float duration = 1f;

        Vector3 largeScale = originalScale * 1.5f;
        Vector2 originalPosition = text.rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            float current = Mathf.Lerp(from, to, elapsed / duration);
            text.text = $"{current:F2}";
            text.rectTransform.localScale = Vector3.Lerp(originalScale, largeScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        text.text = "0.00";
        text.rectTransform.localScale = originalScale;

        float shakeTime = 0.3f;
        float shakeMagnitude = 3f;
        elapsed = 0f;
        while (elapsed < shakeTime)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;
            text.rectTransform.anchoredPosition = originalPosition + randomOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        text.rectTransform.anchoredPosition = originalPosition;
    }

    public IEnumerator AnimateBalance(TextMeshProUGUI text, float from, float to)
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float current = Mathf.Lerp(from, to, elapsed / duration);
            text.text = $"{current:F2}USDT";
            elapsed += Time.deltaTime;
            yield return null;
        }

        text.text = $"{to:F2}USDT";
    }
}